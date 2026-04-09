

export interface Game {

    id?: string;
    title: string;
    price?: string;
    description?: string;
    imageURL: string;
    screenshots?: string[];
    steamAppId: number;
    genre?: string[];
    developers?: string[];

}
export const EMPTHY_GAME: Game = {

    title: "",
    imageURL: "",
    steamAppId: 0,




}