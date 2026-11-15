import { Link, Outlet } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

/**
 * The shared page layout: navigation links at the top, with the current page
 * rendered below. Approver-only links only show up for approvers.
 */
function Layout() {
    const { isApprover, logout } = useAuth();

    return (
        <div>
            <nav>
                <Link to="/calendar">Calendar</Link>
                <Link to="/submit-event">Submit Event</Link>
                {isApprover && <Link to="/pending-events">Pending Events</Link>}
                {isApprover && <Link to="/users">Users</Link>}
                {isApprover && <Link to="/manage-events">Manage Events</Link>}
                <button type="button" onClick={logout}>Log Out</button>
            </nav>
            <Outlet />
        </div>
    );
}

export default Layout;
