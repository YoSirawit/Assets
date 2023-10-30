using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    Vector3 noTerrainPosition;
    public LayerMask terrainMask;
    PlayerController pm;
    public GameObject currentChunk;
    [Header("Optimiztion")]
    public List<GameObject> spawnChunks;
    public GameObject latestChunk;
    public float maxOpDist;// Must be greater that the length and width of tilemap 
    float OpDist;
    public float optimizerCooldownDur;
    float optimizerCooldown;
    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }

    void ChunkChecker()
    {
        if (!currentChunk)
        {
            return;
        }
        if(pm.moveVector.x > 0 && pm.moveVector.y == 0) // check if player is moving to right side
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right").position;
                SpawnChunk();
            }
        }
        if(pm.moveVector.x < 0 && pm.moveVector.y == 0)// Check if player is moving to left side
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left").position;
                SpawnChunk();
            }
        }
        else if(pm.moveVector.y > 0 && pm.moveVector.x == 0)// Check if player is moving upward
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Up").position;
                SpawnChunk();
            }
        }
        else if(pm.moveVector.y < 0 && pm.moveVector.x == 0)// Check if player is moving down
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Down").position;
                SpawnChunk();
            }
        }
        else if(pm.moveVector.y > 0 && pm.moveVector.x > 0)// Check if player is moving upright
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("UpRight").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("UpRight").position;
                SpawnChunk();
            }
        }
        else if(pm.moveVector.y > 0 && pm.moveVector.x < 0)// Check if player is moving upleft
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("UpLeft").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("UpLeft").position;
                SpawnChunk();
            }
        }
        else if(pm.moveVector.y < 0 && pm.moveVector.x > 0)// Check if player is moving downright
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("DownRight").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("DownRight").position;
                SpawnChunk();
            }
        }
        else if(pm.moveVector.y < 0 && pm.moveVector.x < 0)// Check if player is moving downleft
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("DownLeft").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("DownLeft").position;
                SpawnChunk();
            }
        }
    }

    void SpawnChunk()
    {
        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], noTerrainPosition, Quaternion.identity);
        spawnChunks.Add(latestChunk);
    }

    void ChunkOptimizer()
    {
        optimizerCooldown = Time.deltaTime;

        if(optimizerCooldown <= 0f)
        {
            optimizerCooldown = optimizerCooldownDur;
        }
        else
        {
            foreach (GameObject chunk in spawnChunks)
            {
                OpDist = Vector3.Distance(player.transform.position, chunk.transform.position);
                if(OpDist > maxOpDist)
                {
                    chunk.SetActive(false);
                }
                else
                {
                    chunk.SetActive(true);
                }
            }
        }

        
    }
}
