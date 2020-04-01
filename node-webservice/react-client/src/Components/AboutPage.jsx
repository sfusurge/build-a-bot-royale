import React from 'react';
import { Link } from 'react-router-dom';

const aboutPage = {
    width: '700px',
    maxWidth: '700px',
    marginRight: '30px',
    marginLeft: '30px',
}

const p = {
    display: 'inline_block',
    textAlign: 'left',
    fontSize: '16pt',
    margin: '0 auto',
}

const myHeader = {
    fontSize: '21pt',
    marginTop: '21pt',
}

const listContainer = {
    display: 'flex',
    alignItems: 'flex_eslkgndsnd',
    width: 'auto',
    margin: '0 auto',
    textAlign: 'right',
}

const AboutPage = props => (
    <div className="about-page" style={aboutPage}>
        <h1>About</h1>
        <div style={myHeader}>Surge SFU Project Spring 2020</div> 
        <GameDescription />
        <div style={myHeader}>Created by:</div> 
        <Contributors />
        <p><a href="https://www.sfusurge.com/" target="_blank" rel="noopener noreferrer">SFU Surge Website</a></p>
        <Link to="/">Back</Link>
    </div>
);

const GameDescription = props => (
    <p style={p}>
        Build-a-Bot Royale is a party game for 2 to 100+ players.
        Using a Jackbox- or Kahoot-like setup, build a bot to end all bots,
        and watch it fight off the competition for a Victory Royale!  
        During the match, control the aggression of your bot to extend your survival.
    </p>
)

const Contributors = props => (
    <div style={listContainer}>
        <ul style={p}>
            <li>Thomas</li>
            <li>Shea</li>
            <li>Rustem</li>
            <li>Jocelyn</li>
            <li>Jason</li>
            <li>Harry</li>
        </ul>
    </div>
)

export default AboutPage;
