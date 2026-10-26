import React, { createContext, useContext, useEffect, useState, ReactNode } from "react";
import { HubConnection } from "@microsoft/signalr";
import { createConnection } from "./sockethub";
import { useCurrentUser } from "../Auth/CurrentUserProvider";

// "connected"    - WebSocket is open, invoke() works
// "reconnecting" - the socket dropped, withAutomaticReconnect() is retrying (0s, 2s, 10s, 30s)
// "disconnected" - not logged in, or reconnect gave up
export type SocketStatus = "connecting" | "connected" | "reconnecting" | "disconnected";

interface SocketContextValue {
    connection: HubConnection | null;
    status: SocketStatus;
}

// 1. Create the Context (the "box")
const SocketContext = createContext<SocketContextValue>({ connection: null, status: "disconnected" });

// 2. Create the Hook (shortcut to open the box)
export const useSocket = () => useContext(SocketContext);

// 3. Create the Provider (fills the box and wraps children)
// One connection for the whole app - every page shares it instead of opening its own socket.
export const SocketProvider = ({ children }: { children: ReactNode }) => {
    const { user } = useCurrentUser();
    const [connection, setConnection] = useState<HubConnection | null>(null);
    const [status, setStatus] = useState<SocketStatus>("disconnected");

    useEffect(() => {
        if (!user?.id) {
            return; // no user -> no socket. When the user logs in, `user` changes and this effect runs again.
        }

        const conn = createConnection(user.id);
        let stopped = false;

        conn.onreconnecting(() => setStatus("reconnecting"));
        conn.onreconnected(() => setStatus("connected"));
        conn.onclose(() => setStatus("disconnected"));

        setStatus("connecting");
        conn.start()
            .then(() => setStatus("connected"))
            .catch((err) => {
                // React StrictMode mounts twice in dev, so the first connection is stopped mid-start. That's expected.
                if (!stopped) {
                    console.error("SignalR error:", err);
                    setStatus("disconnected");
                }
            });

        setConnection(conn);

        // Cleanup = close the WebSocket when the user logs out or the app unmounts.
        return () => {
            stopped = true;
            conn.stop();
            setConnection(null);
            setStatus("disconnected");
        };
    }, [user?.id]);

    return (
        <SocketContext.Provider value={{ connection, status }}>
            {children}
        </SocketContext.Provider>
    );
};
