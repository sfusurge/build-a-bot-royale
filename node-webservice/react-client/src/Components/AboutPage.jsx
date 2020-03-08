import React from 'react';
import { Link } from 'react-router-dom';

const myHeader = {
    fontSize: '16pt',
}

const listContainer = {
    display: 'flex',
    alignItems: 'flex_eslkgndsnd',
    width: 'auto',
    //padding: '2em',
    //background: '#eee',
    margin: '0 auto',
    textAlign: 'right',
  }

const ul = {
    display: 'inline_block',
    textAlign: 'left',
  }


const AboutPage = props => (
    <div className="about-page">
        <h1>About</h1>
        <p>Surge SFU Project Spring 2020</p> 
        <div style={myHeader}>Created by:</div> 
        <Contributors />
        <Link to="/">Back</Link>
    </div>
);


const Contributors = props => (
    <div style={listContainer}>
        <ul style={ul}>
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
