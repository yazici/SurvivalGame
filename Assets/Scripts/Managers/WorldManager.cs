using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pamux.Utilities;
using Pamux.Lib.WorldGen;

namespace Pamux.Lib.Managers
{
    public class WorldManager : Singleton<WorldManager>
    {
        public WorldGenerator WorldGenerator;
        public WorldGeneratorSettings WorldGeneratorSettings;
        public TimeManager TimeManager;
        public WeatherManager WeatherManager;
        

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}