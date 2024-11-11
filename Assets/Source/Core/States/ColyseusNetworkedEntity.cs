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

	public partial class ColyseusNetworkedEntity : Schema {
#if UNITY_5_3_OR_NEWER
[Preserve] 
#endif
public ColyseusNetworkedEntity() { }
		[Type(0, "string")]
		public string id = default(string);

		[Type(1, "string")]
		public string ownerId = default(string);

		[Type(2, "string")]
		public string creationId = default(string);

		[Type(3, "number")]
		public float xPos = default(float);

		[Type(4, "number")]
		public float yPos = default(float);

		[Type(5, "number")]
		public float zPos = default(float);

		[Type(6, "number")]
		public float xRot = default(float);

		[Type(7, "number")]
		public float yRot = default(float);

		[Type(8, "number")]
		public float zRot = default(float);

		[Type(9, "number")]
		public float wRot = default(float);

		[Type(10, "number")]
		public float xScale = default(float);

		[Type(11, "number")]
		public float yScale = default(float);

		[Type(12, "number")]
		public float zScale = default(float);

		[Type(13, "number")]
		public float xVel = default(float);

		[Type(14, "number")]
		public float yVel = default(float);

		[Type(15, "number")]
		public float zVel = default(float);

		[Type(16, "number")]
		public float timestamp = default(float);

		[Type(17, "map", typeof(MapSchema<string>), "string")]
		public MapSchema<string> attributes = new MapSchema<string>();

		/*
		 * Support for individual property change callbacks below...
		 */

		protected event PropertyChangeHandler<string> __idChange;
		public Action OnIdChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.id));
			__idChange += __handler;
			if (__immediate && this.id != default(string)) { __handler(this.id, default(string)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(id));
				__idChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<string> __ownerIdChange;
		public Action OnOwnerIdChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.ownerId));
			__ownerIdChange += __handler;
			if (__immediate && this.ownerId != default(string)) { __handler(this.ownerId, default(string)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(ownerId));
				__ownerIdChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<string> __creationIdChange;
		public Action OnCreationIdChange(PropertyChangeHandler<string> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.creationId));
			__creationIdChange += __handler;
			if (__immediate && this.creationId != default(string)) { __handler(this.creationId, default(string)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(creationId));
				__creationIdChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __xPosChange;
		public Action OnXPosChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.xPos));
			__xPosChange += __handler;
			if (__immediate && this.xPos != default(float)) { __handler(this.xPos, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(xPos));
				__xPosChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __yPosChange;
		public Action OnYPosChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.yPos));
			__yPosChange += __handler;
			if (__immediate && this.yPos != default(float)) { __handler(this.yPos, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(yPos));
				__yPosChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __zPosChange;
		public Action OnZPosChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.zPos));
			__zPosChange += __handler;
			if (__immediate && this.zPos != default(float)) { __handler(this.zPos, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(zPos));
				__zPosChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __xRotChange;
		public Action OnXRotChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.xRot));
			__xRotChange += __handler;
			if (__immediate && this.xRot != default(float)) { __handler(this.xRot, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(xRot));
				__xRotChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __yRotChange;
		public Action OnYRotChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.yRot));
			__yRotChange += __handler;
			if (__immediate && this.yRot != default(float)) { __handler(this.yRot, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(yRot));
				__yRotChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __zRotChange;
		public Action OnZRotChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.zRot));
			__zRotChange += __handler;
			if (__immediate && this.zRot != default(float)) { __handler(this.zRot, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(zRot));
				__zRotChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __wRotChange;
		public Action OnWRotChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.wRot));
			__wRotChange += __handler;
			if (__immediate && this.wRot != default(float)) { __handler(this.wRot, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(wRot));
				__wRotChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __xScaleChange;
		public Action OnXScaleChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.xScale));
			__xScaleChange += __handler;
			if (__immediate && this.xScale != default(float)) { __handler(this.xScale, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(xScale));
				__xScaleChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __yScaleChange;
		public Action OnYScaleChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.yScale));
			__yScaleChange += __handler;
			if (__immediate && this.yScale != default(float)) { __handler(this.yScale, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(yScale));
				__yScaleChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __zScaleChange;
		public Action OnZScaleChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.zScale));
			__zScaleChange += __handler;
			if (__immediate && this.zScale != default(float)) { __handler(this.zScale, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(zScale));
				__zScaleChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __xVelChange;
		public Action OnXVelChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.xVel));
			__xVelChange += __handler;
			if (__immediate && this.xVel != default(float)) { __handler(this.xVel, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(xVel));
				__xVelChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __yVelChange;
		public Action OnYVelChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.yVel));
			__yVelChange += __handler;
			if (__immediate && this.yVel != default(float)) { __handler(this.yVel, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(yVel));
				__yVelChange -= __handler;
			};
		}

		protected event PropertyChangeHandler<float> __zVelChange;
		public Action OnZVelChange(PropertyChangeHandler<float> __handler, bool __immediate = true) {
			if (__callbacks == null) { __callbacks = new SchemaCallbacks(); }
			__callbacks.AddPropertyCallback(nameof(this.zVel));
			__zVelChange += __handler;
			if (__immediate && this.zVel != default(float)) { __handler(this.zVel, default(float)); }
			return () => {
				__callbacks.RemovePropertyCallback(nameof(zVel));
				__zVelChange -= __handler;
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
				case nameof(id): __idChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
				case nameof(ownerId): __ownerIdChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
				case nameof(creationId): __creationIdChange?.Invoke((string) change.Value, (string) change.PreviousValue); break;
				case nameof(xPos): __xPosChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(yPos): __yPosChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(zPos): __zPosChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(xRot): __xRotChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(yRot): __yRotChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(zRot): __zRotChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(wRot): __wRotChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(xScale): __xScaleChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(yScale): __yScaleChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(zScale): __zScaleChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(xVel): __xVelChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(yVel): __yVelChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(zVel): __zVelChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(timestamp): __timestampChange?.Invoke((float) change.Value, (float) change.PreviousValue); break;
				case nameof(attributes): __attributesChange?.Invoke((MapSchema<string>) change.Value, (MapSchema<string>) change.PreviousValue); break;
				default: break;
			}
		}
	}

