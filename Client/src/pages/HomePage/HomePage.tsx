import React, { useEffect, useState } from 'react';
import './Home.scss';
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

    const appIds=games.map(game=>game.steamappid);
    const imagesUrls=games.map(game=>game.imageUrl);

    

    return (
        <div className="home-container">

            <TopPage />
            <div className='sweeper-warpper'>
                <Sweeper
                    ImageUrls={imagesUrls}
                    appId={appIds}
                />
            </div>
            <div className='gamegrid-warpper'>
                <GameGrid
                    ImageUrls={imagesUrls}
                    appIds={appIds}

                />
            </div>
        </div >
    );
};

export default HomePage;