import React, { Component } from 'react';
import Block from "../Images/block.PNG";
import Center from "../Images/center.PNG";
import SpikeNorth from "../Images/spikeNorth.PNG";
import SpikeEast from "../Images/spikeEast.PNG";
import SpikeSouth from "../Images/spikeSouth.PNG";
import SpikeWest from "../Images/spikeWest.PNG";
import Empty from "../Images/empty.PNG";

class GridCell extends Component {
    imageForType() {
        const direction = this.props.partDirection;
        if(this.props.partHealth > 0){
            switch(this.props.partType){
                case "block":
                    return `url(${Block})`;
                case "center":
                    return `url(${Center})`;
                case "spike":
                    if(direction === "south"){
                        return `url(${SpikeNorth})`;
                    }
                    if(direction === "west"){
                        return `url(${SpikeWest})`;
                    }
                    if(direction === "north"){
                        return `url(${SpikeSouth})`;
                    }
                    return `url(${SpikeEast})`;
                default:
                    return `url(${Empty})`;
            }
        }
        return `url(${Empty})`;
    }

    style() {
        return {
            backgroundImage: this.imageForType(),
            backgroundSize: 'calc(15vmin - 2px) calc(15vmin - 2px)',
            height: "15vmin",
            width: "15vmin",
            border: "1px solid black",
            outline:"none"
        };
    }

    render() {
        return (
            <button  onClick={this.props.onClicked} style={ this.style() }></button>
        );
    }
}

export default GridCell;
