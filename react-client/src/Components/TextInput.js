import React, { Component } from 'react';
class TextInput extends Component {
    constructor(props) {
        super(props);
        this.state = {value:''}
        this.handleChange = this.handleChange.bind(this);
        
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
            <form onSubmit={this.handleSubmit}>
                <label>
                    GAME CODE:
                    <input type="text" value={this.state.value} onChange={this.handleChange}/>
                </label>
                <input type="submit" value="Submit"/>
            </form>
         );
    }
}
 
export default TextInput;