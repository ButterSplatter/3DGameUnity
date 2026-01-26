using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public Transform Player;
    public GameObject TilePrefab;
    public GameObject ObstaclePrefab;
    public GameObject CoinPrefab;
    public GameObject ShoePowerupPrefab;

    public GameObject EnemyPrefab;
    [Range(0f, 1f)]
    public float EnemySpawnChance = 0.18f;
    public float EnemySpeed = 2.5f;
    public float EnemySpawnHeight = 1f;


    public int StartTiles = 12;
    public float TileLength = 20f;

    public float LaneOffset = 2f;

    public int ObstaclesMin = 1;
    public int ObstaclesMax = 3;

    public int CoinsMin = 2;
    public int CoinsMax = 6;

    public float ShoeSpawnChance = 0.12f;

    public GameObject LowObstaclePrefab;
    [Range(0f, 1f)]
    public float LowObstacleChance = 0.4f;


    Queue<GameObject> tiles = new Queue<GameObject>();
    float spawnZ = 0f;

    void Start()
    {
        for (int i = 0; i < StartTiles; i++)
            SpawnTile(i < 2);
    }

    void Update()
    {
        if (GameManager.I != null && GameManager.I.IsGameOver) return;

        float preloadDistance = TileLength * 4f;

        while (Player.position.z + preloadDistance > spawnZ)
            SpawnTile(false);

        while (tiles.Count > StartTiles + 4)
            RemoveOldest();
    }

    void SpawnTile(bool empty)
    {
        GameObject tile = Instantiate(TilePrefab, Vector3.forward * spawnZ, Quaternion.identity);
        tiles.Enqueue(tile);

        if (!empty)
        {
            SpawnObstacles(spawnZ);
            SpawnCoins(spawnZ);
            SpawnShoe(spawnZ);
            SpawnEnemy(spawnZ);

        }

        spawnZ += TileLength;
    }
    void SpawnEnemy(float tileZ)
    {
        if (EnemyPrefab == null) return;
        if (Random.value > EnemySpawnChance) return;

        int lane = Random.Range(-1, 2);
        float localZ = Random.Range(8f, TileLength - 3f);

        Vector3 pos = new Vector3(lane * LaneOffset, EnemySpawnHeight, tileZ + localZ);
        Instantiate(EnemyPrefab, pos, Quaternion.identity);
    }


    void SpawnObstacles(float tileZ)
    {
        int count = Random.Range(ObstaclesMin, ObstaclesMax + 1);

        for (int i = 0; i < count; i++)
        {
            int lane = Random.Range(-1, 2);
            float localZ = Random.Range(4f, TileLength - 3f);

            Vector3 pos;
            GameObject prefab;

            if (Random.value < LowObstacleChance)
            {
                prefab = LowObstaclePrefab;
                pos = new Vector3(lane * LaneOffset, 0f, tileZ + localZ);
            }
            else
            {
                prefab = ObstaclePrefab;
                pos = new Vector3(lane * LaneOffset, 1f, tileZ + localZ);
            }

            GameObject o = Instantiate(prefab, pos, Quaternion.identity);
        }
    }


    void SpawnCoins(float tileZ)
    {
        int count = Random.Range(CoinsMin, CoinsMax + 1);

        for (int i = 0; i < count; i++)
        {
            int lane = Random.Range(-1, 2);
            float localZ = Random.Range(3f, TileLength - 2f);

            Vector3 pos = new Vector3(lane * LaneOffset, 1.2f, tileZ + localZ);
            GameObject c = Instantiate(CoinPrefab, pos, Quaternion.identity);
            c.tag = "Pickup";
        }
    }

    void SpawnShoe(float tileZ)
    {
        if (ShoePowerupPrefab == null) return;
        if (Random.value > ShoeSpawnChance) return;

        int lane = Random.Range(-1, 2);
        float localZ = Random.Range(4f, TileLength - 3f);

        Vector3 pos = new Vector3(lane * LaneOffset, 1.2f, tileZ + localZ);
        GameObject s = Instantiate(ShoePowerupPrefab, pos, Quaternion.identity);
        s.tag = "Powerup";
    }

    void RemoveOldest()
    {
        GameObject old = tiles.Dequeue();
        Destroy(old);
    }
}
