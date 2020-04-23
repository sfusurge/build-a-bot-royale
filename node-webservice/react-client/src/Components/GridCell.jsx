import React, { Component } from 'react';
import Block from "../Images/block.PNG";
import Center from "../Images/center.PNG";
import SpikeNorth from "../Images/spikeNorth.PNG";
import SpikeEast from "../Images/spikeEast.PNG";
import SpikeSouth from "../Images/spikeSouth.PNG";
import SpikeWest from "../Images/spikeWest.PNG";
import ShieldNorth from "../Images/shieldNorth.PNG";
import ShieldEast from "../Images/shieldEast.PNG";
import ShieldSouth from "../Images/shieldSouth.PNG";
import ShieldWest from "../Images/shieldWest.PNG";
import Empty from "../Images/empty.PNG";

class GridCell extends Component {
    imageForType() {
        const direction = this.props.partDirection;
        if (this.props.partHealth > 0) {
            switch (this.props.partType) {
                case "block":
                    return `url(${Block})`;
                case "center":
                    return `url(${Center})`;
                case "spike":
                    if (direction === "south") {
                        return `url(${SpikeNorth})`;
                    }
                    if (direction === "west") {
                        return `url(${SpikeWest})`;
                    }
                    if (direction === "north") {
                        return `url(${SpikeSouth})`;
                    }
                    return `url(${SpikeEast})`;
                case "shield":
                    if (direction === "south") {
                        return `url(${ShieldNorth})`;
                    }
                    if (direction === "west") {
                        return `url(${ShieldWest})`;
                    }
                    if (direction === "north") {
                        return `url(${ShieldSouth})`;
                    }
                    return `url(${ShieldEast})`;
                default:
                    return `url(${Empty})`;
            }
        }
        return `url(${Empty})`;
    }

    style() {
        return {
            backgroundImage: this.imageForType(),
            backgroundSize: (this.props.gameplayPhase === "build") ? 'calc(14vmin - 2px) calc(14vmin - 2px)' : 'calc(12vmin - 2px) calc(12vmin - 2px)',
            height: (this.props.gameplayPhase === "build") ? "14vmin" : "12vmin",
            width: (this.props.gameplayPhase === "build") ? "14vmin" : "12vmin",
            border: "1px solid black",
            outline: "none"
        };
    }

    render() {
        console.log(this.props.gameplayPhase);
        return (
            <button onClick={this.props.onClicked} style={this.style()}></button>
        );
    }
}

export default GridCell;
