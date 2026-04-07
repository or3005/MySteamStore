import React,{} from 'react';
import './GameGrid.scss'
import 'bootstrap/dist/css/bootstrap.min.css';
import { Button, Container } from 'react-bootstrap';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import {GamePage} from '../../pages/GamePage'
interface GameGridProp {

    appIds: number[],
    ImageUrls: string[],

}


const GameGrid:React.FC<GameGridProp> = ({appIds, ImageUrls}) => {





return(
    <ul className='card-container'>

        {appIds.map((id, index) => (
            <li
            className='game-card' key={id}
                    onClick={()=>
                    {
                        
                    }
                    }
                    >
                <img src={ImageUrls[index]}
                ></img>

            </li>
        ))}

    </ul>
)
}
export default GameGrid;