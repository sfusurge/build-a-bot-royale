import React, { Component } from 'react';

const inputStyle = {
    width: 'calc((60px + 14vmin))',
    padding:'0.5vw', 
    fontSize: 'calc(10px + 2vmin)', 
    textAlign:'center',
    margin:'1vh'
}

const submitStyle = {
    width:'calc((10px + 2vmin)*5)',
    fontSize: 'calc(10px + 2vmin)',
    textAlign:'center',
}

class TextInput extends Component {
    constructor(props) {
        super(props);
        this.state = {
            gameID: '',
            nickname: ''
        }

        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleSubmit(event) {
        this.props.onSubmit({
            gameID: this.state.gameID,
            username: this.state.nickname
        });
        
        event.preventDefault();
    }

    render() { 
        return ( 
            <form onSubmit={this.handleSubmit} style={{display:'flex', flexDirection:'column', alignItems:'center'}}>
                <input
                    type="text"
                    placeholder="Game Code"
                    style={ inputStyle }
                    value={ this.state.gameID }
                    onChange={e => this.setState({ gameID: e.target.value.toUpperCase() })}
                />
                <input
                    type="text"
                    placeholder="Nickname"
                    style={ inputStyle }
                    value={ this.state.nickname }
                    onChange={e => this.setState({ nickname: e.target.value})}
                />
                <input type="submit" value='Enter' style={submitStyle}/>
            </form>
         );
    }
}
 
export default TextInput;
