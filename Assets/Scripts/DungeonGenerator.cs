using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class DungeonGenerator : MonoBehaviour
{
    public int roomLimit = 5;
    public int numberOfRoomsSpawned = 0;

    public bool canSpawnRooms = true;
    public bool doneSpawning = false;

    public float enemySpawnChance = 0.5f;
    public bool canSpawnEnemies = true;

    public GameObject enemy;

    private float deadEndProb = 0.1f;

    public GameObject[] cells = new GameObject[5];
    //0 corner
    //1 dead end
    //2 fourway
    //3 straight
    //4 threeway

    //refers to the entrance location
    //0-1 corner
    //2 dead end
    //3 fourway
    //4 straight
    //5-7 threeway
    public GameObject[] bottomCells = new GameObject[8];
    public GameObject[] topCells = new GameObject[8];
    public GameObject[] leftCells = new GameObject[8];
    public GameObject[] rightCells = new GameObject[8];

    private void Awake()
    {
        
    }

    void Start()
    {
        if (canSpawnRooms)
        {
            GameObject initialCell = SpawnCell(0);
            Instantiate(initialCell, Vector3.zero, initialCell.transform.rotation);
        }
    }

    private void SpawnEnemies()
    {
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Cell");

        foreach (GameObject room in rooms)
        {
            //if this is the initial room
            if (room.transform.position == Vector3.zero)
            {
                continue;
            }
            else
            {
                //set the spawn above the ground
                Vector3 spawnOffset = new Vector3(room.transform.position.x, room.transform.position.y + 2f, room.transform.position.z);

                float enemyRoll = Random.Range(0f, 1f);
                if (enemyRoll <= enemySpawnChance)
                {
                    Instantiate(enemy, spawnOffset, Quaternion.identity);
                }
            }
        }

        canSpawnEnemies = false;
    }
    
    private void FixedUpdate()
    {
        //if(Mouse.current.leftButton.isPressed)
        
        //cleaning up the open spaces (size control)
        if (doneSpawning)
        {
            CloseOpenings();

            if(canSpawnEnemies)
                SpawnEnemies();
        }
        else
        {
            SpawnCells();
        }
    }

    private void CloseOpenings()
    {
        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();

        foreach (SpawnPoint spawnPoint in spawnPoints)
        {

            if (spawnPoint.CompareTag("RoomSpawnTop"))
            {
                //bottom-entrance dead end
                GameObject deadendCell = SpawnCell(2);
                Instantiate(deadendCell, spawnPoint.transform.position, deadendCell.transform.rotation);
            }
            else if (spawnPoint.CompareTag("RoomSpawnBottom"))
            {
                //top-entrance deadend
                GameObject deadendCell = SpawnCell(0);
                Instantiate(deadendCell, spawnPoint.transform.position, deadendCell.transform.rotation);
            }
            else if (spawnPoint.CompareTag("RoomSpawnRight"))
            {
                //left-entrance deadend
                GameObject deadendCell = SpawnCell(3);
                Instantiate(deadendCell, spawnPoint.transform.position, deadendCell.transform.rotation);
            }
            else if (spawnPoint.CompareTag("RoomSpawnLeft"))
            {
                //right-entrance dead end
                GameObject deadendCell = SpawnCell(1);
                Instantiate(deadendCell, spawnPoint.transform.position, deadendCell.transform.rotation);
            }

            numberOfRoomsSpawned++;


        }
    }

    //randomly spawncells based on the direction of their entrances
    private void SpawnCells()
    {
        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();

        if (numberOfRoomsSpawned < roomLimit && !doneSpawning)
        {
            if (spawnPoints.Length == 0)
            {
                doneSpawning = true;
            }

            foreach (SpawnPoint spawnPoint in spawnPoints)
            {

                //if the room limit has not been reached, spawn a new room
                if (canSpawnRooms)
                {
                    if (spawnPoint.CompareTag("RoomSpawnTop"))
                    {
                        //random bottom-entrance room
                        GameObject randomCell = RandomCell(2);
                        Instantiate(randomCell, spawnPoint.transform.position, randomCell.transform.rotation);
                    }
                    else if (spawnPoint.CompareTag("RoomSpawnBottom"))
                    {
                        //random top-entrance room
                        GameObject randomCell = RandomCell(0);
                        Instantiate(randomCell, spawnPoint.transform.position, randomCell.transform.rotation);
                    }
                    else if (spawnPoint.CompareTag("RoomSpawnRight"))
                    {
                        //random left-entrance room
                        GameObject randomCell = RandomCell(3);
                        Instantiate(randomCell, spawnPoint.transform.position, randomCell.transform.rotation);
                    }
                    else if (spawnPoint.CompareTag("RoomSpawnLeft"))
                    {
                        //random right-entrance room
                        GameObject randomCell = RandomCell(1);
                        Instantiate(randomCell, spawnPoint.transform.position, randomCell.transform.rotation);
                    }

                    numberOfRoomsSpawned++;
                    if(numberOfRoomsSpawned >= roomLimit)
                    {
                        doneSpawning = true;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            canSpawnRooms = false;
        }
    }

    //0, top; 1, right; 2, bottom; 3, left
    public GameObject RandomCell(int direction)
    {
        GameObject[] refList;
        switch (direction)
        {
            //top
            case 0:
                refList = topCells;
                break;
            //right
            case 1:
                refList = rightCells;
                break;
            //bottom
            case 2:
                refList = bottomCells;
                break;
            //left
            case 3:
                refList = leftCells;
                break;
            default:
                Debug.LogError("Error incorrect direction");
                return null;
        }

        //generate a random number for the cell template, reference declaration
        while (true)
        {
            int randomCell = Random.Range(0, cells.Length);

            switch (randomCell)
            {
                case 0:
                    //generate a number for the corners
                    return refList[Random.Range(0, 2)];
                case 1:
                    //generate a number for the corners
                    return refList[Random.Range(5, 8)];
                case 2:
                    float deadEndRoll = Random.Range(0f, 1f);
                    if (deadEndRoll <= deadEndProb)
                        return refList[2];
                    else
                        Debug.Log("Choosing room again...");
                        break;
                case 3:
                    return refList[3];
                case 4:
                    return refList[4];
                default:
                    Debug.Log("Error generating random cell");
                    return null;
            }
        }
    }

    //spawn deadend cells
    private GameObject SpawnCell(int direction)
    {
        GameObject[] refList;
        switch (direction)
        {
            //top
            case 0:
                refList = topCells;
                break;
            //right
            case 1:
                refList = rightCells;
                break;
            //bottom
            case 2:
                refList = bottomCells;
                break;
            //left
            case 3:
                refList = leftCells;
                break;
            default:
                Debug.LogError("Error incorrect direction");
                return null;
        }

        return refList[2];
    }

    public void SetNumberOfRoomsSpawned(int numberOfRoomsSpawned)
    {
        this.numberOfRoomsSpawned = numberOfRoomsSpawned;
    }

    public int GetNumberOfRoomsSpawned()
    {
        return numberOfRoomsSpawned;
    }

    public void SetCanSpawnRooms(bool canSpawnRooms)
    {
        this.canSpawnRooms = canSpawnRooms;
    }

    public bool GetCanSpawnRooms()
    {
        return canSpawnRooms;
    }
}
