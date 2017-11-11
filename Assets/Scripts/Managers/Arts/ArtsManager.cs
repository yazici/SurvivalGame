using UnityEngine;
using Pamux.Lib.Utilities;

namespace Pamux.Lib.Managers
{
    [RequireComponent(typeof(AudioManager))]
    [RequireComponent(typeof(CinematographyManager))]
    [RequireComponent(typeof(PhotographyManager))]
    [RequireComponent(typeof(ContentManager))]
    [RequireComponent(typeof(NarrativeManager))]
    [RequireComponent(typeof(SfxManager))]
    [RequireComponent(typeof(VfxManager))]

    public class ArtsManager : Singleton<ArtsManager>
    {
    }
}