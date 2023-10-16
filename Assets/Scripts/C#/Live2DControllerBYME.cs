using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Live2DControllerBYME : MonoBehaviour
{
    public GameObject Live2DObject;
    private Animator animator;
    public static string appearance;
    // Start is called before the first frame update
    void Start()
    {
        Live2DObject = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //appearance =GetComponent <Live2DController>().Appearance;
    }
}
