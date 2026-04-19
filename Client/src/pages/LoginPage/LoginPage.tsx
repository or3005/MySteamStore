import React, { useState } from "react";
import { login, Register } from "../../services/UserService";
import { useNavigate } from "react-router-dom";
import "./LoginPage.scss";




const LoginPage: React.FC = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [showPopup, setShowPopup] = useState(false);
    const [isRegister, setIsRegister] = useState(false);
    const [error, setError] = useState("");
    const navigate = useNavigate();

    const handleSubmit = async () => {
        setError("");
        try {
            if (isRegister) {
                const user = await Register(username, password);
                if (user) {
                    setIsRegister(false);
                    setError("registered successfully! you can login now");
                }
            } else {
                const user = await login(username, password);
                if (user) {
                    navigate("/home");
                }
            }
        } catch {
            setError(isRegister ? "registration failed" : "login failed - check your credentials");
        }
    };

    const openPopup = (registerMode: boolean) => {
        setIsRegister(registerMode);
        setShowPopup(true);
        setError("");
        setUsername("");
        setPassword("");
    };

    return (
        <div className="login-wrapper">
            <div className="login-container">
                <h1 className="login-title">My Steam Store</h1>
                <button className="login-button" onClick={() => openPopup(false)}>Login</button>
                <button className="register-button" onClick={() => openPopup(true)}>Register</button>
            </div>

            {showPopup && (
                <div className="popup-overlay" onClick={() => setShowPopup(false)}>
                    <div className="popup-container" onClick={(e) => e.stopPropagation()}>
                        <button className="popup-close" onClick={() => setShowPopup(false)}>X</button>
                        <h2 className="popup-title">{isRegister ? "Register" : "Login"}</h2>
                        <div className="name-container">
                            <span>user name:</span>
                            <input
                                className="input-item"
                                type="text"
                                placeholder="enter your name"
                                value={username}
                                onChange={(e) => setUsername(e.target.value)}
                            />
                        </div>
                        <div className="password-container">
                            <span>password:</span>
                            <input
                                className="input-item"
                                type="password"
                                placeholder="enter your password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                            />
                        </div>
                        {error && <span className={`error-message ${error.includes("successfully") ? "success" : ""}`}>{error}</span>}
                        <button className="login-button" onClick={handleSubmit}>
                            {isRegister ? "Register" : "Login"}
                        </button>
                    </div>
                </div>
            )}
        </div>
    );
};
export default LoginPage;