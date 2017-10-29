using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pamux.Lib.LevelData.Generator
{
    public class ObjectCreationParameters
    {
        public static LevelDataGeneratorSettings S { get { return LevelDataGeneratorSettings.Instance; } }
        public static LevelDataGenerator G { get { return LevelDataGenerator.Instance; } }


        public int Count;
        public GameObject[] Templates;

        public float MinHeight;
        public float MaxHeight;
        public float HeightWindow => MaxHeight - MinHeight;

        public float RandomHeight => MinHeight + ((float) G.Random.NextDouble() * HeightWindow);

        public GameObject RandomTemplate => Templates.Length == 0 ? null : Templates[G.Random.Next(Templates.Length)];

        public Vector3 GetRandomScale(GameObject prefab)
        {
            if (HeightWindow == 0)
            {
                return Vector3.one;
            }

            var meshFilter = prefab.GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                return Vector3.one;
            }

            var mesh = meshFilter.sharedMesh;
            if (mesh == null)
            {
                return Vector3.one;
            }


            //Debug.Log($"Bounds: {mesh.bounds.size.x},{mesh.bounds.size.y},{mesh.bounds.size.z}");
            var scale = RandomHeight / mesh.bounds.size.y;
            
            return new Vector3(scale, scale, scale);
        }
    }
}
