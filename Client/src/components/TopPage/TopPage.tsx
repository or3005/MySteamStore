import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import './TopPage.scss';

const TopPage: React.FC = () => {
    const navigate = useNavigate();
    const location = useLocation();

    const isHomePage = location.pathname === '/';

    return (
        <div className="top-page-container">
            {!isHomePage && (
                <button className="back-button" onClick={() => navigate(-1)}>
                    ← Back
                </button>
            )}
            <div className="logo">GAME STORE</div>
            <div className="placeholder"></div>
        </div>
    );

};
export default TopPage;