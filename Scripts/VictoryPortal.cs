using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryPortal : MonoBehaviour
{
    private RunManager runManager;

    // Start is called before the first frame update
    void Start()
    {
        runManager = GameObject.Find("Controller").GetComponent<RunManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerController>() != null)
        {
            runManager.NextLevel();
        }
    }
}
