using UnityEngine;
using Pamux.Lib.Utilities;

namespace Pamux.Lib.Managers
{
    [RequireComponent(typeof(AssetManager))]
    [RequireComponent(typeof(CacheManager))]

    [RequireComponent(typeof(DatabaseManager))]
    
    [RequireComponent(typeof(InputManager))]

    [RequireComponent(typeof(ObjectPoolManager))]
    [RequireComponent(typeof(NetworkingManager))]
    [RequireComponent(typeof(ApiManager))]
    [RequireComponent(typeof(PlatformManager))]
    [RequireComponent(typeof(PersistenceManager))]

    [RequireComponent(typeof(TelemetryManager))]

    public class UtilitiesManager : Singleton<UtilitiesManager>
    {
    }
}