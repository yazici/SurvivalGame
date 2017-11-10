using Assets.Scripts.GameObjects;
using Pamux.Lib.WorldGen;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;
public class ClickToMove : MonoBehaviour
{
    private Vector3? targetPosition;

    private ThirdPersonCharacter thirdPersonCharacter;
    public Transform ShowMouseClick;

    public Image image;
    private float originalDistance;

    void Awake()
    {
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
    }

    GameObject GetHitGameObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    Vector3? GetMousePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hitdist = 0f;
        var playerPlane = new Plane(Vector3.up, transform.position);

        if (!playerPlane.Raycast(ray, out hitdist))
        {
            return null;
        }

        var tp = ray.GetPoint(hitdist);
        return new Vector3(tp.x, transform.position.y, tp.z);            
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var pos = GetMousePosition();
            if (pos.HasValue)
            { 
                OnLeftButtonDown(pos.Value);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            var go = GetHitGameObject();
            if (go != null)
            {
                OnRightButtonDown(go);
            }
        }

        if (targetPosition.HasValue)
        {
            var a = (targetPosition.Value - transform.position).magnitude / originalDistance;

            thirdPersonCharacter.Move(targetPosition.Value - transform.position, false, false);
            if ((targetPosition.Value - transform.position).magnitude < 0.25f)
            {
                targetPosition = null;
                a = 0.0f;
            }
            image.color = new Color(image.color.r, image.color.g, image.color.b, a);
        }
    }

    private void OnRightButtonDown(GameObject go)
    {
        
        var taggable = go.GetComponentInParent<Taggable>();
        if (taggable == null)
        {
            return;
        }

        Debug.Log(go.name);

        
        if (taggable.Opinion == "yes")
        {
            taggable.Opinion = "no";
        }
        else if (taggable.Opinion == "no")
        {
            taggable.Opinion = "";
        }
        else
        {
            taggable.Opinion = "yes";
        }

        AssetLibrary.AssetOpinions.Set(taggable);
    }

    private void OnLeftButtonDown(Vector3 pos)
    {
        targetPosition = pos;

        ShowMouseClick.position = new Vector3(targetPosition.Value.x, 0.1f, targetPosition.Value.z);
        originalDistance = (targetPosition.Value - transform.position).magnitude;
    }
}
