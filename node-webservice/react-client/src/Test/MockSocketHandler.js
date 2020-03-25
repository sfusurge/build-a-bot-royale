class MockSocket {
    constructor() {
        this.clientEvents = {};
        this.clientEmits = [];
    }

    // Normal Socket client API
    on(eventName, func) {
        if (!this.clientEvents[eventName]) {
            this.clientEvents[eventName] = [];
        }
        this.clientEvents[eventName].push(func);
    }

    emit(eventName, eventData, ack) {
        this.clientEmits.push({ messageName: eventName, messageData: eventData, ack: ack });
    }

    // Extra methods for tests to call
    serverEmit(eventName, eventData) {
        if (this.clientEvents[eventName]) {
            this.clientEvents[eventName].forEach(eventFunction => {
                eventFunction(eventData);
            });
        }
    }

    get EmitLog() {
        return this.clientEmits;
    }
}

export default MockSocket;
