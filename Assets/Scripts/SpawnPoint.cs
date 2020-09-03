using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public int spawnWeight;

    void Awake()
    {
        if (tag == "RoomSpawnTop")
        {
            spawnWeight = 4;
        }
        if (tag == "RoomSpawnRight")
        {
            spawnWeight = 3;
        }
        if (tag == "RoomSpawnBottom")
        {
            spawnWeight = 2;
        }
        if (tag == "RoomSpawnLeft")
        {
            spawnWeight = 1;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        SpawnPoint colSpawn = collision.gameObject.GetComponent<SpawnPoint>();
        Debug.Log("Spawn weight: " + spawnWeight);

        if (!collision.gameObject.CompareTag("Cell"))
        {
            if (colSpawn.spawnWeight < spawnWeight)
            {
                Debug.Log("Removing spawn point due to collision with spawn point.");
                Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Cell"))
        {
            Debug.Log("Removing spawn point due to collision with cell.");
            Destroy(this.gameObject);
        }
    }
}
