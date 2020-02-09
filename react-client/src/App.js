import React, { Component } from 'react';
import background from './background.jpg';
import TextInput from './Components/TextInput'
import './App.css';

const backgroundStyle = {
  backgroundImage:`url(${background})`,
  flex: 1,
  resizeMode: 'cover', 
  backgroundPosition:"center",
}

class App extends Component {
  constructor(props) {
    super(props);
    this.state = { apiResponse: "" };
  }

  callAPI() {
      fetch("http://localhost:9000/testAPI")
          .then(res => res.text())
          .then(res => this.setState({ apiResponse: res }));
  }

  componentWillMount() {
      this.callAPI();
  }

  render() {
    return (
      <div className="App">
        <header className="App-header">
          <h1>Build-a-Bot Royale</h1>
        </header>
        <div className="App-body" style={backgroundStyle}>
          <TextInput/>
        </div>
      </div>
    );
  }
}

export default App;
