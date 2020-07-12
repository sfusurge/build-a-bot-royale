import React from 'react';
import { Link } from 'react-router-dom';


const HostGamePage = props => (
    <div className="host-game-page">
        <iframe title="game-embed" frameBorder="0" src="https://itch.io/embed-upload/1981246?color=333333" allowFullScreen="true"><a href="https://buildabot.itch.io/build-a-bot-royale">Play Build-A-Bot Royale on itch.io</a></iframe>
        <br/>
        <Link to="/">Back</Link>
    </div>
);

export default HostGamePage;
