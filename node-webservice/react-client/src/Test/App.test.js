import React from 'react';
import { render, waitForElementToBeRemoved } from '@testing-library/react';
import App from '../App';
import { act } from 'react-dom/test-utils';
import MockSocket from './MockSocketHandler';
import { overrideSocket } from '../API/socketHandler.js';

const useMockSocket = function() {
  const mockSocket = new MockSocket();
  overrideSocket(mockSocket);
  return mockSocket;
}

it ("shows connection message before connected to socket", () => {
  const mockSocket = useMockSocket();

  const { getByText } = render(<App />);
  expect(getByText(/connecting/i)).toBeInTheDocument();
});

it ("does not show connection message after connected to socket", () => {
  const mockSocket = useMockSocket();

  const { queryByText } = render(<App />);
  expect(queryByText(/connecting/i)).toBeInTheDocument();
  mockSocket.serverEmit("connect");
  expect(queryByText(/connecting/i)).not.toBeInTheDocument();
});

it ("shows connection message after connecting then disconnecting from socket", () => {
  const mockSocket = useMockSocket();

  const { queryByText } = render(<App />);
  mockSocket.serverEmit("connect");
  mockSocket.serverEmit("disconnect");
  expect(queryByText(/connecting/i)).toBeInTheDocument();
});
