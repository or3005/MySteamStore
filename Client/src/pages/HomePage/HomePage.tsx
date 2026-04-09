import React, { useEffect, useState } from 'react';
import './HomePage.scss';
import  {Sweeper}  from "../../components/MySweeper";
import { GameGrid } from "../../components/GameGrid";
import { TopPage } from "../../components/TopPage";
import { Game } from "../../Model/Game"
import { getAllGames } from "../../services/gameService"

const HomePage = () => {


    const [games, setAllGames] = useState<Game[]>([]);
    useEffect(() => {

        getAllGames().then((game) => {
            setAllGames(game);
        })

    }, [])

    // const appIds=games.map(game=>game.steamappid);
    // const imagesUrls=games.map(game=>game.imageUrl);
    // const gameNames=games.map(game=>game.title); 
    

    return (
        <div className="home-container">

            <TopPage />
            <div className='sweeper-warpper'>
                <Sweeper
                    games={games}
                />
            </div>
            <div className='gamegrid-warpper'>
                <GameGrid
                    games={games}
                />
            </div>
        </div >
    );
};

export default HomePage;