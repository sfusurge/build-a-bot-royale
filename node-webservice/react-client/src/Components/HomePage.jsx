import React from 'react';
import JoinGameForm from './JoinGameForm';
import { Link } from 'react-router-dom';

const listContainer = {
    display: 'flex',
    alignItems: 'flex_eslkgndsnd',
    width: 'auto',
    margin: '0 auto',
}

const HomePage = props => (
    <div className="home-page">
        <h1>Join a game</h1>
        <div style={listContainer}>
            <JoinGameForm {...props} style={listContainer}/>
        </div>
        <br/>
        <Link to="/host">Host a game</Link><br/>
        <Link to="/about">About</Link>
    </div>
);

export default HomePage;
