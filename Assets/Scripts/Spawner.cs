using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int numberToSpawn;
    public float spawnTimer;
    public Node spawnLocation;
    public List<GameObject> spawnPool;
    private int numberSpawned;

    void Start()
    {
        spawnPool[0].GetComponent<Virus>().virusType = Virus.VirusType.Virus1;
        spawnPool[0].GetComponent<Virus>().isInBase = true;
        Instantiate(spawnPool[0], spawnLocation.transform.position, spawnPool[0].transform.rotation);
        numberSpawned = 1;
    }

    void Update()
    {
        SpawnVirus();
    }

    public void spawnObjects()
    {
        int randomItem = 0;
        GameObject toSpawn;

        for (int i = 1; i < numberToSpawn; i++)
        {
            randomItem = Random.Range(0, spawnPool.Count);
            toSpawn = spawnPool[randomItem];

            if (spawnTimer < 0)
            {
                Instantiate(toSpawn, spawnLocation.transform.position, toSpawn.transform.rotation);
                spawnTimer = 5;
            }
            numberSpawned = i;

            if (numberSpawned == numberToSpawn) break;
        }

    }

    void SpawnVirus()
    {
        if (numberSpawned < numberToSpawn)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer < 0)
            {
                spawnPool[0].GetComponent<Virus>().virusType = (Virus.VirusType)numberSpawned;
                spawnPool[0].GetComponent<Virus>().isInBase = true;
                Instantiate(spawnPool[0], spawnLocation.transform.position, spawnPool[0].transform.rotation);
                spawnTimer = 5;
                numberSpawned++;
            }
        }
    }

    private void destroyObjects()
    {
        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Spawnable"))
        {
            Destroy(o);
        }
    }
}
