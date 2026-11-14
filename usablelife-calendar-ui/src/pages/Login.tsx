import { useState, type SubmitEvent } from "react";

function Login() {
    const [logonName, setLogonName] = useState('');
    const [password, setPassword] = useState('');

    function handleSubmit(e: SubmitEvent) {
        e.preventDefault();
        console.log('Logon name:', logonName);
        console.log('Password:', password);
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
            <button type="submit">Log In</button>
        </form>
    );
}

export default Login;