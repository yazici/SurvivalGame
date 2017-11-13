using Pamux.Lib.Enums;
using UnityEngine;

namespace Pamux.Lib.Utilities
{
    public class ThirdPersonPlayerPointOfView : PlayerPointOfView
    {
        private GameObject thirdPersonCamera;

        private void Awake()
        {
            var res = Resources.Load("Prefabs/CameraWithWeather") as GameObject;
            thirdPersonCamera = Instantiate(res);
            thirdPersonCamera.transform.parent = transform;
            thirdPersonCamera.SetActive(false);

            var cameraFollow = thirdPersonCamera.GetComponent<SmoothFollow>();
            cameraFollow.target = transform;
        }

        private void OnEnable()
        {
            thirdPersonCamera.SetActive(true);
        }

        private void OnDisable()
        {
            thirdPersonCamera.SetActive(false);
        }
    }
}
