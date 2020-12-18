using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityStandardAssets.SceneUtils;

public class Crosshair : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Hide mouse cursor
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Input.mousePosition;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.lockState = CursorLockMode.Confined;
        ////Cursor.lockState = CursorLockMode.None;
    }
}
