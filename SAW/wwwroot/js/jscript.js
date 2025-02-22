console.log("JS działa!");

async function fetchEvents() {
    try {
        const response = await fetch("/event");

        if (!response.ok) {
            throw new Error("Błąd pobierania wydarzeń.");
        }

        const events = await response.json();
        console.log("Pobrane wydarzenia:", events);

        const eventList = document.getElementById("eventList");
        if (!eventList) return;

        eventList.innerHTML = events.map(event => `
            <li>
                <p>Nazwa: ${event.name}</p>
                <p>Data: ${event.date}</p>
                <p>Miejsce: ${event.location}</p>
                <button class="delete-event" data-id="${event.id}">Usuń</button>
            </li>
        `).join('');
    } catch (error) {
        console.error("Błąd:", error);
    }
}

function handleCategoryClick(category) {
    const categories = ["events", "tickets", "reviews", "users"];
    categories.forEach(cat => $("#" + cat).hide());

    $("#" + category).fadeIn();

    const handlers = {
        "events": () => { fetchEvents(); attachEventHandlers(); },
        "tickets": attachTicketHandlers,
        "reviews": attachReviewHandlers,
        "users": attachUserHandlers
    };

    if (handlers[category]) handlers[category]();
    else console.warn("Nieznana kategoria:", category);
}

