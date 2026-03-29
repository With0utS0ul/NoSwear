using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //[SerializeField] private float spawnInterval;
    //private float currentSpawnTimer
    //currentSpawnTimer+=Time.deltaTime;
    //if(currentSpawnTimer >= spawnInterval)
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    [SerializeField] private float height;
    



    // Update is called once per frame
    void Start()
    {
        
        var enemyInstance = Instantiate(enemyPrefab);
        var newPosition = GenerateStartPosition();
        enemyInstance.transform.position = newPosition;
        
        
    }
    private Vector3 GenerateStartPosition()
    {
        var startPos = new Vector3(Random.Range(minX, maxX), height, Random.Range(minY, maxY));
        return startPos;
    }
}
