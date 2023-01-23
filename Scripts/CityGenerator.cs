using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/* Questa classe instanzia un insieme di tile che hanno lo script RogueTile in modo da formare una
 * mappa rettangolare consistente con il tipo di uscite che i tile hanno (strada con strada, muro con muro ecc).
 * Si basa sull'algoritmo binary tree maze con delle euristiche nella generazione dei tile centrali per creare
 * percorsi dalla partenza all'uscita. Informazioni piu dettagliate ci sono sui singoli metodi/campi.
 * Si assume che tutti i tile utilizzati:
 * - hanno attaccato almeno uno script RogueTile;
 * - il RogueTile ha un GateTuple diverso dai suoi altri script RogueTile e dagli script degli altri tile (in base al metodo equals di GateTuple);
 * - hanno forma quadrata;
 * - c'e' un tile per ogni possibile combinazione delle uscite.
 * Vedi anche RogueTile e GateTuple.
 */
public class CityGenerator : MonoBehaviour
{
    public GameObject[] tiles;      //L'elenco dei tiles che voglio usare nella generazione
    public float tileSize = 40;          //Larghezza del singolo tile (ricorda che sono quadrati)
    public int height = 4;              //Numero di tile in altezza della mappa da generare
    public int width = 4;               //idem in larghezza

    [Range(0.0f, 1.0f)] public float crosswayParam = 0.5f;       //Indica la probabilita' che un tile che non e' sul bordo ha sia un'uscita a est
                                                                 //che una a sud, vedi GenerateMap e GenerateCentralArea per maggiori dettagli.

    public GateTuple.GateType startType;        //Indica il tipo delle uscite che deve avere il tile iniziale

    public bool generateWeapons = true;
    public bool generateEnemy = true;

    private Dictionary<GateTuple, RogueTile> _tileDataset;      //Serve per prendere al volo un tile con una certa disposizione delle uscite
    private int _numTypes;          //Numero di tipi delle uscite escluso Wall, serve per generare casualmente il tipo di uscita dei tile della mappa
    private Vector3 _endTilePosition;
    private GateTuple _finalGateTuple;
    private Transform _mapRoot;     //La transform di un oggetto vuoto che uso come root dei tile, per maneggiarli meglio nella hiearchy

    private void Awake()
    {
        GetComponent<RunManager>().enabled = false;
    }

    void Start()
    {
        //Inizializzo i campi privati, PrepareTileDataset inizializza _tileDataset
        _mapRoot = new GameObject("Map Root").transform;
        _numTypes = Enum.GetNames(typeof(GateTuple.GateType)).Length;
        _finalGateTuple = new GateTuple();
        _finalGateTuple.isFinal = true;
        _endTilePosition = Vector3.zero;
        PrepareTileDataset();

        GateTuple[,] map = GenerateMap();       //Creo una matrice di che rappresenta la mappa, cioe' l'insieme dei tile da istanziare
        InstantiateMap(map);                    //Istanzio i tile codificati nella matrice
        InstantiateFinalTile();
        if (generateEnemy)
        {
            //Debug.Log(map[0,0].south == startType);
            Vector3 startPosition;
            if (map[0, 0].south == startType)
            {
                startPosition = new Vector3(tileSize, 0, 0);
            }
            else
            {
                startPosition = new Vector3(0, 0, tileSize);
            }
            GetComponent<EnemySpawner>().SpawnEnemiesInMap(_mapRoot, startPosition, _endTilePosition, GetComponent<DropAssigner>().SetDrop);
        }
        if (generateWeapons)
        {
            GetComponent<WeaponSpawner>().SpawnWeaponInMap(_mapRoot);
        }
        GetComponent<RunManager>().enabled = true;
    }

    /*Istanzia e popola _tileDataset con i GameObject dentro tiles[],
     * di ogni GameObject prendo i suoi RogueTile nel dictionary associo a ogni
     * GateTuple il RogueTile che lo contiene, cosi a partire dal GateTuple posso
     * accedere sia al RogueTile sia al GameObject a esso associato in tempo costante.
     */
    private void PrepareTileDataset()
    {
        _tileDataset = new Dictionary<GateTuple, RogueTile>();
        foreach (GameObject go in tiles)
        {
            RogueTile[] comps = go.GetComponents<RogueTile>();  //Questo prende tutti i RogueTile di un GameObject, perche' puo' averne piu' d'uno
            foreach (RogueTile rTile in comps)
            {
                _tileDataset.Add(rTile.gates, rTile);
            }
        }
    }

