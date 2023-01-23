using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pivotRotation : MonoBehaviour
{
    public float ObstacleRange = 10f;
    [SerializeField] GameObject Enemy;
    private Quaternion startRot;
    public bool isReturning = false;
    // Start is called before the first frame update
    void Start()
    {
        startRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (isReturning)
        {
            transform.rotation = startRot;
            isReturning = false;
        }*/
        baseRot();
        transform.position = Enemy.transform.position;
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.SphereCast(ray, 0.75f, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;
            if (!(hitObject==Enemy))
            {
                if (hit.distance < ObstacleRange)
                {
                    float angle = Random.Range(-45, 45);
                    transform.Rotate(0, angle, 0);

                }
            }
            
        }
    }
    void baseRot()
    {
        if (transform.rotation.eulerAngles.x != 0 || transform.rotation.eulerAngles.z != 0)
        {
            transform.rotation.eulerAngles.Set(0, transform.rotation.eulerAngles.y, 0);
        }
    }

}
