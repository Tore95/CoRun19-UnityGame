using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MainCameraMenu : MonoBehaviour
{
    Camera _maincam;
    Vector3 startpos;
    Quaternion startrot;
    public Camera _colcam;
    public Camera _exitcam;

    private Vector3 _nextPos;
    private Quaternion _nextRot;
    private float _speed = 15;
    // Start is called before the first frame update
    void Start()
    {
        _maincam = GetComponent<Camera>();
        startpos = _maincam.transform.position;
        startrot = _maincam.transform.rotation;
        _nextPos = startpos;
        _nextRot = startrot;
    }

    // Update is called once per frame
    void Update()
    {
        _maincam.transform.position = Vector3.Lerp(_maincam.transform.position, _nextPos,Time.deltaTime * _speed);
        _maincam.transform.rotation = Quaternion.Lerp(_maincam.transform.rotation, _nextRot, Time.deltaTime * _speed);
    }
    public void moveToExit()
    {
        _nextRot = _exitcam.transform.rotation;
        _nextPos = _exitcam.transform.position;
    }
    public void moveToColl()
    {

       _nextRot = _colcam.transform.rotation;
       _nextPos = _colcam.transform.position;

    }
    public void moveToStart()
    {
        _nextRot = startrot;
        _nextPos = startpos;
    }
}
