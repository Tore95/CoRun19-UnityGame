using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simplerotation : MonoBehaviour
{

    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up);
        transform.Rotate(Vector3.right);
    }
}
