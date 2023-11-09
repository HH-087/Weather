using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ChinarDragImage : MonoBehaviour
{
    private Transform beginParentTransform; //记录开始拖动时的父级对象        
    /// <summary>
    /// UI界面的顶层，这里我用的是 Canvas
    /// (这个变量在开发中设置到单例中较好，不然每一个物品都会初始化查找
    /// GameObject.Find("Canvas").transform;)
    /// </summary>
    private Transform topOfUiT;
    Vector3 screenpos;
    Vector3 worldpos;
    Vector3 pos;
    private Vector2 Drop;

    void Start()
    {
        topOfUiT = GameObject.Find("1-2Wire").transform;
    }

    public void Begin(BaseEventData data)
    {
        if (transform.parent == topOfUiT) return;
        beginParentTransform = transform.parent;
        transform.SetParent(topOfUiT);
    }

    /// <param name="_">UI事件数据</param>
    public void OnDrag(BaseEventData _)
    {
        //transform.position = Input.mousePosition;
        transform.position = worldpos;
        if (transform.GetComponent<Image>().raycastTarget) transform.GetComponent<Image>().raycastTarget = false;
    }

    public void End(BaseEventData data)
    {
        PointerEventData _ = data as PointerEventData;
        if (_ == null) return;
        GameObject go = _.pointerCurrentRaycast.gameObject;
        if (go.tag == "Grid") //如果当前拖动物体下是：格子 -（没有物品）时
        {
            SetPosAndParent(transform, go.transform);
            transform.GetComponent<Image>().raycastTarget = true;
        }
        else if (go.tag == "Good") //如果是物品
        {
            SetPosAndParent(transform, go.transform.parent);                              //将当前拖动物品设置到目标位置
            go.transform.SetParent(topOfUiT);                                             //目标物品设置到 UI 顶层
            if (Math.Abs(go.transform.position.x - beginParentTransform.position.x) <= 0) //以下 执行置换动画，完成位置互换 （关于数据的交换，根据自己的工程情况，在下边实现）
            {
                go.transform.SetParent(beginParentTransform);
                transform.GetComponent<Image>().raycastTarget = true;
            }
            else
            {
                go.transform.SetParent(beginParentTransform);
                transform.GetComponent<Image>().raycastTarget = true;
            }
        }
        else //其他任何情况，物体回归原始位置
        {
            SetPosAndParent(transform, beginParentTransform);
            transform.GetComponent<Image>().raycastTarget = true;
        }
    }

    private void SetPosAndParent(Transform t, Transform parent)
    {
        t.SetParent(parent);
        t.position = parent.position;
    }
    private void Update()
    {
        screenpos = Input.mousePosition;
        screenpos.z = Camera.main.nearClipPlane + 1;
        worldpos = Camera.main.ScreenToWorldPoint(screenpos);
    }
}