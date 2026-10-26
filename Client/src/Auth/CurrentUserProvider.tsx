import React, { createContext, useContext, useState, ReactNode } from "react";
import { User } from "../Model/User";

// Who is logged in. Kept in localStorage so a page refresh doesn't log you out.
// TEMPORARY: once we have JWT, we'll keep the access token here instead of just the user.
const STORAGE_KEY = "currentUser";

interface CurrentUserContextValue {
    user: User | null;
    setUser: (user: User | null) => void;
}

const CurrentUserContext = createContext<CurrentUserContextValue>({ user: null, setUser: () => { } });

export const useCurrentUser = () => useContext(CurrentUserContext);

function readStoredUser(): User | null {
    try {
        const raw = localStorage.getItem(STORAGE_KEY);
        return raw ? JSON.parse(raw) : null;
    } catch {
        return null;
    }
}

export const CurrentUserProvider = ({ children }: { children: ReactNode }) => {
    const [user, setUserState] = useState<User | null>(readStoredUser);

    const setUser = (newUser: User | null) => {
        setUserState(newUser);
        try {
            if (newUser) {
                localStorage.setItem(STORAGE_KEY, JSON.stringify(newUser));
            } else {
                localStorage.removeItem(STORAGE_KEY);
            }
        } catch {
            // storage blocked (private mode) - the user just stays logged in for this tab only
        }
    };

    return (
        <CurrentUserContext.Provider value={{ user, setUser }}>
            {children}
        </CurrentUserContext.Provider>
    );
};
