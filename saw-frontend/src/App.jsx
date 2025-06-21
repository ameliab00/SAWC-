import React, { useState } from 'react';
import NavigationTitles from './components/NavigationTitles';
import Events from './components/Events';
import Tickets from './components/Tickets';
import Reviews from './components/Reviews';
import Users from './components/Users';

function App() {
    const [activeTab, setActiveTab] = useState('events');

    const renderContent = () => {
        switch (activeTab) {
            case 'events':
                return <Events />;
            case 'tickets':
                return <Tickets />;
            case 'reviews':
                return <Reviews />;
            case 'users':
                return <Users />;
            default:
                return <Events />;
        }
    };

    return (
        <div className="container mt-4">
            <NavigationTitles onClick={setActiveTab} active={activeTab} />
            <hr />
            {renderContent()}
        </div>
    );
}

export default App;
