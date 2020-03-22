import React, { Component } from 'react';
import Block from "../Images/block.PNG";
import Delete from "../Images/delete.PNG";
import Spike from "../Images/spikeNorth.PNG";
class TypeToolbar extends Component {
    
    typeChooser(){
        return(
         <table>
            <tr>
                <input  type="radio"  value="block"  id="block" class="input-hidden"  name="controller"
                defaultChecked = "checked"  
                onClick={() => this.props.onChangeType("block") } style={ this.style(Block) }
                />
                <label for="block">  
                    <img style={ this.style(Block) } src={Block} alt="Block"/>
                 </label>           
            </tr>
            <tr>
                <input type="radio"  value="spike" id="spike"  class="input-hidden" name="controller" 
                onClick={() => this.props.onChangeType("spike") } style={ this.style(Spike) }
                />
                <label for="spike">  
                    <img style={ this.style(Spike) } src={Spike} alt="Spike"/>
                </label>    
            </tr>
            <tr>
                 <input type="radio" value="delete" id="empty" class="input-hidden" name="controller" 
                 onClick={() => this.props.onChangeType("empty") } style={ this.style(Delete) }
                />
                <label for="empty">  
                    <img  style={ this.style(Delete) } src={Delete} alt="Delete"/>    
                </label>       
            </tr>
        </table>
    
    );
    }
    style(type1) {
        return {
            backgroundImage: `url(${type1})`,
            backgroundSize: 'calc(10vmin - 2px) calc(10vmin - 2px)',
            height: "10vmin",
            width: "10vmin",
            border: "1px solid black",
            outline:"none",    
        };
    }

    render() {
        return (
            this.typeChooser()
        );
    }

}
export default TypeToolbar;