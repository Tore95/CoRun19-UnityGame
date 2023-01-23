using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveCollectibles
{
    public static void Save(string[] lista)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath +"/Collezionabili";
        FileStream stream = new FileStream(path, FileMode.Create);
        CollectiblesData data =new CollectiblesData(lista);
        bf.Serialize(stream, data);
        stream.Close();
        Debug.Log("Salvataggio automatico effettuato");

    }
    public static CollectiblesData Load()
    {
        string path = Application.persistentDataPath + "/Collezionabili";
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            CollectiblesData data = bf.Deserialize(stream) as CollectiblesData;
            stream.Close();
            Debug.Log("caricamento effettuato");

            return data;

        }
        else
        {
            Debug.Log("Save file not found in " + path);
            string[] empty = { };
            return new CollectiblesData(empty);
        }
    }
}
