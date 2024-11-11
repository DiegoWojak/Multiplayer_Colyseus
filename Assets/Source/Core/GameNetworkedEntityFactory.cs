using Assets.Source.Core.Components.Views;
using Assets.Source.Core.Views;
using GameDevWare.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
