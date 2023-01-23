using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesLoader : MonoBehaviour
{
    HashSet<string> unlockedItems;

    // Start is called before the first frame update
    void Start()
    {
        unlockedItems = new HashSet<string>(SaveCollectibles.Load().lista);
        foreach( ItemButton ib in GetComponentsInChildren<ItemButton>())
        {
            if (unlockedItems.Contains(ib.item.itemName))
            {
                ib.Unlock();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void load()
    {
        unlockedItems = new HashSet<string>(SaveCollectibles.Load().lista);
        foreach (ItemButton ib in GetComponentsInChildren<ItemButton>())
        {
            if (unlockedItems.Contains(ib.item.itemName))
            {
                ib.Unlock();
            }
        }
    }
}
