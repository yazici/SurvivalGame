using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
public class ClickToMove : MonoBehaviour
{
    public Vector3? targetPosition;
    public int smooth;

    public ThirdPersonCharacter thirdPersonCharacter;
    void Awake()
    {
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var playerPlane = new Plane(Vector3.up, transform.position);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hitdist = 0f;

            if (playerPlane.Raycast(ray, out hitdist))
            {
                var tp = ray.GetPoint(hitdist);
                targetPosition = new Vector3(tp.x, transform.position.y, tp.z);
            }
        }

        if (targetPosition.HasValue)
        {
            //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
            thirdPersonCharacter.Move(targetPosition.Value - transform.position, false, false);
            if ((targetPosition.Value - transform.position).magnitude < 0.25f)
            {
                targetPosition = null;
            }
        }


    }
}
