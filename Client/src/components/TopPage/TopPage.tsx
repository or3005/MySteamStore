import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import './TopPage.scss';
import { useCurrentUser } from '../../Auth/CurrentUserProvider';

const TopPage: React.FC = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const { user } = useCurrentUser();

    const isHomePage = location.pathname === '/';

    return (
        <div className="top-page-container">
            {!isHomePage && (
                <button className="back-button" onClick={() => navigate(-1)}>
                    ← Back
                </button>
            )}
            <div className="logo">GAME STORE</div>
            {user && location.pathname !== '/chat' ? (
                <button className="back-button" onClick={() => navigate('/chat')}>
                    Chat
                </button>
            ) : (
                <div className="placeholder"></div>
            )}
        </div>
    );

};
export default TopPage;