import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

export class MyVector3 extends Schema
{
    @type("number")
    x = 0;

    @type("number")
    y = 0;

    @type("number")
    z = 0;

    constructor (x: number, y: number, z: number)
    {
        super();

        this.x = x;
        this.y = y;
        this.z = z;
    }

    SetValues(newValues: MyVector3)
    {
        this.x = newValues.x;
        this.y = newValues.y;
        this.z = newValues.z;
    }
}

export class Player extends Schema {
    @type(MyVector3)
    Position = new MyVector3(0, 0, 0);
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    something = "This attribute won't be sent to the client-side";

    CreatePlayer(sessionId: string) {
        this.players.set(sessionId, new Player());
    }

    RemovePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    MovePlayer (sessionId: string, movement: any) 
    {

    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 4;

    onCreate (options) {
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        this.onMessage("move", (client, data) => {
            console.log("StateHandlerRoom received message from", client.sessionId, ":", data);
            this.state.MovePlayer(client.sessionId, data);
        });
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client) {
        client.send("hello", "world");
        this.state.CreatePlayer(client.sessionId);
    }

    onLeave (client) {
        this.state.RemovePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }

}
