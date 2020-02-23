import React from 'react';
import { Link } from 'react-router-dom';

const AboutPage = props => (
    <div className="about-page">
        <h1>About</h1>
        <p>Info about the project goes here</p>
        <Link to="/">Back</Link>
    </div>
);

export default AboutPage;
