import React from 'react';
import { render } from '@testing-library/react';
import HostGamePage from '../Components/HostGamePage';
import { BrowserRouter as Router } from 'react-router-dom';

it("Renders an iframe that embeds an Itch.io widget", () => {
  const { getByTitle } = render(
    <Router>
      <HostGamePage />
    </Router>
  );

  expect(getByTitle('game-embed')).toBeInTheDocument();
});
