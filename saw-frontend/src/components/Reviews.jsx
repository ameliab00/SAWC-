import React, { useState } from 'react';

function Reviews() {
    const [eventId, setEventId] = useState('');
    const [title, setTitle] = useState('');
    const [content, setContent] = useState('');
    const [rating, setRating] = useState('');
    const [fetchEventId, setFetchEventId] = useState('');
    const [deleteReviewId, setDeleteReviewId] = useState('');
    const [reviews, setReviews] = useState([]);

    const handleCreateReview = async (e) => {
        e.preventDefault();

        const reviewData = {
            title,
            content,
            rating: parseInt(rating),
        };

        try {
            const response = await fetch(`/api/reviews/${eventId}`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(reviewData)
            });

            const result = await response.json();

            if (response.ok) {
                alert(result.message || 'Recenzja dodana!');
                setTitle('');
                setContent('');
                setRating('');
                setEventId('');
                // Odświeżanie
                if (eventId === fetchEventId) handleFetchReviews();
            } else {
                alert(result.message || 'Błąd dodawania recenzji.');
            }
        } catch (error) {
            console.error('Błąd sieci:', error);
            alert('Błąd sieci podczas dodawania recenzji.');
        }
    };

    const handleFetchReviews = async () => {
        if (!fetchEventId) return alert('Podaj ID wydarzenia.');

        try {
            const response = await fetch(`/api/reviews/${fetchEventId}`);
            const result = await response.json();

            if (response.ok && result.data && Array.isArray(result.data.$values)) {
                setReviews(result.data.$values);
            } else {
                setReviews([]);
                alert(result.message || 'Nie znaleziono recenzji.');
            }
        } catch (error) {
            console.error('Błąd pobierania recenzji:', error);
            alert('Błąd pobierania recenzji.');
        }
    };

    const handleDeleteReview = async () => {
        if (!deleteReviewId) return alert('Podaj ID recenzji do usunięcia.');

        try {
            const response = await fetch(`/api/reviews/${deleteReviewId}`, {
                method: 'DELETE'
            });

            const result = await response.json();

            if (response.ok) {
                alert(result.message || 'Recenzja usunięta!');
                setReviews(prev => prev.filter(r => r.id !== parseInt(deleteReviewId)));
                setDeleteReviewId('');
            } else {
                alert(result.message || 'Nie udało się usunąć recenzji.');
            }
        } catch (error) {
            console.error('Błąd usuwania recenzji:', error);
            alert('Błąd podczas usuwania recenzji.');
        }
    };

    return (
        <div>
            <h2>Recenzje</h2>

            <form onSubmit={handleCreateReview}>
                <input
                    type="number"
                    className="form-control mb-2"
                    placeholder="ID wydarzenia"
                    value={eventId}
                    onChange={(e) => setEventId(e.target.value)}
                    required
                />
                <input
                    type="text"
                    className="form-control mb-2"
                    placeholder="Tytuł recenzji"
                    value={title}
                    onChange={(e) => setTitle(e.target.value)}
                    required
                />
                <textarea
                    className="form-control mb-2"
                    placeholder="Napisz recenzję"
                    value={content}
                    onChange={(e) => setContent(e.target.value)}
                    required
                />
                <input
                    type="number"
                    className="form-control mb-2"
                    placeholder="Ocena (1-5)"
                    min="1"
                    max="5"
                    value={rating}
                    onChange={(e) => setRating(e.target.value)}
                    required
                />
                <button type="submit" className="btn btn-warning">Dodaj recenzję</button>
            </form>

            <div className="mt-4">
                <input
                    type="number"
                    className="form-control mb-2"
                    placeholder="ID wydarzenia do pobrania recenzji"
                    value={fetchEventId}
                    onChange={(e) => setFetchEventId(e.target.value)}
                />
                <button className="btn btn-primary" onClick={handleFetchReviews}>Pobierz recenzje</button>
            </div>

            <ul className="list-group mt-3">
                {reviews.length === 0 ? (
                    <li className="list-group-item">Brak recenzji do wyświetlenia.</li>
                ) : (
                    reviews.map((review) => (
                        <li key={review.id} className="list-group-item">
                            <strong>{review.title}</strong> – Ocena: {review.rating}/5
                            <br />
                            {review.content}
                        </li>
                    ))
                )}
            </ul>

            <div className="mt-4">
                <input
                    type="number"
                    className="form-control mb-2"
                    placeholder="ID recenzji do usunięcia"
                    value={deleteReviewId}
                    onChange={(e) => setDeleteReviewId(e.target.value)}
                />
                <button className="btn btn-danger" onClick={handleDeleteReview}>Usuń recenzję</button>
            </div>
        </div>
    );
}

export default Reviews;
