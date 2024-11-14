using UnityEngine;
using Colyseus;

using Assets.Source.Core.Components.Lobby;
using Assets.Source.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

    ColyseusRoom<ColyseusRoomState> _room;


    protected override void Awake()
    {
        base.Awake();
        InitializeClient();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public override void InitializeClient()
    {
        base.InitializeClient();
        Debug.Log($"Server: {client}");

        JoinOCreateRoom();
    }

    ColyseusNetworkedUser _current;

    public async void JoinOCreateRoom() {
        Dictionary<string, object> options = new Dictionary<string, object>();
        options.Add("roomId", 123);
        options.Add("logic", "starBossCoop");
        //options.Add(option.Key, option.Value);


        _room = await client.JoinOrCreate<ColyseusRoomState>("Main_Room", options);

        _room.OnMessage<string>("welcomeMessage", (_type) =>
        {
            Debug.Log("Received message from server: " + _type);
        });

        _room.OnMessage<OnJoinMessage>("onJoin", (_type) => {
            _current = _type.newNetworkedUser;
            Debug.Log("Received On Join message from server: "+ _type.newNetworkedUser.sessionId + " - " + _type.newNetworkedUser.connected);
        });



        _room.OnJoin += _room_OnJoin;
        _room.OnStateChange += _room_OnStateChange;
    }


    private void _room_OnStateChange(ColyseusRoomState state, bool isFirstState)
    {
        if (isFirstState)
        {
            if (state is ColyseusRoomState) { 
                Debug.Log($"Yes: {state}");
                
            }
            // Initial setup based on the initial state
            Debug.Log("Initial state received.");
            // For example, you might initialize players or set up room configurations here.
        }
        else
        {
            // Handle subsequent state updates
            Debug.Log ("State updated.");
        }
    }

    private void _room_OnJoin() {
        Debug.Log("Joined .");
    }

    [ContextMenu("Send Message to the server")]
    void SendMessageToTheServer() {
        SendToSever("perrosHDP", new { client = _current.sessionId , lol = "EL DIABLO CO" });
    }

    async void SendToSever(string _type, object message) {
        await _room.Send(_type, message);
        //await _room.Send(_type, new { x=1.3, y = -1.4 });
    }
}

[Serializable]
public class OnJoinMessage {
    public ColyseusNetworkedUser newNetworkedUser;
    public string customLogic;
}
