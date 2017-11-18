using Pamux.Lib.Enums;
using UnityEngine;

namespace Pamux.Lib.Utilities
{
    public class ThirdPersonPlayerPointOfView : PlayerPointOfView
    {
        private Camera thirdPersonCamera;

        private void Awake()
        {
            thirdPersonCamera = gameObject.GetComponentInChildren<Camera>();
        }

        private void OnEnable()
        {
            thirdPersonCamera.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            thirdPersonCamera.gameObject.SetActive(false);
        }
    }
}
