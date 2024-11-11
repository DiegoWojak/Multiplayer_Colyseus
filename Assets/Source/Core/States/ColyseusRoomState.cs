// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 2.0.36
// 

using Colyseus.Schema;
using Action = System.Action;
#if UNITY_5_3_OR_NEWER
using UnityEngine.Scripting;
#endif


	public partial class ColyseusRoomState : Schema {
#if UNITY_5_3_OR_NEWER
[Preserve] 
#endif
public ColyseusRoomState() { }
		[Type(0, "map", typeof(MapSchema<ColyseusNetworkedEntity>))]
		public MapSchema<ColyseusNetworkedEntity> networkedEntities = new MapSchema<ColyseusNetworkedEntity>();

		[Type(1, "map", typeof(MapSchema<ColyseusNetworkedUser>))]
		public MapSchema<ColyseusNetworkedUser> networkedUsers = new MapSchema<ColyseusNetworkedUser>();

		[Type(2, "map", typeof(MapSchema<string>), "string")]
		public MapSchema<string> attributes = new MapSchema<string>();

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<MapSchema<ColyseusNetworkedEntity>> __networkedEntitiesChange;
		public Action OnNetworkedEntitiesChange(PropertyChangeHandler<MapSchema<ColyseusNetworkedEntity>> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.networkedEntities));
			__networkedEntitiesChange += __handler;
			if (__immediate && this.networkedEntities != null) { __handler(this.networkedEntities, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(networkedEntities));
				__networkedEntitiesChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<MapSchema<ColyseusNetworkedUser>> __networkedUsersChange;
		public Action OnNetworkedUsersChange(PropertyChangeHandler<MapSchema<ColyseusNetworkedUser>> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.networkedUsers));
			__networkedUsersChange += __handler;
			if (__immediate && this.networkedUsers != null) { __handler(this.networkedUsers, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(networkedUsers));
				__networkedUsersChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<MapSchema<string>> __attributesChange;
		public Action OnAttributesChange(PropertyChangeHandler<MapSchema<string>> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.attributes));
			__attributesChange += __handler;
			if (__immediate && this.attributes != null) { __handler(this.attributes, null); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(attributes));
				__attributesChange -= __handler;
			};
		}

		protected override void TriggerFieldChange(DataChange change) {
			switch (change.Field) {
				case nameof(networkedEntities): __networkedEntitiesChange?.Invoke((MapSchema<ColyseusNetworkedEntity>) change.Value, (MapSchema<ColyseusNetworkedEntity>) change.PreviousValue); break;
				case nameof(networkedUsers): __networkedUsersChange?.Invoke((MapSchema<ColyseusNetworkedUser>) change.Value, (MapSchema<ColyseusNetworkedUser>) change.PreviousValue); break;
				case nameof(attributes): __attributesChange?.Invoke((MapSchema<string>) change.Value, (MapSchema<string>) change.PreviousValue); break;
				default: break;
			}
		}
	}

