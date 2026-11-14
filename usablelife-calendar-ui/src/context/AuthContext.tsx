import { createContext, useContext, useState } from 'react';
import type { ReactNode } from 'react';

interface AuthContextType {
    token: string | null;
    isApprover: boolean;
    login: (token: string, isApprover: boolean) => void;
    logout: () => void;
}

const AuthContext = createContext<AuthContextType | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
    const [token, setToken] = useState<string | null>(localStorage.getItem('token'));
    const [isApprover, setIsApprover] = useState<boolean>(localStorage.getItem('isApprover') === 'true');

    function login(newToken: string, newIsApprover: boolean) {
        localStorage.setItem('token', newToken);
        localStorage.setItem('isApprover', String(newIsApprover));
        setToken(newToken);
        setIsApprover(newIsApprover);
    }

    function logout() {
        localStorage.removeItem('token');
        localStorage.removeItem('isApprover');
        setToken(null);
        setIsApprover(false);
    }

    return (
        <AuthContext.Provider value={{ token, isApprover, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
}

export function useAuth() {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuth must be used inside an AuthProvider');
    }
    return context;
}
