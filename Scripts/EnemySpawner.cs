using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int minEnemies = 2;
    public int maxEnemies = 10;
    public float spawnAreaSize = 4f;
    public float spawnHeight = 0.1f;

    public void SpawnEnemiesInMap(Transform mapRoot, Vector3 startPosition, Vector3 endPosition, Action<GameObject> postInstantiate)
    {
        float distStartToEnd = Vector3.Distance(startPosition, endPosition);
        for (int i = 1; i < (mapRoot.childCount - 1); i++)
        {
            Transform tileTransform = mapRoot.GetChild(i);
            float distTileToStart = Vector3.Distance(startPosition, tileTransform.position);
            float distTileToEnd = Vector3.Distance(endPosition, tileTransform.position);
            int numEnemies = CalculateNumEnemies(distTileToStart, distTileToEnd, distStartToEnd);
            SpawnEnemiesInTile(numEnemies, tileTransform.gameObject.GetComponent<TileUtils>().EnemySpawnPoint,postInstantiate);
        }
    }

    private int CalculateNumEnemies(float distTileToStart, float distTileToEnd, float distStartToEnd)
    {
        float relativeDist = distTileToStart - distTileToEnd + distStartToEnd;
        float interpolVal = relativeDist / (2 * distStartToEnd);
        return Mathf.FloorToInt(Mathf.Lerp(minEnemies, maxEnemies, interpolVal));
    }

    public void SpawnEnemiesInTile(int numEnemies, Transform enemySpawnTransform, Action<GameObject> postInstantiate)
    {
        //Debug.Log(numEnemies);
        for (int i = 0; i < numEnemies; i++)
        {
            float randomX = Random.Range(-spawnAreaSize / 2, spawnAreaSize / 2);
            float randomZ = Random.Range(-spawnAreaSize / 2, spawnAreaSize / 2);
            float randomAngle = Random.Range(0, 180f);
            GameObject instance = Instantiate(enemyPrefab,
                new Vector3(enemySpawnTransform.position.x + randomX,
                enemySpawnTransform.position.y + spawnHeight,
                enemySpawnTransform.position.z + randomZ),
                Quaternion.Euler(0, randomAngle, 0));
            //Inserire configurazione aggiuntiva per la AI
            instance.GetComponentInChildren<SkinnedMeshRenderer>().gameObject.SetActive(false);
            Renderer[] meshes = instance.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            int randomMesh = Random.Range(0, meshes.Length);
            meshes[randomMesh].gameObject.SetActive(true);
            instance.transform.SetParent(enemySpawnTransform);
            postInstantiate(instance);
        }
    }

    public void Dummy(GameObject go)
    {
        //Stub
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
