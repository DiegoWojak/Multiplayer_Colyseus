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

	public partial class ColyseusNetworkedUser : Schema {
#if UNITY_5_3_OR_NEWER
[Preserve] 
#endif
public ColyseusNetworkedUser() { }
		[Type(0, "string")]
		public string sessionId = default(string);

		[Type(1, "boolean")]
		public bool connected = default(bool);

		[Type(2, "number")]
		public float timestamp = default(float);

		[Type(3, "map", typeof(MapSchema<string>), "string")]
		public MapSchema<string> attributes = new MapSchema<string>();

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<string> __sessionIdChange;
		public Action OnSessionIdChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.sessionId));
			__sessionIdChange += __handler;
			if (__immediate && this.sessionId != default(string)) { __handler(this.sessionId, default(string)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(sessionId));
				__sessionIdChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<bool> __connectedChange;
		public Action OnConnectedChange(PropertyChangeHandler<bool> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.connected));
			__connectedChange += __handler;
			if (__immediate && this.connected != default(bool)) { __handler(this.connected, default(bool)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(connected));
				__connectedChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __timestampChange;
		public Action OnTimestampChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.timestamp));
			__timestampChange += __handler;
			if (__immediate && this.timestamp != default(float)) { __handler(this.timestamp, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(timestamp));
				__timestampChange -= __handler;
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
				case nameof(sessionId): __sessionIdChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
				case nameof(connected): __connectedChange?.Invoke((bool) change.Value, (bool) change.PreviousValue); break;
				case nameof(timestamp): __timestampChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(attributes): __attributesChange?.Invoke((MapSchema<string>) change.Value, (MapSchema<string>) change.PreviousValue); break;
				default: break;
			}
		}
	}

