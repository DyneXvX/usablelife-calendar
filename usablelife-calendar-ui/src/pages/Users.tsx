import { useEffect, useState } from 'react';
import type { SubmitEvent as ReactSubmitEvent } from 'react';
import { useAuth } from '../context/AuthContext';
import { apiFetch } from '../api/api';
import { API_BASE_URL } from '../api/config';
import { formatName } from '../utils/formatName';
import { parseErrorMessage } from '../utils/parseErrorMessage';
import Modal from '../components/Modal';

interface UserRecord {
    userId: number;
    firstName: string;
    middleName: string | null;
    lastName: string;
    logonName: string;
    isApprover: boolean;
}

/**
 * Approver-only page for managing user accounts: list, create, edit, delete.
 */
function Users() {
    const { token } = useAuth();
    const [users, setUsers] = useState<UserRecord[]>([]);
    const [error, setError] = useState('');

    const [editingId, setEditingId] = useState<number | null>(null);
    const [firstName, setFirstName] = useState('');
    const [middleName, setMiddleName] = useState('');
    const [lastName, setLastName] = useState('');
    const [logonName, setLogonName] = useState('');
    const [password, setPassword] = useState('');
    const [isApprover, setIsApprover] = useState(false);

    async function loadUsers() {
        const response = await apiFetch(`${API_BASE_URL}/api/users`, token);

        if (!response.ok) {
            setError('Could not load users.');
            return;
        }

        const data = await response.json();
        setUsers(data);
    }

    useEffect(() => {
        loadUsers();
    }, []);

    function startCreate() {
        setEditingId(0);
        setFirstName('');
        setMiddleName('');
        setLastName('');
        setLogonName('');
        setPassword('');
        setIsApprover(false);
    }

    function startEdit(user: UserRecord) {
        setEditingId(user.userId);
        setFirstName(user.firstName);
        setMiddleName(user.middleName ?? '');
        setLastName(user.lastName);
        setLogonName(user.logonName);
        setIsApprover(user.isApprover);
    }

    async function handleSubmit(e: ReactSubmitEvent) {
        e.preventDefault();
        setError('');

        if (editingId === 0) {
            const response = await apiFetch(`${API_BASE_URL}/api/users`, token, {
                method: 'POST',
                body: JSON.stringify({ firstName, middleName, lastName, logonName, password, isApprover }),
            });

            if (!response.ok) {
                const message = await parseErrorMessage(response);
                setError(message || 'Could not create the user.');
                return;
            }
        } else if (editingId !== null) {
            const response = await apiFetch(`${API_BASE_URL}/api/users/${editingId}`, token, {
                method: 'PUT',
                body: JSON.stringify({ firstName, middleName, lastName, logonName, isApprover }),
            });

            if (!response.ok) {
                const message = await parseErrorMessage(response);
                setError(message || 'Could not update the user.');
                return;
            }
        }

        setEditingId(null);
        loadUsers();
    }

    async function handleDelete(id: number) {
        const confirmed = window.confirm('Delete this user?');

        if (!confirmed) {
            return;
        }

        await apiFetch(`${API_BASE_URL}/api/users/${id}`, token, {
            method: 'DELETE',
        });
        loadUsers();
    }

    return (
        <div className="page">
            <h1>Users</h1>
            {error && <p className="error">{error}</p>}
            <button type="button" className="btn btn-primary" onClick={startCreate}>Add User</button>
            <table>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Logon Name</th>
                        <th>Approver</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {users.map((u) => (
                        <tr key={u.userId}>
                            <td>{formatName(u.firstName, u.middleName, u.lastName)}</td>
                            <td>{u.logonName}</td>
                            <td>{u.isApprover ? 'Yes' : 'No'}</td>
                            <td>
                                <button type="button" className="btn" onClick={() => startEdit(u)}>Edit</button>
                                <button type="button" className="btn btn-danger" onClick={() => handleDelete(u.userId)}>Delete</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {editingId !== null && (
                <Modal onClose={() => setEditingId(null)}>
                    <form onSubmit={handleSubmit}>
                        <h2>{editingId === 0 ? 'Add User' : 'Edit User'}</h2>
                        <div className="field">
                            <label>First Name</label>
                            <input type="text" value={firstName} onChange={(e) => setFirstName(e.target.value)} />
                        </div>
                        <div className="field">
                            <label>Middle Name</label>
                            <input type="text" value={middleName} onChange={(e) => setMiddleName(e.target.value)} />
                        </div>
                        <div className="field">
                            <label>Last Name</label>
                            <input type="text" value={lastName} onChange={(e) => setLastName(e.target.value)} />
                        </div>
                        <div className="field">
                            <label>Logon Name</label>
                            <input type="text" value={logonName} onChange={(e) => setLogonName(e.target.value)} />
                        </div>
                        {editingId === 0 && (
                            <div className="field">
                                <label>Password</label>
                                <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
                            </div>
                        )}
                        <div className="field">
                            <label>
                                <input type="checkbox" checked={isApprover} onChange={(e) => setIsApprover(e.target.checked)} />
                                {' '}Is Approver
                            </label>
                        </div>
                        <button type="submit" className="btn btn-primary">Save</button>
                        <button type="button" className="btn" onClick={() => setEditingId(null)}>Cancel</button>
                    </form>
                </Modal>
            )}
        </div>
    );
}

export default Users;
