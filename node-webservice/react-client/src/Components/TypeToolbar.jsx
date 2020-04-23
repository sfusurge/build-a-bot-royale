import React, { Component } from 'react';
import Block from "../Images/block.PNG";
import Delete from "../Images/delete.PNG";
import Spike from "../Images/spikeNorth.PNG";
import Shield from "../Images/shieldNorth.PNG";
class TypeToolbar extends Component {

    typeChooser() {
        return (
            <div style={{ "backgroundColor": "rgb(0,0,0)", margin: "2vmin", height: "70vmin", "marginLeft": "0" }}>
                <table style={{ margin: "1vmin" }}>
                    <tbody>
                        <tr>
                            <td>
                                <input type="radio" value="block" id="block" className="input-hidden" name="controller"
                                    defaultChecked="checked"
                                    onClick={() => this.props.onChangeType("block")} style={this.style(Block)}
                                />
                                <label htmlFor="block">
                                    <img style={this.style(Block)} src={Block} alt="Block" />
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="radio" value="spike" id="spike" className="input-hidden" name="controller"
                                    onClick={() => this.props.onChangeType("spike")} style={this.style(Spike)}
                                />
                                <label htmlFor="spike">
                                    <img style={this.style(Spike)} src={Spike} alt="Spike" />
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="radio" value="shield" id="shield" className="input-hidden" name="controller"
                                    onClick={() => this.props.onChangeType("shield")} style={this.style(Spike)}
                                />
                                <label htmlFor="shield">
                                    <img style={this.style(Shield)} src={Shield} alt="Shield" />
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="radio" value="delete" id="empty" className="input-hidden" name="controller"
                                    onClick={() => this.props.onChangeType("empty")} style={this.style(Delete)}
                                />
                                <label htmlFor="empty">
                                    <img style={this.style(Delete)} src={Delete} alt="Delete" />
                                </label>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>

        );
    }
    style(type1) {
        return {
            backgroundImage: `url(${type1})`,
            backgroundSize: 'calc(12vmin-2px) calc(12vmin-2px)',
            height: "12vmin",
            width: "12vmin",
            margin: "calc((22vmin - 40px)/5) 0 0 0",
            border: "2px solid white",
            outline: "none",
        };
    }

    render() {
        return (
            this.typeChooser()
        );
    }

}
export default TypeToolbar;