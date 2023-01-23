using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunManager : MonoBehaviour, IGameManager
{
    private bool isSpawned = false;
    private GameObject gate;
    private GameObject gateClose;
    private Vector3 _closeGatePos;
    private Vector3 _openGatePos;
    private int fenceStatus;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject death;
    [SerializeField] private NotificationExample notification;
    //private bool _doorIsMoving;
    public bool IsPaused { get; private set; }
    public bool IsEnded { get; private set; } = false;
    public NotificationExample Notification => notification;
    [SerializeField] public AudioClip[] playlist;
    public string victoryMusic;
    public string deathMusic;
    public string finalMusic;
    private AudioSource audioSource;
    private int soundState;
    private int inPlay = 0;
    private AudioClip prevPlay;
    
    void Awake()
    {
        GetComponent<CollectiblesManager>().Load();
    }
    void Start()
    {
        soundState = 1;
        audioSource = GetComponent<AudioSource>();
        gate = GameObject.Find("Gate");
        gateClose = GameObject.Find("GateClose");
        _openGatePos = gate.transform.position;
        _closeGatePos = gateClose.transform.position; //non ho inserito uno spostamento fisso perché in base a come viene generato si deve spostare in x o in y
        IsPaused = false;
        playlist = ShuffleArray(playlist);
    }

    //preso dalla Lezione 14
    private T[] ShuffleArray<T>(T[] numbers)
    {
        T[] newArray = numbers.Clone() as T[];
        for (int i = 0; i < newArray.Length; i++)
        {
            T tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }
        return newArray;
    }

    public void Death()
    {
        IsEnded = true;
        Time.timeScale = 0;
        //////////////////////////////// abilitare qua il menù di morte
        death.SetActive(true);
        hud.SetActive(false);
        player.GetComponent<MouseLook>().enabled = false;
        player.transform.Find("Pivot").GetComponent<MouseLook>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Pause()
    {
        if (!IsPaused)
        {
            player.GetComponent<MouseLook>().enabled = false;
            player.transform.Find("Pivot").GetComponent<MouseLook>().enabled = false;
            Time.timeScale = 0;
            pause.SetActive(true);
            hud.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        else
        {
            Time.timeScale = 1;
            /////////////////////////////disabilitare qua il menù di pausa
            pause.SetActive(false);
            player.GetComponent<MouseLook>().enabled = true; 
            player.transform.Find("Pivot").GetComponent<MouseLook>().enabled = true;
            hud.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        IsPaused = !IsPaused;

    }
    public void Victory()
    {
        fenceStatus = -1;
        soundState = 2;
    }

    public void playDeathMusic()
    {
        soundState = 3;
    }

    public void NextLevel()
    {
        Time.timeScale = 0;
        IsEnded = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.GetComponent<MouseLook>().enabled = false;
        player.transform.Find("Pivot").GetComponent<MouseLook>().enabled = false;
        win.SetActive(true);
        hud.SetActive(false);
    }

    //uso un intero così da poter usare 0 come valore in cui non deve fare nulla, 1 per chiudersi e -1 per aprirsi
    public void FinalTileFence(int value)
    {
        fenceStatus = value;
    }

    public void FinalTileEntrance()
    {
        Debug.Log("Gate triggerato");
        if (!isSpawned)
        {
            fenceStatus = 1;
            gate.GetComponentInParent<FinalTileController>().SpawnEnemies();
            soundState = 4;
            isSpawned = true;
        }
    }
    void Update()
    {
        if (fenceStatus == -1 || fenceStatus == 1)
        {
            OpenCloseGate();
        }
        Sound();
    }

    void OpenCloseGate()
    {
        switch (fenceStatus)
        {   //una volta dentro non si potrebbe uscire se non tramite vittoria, ma, dato che dobbiamo creare i più svariati effetti degli oggetti uno potrebbe proprio aprire i cancelli del tile finale per tornare indietro senza averlo completato
            case -1:
                OpenFinalTile();
                break;
            case 1:

                CloseFinalTile();
                break;
            default:
                break;
        }

    }


    public void CloseFinalTile() {
        if (gate.transform.position != _closeGatePos)
        {
            gate.transform.position = Vector3.Lerp(gate.transform.position, _closeGatePos, 3f * Time.deltaTime);
        }
        else
        {
            fenceStatus= 0;
        }

    }
    public void OpenFinalTile() {
        if (gate.transform.position != _openGatePos)
        {
            gate.transform.position = Vector3.Lerp(gate.transform.position, _openGatePos, 3f * Time.deltaTime);
        }
        else
        {
           fenceStatus = 0;
        }
    }
    void Sound()
    {
        switch (soundState)
        {
            case 0: break;
            case 1: if (!audioSource.isPlaying)
                    playRandomMusic();break;
            case 2:
                //audioSource.loop = true;
                audioSource.Stop();
                audioSource.loop = true;
                audioSource.clip = (AudioClip)Resources.Load("Music/" + victoryMusic); 
                audioSource.Play();
                soundState = 0;
                break;
            case 3:
                audioSource.Stop();
                audioSource.loop = true;
                audioSource.clip = (AudioClip)Resources.Load("Music/" + deathMusic);
                audioSource.Play();
                soundState = 0;
                break;
            case 4:
                audioSource.Stop();
                audioSource.loop = true;
                audioSource.clip = (AudioClip)Resources.Load("Music/" + finalMusic);
                audioSource.Play();
                soundState = 0;
                break;
            default: break;
        }
    }
    void playRandomMusic()
    {
        audioSource.clip = playlist[inPlay];
        prevPlay = playlist[inPlay];
        audioSource.Play();
        Debug.Log(playlist[inPlay].name);
                if(inPlay == (playlist.Length - 1))
        {
            playlist = ShuffleArray(playlist);
            if(playlist[0] == prevPlay)
            {
                AudioClip tmp = playlist[0];
                playlist[0] = playlist[1];
                playlist[1] = tmp;
            }
            inPlay = 0;
        }
        else
        {
            inPlay++;
        }
    }


}
