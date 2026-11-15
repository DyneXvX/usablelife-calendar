import { NavLink, Outlet } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

/**
 * The shared page layout: navigation links at the top, with the current page
 * rendered below. Approver-only links only show up for approvers.
 */
function Layout() {
    const { isApprover, logout } = useAuth();

    return (
        <div>
            <nav className="nav">
                <span className="nav-title">School Event Calendar</span>
                <NavLink to="/calendar">Calendar</NavLink>
                <NavLink to="/submit-event">Submit Event</NavLink>
                {isApprover && <NavLink to="/pending-events">Pending Events</NavLink>}
                {isApprover && <NavLink to="/users">Users</NavLink>}
                {isApprover && <NavLink to="/manage-events">Manage Events</NavLink>}
                <button type="button" className="btn" onClick={logout}>Log Out</button>
            </nav>
            <Outlet />
        </div>
    );
}

export default Layout;
