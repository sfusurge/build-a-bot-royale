import React, { Component } from 'react';

const inputStyle = {
    width: '25vw',
    padding:'0.5vw', 
    fontSize: 'calc(10px + 2vmin)', 
    textAlign:'center',
    margin:'1vh'
}

const submitStyle = {
    width:'15vw',
    fontSize: 'calc(10px + 2vmin)',
    textAlign:'center',
}



class TextInput extends Component {
    constructor(props) {
        super(props);
        this.state = {value:''}
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleChange(event){
        this.setState({value: event.target.value})
    }

    handleSubmit(event){
        alert("A game code was submitted: " + this.state.value);
        event.preventDefault();
    }

    render() { 
        return ( 
            <div style={{display:'table', height:'calc(100vh - calc(30px + 6vmin))'}}>
                <form onSubmit={this.handleSubmit} style={{display:'table-cell', verticalAlign:'middle', width:'30vw'}}>
                    <input type="text" placeholder="Game Code" style={inputStyle}value={this.state.value} onChange={this.handleChange}/>
                    <input type="submit" value='Enter' style={submitStyle}/>
                </form>
            </div>
         );
    }
}
 
export default TextInput;
