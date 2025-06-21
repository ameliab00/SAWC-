import React from 'react';

const tiles = [
    { key: 'events', label: 'Wydarzenia', className: 'bg-primary text-white' },
    { key: 'tickets', label: 'Bilety', className: 'bg-success text-white' },
    { key: 'reviews', label: 'Recenzje', className: 'bg-warning text-dark' },
    { key: 'users', label: 'Użytkownicy', className: 'bg-danger text-white' }
];

const NavigationTitles = ({ onClick, active }) => (
    <div className="row text-center">
        {tiles.map(({ key, label, className }) => (
            <div
                key={key}
                className={`col-md-3 tile ${className} ${active === key ? 'border border-4 border-light' : ''}`}
                style={{ cursor: 'pointer', padding: '20px 0' }}
                onClick={() => onClick(key)}
            >
                {label}
            </div>
        ))}
    </div>
);

export default NavigationTitles;
