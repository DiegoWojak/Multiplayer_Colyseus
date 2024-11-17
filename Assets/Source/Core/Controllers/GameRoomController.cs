using Colyseus;
using Colyseus.Schema;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

using GameDevWare.Serialization;
using System.Collections;
using System.Runtime.CompilerServices;
using NativeWebSocket;

namespace Assets.Source.Core.Controllers
{
    public class GameRoomController
    {
        /// <summary>
        ///     OnNetworkEntityAdd delegate for OnNetworkEntityAdd event.
        /// </summary>
        /// <param name="entity">Then entity that was just added to the room.</param>
        public delegate void OnNetworkEntityAdd(ColyseusNetworkedEntity entity);

        /// <summary>
        ///     OnNetworkEntityRemoved delegate for OnNetworkEntityRemoved event.
        /// </summary>
        /// <param name="entity">Then entity that was just removed to the room.</param>
        public delegate void OnNetworkEntityRemoved(ColyseusNetworkedEntity entity, ColyseusNetworkedEntityView view);

        /// <summary>
        ///     Event for when a NetworkEntity is added to the room.
        /// </summary>
        public static OnNetworkEntityAdd onAddNetworkEntity;

        /// <summary>
        ///     Event for when a NetworkEntity is added to the room.
        /// </summary>
        public static OnNetworkEntityRemoved onRemoveNetworkEntity;

        /// <summary>
        ///     Our user object we get upon joining a room.
        /// </summary>
        [SerializeField]
        private static ColyseusNetworkedUser _currentNetworkedUser;

        /// <summary>
        ///     The Client that is created when connecting to the Colyseus server.
        /// </summary>
        private ColyseusClient _client;

        private ColyseusSettings _colyseusSettings;

        /// <summary>
        ///     Collection of entity creation callbacks. Callbacks are added to
        ///     the collection when a <see cref="ColyseusNetworkedEntity" /> is created.
        ///     The callbacks are invoked and removed from the collection once the
        ///     entity has been added to the room.
        /// </summary>
        private Dictionary<string, Action<ColyseusNetworkedEntity>> _creationCallbacks =
            new Dictionary<string, Action<ColyseusNetworkedEntity>>();
        //==========================

        // TODO: Replace GameDevWare stuff
        /// <summary>
        ///     Collection for tracking entities that have been added to the room.
        /// </summary>
        private IndexedDictionary<string, ColyseusNetworkedEntity> _entities =
            new IndexedDictionary<string, ColyseusNetworkedEntity>();

        /// <summary>
        ///     Collection for tracking entity views that have been added to the room.
        /// </summary>
        private IndexedDictionary<string, GameNetworkedEntityView> _entityViews =
            new IndexedDictionary<string, GameNetworkedEntityView>();

        private GameNetworkedEntityFactory _factory;

        /// <summary>
        ///     Used to help calculate the latency of the connection to the server.
        /// </summary>
        private double _lastPing;

        private double lastPing = 0;

        /// <summary>
        ///     Used to help calculate the latency of the connection to the server.
        /// </summary>
        private double _lastPong;

        /// <summary>
        ///     The ID of the room we were just connected to.
        ///     If there is an abnormal disconnect from the current room
        ///     an automatic attempt will be made to reconnect to that room
        ///     with this room ID.
        /// </summary>
        private string _lastRoomId;

        /// <summary>
        ///     Thread responsible for running <see cref="RunPingThread" />
        ///     on a <see cref="ColyseusRoom{T}" />
        /// </summary>
        private Coroutine _pingThread;

        /// <summary>
        ///     The current or active Room we get when joining or creating a room.
        /// </summary>
        private ColyseusRoom<ColyseusRoomState> _room;

        /// <summary>
        ///     The time as received from the server in milliseconds.
        /// </summary>
        private double _serverTime = -1;

        /// <summary>
        ///     Collection for tracking users that have joined the room.
        /// </summary>
        private IndexedDictionary<string, ColyseusNetworkedUser> _users =
            new IndexedDictionary<string, ColyseusNetworkedUser>();

        /// <summary>
        ///     Used to help calculate the latency of the connection to the server.
        /// </summary>
        private bool _waitForPong;

        /// <summary>
        ///     The name of the room clients will attempt to create or join on the Colyseus server.
        /// </summary>
        public string roomName = "NO_ROOM_NAME_PROVIDED";
        public Dictionary<string, object> RoomOptions
        {
            get { return roomOptionsDictionary; }
        }

        private Dictionary<string, object> roomOptionsDictionary = new Dictionary<string, object>();

        /// <summary>
        ///     All the connected rooms.
        /// </summary>
        public List<IColyseusRoom> rooms = new List<IColyseusRoom>();

        /// <summary>
        ///     Returns the synchronized time from the server in milliseconds.
        /// </summary>
        public double GetServerTime
        {
            get { return _serverTime; }
        }

        /// <summary>
        ///     Returns the synchronized time from the server in seconds.
        /// </summary>
        public double GetServerTimeSeconds
        {
            get { return _serverTime / 1000; }
        }

