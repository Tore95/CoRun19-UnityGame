using Michsky.UI.ModernUIPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Playables;

public class Tutorial : MonoBehaviour
{
    public string title;
    public string description;
    [SerializeField] Sprite icon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void execute(GameObject notify)
    {
       notify.GetComponent<NotificationExample>().ShowNotification(title, description, icon);
       Destroy(this.gameObject.transform.parent.gameObject);
    }



}