    /* Crea una rappresentazione della mappa da generare sottoforma di matrice di GateTuple, dove ogni GateTuple e'
     * un tile da istanziare, parte da tile tutti senza uscite e crea dei passaggi aggiungendo a ogni tile un percorso
     * o al tile a destra o a quello in basso o a entrambi. Il tipo di percorso (strada, marciapiede) e' casuale a parte
     * per il tile iniziale, mentre la direzione del percorso (destra, basso o entrambi) dipende dalla posizione del tile
     * (sul bordo o centrale). Genera anche un eventuale percorso d'uscita.
     */
    private GateTuple[,] GenerateMap()
    {
        //Inizializza con tutti GateTuple senza uscita
        GateTuple[,] resultMap = new GateTuple[height,width];
        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                resultMap[i,j] = new GateTuple();
            }
        }

        //Aggiungono percorsi nelle varie sezioni della mappa
        GenerateStart(resultMap);
        GenerateSouthernBorder(resultMap);
        GenerateEasternBorder(resultMap);
        GenerateNorthernBorder(resultMap);
        GenerateWesternBorder(resultMap);
        GenerateCentralArea(resultMap);

        //Genera l'uscita
        int iEnd, jEnd;

        if (Random.value < 0.5f)
        {
            //Sceglie a caso un tile e un tipo di percorso sul lato inferiore
            iEnd = height - 1;
            jEnd = Random.Range(0, width);
            GateTuple.GateType gateType = (GateTuple.GateType)Random.Range(1, _numTypes);
            resultMap[iEnd,jEnd].south = gateType;
            _finalGateTuple.north = gateType;
        }
        else
        {
            //Come l'if ma sul lato destro
            iEnd = Random.Range(0, height);
            jEnd = width - 1;
            GateTuple.GateType gateType = (GateTuple.GateType)Random.Range(1, _numTypes);
            resultMap[iEnd,jEnd].east = gateType;
            _finalGateTuple.west = gateType;
        }

        _endTilePosition.x = iEnd * tileSize;
        _endTilePosition.z = jEnd * tileSize;

        return resultMap;
    }

    //Marca il tile in posizione [0,0] come iniziale e crea un percorso o a destra o in basso del tipo startType
    private void GenerateStart(GateTuple[,] map)
    {
        map[0, 0].isStart = true;
        if (Random.value < 0.5f)
        {
            //Crea percorso in basso
            map[0, 0].south = startType;
            map[1, 0].north = startType;
        }
        else
        {
            //Crea percorso a destra
            map[0, 0].east = startType;
            map[0, 1].west = startType;
        }
    }

    //Crea un percorso a destra in ogni tile del lato inferiore della mappa
    private void GenerateSouthernBorder(GateTuple[,] map)
    {
        for (int j = 0; j < width - 1; j++)
        {
            AddRandomPathRightward(map, height - 1, j);
        }
    }

    //Analogo a GenerateSouthernBorder ma sul lato destro
    private void GenerateEasternBorder(GateTuple[,] map)
    {
        for (int i = 0; i < height - 1; i++)
        {
            AddRandomPathDownward(map, i, width - 1);
        }
    }

    //Crea un percorso scegliendo casualmente se crearlo in basso o a destra (ma non entrambi)
    //sul lato superiore della mappa (esclusi gli estremi, gia' gestiti dai metodi sopra)
    private void GenerateNorthernBorder(GateTuple[,] map)
    {
        for (int j = 1; j < width - 1; j++)
        {
            if (Random.value < 0.5f)
            {
                AddRandomPathDownward(map, 0, j);
            }
            else
            {
                AddRandomPathRightward(map, 0, j);
            }
        }
    }

    //Analogo a GenerateNortherBorder, ma sul lato sinistro
    private void GenerateWesternBorder(GateTuple[,] map)
    {
        for (int i = 1; i < height - 1; i++)
        {
            if (Random.value < 0.5f)
            {
                AddRandomPathDownward(map, i, 0);
            }
            else
            {
                AddRandomPathRightward(map, i, 0);
            }
        }
    }

    /* Crea un percorso per i tile che non si trovano su nessuno dei bordi della mappa, gia' gestiti
     * dai metodi sopra, il percorso puo' essere creato o in basso o a destra o entrambi, una volta
     * scelto casualmente se generare un percorso in una delle due direzioni (con probabilita 0.5),
     * la probabilita' che venga generato un percorso anche nell'altra direzione rimasta e' pari al
     * campo crosswayParam, valori alti creano piu' incroci e interconnessioni, valori bassi creano
     * una struttura simile a un labirinto, in ogni caso esistera' sempre un percorso tra due tile 
     * qualunque.
     */
    private void GenerateCentralArea(GateTuple[,] map)
    {
        for(int i = 1; i < height - 1; i++)
            for(int j = 1; j < width - 1; j++)
            {
                if (Random.value < 0.5f)
                {
                    AddRandomPathDownward(map, i, j);
                    if(Random.value < crosswayParam)
                    {
                        AddRandomPathRightward(map, i, j);
                    }
                }
                else
                {
                    AddRandomPathRightward(map, i, j);
                    if(Random.value < crosswayParam)
                    {
                        AddRandomPathDownward(map, i, j);
                    }
                }
            }
    }
    
    //Modifica il GateTuple in posizione [i,j] e quello piu' in basso in posizione
    //[i,j+1] in modo da creare un percorso tra i due di un tipo casuale diverso da Wall
    private void AddRandomPathDownward(GateTuple[,] map, int i, int j)
    {
        GateTuple.GateType gateType = (GateTuple.GateType)Random.Range(1, _numTypes);
        map[i, j].south = gateType;
        map[i + 1, j].north = gateType;
    }

    //Come AddRandomPathDownward ma tra il GateTuple [i,j] e quello a destra [i+1,j]
    private void AddRandomPathRightward(GateTuple[,] map, int i, int j)
    {
        GateTuple.GateType gateType = (GateTuple.GateType)Random.Range(1, _numTypes);
        map[i, j].east = gateType;
        map[i, j + 1].west = gateType;
    }

    /* Istanzia i tile corrispondenti ai GateTuple presenti nella mappa, per farlo cerca nel _tileDataset
     * i RogueTile corrispondenti ai GateTuple e da esso prende il GameObject prefab a cui e' attaccato
     * e l'angolo con il quale va ruotato. La posizione viene calcolata con tileSize e tutti a tutti i tile
     * viene assegnato _mapRoot come parent.
     */
    private void InstantiateMap(GateTuple[,] map)
    {
        for (int i = 0; i < height; i++)
            for (int j = 0; j < width; j++)
            {
                _tileDataset.TryGetValue(map[i, j], out RogueTile prefabComp);
                if (prefabComp != null)
                {
                    GameObject newTile = Instantiate(prefabComp.gameObject,
                        new Vector3(i * tileSize, 0, j * tileSize),
                        Quaternion.Euler(0, prefabComp.angle, 0));
                    newTile.transform.SetParent(_mapRoot);
                }
                
            }
    }

    private void InstantiateFinalTile()
    {
        float finalX;
        float finalZ;
        float offset = 20f;
        if(_finalGateTuple.north == GateTuple.GateType.Wall)
        {
            finalX = _endTilePosition.x;
            finalZ = _endTilePosition.z + tileSize + offset;
        }
        else
        {
            finalX = _endTilePosition.x + tileSize + offset;
            finalZ = _endTilePosition.z;
        }
        _tileDataset.TryGetValue(_finalGateTuple, out RogueTile prefabComp);
        if (prefabComp != null)
        {
            GameObject newTile = Instantiate(prefabComp.gameObject,
                new Vector3(finalX, 0, finalZ),
                Quaternion.Euler(0, prefabComp.angle, 0));
            newTile.transform.SetParent(_mapRoot);
        }
    }

    // Per ora e' vuoto, l'ho lasciato in caso serve in futuro
    void Update()
    {
        
    }
}
