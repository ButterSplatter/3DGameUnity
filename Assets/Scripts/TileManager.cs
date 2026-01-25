using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public Transform Player;
    public GameObject TilePrefab;
    public GameObject ObstaclePrefab;

    public int StartTiles = 12;
    public float TileLength = 20f;

    public float LaneOffset = 2f;

    public int ObstaclesMin = 1;
    public int ObstaclesMax = 3;

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
            SpawnObstacles(spawnZ);

        spawnZ += TileLength;
    }

    void SpawnObstacles(float tileZ)
    {
        int count = Random.Range(ObstaclesMin, ObstaclesMax + 1);

        for (int i = 0; i < count; i++)
        {
            int lane = Random.Range(-1, 2);
            float localZ = Random.Range(4f, TileLength - 3f);

            Vector3 pos = new Vector3(lane * LaneOffset, 1f, tileZ + localZ);
            GameObject o = Instantiate(ObstaclePrefab, pos, Quaternion.identity);
            o.tag = "Obstacle";
        }
    }

    void RemoveOldest()
    {
        GameObject old = tiles.Dequeue();
        Destroy(old);
    }
}