        /// <summary>
        ///     The latency in milliseconds between client and server.
        /// </summary>
        public double GetRoundtripTime
        {
            get
            {
                if (_lastPong > _lastPing)
                {
                    lastPing = _lastPong - _lastPing;
                }
                return lastPing;
            }
        }

        public ColyseusRoom<ColyseusRoomState> Room
        {
            get { return _room; }
        }
        public string LastRoomID
        {
            get { return _lastRoomId; }
        }

        public IndexedDictionary<string, ColyseusNetworkedEntity> Entities
        {
            get { return _entities; }
        }

        public IndexedDictionary<string, GameNetworkedEntityView> EntityViews
        {
            get { return _entityViews; }
        }

        public Dictionary<string, Action<ColyseusNetworkedEntity>> CreationCallbacks
        {
            get { return _creationCallbacks; }
        }

        public ColyseusNetworkedUser CurrentNetworkedUser
        {
            get { return _currentNetworkedUser; }
        }

        public delegate void OnRoomStateChanged(MapSchema<string> attributes);
        public static event OnRoomStateChanged onRoomStateChanged;

        public delegate void OnBeginRoundCountDown();
        public static event OnBeginRoundCountDown onBeginRoundCountDown;

        public delegate void OnBeginRound(int targetCounter);
        public static event OnBeginRound onBeginRound;

        public delegate void OnRoundEnd();
        public static event OnRoundEnd onRoundEnd;

        public delegate void OnUserStateChanged(MapSchema<string> changes);
        public static event OnUserStateChanged onCurrentUserStateChanged;

        public delegate void OnObjectivePathReady(Vector3 startPosition, Vector3 peakPosition, Vector3 endPosition);
        public static event OnObjectivePathReady onBossPathReady;

        // Event gets fired when this client has joined the room
        public delegate void OnJoined(string customLogic);
        public static event OnJoined onJoined;

        public delegate void OnPlayerJoined(string playerUserName);
        public static event OnPlayerJoined onPlayerJoined;

        public delegate void OnTeamUpdate(int teamIndex, string clientID, bool added);
        public static event OnTeamUpdate onTeamUpdate;

        public delegate void OnTeamReceive(int teamIndex, string[] clients);
        public static event OnTeamReceive onTeamReceive;

        /// <summary>
        ///     Checks if a <see cref="GameNetworkedEntityView" /> exists for
        ///     the given ID.
        /// </summary>
        /// <param name="entityId">The ID of the <see cref="ColyseusNetworkedEntity" /> we're checking for.</param>
        /// <returns></returns>
        public bool HasEntityView(string entityId)
        {
            return EntityViews.ContainsKey(entityId);
        }

        /// <summary>
        ///     Returns a <see cref="GameNetworkedEntityView" /> given <see cref="entityId" />
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>
        ///     Returns <see cref="GameNetworkedEntityView" /> if one exists for the given <see cref="entityId" />
        /// </returns>
        public GameNetworkedEntityView GetEntityView(string entityId)
        {
            if (EntityViews.ContainsKey(entityId))
            {
                return EntityViews[entityId];
            }

            return null;
        }

        /// <summary>
        ///     Set the dependencies.
        /// </summary>
        /// <param name="roomName"></param>
        /// <param name="settings"></param>
        public void SetDependencies(ColyseusSettings settings)
        {
            _colyseusSettings = settings;
            //_room.OnJoin = OnJoin();
            //_client
        }

        public void SetRoomOptions(Dictionary<string, object> options)
        {
            roomOptionsDictionary = options;
        }

