import React from 'react';
import './Home.scss';
import Sweeper from "../../components/MySweeper";
import GameGrid from "../../components/GameGrid";
import TopPage from "../../components/TopPage";


const HomePage = () => {
    return (
        <div className="home-container">
        
        <TopPage/>
        <Sweeper/> 
        <GameGrid/> 
                 
        </div>
    );
};

export default HomePage;