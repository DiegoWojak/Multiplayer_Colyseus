

using Assets.Source.Core.Components.Views;
using Colyseus;
using System.Collections.Generic;
using System;
using UnityEngine;
using GameDevWare.Serialization;
using Assets.Source.Core.Views;
using Colyseus.Schema;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Collections;

namespace Assets.Source.Core.Controllers
{
    public class GameRoomController
    {
        #region NetworkEvents
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

        /// <summary>
        ///     Create a room with the given roomId.
        /// </summary>
        /// <param name="roomId">The ID for the room.</param>
        public async Task CreateSpecificRoom(ColyseusClient client, string roomName, string roomId,
            Action<bool> onComplete = null)
        {
            Debug.Log($"Creating Room {roomId}");

            try
            {
                //Populate an options dictionary with custom options provided elsewhere as well as the critical option we need here, roomId
                Dictionary<string, object> options = new Dictionary<string, object> { ["roomId"] = roomId };
                foreach (KeyValuePair<string, object> option in roomOptionsDictionary)
                {
                    options.Add(option.Key, option.Value);
                }

                _room = await client.Create<ColyseusRoomState>(roomName, options);
            }
            catch (Exception ex)
            {
                Debug.Log($"Failed to create room {roomId} : {ex.Message}");
                onComplete?.Invoke(false);
                return;
            }

            onComplete?.Invoke(true);
            Debug.Log($"Created Room: {roomId}");
            _lastRoomId = roomId;
            RegisterRoomHandlers();
        }

        /// <summary>
        ///     Join an existing room or create a new one using <see cref="roomName" /> with no options.
        ///     <para>Locked or private rooms are ignored.</para>
        /// </summary>
        public async Task JoinOrCreateRoom(Action<bool> onComplete = null)
        {
            Debug.Log($"Join Or Create Room - Name = {roomName}.... ");
            try
            {
                // Populate an options dictionary with custom options provided elsewhere
                Dictionary<string, object> options = new Dictionary<string, object>();
                foreach (KeyValuePair<string, object> option in roomOptionsDictionary)
                {
                    options.Add(option.Key, option.Value);
                }

                _room = await _client.JoinOrCreate<ColyseusRoomState>(roomName, options);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Room Controller Error - {ex.Message + ex.StackTrace}");
                onComplete?.Invoke(false);
                return;
            }

            onComplete?.Invoke(true);
            Debug.Log($"Joined / Created Room: {_room.Id}");
            _lastRoomId = _room.Id;
            RegisterRoomHandlers();
        }

        public async Task LeaveAllRooms(bool consented, Action onLeave = null)
        {
            if (_room != null && rooms.Contains(_room) == false)
            {
                await _room.Leave(consented);
            }

            for (int i = 0; i < rooms.Count; i++)
            {
                await rooms[i].Leave(consented);
            }

            ClearRoomHandlers();
            onLeave?.Invoke();
        }

        /// <summary>
        ///     Asynchronously gets all the available rooms of the <see cref="_client" />
        ///     named <see cref="roomName" />
        /// </summary>
        public async Task<ColyseusRoomAvailable[]> GetRoomListAsync()
        {
            ColyseusRoomAvailable[] allRooms = await _client.GetAvailableRooms(roomName);

            return allRooms;
        }

        public async Task JoinRoomId(string roomId, Action<bool> onJoin = null)
        {
            Debug.Log($"Joining Room ID {roomId}....");
            ClearRoomHandlers();

            try
            {
                while (_room == null || !_room.colyseusConnection.IsOpen)
                {
                    _room = await _client.JoinById<ColyseusRoomState>(roomId, null);

                    if (_room == null || !_room.colyseusConnection.IsOpen)
                    {
                        Debug.LogWarning($"Failed to Connect to {roomId}.. Retrying in 5 Seconds...");
                        await Task.Delay(5000);
                    }
                }

                _lastRoomId = roomId;
                RegisterRoomHandlers();
                onJoin?.Invoke(true);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                onJoin?.Invoke(false);
                //Debug.LogError("Failed to joining room, try another...");
                //await CreateSpecificRoom(_client, roomName, roomId, onJoin);
                //
            }
        }



        public virtual void RegisterRoomHandlers() 
        {
            //StopPing
            //Start Routing Ping
            //_Room addOnLeave
            //_Room AddOnStateChange
            //_Room OnMessage<OnJoinMessage>
            //_Room OnMessage<RFCMessage>
            //_Room OnMessage<PongMessage>
            //_Room EmptyMessage
            //_Room OnBeginNewTargetMessage
            //_Room OnJoinPlayer

            //========================
            /*_room.State.networkedEntities.OnAdd += OnEntityAdd;
            _room.State.networkedEntities.OnRemove += OnEntityRemoved;

            _room.State.networkedUsers.OnAdd += OnUserAdd;
            _room.State.networkedUsers.OnRemove += OnUserRemove;

            _room.State.TriggerAll();*/
            //========================

            /*_room.colyseusConnection.OnError += Room_OnError;
            _room.colyseusConnection.OnClose += Room_OnClose;*/
        }

