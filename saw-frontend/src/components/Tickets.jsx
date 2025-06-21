import React, { useState } from 'react';

function Tickets() {
    const [eventIdToFetch, setEventIdToFetch] = useState('');
    const [eventIdToCreate, setEventIdToCreate] = useState('');
    const [ticketIdToDelete, setTicketIdToDelete] = useState('');
    const [tickets, setTickets] = useState([]);

    const handleFetchTickets = async (e) => {
        e.preventDefault();

        if (!eventIdToFetch) {
            alert('Podaj ID wydarzenia.');
            return;
        }

        try {
            const response = await fetch(`/api/tickets/${eventIdToFetch}`);
            const data = await response.json();

            if (!response.ok) {
                alert(data.message || 'Błąd pobierania biletów.');
                return;
            }

            setTickets(data?.$values || []);
        } catch (error) {
            console.error('Błąd pobierania biletów:', error);
            alert('Wystąpił błąd podczas pobierania biletów.');
        }
    };

    const handleCreateTicket = async (e) => {
        e.preventDefault();

        if (!eventIdToCreate) {
            alert('Podaj ID wydarzenia.');
            return;
        }

        try {
            const response = await fetch(`/api/tickets/${eventIdToCreate}`, {
                method: 'POST'
            });

            const data = await response.json();

            if (response.ok) {
                alert(data.message || 'Bilet dodany!');
                setEventIdToCreate('');
            } else {
                alert(data.message || 'Nie udało się dodać biletu.');
            }
        } catch (error) {
            console.error('Błąd dodawania biletu:', error);
        }
    };

    const handleDeleteTicket = async () => {
        if (!ticketIdToDelete) {
            alert('Podaj ID biletu do usunięcia.');
            return;
        }

        try {
            const response = await fetch(`/api/tickets/${ticketIdToDelete}`, {
                method: 'DELETE'
            });

            const data = await response.json();

            if (response.ok) {
                alert(data.message || 'Bilet usunięty!');
                setTicketIdToDelete('');
                setTickets(prev => prev.filter(t => t.id !== parseInt(ticketIdToDelete)));
            } else {
                alert(data.message || 'Nie udało się usunąć biletu.');
            }
        } catch (error) {
            console.error('Błąd usuwania biletu:', error);
        }
    };

    return (
        <div>
            <h2>Bilety</h2>

            <form onSubmit={handleFetchTickets}>
                <input
                    type="number"
                    className="form-control mb-2"
                    placeholder="ID wydarzenia (do pobrania biletów)"
                    value={eventIdToFetch}
                    onChange={(e) => setEventIdToFetch(e.target.value)}
                    required
                />
                <button type="submit" className="btn btn-primary">Pobierz bilety</button>
            </form>

            <form onSubmit={handleCreateTicket}>
                <input
                    type="number"
                    className="form-control mb-2"
                    placeholder="ID wydarzenia (do utworzenia biletu)"
                    value={eventIdToCreate}
                    onChange={(e) => setEventIdToCreate(e.target.value)}
                    required
                />
                <button type="submit" className="btn btn-info">Dodaj bilet</button>
            </form>

            <ul className="list-group mt-3">
                {tickets.length === 0 ? (
                    <li className="list-group-item">Brak biletów do wyświetlenia.</li>
                ) : (
                    tickets.map(ticket => (
                        <li key={ticket.id} className="list-group-item">
                            Bilet ID: {ticket.id}, Wydarzenie ID: {ticket.eventId}
                        </li>
                    ))
                )}
            </ul>

            <div className="mt-3">
                <input
                    type="number"
                    className="form-control mb-2"
                    placeholder="ID biletu do usunięcia"
                    value={ticketIdToDelete}
                    onChange={(e) => setTicketIdToDelete(e.target.value)}
                    required
                />
                <button className="btn btn-danger" onClick={handleDeleteTicket}>Usuń bilet</button>
            </div>
        </div>
    );
}

export default Tickets;
