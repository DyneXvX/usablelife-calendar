import { Navigate } from 'react-router-dom';
import type { ReactNode } from 'react';
import { useAuth } from '../context/AuthContext';

/**
 * Wraps a page so it requires a logged-in user, and optionally an approver.
 * Redirects to login (or the calendar) if the requirement isn't met.
 */
function ProtectedRoute({ children, approverOnly = false }: { children: ReactNode; approverOnly?: boolean }) {
    const { token, isApprover } = useAuth();

    if (!token) {
        return <Navigate to="/login" replace />;
    }

    if (approverOnly && !isApprover) {
        return <Navigate to="/calendar" replace />;
    }

    return <>{children}</>;
}

export default ProtectedRoute;
