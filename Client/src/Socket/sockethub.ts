import { HubConnectionBuilder, HubConnection } from "@microsoft/signalr";

export const createConnection = (userId: string): HubConnection => {
    return new HubConnectionBuilder()
        .withUrl(`http://localhost:5210/chat?userId=${userId}`)
        .withAutomaticReconnect()
        .build();
};