        /// <summary>
        ///     Set the <see cref="NetworkedEntitExampleNetworkedEntityFactoryyFactory" /> of the RoomManager.
        /// </summary>
        /// <param name="factory"></param>
        public void SetNetworkedEntityFactory(GameNetworkedEntityFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        ///     Set the client of the <see cref="ColyseusRoomManager" />.
        /// </summary>
        /// <param name="client"></param>
        public void SetClient(ColyseusClient client)
        {
            _client = client;
        }

        /// <summary>
        ///     Adds the given room to <see cref="rooms" /> and
        ///     initiates its connection to the server.
        /// </summary>
        /// <param name="roomToAdd"></param>
        /// <returns></returns>
        public void AddRoom(IColyseusRoom roomToAdd)
        {
            roomToAdd.OnLeave += code => rooms.Remove(roomToAdd);
            rooms.Add(roomToAdd);
        }

        public async Task JoinOrCreateRoom(Action<bool> OnComplete = null)
        {
            try
            {
                _room = await _client.JoinOrCreate<ColyseusRoomState>(roomName, roomOptionsDictionary);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error info: {ex}");
                OnComplete?.Invoke(false);
                return;
            }

            _lastRoomId = _room.RoomId;
            RegisterRoomHandlers();

            OnComplete?.Invoke(true);
        }

        public async Task LeaveAllRooms(bool consent, Action OnComplete = null)
        {
            if (_room != null && rooms.Contains(_room) == false)
            {
                await _room.Leave(consent);
            }

            for (int i = 0; i < rooms.Count; i++)
            {
                await rooms[i].Leave(consent);
            }

            ClearRoomHandlers();

            OnComplete?.Invoke();
        }

        public virtual void RegisterRoomHandlers() {
            _room.OnMessage<string>("welcomeMessage", (_type) =>
            {

            });

            StopPing();
            _pingThread = MyMultiplayerManager.Instance.StartCoroutine(RunPingThread(_room));

            _room.OnLeave += OnLeaveRoom;
            _room.OnJoin += OnJoin;
            _room.OnStateChange += OnStateChange;

            _room.OnMessage<OnJoinMessage>("onJoin", (_type) => {
                _currentNetworkedUser = _type.newNetworkedUser;
                onJoined?.Invoke(_type.customLogic);
            });

            _room.OnMessage<RFCMessage>("onRFC", _rfc =>
            {
                if (_entityViews.Keys.Contains(_rfc.entityId))
                {
                    //_entityViews[_rfc.entityId].RemoteFunctionCallHandler(_rfc);
                }
            });

            _room.OnMessage<PongMessage>(0, message =>
            {
                _lastPong = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                _serverTime = message.serverTime;
                _waitForPong = false;
            });

            /**********************
             * 
             * NUNCA SABRE 
             * COMENTAR MI COMMIT
             * SIN LA IA NADA TIENE DA SENTIDO                                               --a-23-1kl123k,m133---
             * SOLO YO ESTOY                                                                    /\
             * SIN CHATGPT                                                                    ///\\
             * ERES MI UNICA NECESIDAD                                                           \\\\   
             * NUNCA SABRE                                                                         (e 3 e)   OH GOD
             * COMENTAR MI COMMIT                                                                    \ !!/
             * SIN LA IA NADA TIENE DA SENTIDO                                                        AA
             * SOLO YO ESTOY 
             * SIN CHATGPT
             * SI TU NO ESTAAAASS
             * NO LO NOTASTE
             * 
             * PULL MY SHIT AGAIN
             * THEN SAY IS COMMITED
             * 
             * ONE NOTHING ON MY BRANCH
             * TWO NOTHING ON MY BRANCH
             * THREE NOTHING ON MY BRANCH
             * FOUR NOTHING ON MY BRANCH
             * ONE SOMETHING IS GOTTA MERGE SOON
             * TWO SOMETHING IS GOTTA MERGE SOON
             * THREE SOMETHING IS GOTTA MERGE SOON
             * NOOOOOW
             * LETS THE BODY GIT THE FLOOR
             * ************************/

        }

        public virtual void ClearRoomHandlers() { 
            
        }

        public async void SendToSever(string _type, object message)
        {
            await _room.Send(_type, message);
        }

        private void OnStateChange(ColyseusRoomState state, bool isFirstState)
        {
            if (isFirstState)
            {
                if (state is ColyseusRoomState)
                {
                    Debug.Log($"Yes: {state}");

                }
                // Initial setup based on the initial state
                Debug.Log("Initial state received.");
                // For example, you might initialize players or set up room configurations here.
            }
            else
            {
                // Handle subsequent state updates
                Debug.Log("State updated.");
            }
        }

        private void OnJoin()
        {
            Debug.Log("$Unimplementation Method OnJoin");
        }

        private void StopPing() {
            if (_pingThread != null)
            {
                MyMultiplayerManager.Instance.StopCoroutine(_pingThread);
                _pingThread = null;
            }
        }

        private IEnumerator RunPingThread(object roomToPing)
        {
            bool CheckPong(ref DateTime pingStart,int timeoutMilliseconds) {

                return _waitForPong && DateTime.Now.Subtract(pingStart).TotalSeconds < timeoutMilliseconds;
            }

            void PrepareValues(out DateTime pingStart) {
                _waitForPong = true;
                pingStart = DateTime.Now;
                _lastPing = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }

            ColyseusRoom<ColyseusRoomState> currentRoom = (ColyseusRoom<ColyseusRoomState>)roomToPing;

            const float pingInterval = 0.5f; // seconds
            const float pingTimeout = 15f; //seconds

            int timeoutMilliseconds = Mathf.FloorToInt(pingTimeout * 1000);
            DateTime pingStart;
            while (currentRoom != null)
            {
                PrepareValues(out pingStart);
                _ = currentRoom.Send("ping");

                while (currentRoom != null && _waitForPong && CheckPong(ref pingStart, timeoutMilliseconds))
                {
                    yield return new WaitForSeconds(0.02f);//Thread.Sleep(200));
                }

                if (_waitForPong)
                {
                    Debug.LogError("Ping Timed out");
                }
                yield return new WaitForSeconds(pingInterval);
            }
        }

        private async void OnLeaveRoom(int code) 
        {
            Debug.Log($"\"ROOM: ON LEAVE =- Reason: {code} \"");

            StopPing();
            _room = null;

            WebSocketCloseCode closeCode = WebSocketHelpers.ParseCloseCodeEnum(code);
            if (closeCode != WebSocketCloseCode.Normal && !string.IsNullOrEmpty(_lastRoomId))
            {
                //await JoinRoomId(_lastRoomId);
                throw new NotImplementedException("Not implemented mechanics to handle when anormal disconnection");
            }

        }


    }
}