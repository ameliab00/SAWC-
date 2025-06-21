import React, { useState } from 'react';

function Events() {
    const [searchQuery, setSearchQuery] = useState('');
    const [searchResults, setSearchResults] = useState([]);
    const [editingEvent, setEditingEvent] = useState(null);
    const [newEvent, setNewEvent] = useState({
        title: '',
        description: '',
        location: '',
        price: '',
        startingDate: '',
        endingDate: '',
        seatingCapacity: ''
    });

    
    function toISOStringWithoutTimeZone(dateStr) {
        const [year, month, day] = dateStr.split('-');
        const date = new Date(Date.UTC(year, month - 1, day, 0, 0, 0));
        return date.toISOString();
    }

    const handleSearch = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch(`/api/events/search?query=${encodeURIComponent(searchQuery)}`);
            const data = await response.json();

            if (response.ok && Array.isArray(data.events?.$values)) {
                setSearchResults(data.events.$values);
            } else {
                setSearchResults([]);
            }
        } catch (error) {
            console.error('Błąd wyszukiwania wydarzeń:', error);
            setSearchResults([]);
        }
    };

    const handleCreateEvent = async (e) => {
        e.preventDefault();

        const payload = {
            title: newEvent.title,
            description: newEvent.description,
            location: newEvent.location,
            price: parseFloat(newEvent.price),
            startingDate: toISOStringWithoutTimeZone(newEvent.startingDate),
            endingDate: toISOStringWithoutTimeZone(newEvent.endingDate),
            seatingCapacity: parseInt(newEvent.seatingCapacity)
        };

        try {
            const response = await fetch('/api/events', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });

            const data = await response.json();

            if (response.ok) {
                alert(data.message);
                setNewEvent({
                    title: '',
                    description: '',
                    location: '',
                    price: '',
                    startingDate: '',
                    endingDate: '',
                    seatingCapacity: ''
                });
            } else {
                alert(`Błąd tworzenia: ${data.message}`);
            }
        } catch (error) {
            console.error('Błąd tworzenia wydarzenia:', error);
        }
    };

    const handleDeleteEvent = async (eventId) => {
        if (!window.confirm('Czy na pewno chcesz usunąć to wydarzenie?')) return;

        try {
            const response = await fetch(`/api/events/${eventId}`, {
                method: 'DELETE'
            });
            const data = await response.json();
            if (response.ok) {
                alert(data.message);
                setSearchResults(prev => prev.filter(e => e.id !== eventId));
                if (editingEvent?.id === eventId) setEditingEvent(null);
            } else {
                alert(`Błąd usuwania: ${data.message}`);
            }
        } catch (error) {
            console.error('Błąd usuwania wydarzenia:', error);
        }
    };

    const handleEditClick = (event) => {
        setEditingEvent({
            id: event.id,
            price: event.price,
            startingDate: event.startingDate.slice(0, 10), 
            endingDate: event.endingDate.slice(0, 10),
            seatingCapacity: event.seatingCapacity,
            description: event.description
        });
    };

    const handleUpdateSubmit = async (e) => {
        e.preventDefault();

        const updatePayload = {
            price: parseFloat(editingEvent.price),
            startingDate: toISOStringWithoutTimeZone(editingEvent.startingDate),
            endingDate: toISOStringWithoutTimeZone(editingEvent.endingDate),
            seatingCapacity: parseInt(editingEvent.seatingCapacity),
            description: editingEvent.description
        };

        console.log("Aktualizowany payload:", updatePayload);

        try {
            const response = await fetch(`/api/events/${editingEvent.id}`, {
                method: 'PATCH',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(updatePayload)
            });

            const data = await response.json();

            if (response.ok) {
                alert(data.message);
                setEditingEvent(null);
                setSearchResults(prev =>
                    prev.map(ev => ev.id === data.event.id ? data.event : ev)
                );
            } else {
                alert(`Błąd aktualizacji: ${data.message || JSON.stringify(data.errors)}`);
            }
        } catch (error) {
            console.error('Błąd aktualizacji wydarzenia:', error);
        }
    };

    return (
        <div>
            <h2>Dodaj nowe wydarzenie</h2>
            <form onSubmit={handleCreateEvent} className="mb-4">
                <input type="text" className="form-control mb-2" placeholder="Tytuł"
                       value={newEvent.title}
                       onChange={(e) => setNewEvent({ ...newEvent, title: e.target.value })} required />

                <textarea className="form-control mb-2" placeholder="Opis"
                          value={newEvent.description}
                          onChange={(e) => setNewEvent({ ...newEvent, description: e.target.value })} required />

                <input type="text" className="form-control mb-2" placeholder="Lokalizacja"
                       value={newEvent.location}
                       onChange={(e) => setNewEvent({ ...newEvent, location: e.target.value })} required />

                <input type="number" className="form-control mb-2" placeholder="Cena"
                       value={newEvent.price}
                       onChange={(e) => setNewEvent({ ...newEvent, price: e.target.value })} required />

                <input type="date" className="form-control mb-2"
                       value={newEvent.startingDate}
                       onChange={(e) => setNewEvent({ ...newEvent, startingDate: e.target.value })} required />

                <input type="date" className="form-control mb-2"
                       value={newEvent.endingDate}
                       onChange={(e) => setNewEvent({ ...newEvent, endingDate: e.target.value })} required />

                <input type="number" className="form-control mb-2" placeholder="Liczba miejsc"
                       value={newEvent.seatingCapacity}
                       onChange={(e) => setNewEvent({ ...newEvent, seatingCapacity: e.target.value })} required />

                <button type="submit" className="btn btn-primary">Dodaj wydarzenie</button>
            </form>

            <h2>Wyszukaj wydarzenie</h2>
            <form onSubmit={handleSearch} className="mb-4">
                <input
                    type="text"
                    className="form-control mb-2"
                    placeholder="Szukaj po tytule"
                    value={searchQuery}
                    onChange={(e) => setSearchQuery(e.target.value)}
                    required
                />
                <button type="submit" className="btn btn-info">Szukaj</button>
            </form>

            <h5>Wyniki wyszukiwania:</h5>
            <ul className="list-group">
                {searchResults.map(event => (
                    <li key={event.id} className="list-group-item">
                        <strong>{event.title}</strong><br />
                        <span>Miejsce: {event.location}</span><br />
                        <span>Data: {new Date(event.startingDate).toLocaleDateString()} - {new Date(event.endingDate).toLocaleDateString()}</span><br />
                        <span>Cena: {event.price} zł</span><br />
                        <span>Miejsca: {event.seatingCapacity}</span><br />
                        <span>Opis: {event.description}</span><br />
                        <button className="btn btn-warning btn-sm mt-2 me-2" onClick={() => handleEditClick(event)}>Edytuj</button>
                        <button className="btn btn-danger btn-sm mt-2" onClick={() => handleDeleteEvent(event.id)}>Usuń</button>
                    </li>
                ))}
            </ul>

            {editingEvent && (
                <form className="mt-4" onSubmit={handleUpdateSubmit}>
                    <h4>Edytuj wydarzenie</h4>
                    <input type="number" className="form-control mb-2" placeholder="Cena"
                           value={editingEvent.price}
                           onChange={(e) => setEditingEvent({ ...editingEvent, price: e.target.value })} required />

                    <input type="date" className="form-control mb-2"
                           value={editingEvent.startingDate}
                           onChange={(e) => setEditingEvent({ ...editingEvent, startingDate: e.target.value })} required />

                    <input type="date" className="form-control mb-2"
                           value={editingEvent.endingDate}
                           onChange={(e) => setEditingEvent({ ...editingEvent, endingDate: e.target.value })} required />

                    <input type="number" className="form-control mb-2" placeholder="Liczba miejsc"
                           value={editingEvent.seatingCapacity}
                           onChange={(e) => setEditingEvent({ ...editingEvent, seatingCapacity: e.target.value })} required />

                    <textarea className="form-control mb-2" placeholder="Opis"
                              value={editingEvent.description}
                              onChange={(e) => setEditingEvent({ ...editingEvent, description: e.target.value })} required />

                    <button type="submit" className="btn btn-success">Zapisz zmiany</button>
                    <button type="button" className="btn btn-secondary ms-2" onClick={() => setEditingEvent(null)}>Anuluj</button>
                </form>
            )}
        </div>
    );
}

export default Events;
