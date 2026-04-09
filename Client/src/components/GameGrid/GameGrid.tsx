import React, { } from 'react';
import './GameGrid.scss'
import 'bootstrap/dist/css/bootstrap.min.css';
import { useNavigate } from 'react-router-dom';
import {Game} from "../../Model/Game"
interface GameGridProp {

    games:Game[],

}


const GameGrid: React.FC<GameGridProp> = ({ games }) => {

    const navigate = useNavigate();



    return (
        <ul className='card-container'
        style={{"--length":games.length}as React.CSSProperties}
        >

            {games.map((game) => (
                <li
                    className='game-card' key={game.id}
                    onClick={() => {
                            navigate(`/game/${game.id}`,{state:{id:game.id}})
                    }
                    }
                >
                    <div className='card-elements'>

                        <span className='game-name' >{game.title}</span>
                        <img className='game-image' src={game.imageURL} />

                    </div>
                </li>
            ))}

        </ul>
    )
}
export default GameGrid;