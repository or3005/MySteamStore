import React, { createContext, useContext, useEffect, useState, ReactNode } from "react";
import { HubConnection } from "@microsoft/signalr";
import { createConnection } from "./sockethub";

// 1. Create the Context (the "box")
const SocketContext = createContext<HubConnection | null>(null);

// 2. Create the Hook (shortcut to open the box)
export const useSocket = () => useContext(SocketContext);

// 3. Create the Provider (fills the box and wraps children)
export const SocketProvider = ({ userId, children }: { userId: string; children: ReactNode }) => {
    const [connection, setConnection] = useState<HubConnection | null>(null);

    useEffect(() => {
        const conn = createConnection(userId);

        conn.start()
            .then(() => console.log("SignalR connected"))
            .catch((err) => console.error("SignalR error:", err));

        setConnection(conn);

        return () => { conn.stop(); };
    }, [userId]);

    return (
        <SocketContext.Provider value={connection}>
            {children}
        </SocketContext.Provider>
    );
};