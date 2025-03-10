using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject[] chunkPrefabs;
    [SerializeField] Transform chunkParent;
    [SerializeField] GameObject checkpointChunkPrefab;

    
    [Header("Level Settings")]
    [Tooltip("The amount of chunks that will be generated at the start")]
    [SerializeField] int chunkAmount = 2;
    [SerializeField] int checkpointChunkInterval = 8;
    [Tooltip("Do not change chunk length unless chunk size reflects changes")]
    [SerializeField] float chunkLength = 200;
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float minMoveSpeed = 2f;
    [SerializeField] float maxMoveSpeed = 15f;
    
    List<Chunk> chunks = new List<Chunk>();
    int chunkSpawned = 0;
    GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        SpawnStartingChunks();
    }

    void Update()
    {
        MoveChunks();
    }
    
    void SpawnStartingChunks()
    {
        for (int i = 0; i < chunkAmount; i++)
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        var spawnPositionX = CalculateSpawnPositionX();

        Vector2 chunkSpawnPosition = new Vector2(CalculateSpawnPositionX(), transform.position.y);
        
        GameObject chunkPrefab = ChooseChunk();

        Chunk newChunk = Instantiate(chunkPrefab, chunkSpawnPosition, Quaternion.identity, chunkParent).GetComponent<Chunk>();
            
        chunks.Add(newChunk);
        newChunk.Init(this);
        
        chunkSpawned++;
    }

    private GameObject ChooseChunk()
    {
        GameObject chunkPrefab;

        if (gameManager.GetGameMode().Equals("Time Trial") 
            && chunkSpawned % checkpointChunkInterval == 0 
            && chunkSpawned != 0)
        {
            chunkPrefab = checkpointChunkPrefab;
        }
        else
        {
            chunkPrefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
        }

        return chunkPrefab;
    }

    float CalculateSpawnPositionX()
    {
        float spawnPositionX;

        if (chunks.Count == 0)
        {
            spawnPositionX = transform.position.x;
        }
        else
        {
            spawnPositionX = chunks[chunks.Count - 1].transform.position.x + chunkLength;
        }

        return spawnPositionX;
    }

    void MoveChunks()
    {
        for (int i = 0; i < chunks.Count; i++)
        {
            Chunk chunk = chunks[i];
            chunk.transform.Translate(Vector3.left * (moveSpeed * Time.deltaTime));

            if (chunk.transform.position.x <= Camera.main.transform.position.x - chunkLength)
            {
                chunks.Remove(chunk);
                Destroy(chunk.gameObject);
                SpawnChunk();
            }
        }
    }
    
    // Change move speed to increase difficulty
    public void ChangeChunkMoveSpeed(float speedAmount)
    {
        float newMoveSpeed = moveSpeed + speedAmount;
        newMoveSpeed = Mathf.Clamp(newMoveSpeed, minMoveSpeed, maxMoveSpeed);
        
        if (!Mathf.Approximately(newMoveSpeed, moveSpeed))
        {
            moveSpeed = newMoveSpeed;
        }
    }
}
