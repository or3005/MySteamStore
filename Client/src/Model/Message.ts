

export interface Message {

    id?: string;
    content?: string;
    receiverId?: string;
    senderId?: string;
    createAt?: string;

}
export const EMPTY_MESSAGE: Message = {

    content: "",
    receiverId: "",
    senderId: "",

}
