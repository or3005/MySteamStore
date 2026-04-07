

export interface Game {

    id?: string;
    title: string;
    price?: string;
    description?: string;
    imageUrl: string;
    screenshots?: string[];
    steamappid: number;
    genre?: string[];
    developers?: string[];

}
export const EMPTHY_GAME: Game = {

    title: "",
    imageUrl: "",
    steamappid: 0,




}