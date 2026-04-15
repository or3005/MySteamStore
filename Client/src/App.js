import { SocketProvider } from "./Socket/SocketProvider";

function App() {
    const userId = "some-user-guid"; // from login, state, etc.

    return (
        <SocketProvider userId={userId}>
            <Router>
                <Routes>
                    <Route path="/" element={<HomePage />} />
                    <Route path="/game/:appId" element={<GamePage />} />
                </Routes>
            </Router>
        </SocketProvider>
    );
}