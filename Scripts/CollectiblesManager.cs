using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesManager : MonoBehaviour, IGameManager
{
    public Dictionary<string, Item> Collectibles = new Dictionary<string, Item>();
    public List<string> CollectiblesList = new List<string>();
    public CollectiblesData collectiblesData;
    public int sizeDizionario;
    public int sizelista;

    private void Update()
    {
        sizeDizionario = Collectibles.Count;
        sizelista = CollectiblesList.Count;

    }
     public void Save()
    {

        SaveCollectibles.Save(CollectiblesList.ToArray());  
    }
    public void Load()
    {
        collectiblesData = SaveCollectibles.Load();
        for(int i = 0; i < collectiblesData.lista.Length; i++)
        {
            string name = collectiblesData.lista[i];
            CollectiblesList.Add(name);
        }
    }
}
