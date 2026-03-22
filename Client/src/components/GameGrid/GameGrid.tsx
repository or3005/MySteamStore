import React,{useN} from 'react';
import 'bootstrap/dist/css/bootstrap.min.css';
import { Button, Container } from 'react-bootstrap';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import {GamePage} from '../../pages/GamePage'
interface GameGridProp {

    appId: number[],
    ImageUrls: string[],

}


const GameGrid = (appId: number[], ImageUrls: string[]) => {






    <ul className='card-container'>

        {appId.map((Id, index) => (
            <li
            className='game-card' key={"appId"}>
                    onClick={
                        
                    }
                <img src={ImageUrls[index]}
                ></img>

            </li>
        ))}

    </ul>

}
export default GameGrid;