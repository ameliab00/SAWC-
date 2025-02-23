console.log("JS działa!");

function handleCategoryClick(category) {
    const categories = ["events", "tickets", "reviews", "users"];
    categories.forEach(cat => $("#" + cat).hide());

    $("#" + category).fadeIn();

    const handlers = {
        "events": () => {
            attachEventHandlers();
        },
        "tickets": attachTicketHandlers,
        "reviews": attachReviewHandlers,
        "users": attachUserHandlers
    };

    if (handlers[category]) handlers[category]();
    else console.warn("Nieznana kategoria:", category);
}

function attachTicketHandlers() {
    const fetchTicketsForm = document.getElementById("fetchTicketsForm");
    const ticketList = document.getElementById("ticketList");
    const ticketsSection = document.getElementById("tickets");

    // Jeśli nie ma ticketList, nic nie robimy
    if (!ticketList) return;

    // Obsługuje zdarzenie formularza
    fetchTicketsForm.addEventListener("submit", async (event) => {
        event.preventDefault();
        const eventId = document.getElementById("eventIdForTickets").value;

        if (!eventId) {
            alert("Proszę podać ID wydarzenia.");
            return;
        }

        try {
            const response = await fetch(`/ticket/${eventId}`);
            const data = await response.json();
            console.log(data); // Logowanie odpowiedzi z serwera

            if (!response.ok) {
                alert(data.Message || "Błąd podczas pobierania biletów.");
                return;
            }

            if (data && data.$values && data.$values.length > 0) {
                ticketsSection.classList.remove("hidden");
                ticketList.innerHTML = "";


                data.$values.forEach(ticket => {
                    if (ticket.$ref) return;


                    const event = ticket.eventEntity || {};

                    const li = document.createElement('li');
                    li.innerHTML = `
                <p><strong>Bilet ID:</strong> ${ticket.id}</p>
                <p><strong>Data zakupu:</strong> ${ticket.purchaseDate}</p>
                <p><strong>Kod kreskowy:</strong> ${ticket.barcode}</p>
            `;
                    ticketList.appendChild(li);
                });
            } else {
                alert("Brak biletów dla tego wydarzenia.");
            }

        } catch (error) {
            console.error("Błąd podczas pobierania biletów:", error);
            alert('Wystąpił problem z pobieraniem biletów.');
        }
    });

    // Obsługuje usuwanie biletów
    document.getElementById("deleteTicketButton").addEventListener("click", async () => {
        const ticketId = document.getElementById("ticketIdToDelete").value;

        if (!ticketId) {
            alert("Proszę podać ID biletu do usunięcia.");
            return;
        }

        if (!confirm("Czy na pewno chcesz usunąć ten bilet?")) {
            return;
        }

        try {
            const response = await fetch(`/ticket/${ticketId}`, {
                method: 'DELETE',
                headers: {"Content-Type": "application/json"}
            });

            if (response.ok) {
                alert("Bilet został usunięty.");
                document.getElementById("ticketIdToDelete").value = ""; // Czyścimy pole po usunięciu
                document.querySelector(`li[data-id="${ticketId}"]`)?.remove(); // Usuwamy z listy
            } else {
                const errorData = await response.json();
                alert(errorData.message || "Błąd podczas usuwania biletu.");
            }
        } catch (error) {
            console.error("Błąd podczas usuwania biletu:", error);
            alert("Wystąpił problem z usuwaniem biletu.");
        }
    });

// Obsługa usuwania wydarzeń w wynikach wyszukiwania
    function renderEventList(events, container) {
        container.innerHTML = "";

        events.forEach(event => {
            const li = document.createElement("li");
            li.innerHTML = `
            <p><strong>${event.name}</strong> - ${event.date}</p>
            <button class="btn btn-primary view-tickets" data-id="${event.id}">Pokaż bilety</button>
        `;
            container.appendChild(li);
        });
    }


    // Obsługa formularza tworzenia biletów
    const createTicketForm = document.getElementById('createTicketForm');
    if (createTicketForm) {
        createTicketForm.addEventListener('submit', async (event) => {
            event.preventDefault();
            const eventId = document.getElementById('eventIdForTicket').value;

            try {
                const response = await fetch(`/ticket/${eventId}`, {
                    method: 'POST',
                    headers: {'Content-Type': 'application/json'}
                });

                const data = await response.json();

                if (response.ok) {
                    alert('Bilet dodany!');
                    fetchTickets(eventId); // Odśwież listę
                } else {
                    alert(data.message || 'Błąd przy dodawaniu biletu');
                }
            } catch (error) {
                console.error('Błąd przy dodawaniu biletu:', error);
                alert('Wystąpił problem z dodawaniem biletu.');
            }
        });
    }

// Pobieranie listy biletów dla wydarzenia
    async function fetchTickets(eventId) {
        try {
            const response = await fetch(`/ticket/${eventId}`);
            const data = await response.json();

            if (!response.ok) {
                alert(data.message || "Błąd podczas pobierania biletów.");
                return;
            }

            const ticketList = document.getElementById("ticketList");
            if (!ticketList) return;

            ticketList.innerHTML = ""; // Czyścimy listę przed dodaniem nowych elementów
            data.tickets.forEach(ticket => {
                const li = document.createElement('li');
                li.innerHTML = `
                <p>Bilet: ${ticket.name}</p>
                <p>Cena: ${ticket.price}</p>
                <button class="delete-ticket" data-id="${ticket.id}">Usuń bilet</button>
            `;
                ticketList.appendChild(li);
            });
        } catch (error) {
            console.error("Błąd podczas pobierania biletów:", error);
            alert('Wystąpił problem z pobieraniem biletów.');
        }
    }
}

