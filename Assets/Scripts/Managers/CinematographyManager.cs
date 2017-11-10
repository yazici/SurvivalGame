using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pamux.Utilities;

namespace Pamux.Lib.Managers
{
    public class CinematographyManager : Singleton<CinematographyManager>
    {
        public Camera MainCamera;

        public Camera[] Cameras;
    }
}