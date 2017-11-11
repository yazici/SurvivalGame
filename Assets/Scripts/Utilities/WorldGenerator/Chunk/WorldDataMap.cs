using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pamux.Lib.WorldGen
{
    [Serializable]

    public class WorldDataMap
    {
        public static WorldGeneratorSettings S { get { return WorldGeneratorSettings.Instance; } }
        public static WorldGenerator G { get { return WorldGenerator.Instance; } }

        [SerializeField]
        public int Resolution = 129;
        [SerializeField]
        public readonly float[,] Map;

        public WorldDataMap(int resolution)
        {
            Resolution = resolution;
            Map = new float[resolution, resolution];
        }

        internal void ForEachPoint(int cX, int cZ, Func<float, float, float> valueProvider)
        {
            for (var zRes = 0; zRes < Resolution; ++zRes)
            {
                for (var xRes = 0; xRes < Resolution; ++xRes)
                {
                    var xCoordinate = (cX + (float) (xRes / (Resolution - 1)));
                    var zCoordinate = (cZ + (float) (zRes / (Resolution - 1)));

                    Map[xRes, zRes] = valueProvider.Invoke(xCoordinate, zCoordinate);
                }
            }
        }
    }
}
