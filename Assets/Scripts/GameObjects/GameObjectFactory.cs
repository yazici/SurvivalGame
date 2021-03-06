﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pamux.Lib.WorldGen;
using UnityEngine;

namespace Pamux.Lib.GameObjects
{
    public class GameObjectFactory
    {
        public static WorldGeneratorSettings S { get { return WorldGeneratorSettings.Instance; } }
        public static WorldGenerator G { get { return WorldGenerator.Instance; } }

        private WorldDataChunkContent WorldDataChunkContent;
        public WorldDataMaps DataMaps { get { return WorldDataChunkContent.DataMaps; } }

        public GameObjectFactory(WorldDataChunkContent WorldDataChunkContent)
        {
            this.WorldDataChunkContent = WorldDataChunkContent;
        }

        public GameObject Create(GameObjectTypes type)
        {
            GameObject o = null;
            switch (type)
            {
                case GameObjectTypes.PrimitiveCapsule:
                    o = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    break;
                case GameObjectTypes.PrimitiveCube:
                    o = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    break;
                case GameObjectTypes.PrimitiveCylinder:
                    o = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    break;
                case GameObjectTypes.PrimitiveSphere:
                    o = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    break;
                case GameObjectTypes.PrimitivePlane:
                    o = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    break;
                case GameObjectTypes.PrimitiveQuad:
                    o = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    break;
            }
        
            if (o != null)
            { 
                WorldDataChunkContent.GameObjects.Add(o);
            }
            return o;
        }

        public GameObject CreatePrefab(GameObject prefab, Vector3 position, Vector3 scale)
        {
            if (prefab == null)
            {
                throw new ArgumentNullException(nameof(prefab));
            }
            // Editor only
            //var prefab = AssetDatabase.LoadAssetAtPath("Assets/something.prefab", typeof(GameObject));
            //var o = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;

            // Resources folder
            //return GameObject.Instantiate((GameObject)Resources.Load(prefabName), position, Quaternion.identity);
           

            var o = GameObject.Instantiate(prefab, position, Quaternion.identity);
            
            if (o != null)
            {
                o.transform.localScale = scale;
                o.transform.position = position;
                WorldDataChunkContent.GameObjects.Add(o);
            }

            return o;
        }



        public GameObjectTypes NextRandomGameObjectType
        {
            get
            {
                var v = Enum.GetValues(typeof(GameObjectTypes));
                return (GameObjectTypes)v.GetValue(G.Random.Next(1, v.Length));
            }
        }

        public GameObject CreateRandom(int x, int z)
        {
            return Create(NextRandomGameObjectType, x, z);
        }

        public GameObject CreateRandom(Vector3 position)
        {
            return Create(NextRandomGameObjectType, position);
        }

        public GameObject Create(GameObjectTypes type, Vector3 position)
        {
            var o = Create(type);
            o.transform.position = position;
            return o;
        }

        public GameObject CreateRandom(ObjectCreationParameters parameters, Vector3 position)
        {
            // Editor only
            //var prefab = AssetDatabase.LoadAssetAtPath("Assets/something.prefab", typeof(GameObject));
            //var o = Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;

            // Resources folder
            //return GameObject.Instantiate((GameObject)Resources.Load(prefabName), position, Quaternion.identity);

            var prefab = parameters.RandomTemplate;
            if (prefab == null)
            {
                return null;
            }

            var scale = parameters.GetRandomScale(prefab);

            

            return CreatePrefab(prefab, position, scale);
        }

        internal GameObject Create(GameObjectTypes type, int x, int z)
        {
            return Create(type, S.ChunkCenterWorldAtDefaultElevation(x, z));
        }
    }
}
