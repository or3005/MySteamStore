import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import './TopPage.scss';

const TopPage: React.FC = () => {
    const navigate = useNavigate();
    const location = useLocation();

    // אנחנו לא רוצים להציג כפתור "חזור" אם אנחנו כבר בדף הבית
    const isHomePage = location.pathname === '/';

    return (
        <div className="top-page-container">
            {!isHomePage && (
                <button className="back-button" onClick={() => navigate(-1)}>
                    ← Back
                </button>
            )}
            <div className="logo">GAME STORE</div>
            <div className="placeholder"></div> {/* מקום לחיפוש בעתיד */}
        </div>
    );

};
export default TopPage;