        /// <summary>
        ///     Unsubscribes <see cref="Room" /> from networked events."/>
        /// </summary>
        private void ClearRoomHandlers()
        {
            //StopPing();

            if (_room == null)
            {
                return;
            }

            /*_room.State.networkedEntities.OnAdd -= OnEntityAdd;
            _room.State.networkedEntities.OnRemove -= OnEntityRemoved;
            _room.State.networkedUsers.OnAdd -= OnUserAdd;
            _room.State.networkedUsers.OnRemove -= OnUserRemove;

            _room.colyseusConnection.OnError -= Room_OnError;
            _room.colyseusConnection.OnClose -= Room_OnClose;

            _room.OnStateChange -= OnStateChangeHandler;

            _room.OnLeave -= OnLeaveRoom;*/

            _room = null;
            _currentNetworkedUser = null;
        }

        /// <summary>
        ///     The callback for the event when a <see cref="ColyseusNetworkedEntity" /> is added to a room.
        /// </summary>
        /// <param name="entity">The entity that was just added.</param>
        /// <param name="key">The entity's key</param>
        private async void OnEntityAdd(string key, ColyseusNetworkedEntity entity)
        {
            onAddNetworkEntity?.Invoke(entity);
        }

        /// <summary>
        ///     The callback for the event when a <see cref="ColyseusNetworkedEntity" /> is removed from a room.
        /// </summary>
        /// <param name="entity">The entity that was just removed.</param>
        /// <param name="key">The entity's key</param>
        private void OnEntityRemoved(string key, ColyseusNetworkedEntity entity)
        {
            if (_entities.ContainsKey(entity.id))
            {
                _entities.Remove(entity.id);
            }

            ColyseusNetworkedEntityView view = null;

            if (_entityViews.ContainsKey(entity.id))
            {
                view = _entityViews[entity.id];
                _entityViews.Remove(entity.id);
            }

            onRemoveNetworkEntity?.Invoke(entity, view);
        }

        /// <summary>
        ///     Callback for when a <see cref="ColyseusNetworkedUser" /> is added to a room.
        /// </summary>
        /// <param name="user">The user object</param>
        /// <param name="key">The user key</param>
        private void OnUserAdd(string key, ColyseusNetworkedUser user)
        {
            // Add "player" to map of players
            _users.Add(key, user);

            // On entity update...
            /*user.OnChange += changes =>
            {
                
            };*/
        }

        // <summary>
        ///     Callback for when a user is removed from a room.
        /// </summary>
        /// <param name="user">The removed user.</param>
        /// <param name="key">The user key.</param>
        private void OnUserRemove(string key, ColyseusNetworkedUser user)
        {

            _users.Remove(key);
        }

        /// <summary>
        ///     Callback for when the room's connection closes.
        /// </summary>
        /// <param name="closeCode">Code reason for the connection close.</param>
        private static void Room_OnClose(int closeCode)
        {
            
        }

        /// <summary>
        ///     Callback for when the room get an error.
        /// </summary>
        /// <param name="errorMsg">The error message.</param>
        private static void Room_OnError(string errorMsg)
        {
            
        }

        private static void OnStateChangeHandler(ColyseusRoomState state, bool isFirstState)
        {
            //LSLog.LogImportant("State has been updated!");
            onRoomStateChanged?.Invoke(state.attributes);
        }

        /// <summary>
        ///     Sends "ping" message to current room to help measure latency to the server.
        /// </summary>
        /// <param name="roomToPing">The <see cref="ColyseusRoom{T}" /> to ping.</param>
        private IEnumerator RunPingThread(object roomToPing)
        {
            ColyseusRoom<ColyseusRoomState> currentRoom = (ColyseusRoom<ColyseusRoomState>)roomToPing;

            const float pingInterval = 0.5f; // seconds
            const float pingTimeout = 15f; //seconds

            int timeoutMilliseconds = Mathf.FloorToInt(pingTimeout * 1000);
            DateTime pingStart;
            while (currentRoom != null)
            {
                _waitForPong = true;
                pingStart = DateTime.Now;
                _lastPing = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                _ = currentRoom.Send("ping");
                while (currentRoom != null && _waitForPong &&
                       DateTime.Now.Subtract(pingStart).TotalSeconds < timeoutMilliseconds)
                {
                    yield return new WaitForSeconds(0.02f);//Thread.Sleep(200));
                }

                if (_waitForPong)
                {
                    Debug.Log("Ping Timed out");
                }
                yield return new WaitForSeconds(pingInterval);
            }
        }

        /// <summary>
        ///     Increments the known <see cref="_serverTime" /> by <see cref="Time.fixedDeltaTime" />
        ///     converted into milliseconds.
        /// </summary>
        public void IncrementServerTime()
        {
            _serverTime += Time.fixedDeltaTime * 1000;
        }

        private void StopPing()
        {
            if (_pingThread != null)
            {
                MyMultiplayerManager.Instance.StopCoroutine(_pingThread);
                _pingThread = null;
            }
        }

        public async void CleanUp()
        {
            StopPing();

            List<Task> leaveRoomTasks = new List<Task>();

            foreach (IColyseusRoom roomEl in rooms)
            {
                leaveRoomTasks.Add(roomEl.Leave(false));
            }

            if (_room != null)
            {
                leaveRoomTasks.Add(_room.Leave(false));
            }

            await Task.WhenAll(leaveRoomTasks.ToArray());

            ClearCollectionsAndUser();
        }

        public void ClearCollectionsAndUser()
        {
            if (_entities != null)
                _entities.Clear();

            if (_entityViews != null)
                _entityViews.Clear();

            if (_users != null)
                _users.Clear();

            if (_creationCallbacks != null)
                _creationCallbacks.Clear();

            if (roomOptionsDictionary != null)
                roomOptionsDictionary.Clear();

            _currentNetworkedUser = null;
        }
        #endregion
    }
}
