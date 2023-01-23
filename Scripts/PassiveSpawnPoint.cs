using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSpawnPoint : MonoBehaviour
{
    [SerializeField] private Transform passiveHandler;

    public void InstantiatePassiveObj(GameObject passive)
    {
        GameObject newPassive = Instantiate(passive, passiveHandler);
        newPassive.transform.localPosition = Vector3.zero;
        newPassive.GetComponent<PassiveItem>().spawn = this.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
