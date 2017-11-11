using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Pamux.Lib.GameObjects
{
    public class ThirdPersonCameraMovement : MonoBehaviour
    {
        public Transform player;
        public Vector3 offset = new Vector3(0f, 5f, 6f);
        public float speed = 10.0f;
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
            if (!player.gameObject.active)
            {
                return;
            }


            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.position + offset, step);

            //transform.position = player.position + offset;

            if (needsToLookAtPlayer)
            {
                
                transform.LookAt(player);

                if ((transform.position - player.position + offset).magnitude < 0.1f)
                { 
                    needsToLookAtPlayer = false;
                }
            }
        }
    }
}
