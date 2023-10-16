using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform group;
    GameObject father_gameObject;
    Vector3 screenpos;
    Vector3 worldpos;
    Vector3 pos;
    void Start()
    {
        father_gameObject = GameObject.Find("Inventory");
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        pos = transform.position;
        //transform.SetParent(transform.root);
        //gameObject.transform.parent = father_gameObject.transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = Input.mousePosition;
        transform.position = worldpos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //transform.SetParent(group);
        transform.position = pos;
    }


    void Update()
    {
        //print(this.transform.position);
        screenpos = Input.mousePosition;
        screenpos.z = Camera.main.nearClipPlane + 1;
        worldpos = Camera.main.ScreenToWorldPoint(screenpos);
        
        //print(Input.mousePosition);
    }
}