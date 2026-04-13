import React, { useEffect, useState } from 'react';
import './GamePage.scss';
import { TopPage } from '../../components';
import { getGameById, getMoreGameData } from '../../services/gameService'
import { EMPTHY_GAME, Game } from "../../Model/Game"
import { useNavigate, useParams, useLocation } from 'react-router-dom'
import { Sweeper } from "../../components/MySweeper"

const GamePage = () => {
    const navigate = useNavigate();
    const { appId } = useParams();
    // const location = useLocation();
    // const gameid = location.state?.id;

    const [game, setGame] = useState<Game>(EMPTHY_GAME);

    //use effect to fetch the game from DB 
    useEffect(() => {

        fetchData();

    }, [])

    const fetchData = async () => {
        if (!appId) return;

        try {
            const gamefromDB = await getGameById(appId);

            console.log("Steam App ID from DB:", gamefromDB.steamAppId);

            if (gamefromDB.steamAppId === 0) {
                setGame({
                    ...gamefromDB,
                    description: "Legacy Title: Information not ava ilable.",
                    screenshots: [gamefromDB.imageURL]
                });
                return;
            }

            setGame(gamefromDB);

            if (gamefromDB && gamefromDB.steamAppId) {
                const gamefulldata = await getMoreGameData(gamefromDB.steamAppId);
                console.log("Full data fetched:", gamefulldata);
                setGame(gamefulldata);
            }

        } catch (error) {
            console.error("Failed to fetch game data", error);
        }
    }


    return (

        <div className="game-page-container">
            <TopPage />
            <div className='game-page-wrapper'>
                <div className='sweeper-container'>
                    <Sweeper
                        screenshots={game.screenshots}
                        isSingle={true}
                    />
                </div>
                <h1 className='title-item'>{game.title}</h1>
                <div className='game-container'>
                    <div className='game-container--top'>

                        <span className='top-item'>Genere: {game.genre?.join(' ,')}</span>
                        <span className='top-item'>Developer: {game.developers?.join(' ,')}</span>
                        <span className='top-item'>Price: {game.price}</span>
                    </div>

                    <div className='game-container--detiles'>
                        <div className='about-item'
                            dangerouslySetInnerHTML={{ __html: game.description || "" }}
                        />

                    </div>



                </div>


            </div>
        </div>
    );
};

export default GamePage;