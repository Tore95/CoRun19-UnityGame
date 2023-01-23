using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuBotton : MonoBehaviour, IPointerEnterHandler
{
    public int numeroBottone;
    [SerializeField] private GameObject coll;
    [SerializeField] private RunManager run;
    [SerializeField] private GameObject thanks;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;
    public MainCameraMenu maincam;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (numeroBottone)
        {
            case 1: maincam.moveToStart(); break;
            case 2: maincam.moveToColl(); break;
            case 3: maincam.moveToExit(); break;
            default: break;
        }
    }
    public void execute()
    {
        audioSource.PlayOneShot(clip,1);
        switch (numeroBottone)
        {
            case 1: SceneManager.LoadScene(1); break; //start
            case 2: coll.SetActive(true); break; //collectibles
            case 3: Application.Quit(); break; //exit
            case 4: coll.SetActive(false); break; //back
            case 5: run.Pause(); break; //resume
            case 6: Time.timeScale = 1; SceneManager.LoadScene(0); break; //return to menù
            case 7: thanks.SetActive(true); break;
            case 8: Time.timeScale = 1; SceneManager.LoadScene(1); break; //restart
            case 9: coll.SetActive(true); coll.GetComponentInChildren<CollectiblesLoader>().load(); break;
            default: break;
        }
    }
}
