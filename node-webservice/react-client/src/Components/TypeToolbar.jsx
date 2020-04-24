import React, { Component } from 'react';
import Block from "../Images/block.PNG";
import Delete from "../Images/delete.PNG";
import Spike from "../Images/spikeNorth.PNG";
import Shield from "../Images/shieldNorth.PNG";
import "../App.css"
class TypeToolbar extends Component {

    typeChooser() {
        return (
            <div className="black-rectangle">
                <section className="table">
                    <div className="table-outer-div">
                        <div className="table-inner-div">
                            <div style={{ display: "table-cell" }}>
                                <input type="radio" value="block" id="block" className="input-hidden" name="controller"
                                    defaultChecked="checked"
                                    onClick={() => this.props.onChangeType("block")} style={this.style(Block)}
                                />
                                <label htmlFor="block">
                                    <img className ="toolbar-button" style={this.style(Block)} src={Block} alt="Block" />
                                </label>
                            </div>
                        </div>
                        <div className="table-inner-div">
                            <div style={{ display: "table-cell" }}>
                                <input type="radio" value="spike" id="spike" className="input-hidden" name="controller"
                                    onClick={() => this.props.onChangeType("spike")} style={this.style(Spike)}
                                />
                                <label htmlFor="spike">
                                    <img className ="toolbar-button" style={this.style(Spike)} src={Spike} alt="Spike" />
                                </label>
                            </div>
                        </div>
                        <div className="table-inner-div">
                            <div style={{ display: "table-cell" }}>
                                <input type="radio" value="shield" id="shield" className="input-hidden" name="controller"
                                    onClick={() => this.props.onChangeType("shield")} style={this.style(Spike)}
                                />
                                <label htmlFor="shield">
                                    <img className ="toolbar-button" style={this.style(Shield)} src={Shield} alt="Shield" />
                                </label>
                            </div>
                        </div>
                        <div className="table-inner-div">
                            <div style={{ display: "table-cell" }}>
                                <input type="radio" value="delete" id="empty" className="input-hidden" name="controller"
                                    onClick={() => this.props.onChangeType("empty")} style={this.style(Delete)}
                                />
                                <label htmlFor="empty">
                                    <img className ="toolbar-button" style={this.style(Delete)} src={Delete} alt="Delete" />
                                </label>
                            </div>
                        </div>
                    </div>
                </section>
            </div>

        );
    }
    style(type1) {
        return {
            backgroundImage: `url(${type1})`,
        };
    }

    render() {
        return (
            this.typeChooser()
        );
    }

}
export default TypeToolbar;