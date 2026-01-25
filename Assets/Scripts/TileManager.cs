using UnityEngine;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    public Transform Player;
    public GameObject TilePrefab;
    public int StartTiles = 5;
    public float TileLength = 20f;

    Queue<GameObject> tiles = new Queue<GameObject>();
    float spawnZ = 0f;

    void Start()
    {
        for (int i = 0; i < StartTiles; i++)
            SpawnTile();
    }

    void Update()
    {
        float preloadDistance = TileLength * 3f;

        while (Player.position.z + preloadDistance > spawnZ)
            SpawnTile();

        while (tiles.Count > StartTiles + 4)
            RemoveOldest();
    }


    void SpawnTile()
    {
        GameObject tile = Instantiate(TilePrefab, Vector3.forward * spawnZ, Quaternion.identity);
        tiles.Enqueue(tile);
        spawnZ += TileLength;
    }

    void RemoveOldest()
    {
        GameObject old = tiles.Dequeue();
        Destroy(old);
    }
}
