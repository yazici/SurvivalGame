using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GameObjects
{
    [Serializable]
    public class WeightedTag
    {
        [SerializeField]
        public string Tag;
        [SerializeField]
        public float Weight;
    }
}
