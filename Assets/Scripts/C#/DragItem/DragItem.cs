using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    Vector3 screenpos;
    Vector3 worldpos;
    Vector3 pos;
    private Vector2 Drop;
    private string CheckName;

    public void OnBeginDrag(PointerEventData eventData)
    {
        pos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = worldpos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = pos;
    }
    public void OnDrop(PointerEventData eventData)
    {
        Drop = eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition;
        Debug.Log(Drop);
        CheckName = eventData.pointerDrag.name;
        Debug.Log(CheckName);
        PlayerPrefs.SetString("CheckName", CheckName);
    }

    void Update()
    {
        screenpos = Input.mousePosition;
        screenpos.z = Camera.main.nearClipPlane + 1;
        worldpos = Camera.main.ScreenToWorldPoint(screenpos);
        PlayerPrefs.SetFloat("Drop_x", Drop.x);
        PlayerPrefs.SetFloat("Drop_y", Drop.y);
    }
}