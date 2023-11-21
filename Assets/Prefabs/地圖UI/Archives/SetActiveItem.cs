using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using Naninovel;
public class SetActiveItem : MonoBehaviour
{
    //public GameObject CleanData,HideData;
    public Animator Anim;
    void Start()
    {

    }
    void Update()
    {
        var variableManager = Engine.GetService<ICustomVariableManager>();
        variableManager.TryGetVariableValue<bool>("DoorOpening", out var bool2Value);
        variableManager.TryGetVariableValue<bool>("StartClean", out var bool3Value);
        //print(bool2Value);
        //print(bool3Value);
        if (bool3Value)
        {
            print("ass");
            Anim.SetTrigger("Close1");
        }
    }
}
