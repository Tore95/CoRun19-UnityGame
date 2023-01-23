using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float minDistance = 0.0f;
    public float smooth = 10f;
    //[SerializeField] private Transform maxPosition;
    private float maxDistance;
    private LayerMask mask;
    public float Distance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        maxDistance = Vector3.Distance(transform.position, transform.parent.position);
        Distance = maxDistance;
        mask = ~LayerMask.GetMask("Enemies","Ignore Raycast","Items");
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 direction = transform.localPosition.normalized;
        RaycastHit hit;
        if (Physics.Raycast(transform.parent.position,-transform.parent.forward,out hit,maxDistance,mask))
        {
            Distance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            //Debug.Log(hit.transform.gameObject.name);
        }
        else
        {
            Distance = maxDistance;
            //Debug.Log("no");
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition,new Vector3(0, 0, -Distance),Time.deltaTime * smooth);
    }
}
