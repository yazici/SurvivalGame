using Pamux.Lib.Enums;
using Pamux.Lib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Pamux.Lib.WorldGen
{
    [Serializable]
    public class BiomeData
    {
        [SerializeField]
        public string Name;

        public Texture2D Texture { get; private set; }

        public BiomeData(string name)
        {
            Name = name;

            Texture = name.LoadResource<Texture2D>(PamuxResourceTypes.BiomeGroundTexture);
        }

    }
}
