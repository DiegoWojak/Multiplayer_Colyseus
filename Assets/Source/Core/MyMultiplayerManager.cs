using UnityEngine;
using Colyseus;

using Assets.Source.Core.Components.Lobby;
using Assets.Source.Core.Controllers;
public class MyMultiplayerManager : ColyseusManager<MyMultiplayerManager>
{
    public delegate void OnRoomsReceived(GameRoomsAvailables[] rooms);
    public static OnRoomsReceived onRoomsReceived;

    [SerializeField]
    protected GameRoomController _roomController;

    /// <summary>
    ///     Returns a reference to the current networked user.
    /// </summary>
    public ColyseusNetworkedEntity CurrentNetworkedEntity;




    [SerializeField]
    string p_MessageToTheServer;

    ColyseusRoom<dynamic> _room;
    protected override void Awake()
    {
        base.Awake();
        InitializeClient();
    }

    public override void InitializeClient()
    {
        base.InitializeClient();
        Debug.Log($"Server: {client}");

        JoinOCreateRoom();
    }

    public async void JoinOCreateRoom() {
        _room = await client.JoinOrCreate("my_room");

        _room.OnMessage<string>("welcomeMessage", (_type) =>
        {
            Debug.Log("Received message from server: " + _type);
        });

        _room.OnStateChange += _room_OnStateChange;
    }

    private void _room_OnStateChange(dynamic state, bool isFirstState)
    {
        throw new System.NotImplementedException();
    }

    [ContextMenu("Send Message to the server")]
    void SendMessageToTheServer() {
        SendToSever();
    }

    async void SendToSever() {
        await _room.Send(p_MessageToTheServer, new { x=1.3, y = -1.4 });
    }
}

