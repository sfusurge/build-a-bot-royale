import React from 'react';
import JoinGameForm from './JoinGameForm';
import { Link } from 'react-router-dom';

const HomePage = props => (
    <div className="home-page">
        <Link to="/host">Host a game</Link><br/>
        <Link to="/about">About</Link>
        <JoinGameForm {...props}/>
    </div>
);

export default HomePage;
