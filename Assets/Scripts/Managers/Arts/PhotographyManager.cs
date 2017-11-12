using UnityEngine;
using Pamux.Lib.Utilities;
using Pamux.Lib.Extensions;

namespace Pamux.Lib.Managers
{
    [RequireComponent(typeof(VfxManager))]
    [RequireComponent(typeof(Light))]
    public class PhotographyManager : Singleton<PhotographyManager>
    {
        protected override void Awake()
        {
            base.Awake();

            var light = GetComponent<Light>();

            light.type = LightType.Directional;
            light.color = "FFD48FFF".ToColor();
            light.intensity = 1.5f;
            light.transform.rotation = Quaternion.Euler(45f, 0, 45f);
        }
    }
}