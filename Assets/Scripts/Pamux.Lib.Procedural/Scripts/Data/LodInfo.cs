using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pamux.Lib.Procedural.Data
{
    [System.Serializable]
    public struct LodInfo
    {
        public const int numSupportedLODs = 5;

        [Range(0, numSupportedLODs - 1)]
        public int lod;
        public float visibleDstThreshold;


        public float sqrVisibleDstThreshold
        {
            get
            {
                return visibleDstThreshold * visibleDstThreshold;
            }
        }
    }
}