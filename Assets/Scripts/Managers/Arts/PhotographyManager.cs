using UnityEngine;
using Pamux.Lib.Utilities;
using Pamux.Lib.Extensions;

namespace Pamux.Lib.Managers
{
    [RequireComponent(typeof(VfxManager))]
    [RequireComponent(typeof(Light))]
    public class PhotographyManager : Singleton<PhotographyManager>
    {
        private Light light;

        
       
        protected override void Awake()
        {
            base.Awake();

            light = GetComponent<Light>();

            light.type = LightType.Directional;
            light.color = "FFD48FFF".ToColor();
        }
    }
}