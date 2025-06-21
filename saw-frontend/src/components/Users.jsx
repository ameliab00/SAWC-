import React, { useState } from 'react';

function Users() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [email, setEmail] = useState('');
    const [role, setRole] = useState('Participant');
    const [userIdToDelete, setUserIdToDelete] = useState('');
    const [users, setUsers] = useState([]);

    const handleCreateUser = async (e) => {
        e.preventDefault();

        const newUser = {
            username,
            password,
            email,
            role
        };

        try {
            const response = await fetch('/api/users', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(newUser)
            });

            const data = await response.json();

            if (response.ok) {
                alert(data.message || 'Użytkownik dodany!');
                setUsername('');
                setPassword('');
                setEmail('');
                setRole('Participant');
                handleShowUsers(); 
            } else {
                alert(data.message || 'Błąd dodawania użytkownika.');
            }
        } catch (error) {
            console.error('Błąd sieci:', error);
            alert('Błąd sieci podczas dodawania użytkownika.');
        }
    };

    const handleDeleteUser = async () => {
        if (!userIdToDelete.trim()) {
            alert('Wpisz ID użytkownika do usunięcia.');
            return;
        }

        try {
            const response = await fetch(`/api/users/${userIdToDelete}`, {
                method: 'DELETE'
            });

            const data = await response.json();

            if (response.ok) {
                alert(data.message || 'Użytkownik usunięty!');
                setUserIdToDelete('');
                handleShowUsers();
            } else {
                alert(data.message || 'Nie udało się usunąć użytkownika.');
            }
        } catch (error) {
            console.error('Błąd usuwania użytkownika:', error);
            alert('Błąd sieci podczas usuwania użytkownika.');
        }
    };

    const handleShowUsers = async () => {
        try {
            const response = await fetch('/api/users');
            const data = await response.json();

            if (response.ok) {
                const fetchedUsers = data.users?.$values || [];
                setUsers(fetchedUsers);
            } else {
                alert(data.message || 'Nie udało się pobrać użytkowników.');
            }
        } catch (error) {
            console.error('Błąd pobierania użytkowników:', error);
            alert('Błąd sieci podczas pobierania użytkowników.');
        }
    };

    return (
        <div>
            <h2>Użytkownicy</h2>

            <form onSubmit={handleCreateUser}>
                <input
                    type="text"
                    className="form-control mb-2"
                    placeholder="Nazwa użytkownika"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                    required
                />
                <input
                    type="password"
                    className="form-control mb-2"
                    placeholder="Hasło"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />
                <input
                    type="email"
                    className="form-control mb-2"
                    placeholder="Email użytkownika"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                />
                <select
                    className="form-control mb-2"
                    value={role}
                    onChange={(e) => setRole(e.target.value)}
                >
                    <option value="Participant">Participant</option>
                    <option value="Organizer">Organizer</option>
                </select>
                <button type="submit" className="btn btn-primary">Dodaj użytkownika</button>
            </form>

            <input
                type="text"
                className="form-control mb-2 mt-4"
                placeholder="ID użytkownika do usunięcia"
                value={userIdToDelete}
                onChange={(e) => setUserIdToDelete(e.target.value)}
            />
            <button className="btn btn-danger mb-2" onClick={handleDeleteUser}>Usuń użytkownika</button>
            <button className="btn btn-info mb-2 ms-2" onClick={handleShowUsers}>Pokaż użytkowników</button>

            <ul id="userList" className="mt-3">
                {users.length === 0 ? (
                    <li>Brak użytkowników do wyświetlenia.</li>
                ) : (
                    users.map(user => (
                        <li key={user.id}>
                            {user.userName} - {user.email} ({user.role})
                        </li>
                    ))
                )}
            </ul>
        </div>
    );
}

export default Users;
