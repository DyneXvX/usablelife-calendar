import { useState, type SubmitEvent } from "react";
import { useAuth } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";
import { API_BASE_URL } from "../api/config";

function Login() {
    const [logonName, setLogonName] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();
    const { login } = useAuth();

    async function handleSubmit(e: SubmitEvent) {
        e.preventDefault();

        const response = await fetch(`${API_BASE_URL}/api/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ logonName, password }),
        });

        if (!response.ok) {
            setError('Logon name or password is incorrect.');
            return;
        }

        const data = await response.json()
        login(data.token, data.isApprover);
        navigate('/calendar');

    }
    return (
        <form onSubmit={handleSubmit}>
            <div>
                <h1>Login Page</h1>
                <input
                    type="text"
                    value={logonName}
                    onChange={(e) => setLogonName(e.target.value)}>
                </input>
            </div>
            <div>
                <input
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}>
                </input>
            </div>
            <p>{error}</p>
            <button type="submit">Log In</button>
        </form>
    );
}

export default Login;