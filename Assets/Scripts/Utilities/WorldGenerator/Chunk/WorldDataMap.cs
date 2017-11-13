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
            var resolution = (float)(Resolution - 1);
            for (int zRes = 0; zRes < Resolution; ++zRes)
            {
                for (int xRes = 0; xRes < Resolution; ++xRes)
                {
                    float xCoordinate = (cX + (xRes / resolution));
                    float zCoordinate = (cZ + (zRes / resolution));

                    Map[xRes, zRes] = valueProvider.Invoke(xCoordinate, zCoordinate);
                }
            }
        }
    }
}
