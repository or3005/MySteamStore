import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { HomePage } from './pages/HomePage';
import { GamePage } from './pages/GamePage';
import { LoginPage } from './pages/LoginPage';
import { SocketProvider } from "./Socket/SocketProvider";

function App() {
    const userId = "some-user-guid"; // from login, state, etc.

    return (
        <SocketProvider userId={userId}>
            <Router>
                <Routes>
                    <Route path="/" element={<LoginPage />} />
                    <Route path="/home" element={<HomePage />} />
                    <Route path="/game/:appId" element={<GamePage />} />
                </Routes>
            </Router>
        </SocketProvider>
    );
}

export default App;