function attachReviewHandlers() {
    const reviewList = document.getElementById("reviewList");
    if (!reviewList) return;

    document.getElementById("deleteReviewButton").addEventListener("click", async () => {
        const reviewId = document.getElementById("reviewIdToDelete").value;
        if (!reviewId) {
            alert("Proszę podać ID recenzji do usunięcia.");
            return;
        }

        if (!confirm("Czy na pewno chcesz usunąć tą recenzje?")) {
            return;
        }
        try {
            const response = await fetch(`/review/${reviewId}`, {method: 'DELETE',
                headers: {"Content-Type": "application/json"}
            });
            if (response.ok) {
                alert("Recenzja została usunięty.");
                document.getElementById("reviewIdToDelete").value = ""; 
                document.querySelector(`li[data-id="${reviewId}"]`)?.remove(); 
            } else {
                const errorData = await response.json();
                alert('Błąd podczas usuwania recenzji');
            }
        } catch (error) {
            console.error("Błąd podczas usuwania recenzji:", error);
            alert('Wystąpił problem z usuwaniem recenzji.');
        }
    });

// Obsługa formularza dodawania recenzji
const createReviewForm = document.getElementById('createReviewForm');
if (createReviewForm) {
    createReviewForm.addEventListener('submit', async (event) => {
        event.preventDefault();
        const eventId = document.getElementById('eventIdForReview').value;
        const reviewTitle = document.getElementById('reviewTitle').value;
        const reviewText = document.getElementById('reviewText').value;
        const reviewRating = document.getElementById('reviewRating').value;

        const reviewData = {
            title: reviewTitle,
            content: reviewText,
            rating: parseInt(reviewRating)  
        };

        try {
            const response = await fetch(`/review/${eventId}`, {
                method: 'POST',
                headers: {'Content-Type': 'application/json'},
                body: JSON.stringify(reviewData)
            });

            if (response.ok) {
                alert('Recenzja dodana!');
                fetchReviews(eventId);
            } else {
                const errorResponse = await response.json();
                console.error("Błąd:", errorResponse);
                alert(errorResponse.detail || "Błąd przy dodawaniu recenzji");
            }
        } catch (error) {
            console.error('Błąd:', error);
            alert('Błąd przy dodawaniu recenzji');
        }
    });
}
// Obsługa pobierania recenzji dla danego eventu
const fetchReviewsButton = document.getElementById("fetchReviewsButton");
if (fetchReviewsButton) {
    fetchReviewsButton.addEventListener("click", () => {
        const eventId = document.getElementById("eventIdToFetchReviews").value;
        if (eventId) {
            fetchReviews(eventId);
        } else {
            alert("Podaj ID wydarzenia!");
        }
    });
}


// Pobieranie recenzji dla danego eventu
async function fetchReviews(eventId) {
    try {
        const response = await fetch(`/review/${eventId}`);
        const data = await response.json();

        if (response.ok) {
            console.log("Załadowane recenzje:", data);
            const reviewList = document.getElementById("reviewList");
            if (reviewList) {
                reviewList.innerHTML = ""; // Czyścimy listę przed dodaniem nowych elementów

                // Sprawdzamy, czy data.$values istnieje i jest tablicą
                if (data.data && data.data.$values && Array.isArray(data.data.$values)) {
                    data.data.$values.forEach(review => {
                        const li = document.createElement('li');
                        li.innerHTML = `
                        <p><strong>${review.title}</strong></p>
                        <p>Recenzja: ${review.content}</p>
                        <p>Ocena: ${review.rating}/5</p>
                    `;
                        reviewList.appendChild(li);
                    });
                } else {
                    console.warn("Brak recenzji lub niepoprawny format danych:", data);
                    reviewList.innerHTML = "<p>Brak recenzji dla tego wydarzenia.</p>";
                }
            }
        } else {
            console.warn("Nie udało się pobrać recenzji:", data.message);
        }
    } catch (error) {
        console.error("Błąd podczas pobierania recenzji:", error);
    }
}
}
function attachUserHandlers() {
    // Tworzenie nowego użytkownika
    const createUserForm = document.getElementById("createUserForm");
    if (createUserForm) {
        createUserForm.addEventListener("submit", async (event) => {
            event.preventDefault();

            const UserName = document.getElementById("username").value.toString();
            const Password = document.getElementById("password").value.toString();
            const Email = document.getElementById("userEmail").value.toString();
            const UserRole = document.getElementById("userRole").value.toString();

            const requestData = {
                createUserRequest: {
                    UserName: UserName, 
                    Password: Password, 
                    Email: Email, 
                    UserRole: UserRole 
                }
            };
            console.log("Wysyłane dane:", JSON.stringify(requestData));

            try {
                const response = await fetch('/user', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        requestData: JSON.stringify(requestData)
                    })
                });
                if (response.ok) {
                    alert('Użytkownik został utworzony');
                    fetchUsers();
                } else {
                    const errorResponse = await response.json();
                    console.error("Błąd:", errorResponse);
                    const errorMessages = Object.values(errorResponse.errors).flat().join(', ');
                    alert(`Błąd: ${errorMessages}`);
                }
            } catch (error) {
                console.error("Error:", error);
                alert('Wystąpił problem z tworzeniem użytkownika.');
            }
        });
    }

    // Pobieranie listy użytkowników
    const getUserListButton = document.getElementById('getUserListButton');
    if (getUserListButton) {
        getUserListButton.addEventListener('click', async () => {
            await fetchUsers();
        });
    }

    // Usuwanie użytkownika
    const deleteUserButton = document.getElementById('deleteUserButton');
    if (deleteUserButton) {
        deleteUserButton.addEventListener('click', async () => {
            const userId = document.getElementById('userIdToDelete').value;
            await deleteUser(userId);
        });
    }

