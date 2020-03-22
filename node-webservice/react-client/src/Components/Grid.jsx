import React, { Component } from 'react';
import GridCell from "./GridCell";
import Block from "../Images/block.PNG";
import Delete from "../Images/delete.PNG";

import Spike from "../Images/spikeNorth.PNG";
class Grid extends Component {
     currentType = "block";
    partsArrayTo2DArray(partsData) {
        // build empty 5x5 array
        var partsArray = [];
        for (var rowIndex = 0; rowIndex < 5; rowIndex++) {
            var row = [];
            for (var colIndex = 0; colIndex < 5; colIndex++) {
                row.push({ x: colIndex, y: rowIndex });
            }
            partsArray.push(row);
        }

        // put defined parts into the array
        partsData.forEach(element => {
            partsArray[element.y][element.x] = element;
        });

        return partsArray;
    }
    
    typeChooser(){
        return <table>
            <tr>
                <input  type="radio"  value="block"  id="block" class="input-hidden"  name="controller"
                defaultChecked = "checked"  
                onClick={() => this.props.onChangeType("block") } style={ this.style(Block) }
                />
                <label for="block">  
                    <img style={ this.style(Block) } src={Block} />
                 </label>           
            </tr>
            <tr>
                <input type="radio"  value="spike" id="spike"  class="input-hidden" name="controller" 
                onClick={() => this.props.onChangeType("spike") } style={ this.style(Spike) }
                />
                <label for="spike">  
                    <img style={ this.style(Spike) } src={Spike} />
                </label>    
            </tr>
            <tr>
                 <input type="radio" value="delete" id="empty" class="input-hidden" name="controller" 
                 onClick={() => this.props.onChangeType("empty") } style={ this.style(Delete) }
                />
                <label for="empty">  
                    <img  style={ this.style(Delete) } src={Delete} />    
                </label>
                
            </tr>
        </table>


    }

    renderRow(data, index) {
        return <div key={index} style={{ display: "flex", flexDirection: "row" }}>
            { data.map(this.renderCell.bind(this)) }
        </div>
    }

    renderCell(data, index) {
        return <GridCell 
            key={ index }
            partType={ data.type }
            partDirection = {data.direction}
            partHealth = {data.health}
            onClicked={() => this.props.onCellClick(data.x, data.y, this.currentType)}
        />
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
        const partsArray = this.partsArrayTo2DArray(this.props.parts);
        const gridStyle = {
            height: "75vmin",
            width: "75vmin",
        }
        return (
            <div class="flex-container">
                <div className="Grid-Container" style = {gridStyle}>
                    { partsArray.map(this.renderRow.bind(this)) }
                 </div>
           
                 <div>
                    {this.typeChooser()}
                 </div>
           </div>
        );
    }
}

export default Grid;
