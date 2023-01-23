using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapZoom : MonoBehaviour
{
    private Camera _minicamera;
    private Vector3 zoomOutPosition;
    private Vector3 zoomInPosition;
    private float zoomOutSize;
    private float zoomInSize;
    private Vector3 currentPosition;
    private float currentSize;
    private void Awake()
    {
        _minicamera = GameObject.FindGameObjectWithTag("MinimapCamera").GetComponent<Camera>();
        zoomOutPosition = _minicamera.transform.position;
        currentPosition = zoomOutPosition;
        zoomOutSize = _minicamera.orthographicSize;
        currentSize = zoomOutSize;
    }
    // Start is called before the first frame update
    void Start()
    {
        zoomInPosition = new Vector3(this.transform.position.x, _minicamera.transform.position.y, this.transform.position.z);
        zoomInSize = zoomOutSize / 2;
    }

    // Update is called once per frame
    void Update()
    {
        _minicamera.transform.position = Vector3.Lerp(_minicamera.transform.position, currentPosition, Time.deltaTime * 10);
        _minicamera.orthographicSize = Mathf.Lerp(_minicamera.orthographicSize, currentSize, Time.deltaTime * 10);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController pc = other.gameObject.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.minimapIcon.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
            currentPosition = zoomInPosition;
            currentSize = zoomInSize;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController pc = other.gameObject.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.minimapIcon.transform.localScale = new Vector3(1, 1, 1);
            currentPosition = zoomOutPosition;
            currentSize = zoomOutSize;
        }
    }
}
