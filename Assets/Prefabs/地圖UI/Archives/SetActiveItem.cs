using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using Naninovel;
public class SetActiveItem : MonoBehaviour
{
    bool Allow1;
    bool Allow2;
    public GameObject CleanData,CleanShelf,Door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var variableManager = Engine.GetService<ICustomVariableManager>();
        //variableManager.TryGetVariableValue<bool>("ShelfCount", out var bool1Value);
        //variableManager.TryGetVariableValue<bool>("DataCount", out var bool2Value);
        variableManager.TryGetVariableValue<bool>("StartClean", out var bool3Value);
        //Allow1 = bool1Value;
        //Allow2 = bool2Value;
        //print(bool1Value);
        //print(bool2Value);
        if(bool3Value)
        {
            CleanData.SetActive(true);
            CleanShelf.SetActive(true);
            Door.SetActive(true);
        }
    }
}
