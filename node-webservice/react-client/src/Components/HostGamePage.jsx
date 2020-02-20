import React from 'react';
import { Link } from 'react-router-dom';

const HostGamePage = props => (
    <div className="host-game-page">
        <h1>Host a game</h1>
        <p>Download links to builds of the Unity app go here</p>
        <Link to="/">Back</Link>
    </div>
);

export default HostGamePage;
