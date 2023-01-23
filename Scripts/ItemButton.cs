using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField] private GameObject itemGb;
    public Item item;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] Sprite lockedSprite;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip;
    private bool _unlocked;
    private void Awake()
    {
        _unlocked = false;
        GetComponent<Image>().sprite = lockedSprite;
        item = itemGb.GetComponentInChildren<Item>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonClick()
    {
        audioSource.PlayOneShot(clip, 1);
        if (_unlocked)
        {
            title.text = item.itemName;
            description.text = item.description;
        }
        else
        {
            title.text = "Sconosciuto";
            description.text = "Non hai ancora scoperto questo oggetto";
        }
    }

    public void Unlock()
    {
        _unlocked = true;
        GetComponent<Image>().sprite = item.sprite;
    }
}
