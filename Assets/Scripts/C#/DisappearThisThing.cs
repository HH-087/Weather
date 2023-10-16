using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearThisThing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //GameObject This;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DisappearThis()
    {
        gameObject.SetActive(false);
    }
}
