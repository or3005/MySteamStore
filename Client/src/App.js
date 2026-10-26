import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { HomePage } from './pages/HomePage';
import { GamePage } from './pages/GamePage';
import { LoginPage } from './pages/LoginPage';
import { ChatPage } from './pages/ChatPage';
import { CurrentUserProvider } from "./Auth/CurrentUserProvider";
import { SocketProvider } from "./Socket/SocketProvider";

function App() {
    // Order matters: SocketProvider reads the user from CurrentUserProvider, so it must be inside it.
    return (
        <CurrentUserProvider>
            <SocketProvider>
                <Router>
                    <Routes>
                        <Route path="/" element={<LoginPage />} />
                        <Route path="/home" element={<HomePage />} />
                        <Route path="/game/:appId" element={<GamePage />} />
                        <Route path="/chat" element={<ChatPage />} />
                    </Routes>
                </Router>
            </SocketProvider>
        </CurrentUserProvider>
    );
}

export default App;