function attachTicketHandlers() {
    const ticketList = document.getElementById("ticketList");
    if (!ticketList) return;

    ticketList.addEventListener("click", async (event) => {
        if (event.target.classList.contains("view-tickets")) {
            const eventId = event.target.getAttribute("data-id");

            try {
                const response = await fetch(`/ticket/${eventId}`);
                const data = await response.json();

                if (!response.ok) {
                    alert(data.message || "Błąd podczas pobierania biletów.");
                    return;
                }

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
        } else if (event.target.classList.contains("delete-ticket")) {
            const ticketId = event.target.getAttribute("data-id");

            try {
                const response = await fetch(`/ticket/${ticketId}`, { method: 'DELETE' });

                if (response.ok) {
                    event.target.closest('li').remove();
                    alert('Bilet został usunięty');
                } else {
                    alert('Błąd podczas usuwania biletu');
                }
            } catch (error) {
                console.error("Błąd podczas usuwania biletu:", error);
                alert('Wystąpił problem z usuwaniem biletu.');
            }
        }
    });

    // Obsługa formularza tworzenia biletów
    const createTicketForm = document.getElementById('createTicketForm');
    if (createTicketForm) {
        createTicketForm.addEventListener('submit', async (event) => {
            event.preventDefault();
            const eventId = document.getElementById('eventIdForTicket').value;

            try {
                const response = await fetch(`/ticket/${eventId}`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' }
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

function attachReviewHandlers() {
    const reviewList = document.getElementById("reviewList");
    if (!reviewList) return;

    reviewList.addEventListener("click", async (event) => {
        if (event.target.classList.contains("delete-review")) {
            const reviewId = event.target.getAttribute("data-id");
            console.log("ID do usunięcia:", reviewId); // Sprawdzenie ID

            try {
                const response = await fetch(`/review/${reviewId}`, { method: 'DELETE' });

                if (response.ok) {
                    event.target.closest('li').remove();
                    alert('Recenzja została usunięta');
                } else {
                    alert('Błąd podczas usuwania recenzji');
                }
            } catch (error) {
                console.error("Error:", error);
                alert('Wystąpił problem z usuwaniem recenzji.');
            }
        }
    });

    // Obsługa formularza dodawania recenzji
    const createReviewForm = document.getElementById('createReviewForm');
    if (createReviewForm) {
        createReviewForm.addEventListener('submit', async (event) => {
            event.preventDefault();
            const eventId = document.getElementById('eventIdForReview').value;
            const reviewText = document.getElementById('reviewText').value;

            try {
                const response = await fetch(`/review/${eventId}`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ reviewText })
                });

                if (response.ok) {
                    alert('Recenzja dodana!');
                    fetchReviews(eventId);
                } else {
                    alert('Błąd przy dodawaniu recenzji');
                }
            } catch (error) {
                console.error('Błąd:', error);
                alert('Błąd przy dodawaniu recenzji');
            }
        });
    }
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
                data.reviews.forEach(review => {
                    const li = document.createElement('li');
                    li.innerHTML = `
                        <p>Recenzja: ${review.text}</p>
                        <button class="delete-review" data-id="${review.id}">Usuń recenzję</button>
                    `;
                    reviewList.appendChild(li);
                });
            }
        } else {
            console.warn("Nie udało się pobrać recenzji:", data.message);
        }
    } catch (error) {
        console.error("Błąd podczas pobierania recenzji:", error);
    }
}

function attachUserHandlers() {
    // Tworzenie nowego użytkownika
    const createUserForm = document.getElementById("create-user-form");
    if (createUserForm) {
        createUserForm.addEventListener("submit", async (event) => {
            event.preventDefault();

            const userName = document.getElementById("username").value;
            const password = document.getElementById("password").value;
            const email = document.getElementById("userEmail").value;

            console.log("Sending request:", JSON.stringify({
                userName,
                password,
                email
            }));

            try {
                const response = await fetch('/user', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        userName,
                        password,
                        email
                    })
                });

                const data = await response.json();
                if (response.ok) {
                    alert('Użytkownik został utworzony');
                    fetchUsers();
                } else {
                    alert(`Błąd: ${data.message}`);
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
}
function attachEventHandlers() {
    const eventList = document.getElementById("eventList");
    if (!eventList) return;

    // Obsługa usuwania wydarzeń
    eventList.addEventListener("click", async (event) => {
        if (event.target.classList.contains("delete-event")) {
            const eventId = event.target.getAttribute("data-id");
            try {
                const response = await fetch(`/event/${eventId}`, { method: 'DELETE' });

                if (response.ok) {
                    event.target.closest('li').remove();
                    alert('Wydarzenie zostało usunięte.');
                } else {
                    const errorData = await response.json();
                    alert(errorData.Message || 'Błąd podczas usuwania wydarzenia.');
                }
            } catch (error) {
                console.error("Błąd:", error);
                alert('Wystąpił problem z usuwaniem wydarzenia.');
            }
        }
    });

    // Obsługa dodawania wydarzenia
    const createEventForm = document.getElementById("createEventForm");
    if (createEventForm) {
        createEventForm.addEventListener("submit", async (event) => {
            event.preventDefault();
            const eventName = document.getElementById("eventName").value;
            const eventDate = document.getElementById("eventDate").value;
            const eventLocation = document.getElementById("eventLocation").value;

            try {
                const response = await fetch("/event", {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({
                        name: eventName,
                        date: eventDate,
                        location: eventLocation
                    })
                });

                const responseData = await response.json();
                if (response.ok) {
                    alert(responseData.Message);
                    fetchEvents(); // Odśwież listę wydarzeń
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

            try {
                const response = await fetch(`/event/search?query=${encodeURIComponent(searchQuery)}`);
                const responseData = await response.json();

                if (!response.ok) {
                    alert(responseData.Message || "Nie znaleziono wydarzeń.");
                    return;
                }

                eventList.innerHTML = ""; // Czyścimy listę przed dodaniem nowych elementów
                responseData.Events.forEach(event => {
                    const li = document.createElement("li");
                    li.innerHTML = `
                        <p>Nazwa: ${event.name}</p>
                        <p>Data: ${event.date}</p>
                        <p>Miejsce: ${event.location}</p>
                        <button class="delete-event" data-id="${event.id}">Usuń</button>
                    `;
                    eventList.appendChild(li);
                });
            } catch (error) {
                console.error("Błąd:", error);
                alert("Błąd przy wyszukiwaniu wydarzenia.");
            }
        });
    }
}

