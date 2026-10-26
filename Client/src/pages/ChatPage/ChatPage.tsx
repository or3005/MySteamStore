import React, { useEffect, useRef, useState } from "react";
import { Navigate } from "react-router-dom";
import "./ChatPage.scss";
import { TopPage } from "../../components/TopPage";
import { User } from "../../Model/User";
import { Message } from "../../Model/Message";
import { getAllusers } from "../../services/UserService";
import { GetMessages } from "../../services/MessageService";
import { useCurrentUser } from "../../Auth/CurrentUserProvider";
import { useSocket } from "../../Socket/SocketProvider";

const STATUS_TEXT = {
    connecting: "connecting...",
    connected: "online",
    reconnecting: "reconnecting...",
    disconnected: "offline",
};

// The server throws HubException("Receiver not found") and the client gets:
// "An unexpected error occurred invoking 'SendMessage' on the server. HubException: Receiver not found"
function hubErrorText(err: unknown): string {
    const text = err instanceof Error ? err.message : String(err);
    return text.split("HubException: ")[1] ?? "could not send the message";
}

const ChatPage: React.FC = () => {
    const { user } = useCurrentUser();
    const { connection, status } = useSocket();

    const [users, setUsers] = useState<User[]>([]);
    const [selected, setSelected] = useState<User | null>(null);
    const [messages, setMessages] = useState<Message[]>([]);
    const [unread, setUnread] = useState<Record<string, number>>({});
    const [draft, setDraft] = useState("");
    const [error, setError] = useState("");

    // Why a ref? The ReceiveMessage handler below is registered ONCE. If it read `selected` directly,
    // it would see the value from when it was registered forever (a "stale closure").
    // A ref is a box whose .current is always the latest value.
    const selectedIdRef = useRef<string | undefined>(undefined);
    const bottomRef = useRef<HTMLDivElement>(null);

    const isConnected = status === "connected";

    useEffect(() => {
        selectedIdRef.current = selected?.id;
    }, [selected]);

    // REST: the list of people you can talk to
    useEffect(() => {
        if (!user?.id) return;
        getAllusers()
            .then((all) => setUsers(all.filter((u) => u.id !== user.id)))
            .catch(() => setError("could not load users"));
    }, [user?.id]);

    // WebSocket: subscribe to the server's "ReceiveMessage" event (IChatClient.ReceiveMessage on the server).
    useEffect(() => {
        if (!connection) return;

        const onReceive = (message: Message) => {
            // The server sends to BOTH sides, so "the other person" is whoever isn't me.
            const otherId = message.senderId === user?.id ? message.receiverId : message.senderId;

            if (otherId === selectedIdRef.current) {
                // Guard against duplicates (e.g. history reload + live event for the same message)
                setMessages((prev) => (prev.some((m) => m.id === message.id) ? prev : [...prev, message]));
            } else {
                setUnread((prev) => ({ ...prev, [otherId]: (prev[otherId] ?? 0) + 1 }));
            }
        };

        connection.on("ReceiveMessage", onReceive);
        // Without off(), every re-render of this effect would add ANOTHER listener -> each message shows twice, three times...
        return () => connection.off("ReceiveMessage", onReceive);
    }, [connection, user?.id]);

    // REST: load the conversation when you pick someone - and again after a reconnect,
    // because messages sent while the socket was down were never pushed to us.
    useEffect(() => {
        if (!user?.id || !selected?.id) return;
        let cancelled = false;

        GetMessages(user.id, selected.id)
            .then((history) => { if (!cancelled) setMessages(history); })
            .catch(() => { if (!cancelled) setError("could not load the conversation"); });

        return () => { cancelled = true; }; // you clicked another user before this answer came back
    }, [user?.id, selected?.id, isConnected]);

    useEffect(() => {
        bottomRef.current?.scrollIntoView({ behavior: "smooth" });
    }, [messages]);

    if (!user) {
        return <Navigate to="/" replace />;
    }

    const selectUser = (other: User) => {
        setSelected(other);
        setMessages([]);
        setError("");
        setUnread((prev) => ({ ...prev, [other.id!]: 0 }));
    };

    const send = async () => {
        const content = draft.trim();
        if (!content || !selected?.id || !connection || !isConnected) return;

        try {
            // invoke = call a method on the server's ChatHub and wait for it to finish.
            // We don't add the message to the list here - it comes back through "ReceiveMessage"
            // with the real Id and CreateAt from the DB. The server is the source of truth.
            await connection.invoke("SendMessage", { receiverId: selected.id, content });
            setDraft("");
            setError("");
        } catch (err) {
            setError(hubErrorText(err));
        }
    };

    const onKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (e.key === "Enter") {
            e.preventDefault();
            send();
        }
    };

    return (
        <div className="chat-wrapper">
            <TopPage />
            <div className="chat-container">
                <aside className="chat-users">
                    <div className="chat-me">
                        <span className="chat-me-name">{user.userName}</span>
                        <span className={`chat-status ${status}`}>{STATUS_TEXT[status]}</span>
                    </div>
                    {users.map((u) => (
                        <button
                            key={u.id}
                            className={`chat-user ${selected?.id === u.id ? "active" : ""}`}
                            onClick={() => selectUser(u)}
                        >
                            <span>{u.userName}</span>
                            {!!unread[u.id!] && <span className="chat-unread">{unread[u.id!]}</span>}
                        </button>
                    ))}
                    {users.length === 0 && <span className="chat-empty">no other users yet</span>}
                </aside>

                <section className="chat-conversation">
                    {!selected ? (
                        <div className="chat-placeholder">pick someone to talk to</div>
                    ) : (
                        <>
                            <div className="chat-header">{selected.userName}</div>
                            <div className="chat-messages">
                                {messages.map((m) => (
                                    <div key={m.id} className={`chat-message ${m.senderId === user.id ? "mine" : "theirs"}`}>
                                        <span className="chat-text" dir="auto">{m.content}</span>
                                        {m.createAt && (
                                            <span className="chat-time">
                                                {new Date(m.createAt).toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" })}
                                            </span>
                                        )}
                                    </div>
                                ))}
                                <div ref={bottomRef} />
                            </div>
                            {error && <div className="chat-error">{error}</div>}
                            <div className="chat-input-row">
                                <input
                                    className="chat-input"
                                    dir="auto"
                                    placeholder={isConnected ? "write a message..." : "waiting for connection..."}
                                    value={draft}
                                    maxLength={2000}
                                    disabled={!isConnected}
                                    onChange={(e) => setDraft(e.target.value)}
                                    onKeyDown={onKeyDown}
                                />
                                <button className="chat-send" onClick={send} disabled={!isConnected || !draft.trim()}>
                                    Send
                                </button>
                            </div>
                        </>
                    )}
                </section>
            </div>
        </div>
    );
};

export default ChatPage;
