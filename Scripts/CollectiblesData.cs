using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CollectiblesData
{
    public string[] lista;
    public CollectiblesData(string[] collectiblesArray)
    {

        this.lista = new string[collectiblesArray.Length];
        for(int i = 0; i <collectiblesArray.Length ; i++)
        {
            this.lista[i] = collectiblesArray[i];
        }
    }
}
