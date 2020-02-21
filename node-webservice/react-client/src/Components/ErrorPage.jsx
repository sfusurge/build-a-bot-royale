import React from 'react';
import { Link } from "react-router-dom";

const ErrorPage = props => (
    <div>
      <h4>That's an error:</h4>
      <h2>{ props.children }</h2>
      <Link to="/">Back to home</Link>
    </div>
);

export default ErrorPage;
