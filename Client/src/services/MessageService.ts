import { Message } from "../Model/Message";



const BASE_URL_MESSAGE = "http://localhost:5210/api/messages";


async function messageRequest<T>(Url: string, options: RequestInit,): Promise<T> {

    try {
        const response = await fetch(`${BASE_URL_MESSAGE}${Url}`, options);
        if (!response.ok) {
            throw new Error((`Request failed: ${response.status}`));
        }
        const json = await response.json();
        return json;
    } catch (error) {
        console.log(error);
        throw error;
    }

}



export async function GetMessages(id1: string, id2: string) {
    return await messageRequest<Message[]>(`/history/${id1}/${id2}`, {
        method: "GET"
    })
}

// Sending a message goes through the SignalR hub, not REST:
// connection.invoke("SendMessage", { receiverId, content })
