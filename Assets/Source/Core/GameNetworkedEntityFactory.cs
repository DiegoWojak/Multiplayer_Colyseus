
using GameDevWare.Serialization;

using System;
using System.Collections.Generic;


namespace Assets.Source.Core
{
    /// <summary>
    /// Responsible for carrying out the creation of network entities and registering them with the Example Manager.
    /// </summary>
    public class GameNetworkedEntityFactory
    {
        private readonly Dictionary<string, Action<ColyseusNetworkedEntity>> _creationCallbacks;
        // TODO: replace GameDevWare stuff
        private readonly IndexedDictionary<string, ColyseusNetworkedEntity> _entities;
        private readonly IndexedDictionary<string, GameNetworkedEntityView> _entityViews;

        public GameNetworkedEntityFactory(Dictionary<string, Action<ColyseusNetworkedEntity>> creationCallbacks, IndexedDictionary<string, ColyseusNetworkedEntity> entities, IndexedDictionary<string, GameNetworkedEntityView> entityViews)
        {
            _creationCallbacks = creationCallbacks;
            _entities = entities;
            _entityViews = entityViews;
        }
    }
}
