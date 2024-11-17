using UnityEngine;

using Colyseus;

using Assets.Source.Core.Components.Lobby;
using Assets.Source.Core.Controllers;

using System.Collections.Generic;

using Assets.Source.Core;
using TMPro;
using UnityEngine.UI;
using UnityEditor;
public class MyMultiplayerManager : ColyseusManager<MyMultiplayerManager>
{
    public delegate void OnRoomsReceived(GameRoomsAvailables[] rooms);
    public static OnRoomsReceived onRoomsReceived;
    public GameNetworkedEntityFactory _networkedEntityFactory;
    /// <summary>
    ///     Returns a reference to the current networked user.
    /// </summary>
    public ColyseusNetworkedEntity CurrentNetworkedEntity;


    [SerializeField]
    protected GameRoomController _roomController;
    private ColyseusRoom<ColyseusRoomState> _room;

    [SerializeField]
    private string p_MessageToTheServer;

    private bool _isInitialized = false;

    public TextMeshProUGUI Output;
    public Button btnConnect;
    public Button btnDisconnect;

    private bool PlayerStatus 
    { 
        get {
            if (_roomController == null || _roomController.CurrentNetworkedUser == null) {
                return false;
            }
            return _roomController.CurrentNetworkedUser.connected;
        } 
    }

    protected override void Awake()
    {

        base.Awake();

        var _options = new Dictionary<string, object>();
        _options.Add("roomId", 123);
        _options.Add("logic", "starBossCoop");

        Initialize(_options, "Main_Room");
        InitializeClient();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void Initialize(Dictionary<string, object> _options, string _roomName) 
    {
        if (_isInitialized)
        {
            return;
        }

        _roomController = new GameRoomController() { roomName = _roomName };
        _roomController.SetRoomOptions(_options);
        _roomController.SetDependencies(_colyseusSettings);

        _networkedEntityFactory = new GameNetworkedEntityFactory(_roomController.CreationCallbacks,
            _roomController.Entities, _roomController.EntityViews);
    }
    private void PrintCurrentUser(string _instruction) {
        Output.text += $"Connected: {_roomController.CurrentNetworkedUser.connected} \nSessionID: {_roomController.CurrentNetworkedUser.sessionId}";
        UpdateBtnStatus("connect");
    }

    public override void InitializeClient()
    {
        base.InitializeClient();
        
        if (_isInitialized) {
            return;
        }

        _roomController.SetClient(client);

        UpdateBtnStatus("connect");
    }

    private void UpdateBtnStatus(string _nmebtn) {
        switch(_nmebtn)
        {
            case "connect":
                btnConnect.interactable = !PlayerStatus;
                btnDisconnect.interactable = PlayerStatus;
                break;
            
        }
    }

    public async void JoinOrCreateRoom() {
        await _roomController.JoinOrCreateRoom((bool res) =>{ 
            if(res)
            {
                GameRoomController.onJoined += PrintCurrentUser;
            }
        });
    }

    public async void LeaveAllRooms()
    {
        await _roomController.LeaveAllRooms(true);
    }

    [ContextMenu("Send Message to the server")]
    void SendMessageToTheServer() {
        _roomController.SendToSever("perrosHDP", new { lol = "EL DIABLO CO" });
    }

}
