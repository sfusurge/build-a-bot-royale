import React from 'react';
import JoinGameForm from './JoinGameForm';
import { Link } from 'react-router-dom';

const HomePage = props => (
    <div className="home-page">
        <h1>Join a game</h1>
        <JoinGameForm {...props}/>
        <br/>
        <form>
            <button type="submit" formaction="/host" 
            style={{width:'calc(60px + 14vmin)', fontSize: 'calc(10px + 2vmin)', textAlign:'center', color:'green'}}>
                Host a Game
            </button>
        </form>
        <br/>
        <Link to="/about">About</Link>
    </div>
);

export default HomePage;
