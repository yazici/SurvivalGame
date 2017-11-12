using UnityEngine;
using Pamux.Lib.Utilities;

namespace Pamux.Lib.Managers
{
    [RequireComponent(typeof(AudioManager))]
    [RequireComponent(typeof(ContentManager))]
    [RequireComponent(typeof(NarrativeManager))]

    public class ArtsManager : Singleton<ArtsManager>
    {
    }
}