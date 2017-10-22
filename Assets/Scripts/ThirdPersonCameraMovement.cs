using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class ThirdPersonCameraMovement : MonoBehaviour
    {
        public Transform player;
        public Camera camera;
        public Vector3 offset = new Vector3(0f, 5f, 6f);
        private bool needsToLookAtPlayer = true;

        public void ZoomIn()
        {
           offset += player.transform.forward * 5.0f;
           needsToLookAtPlayer = true;
        }
        
        public void ZoomOut()
        {
            offset -= player.transform.forward * 5.0f;
            needsToLookAtPlayer = true;
        }

        private void LateUpdate()
        {
            camera.transform.position = player.position + offset;

            if (needsToLookAtPlayer)
            {
                needsToLookAtPlayer = false;
                camera.transform.LookAt(player);
            }
        }
    }
}
