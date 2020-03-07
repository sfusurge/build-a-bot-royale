import React, { Component } from 'react';

const TestGamePage = (props) => (
  <div>
    <h1>In test game <code>{ props.gameID }</code></h1>
    <p>Change gameplay phase to:</p>
    {
      props.gameStates.map((gamestate, index) =>
        <button
          key={ index }
          onClick={ () => props.onChangeStateClicked(gamestate) }
        >{ gamestate }</button>
      )
    }
  </div>
)

export default TestGamePage;