async function fetchUsers() {
    try {
        const response = await fetch('/user'); 
        if (response.ok) {
            const users = await response.json();
            console.log("Lista użytkowników:", users);
           
        } else {
            console.error("Błąd podczas pobierania użytkowników:", response.statusText);
        }
    } catch (error) {
        console.error("Wystąpił błąd:", error);
    }
}}



function attachEventHandlers() {
    const eventList = document.getElementById("eventList");
    const searchEventList = document.getElementById("searchEventList");

    // Obsługa dodawania wydarzenia
    const createEventForm = document.getElementById("createEventForm");
    if (createEventForm) {
        createEventForm.addEventListener("submit", async (event) => {
            event.preventDefault();

            const eventData = {
                title: document.getElementById("eventName").value,
                location: document.getElementById("eventLocation").value,
                price: parseFloat(document.getElementById("eventPrice").value),
                startingDate: document.getElementById("eventStartDate").value,
                endingDate: document.getElementById("eventEndDate").value,
                seatingCapacity: parseInt(document.getElementById("eventCapacity").value),
                description: document.getElementById("eventDescription").value
            };

            // Walidacja danych
            if (!eventData.title || !eventData.description || isNaN(eventData.price) || isNaN(eventData.seatingCapacity)) {
                alert("Proszę poprawnie wypełnić wszystkie pola.");
                return;
            }

            try {
                const response = await fetch("/event", {
                    method: "POST",
                    headers: {"Content-Type": "application/json"},
                    body: JSON.stringify(eventData)
                });

                const responseData = await response.json();
                if (response.ok) {
                    const successMessage = document.createElement("div");
                    successMessage.className = "alert alert-success mt-2";
                    successMessage.textContent = "Wydarzenie zostało pomyślnie dodane!";
                    createEventForm.appendChild(successMessage);

                    createEventForm.reset();

                    setTimeout(() => successMessage.remove(), 5000);
                } else {
                    alert(responseData.Message || "Błąd przy dodawaniu wydarzenia.");
                }
            } catch (error) {
                console.error("Błąd:", error);
                alert("Błąd przy dodawaniu wydarzenia.");
            }
        });
    }

    // Obsługa wyszukiwania wydarzeń
    const searchEventForm = document.getElementById("searchEventForm");
    if (searchEventForm) {
        searchEventForm.addEventListener("submit", async (event) => {
            event.preventDefault();
            const searchQuery = document.getElementById("searchQuery").value;

            if (!searchQuery) {
                alert("Proszę podać nazwę wydarzenia do wyszukania.");
                return;
            }

            try {
                const response = await fetch(`/event/search?query=${encodeURIComponent(searchQuery)}`);
                const responseData = await response.json();

                if (!response.ok) {
                    alert(responseData.Message || "Nie znaleziono wydarzeń.");
                    return;
                }
                if (searchEventList) {
                    renderEventList(responseData.events, searchEventList);
                } else {
                    console.error("Element 'searchEventList' nie istnieje w DOM");
                }
            } catch (error) {
                console.error("Błąd:", error);
                alert("Błąd przy wyszukiwaniu wydarzenia.");
            }
        });
    }

    // Obsługa usuwania wydarzeń w wynikach wyszukiwania
    searchEventList.addEventListener("click", async (event) => {
        if (event.target.classList.contains("delete-event")) {
            const eventId = event.target.getAttribute("data-id");

            try {
                const response = await fetch(`/event/${eventId}`, {method: 'DELETE'});

                if (response.ok) {
                    event.target.closest('li').remove();
                    alert("Wydarzenie zostało usunięte.");
                } else {
                    alert("Błąd podczas usuwania wydarzenia.");
                }
            } catch (error) {
                console.error("Błąd:", error);
                alert("Wystąpił problem z usuwaniem wydarzenia.");
            }
        }
    });
}

// Funkcja do pobierania listy wydarzeń 
async function fetchEvents() {
    try {
        const response = await fetch('/event');
        const data = await response.json();
        return data.events; // Zakładając, że odpowiedź ma pole 'events'
    } catch (error) {
        console.error('Błąd przy pobieraniu wydarzeń:', error);
        return [];
    }
}

// Funkcja renderująca listę wydarzeń
function renderEventList(events, targetList) {
    targetList.innerHTML = events.map(event => `
        <li class="list-group-item d-flex justify-content-between align-items-center">
            <span>${event.title} - ${event.location} (${event.startingDate} - ${event.endingDate})</span>
            <button class="btn btn-danger btn-sm delete-event" data-id="${event.id}">Usuń</button>
        </li>
    `).join('');


}

