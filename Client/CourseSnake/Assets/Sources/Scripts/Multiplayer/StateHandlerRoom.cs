using Colyseus;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StateHandlerRoom : ColyseusManager<StateHandlerRoom>
{
    private const string GameName = "state_handler";

    private ColyseusRoom<State> _room;
    private LobbyRoomHandler _lobbyRoomHandler;
    public ColyseusRoom<State> Room => _room;

    protected override void Awake()
    {
        base.Awake();

        InitializeClient();
        DontDestroyOnLoad(this);
        _lobbyRoomHandler = LobbyRoomHandler.Instance;
    }

    public void SendPlayerData(string key, object data = null)
    {
        _room.Send(key, data);
    }

    public async Task<bool> JoinOrCreateAny(string mapName = "", string password = "")
    {
        Dictionary<string, object> roomMetaData = new()
        {
            { "RoomName", mapName },
            { "Password", password },
            { "Version", GameConfig.Version },

        };

        _room = await client.JoinOrCreate<State>(GameName, roomMetaData);
        return _room != null;
    }

    public async Task<bool> JoinOrCreateByVersion(string mapName = "", string password = "")
    {
        var rooms = _lobbyRoomHandler.Rooms;

        foreach (var room in rooms)
        {
            string roomVersion = (string)room.Value["Version"];

            if (GameConfig.Version != roomVersion)
            {
                continue;
            }

            _room = await client.JoinById<State>((string)room.Value["roomId"]);
            return true;
        }

        await CreateRoom(mapName, password);
        return true;
    }

    public async Task<bool> JoinRoomById(string id)
    {
        var rooms = await client.GetAvailableRooms();

        foreach (ColyseusRoomAvailable room in rooms)
        {
            if(room.roomId == id)
            {
                if (room.clients >= room.maxClients)
                {
                    return false;
                }

                string roomVersion = _lobbyRoomHandler.GetRoomVersionById(id);

                if (GameConfig.Version != roomVersion)
                    return false;

                _room = await client.JoinById<State>(id);

                return true;
            }
        }

        return false;
    }

    public async Task<bool> CreateRoom(string mapName = "", string password = "")
    {
        Dictionary<string, object> roomMetaData = new()
        {
            { "RoomName", mapName },
            { "Password", password },
            { "Version", GameConfig.Version },
        };

        _room = await client.Create<State>(GameName, roomMetaData);
        return true;
    }

    public async Task<ColyseusRoomAvailable[]> GetRooms()
    {
        return await client.GetAvailableRooms();
    }

    public void Leave()
    {
        _room.Leave();
    }
}
