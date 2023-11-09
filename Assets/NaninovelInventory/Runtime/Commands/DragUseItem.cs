using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUseItem : MonoBehaviour, IDropHandler
{
    private Vector2 Drop;
    private string CheckName;

    void Update()
    {
        PlayerPrefs.SetFloat("Drop_x", Drop.x);
        PlayerPrefs.SetFloat("Drop_y", Drop.y);
    }
    public void OnDrop(PointerEventData eventData)
    {
        Drop = eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition;
        Debug.Log(Drop);
        CheckName = eventData.pointerDrag.name;
        Debug.Log(CheckName);
        PlayerPrefs.SetString("CheckName", CheckName);
    }
}
