
import { Game } from "../Model/Game"
const BASE_URL = "http://localhost:5210/api/games";

async function gameRequest<T>(Url: string ,options: RequestInit,): Promise<T> {

    try {
        const response = await fetch(`${BASE_URL}${Url}`,options);
        if (!response.ok) {
            throw new Error(`Request failed: ${response.status}`);
        }
        const json = await response.json();

        return  json;
    }
    catch (error) {
        console.error(error);
        throw error;
    }

}


export async function getAllGames(): Promise<Game[]> {
    return await gameRequest<Game[]>("/postgres",{
        method:"GET"
    }
        
    );
}

export async function getMoreGameData(appId: number): Promise<Game> {
    return await gameRequest<Game>(`/steam-more-data?appId=${appId}`,{
        method:"GET"
    }
        );
}
export async function getGameById(id:string):Promise<Game>{
    return await gameRequest<Game>(`/${id}`,{
        method:"GET"
    }
        );
}