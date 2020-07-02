import React from 'react';
import JoinGameForm from './JoinGameForm';
import { Link } from 'react-router-dom';

const HomePage = props => (
    <div className="home-square">
        <h1>Join a game!</h1>
        <JoinGameForm {...props}/>
        <br/>
        <Link to="/host">Host a game</Link><br/>
        <Link to="/about">About</Link>
    </div>
);

export default HomePage;
