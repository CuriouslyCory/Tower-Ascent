using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerGenerator : MonoBehaviour
{
    
    [SerializeField] private GameObject towerRoom_1;
    [SerializeField] private Transform grid;

    public List<GameObject> floors;

    private void Awake() {
    }

    private void GenerateFloor(int floor){
        Vector3 position = new Vector3(-10,-2 + (floor * 4));
        GameObject newFloor = Instantiate(towerRoom_1, position, Quaternion.identity, grid);
        newFloor.GetComponent<TowerFloor>().floorNumber = floor + 1;
        //SpawnEnemy(GetRandomEnemyType(), floor, new Vector3(newFloor.transform.position.x + 4, newFloor.transform.position.y), newFloor.transform);
        SpawnEnemy(GetRandomEnemyType(), newFloor);
        floors.Add(newFloor);
    }

    private void GenerateBossFloor(int floor){
        Vector3 position = new Vector3(-10,-2 + (floor * 4));
        GameObject newFloor = Instantiate(towerRoom_1, position, Quaternion.identity, grid);
        newFloor.GetComponent<TowerFloor>().floorNumber = floor + 1;
        //SpawnEnemy(GetRandomEnemyType(), floor, new Vector3(newFloor.transform.position.x + 4, newFloor.transform.position.y), newFloor.transform);
        SpawnEnemy(NonPlayerCharacter.EnemyType.Boss1, newFloor);
        floors.Add(newFloor);
    }

    private NonPlayerCharacter.EnemyType GetRandomEnemyType()
    {
        Array values = Enum.GetValues(typeof(NonPlayerCharacter.EnemyType));
        Debug.Log(values.GetValue(0));
        Debug.Log(values.GetValue(1));
        Debug.Log(values.GetValue(2));
        return (NonPlayerCharacter.EnemyType)values.GetValue(UnityEngine.Random.Range(0, 2));
    }

    private void SpawnEnemy(NonPlayerCharacter.EnemyType enemyType, GameObject floor)
    {
        int floorNumber = floor.GetComponent<TowerFloor>().floorNumber;
        Vector3 spawnLoc = new Vector3(floor.transform.position.x + 4, floor.transform.position.y - 0.65f);
        Transform enemyObject = Instantiate(NonPlayerCharacter.GetPF(enemyType), spawnLoc, Quaternion.identity, floor.transform);
        NonPlayerCharacter enemy = enemyObject.GetComponent<NonPlayerCharacter>();
        
        // every 5 floors increase the level of the mobs
        int enemyLevel = (int)Math.Ceiling(floorNumber /  5.0f);
        
        // minimum level of 1
        enemyLevel = enemyLevel < 1 ? 1 : enemyLevel;
        enemy.SetEnemyLevel(enemyLevel);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 100; i++){
            GenerateFloor(i);
        }
        GenerateBossFloor(100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
