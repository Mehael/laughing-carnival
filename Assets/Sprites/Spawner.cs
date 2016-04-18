using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
    public GameObject[] possiblePrefabs;
    public float spawnCooldown = 1f;
    public bool isSetScale = false;
    public float delayToStartSpawning = 1f;
    public float random = 0f;

    void Start()
    {
        InvokeRepeating("Spawn", delayToStartSpawning, spawnCooldown);
    }

    void Spawn ()
    {
        int enemyIndex = Random.Range(0, possiblePrefabs.Length);
        Vector3 rVector = new Vector3(Random.Range(0, random), Random.Range(0, random), Random.Range(0, random));
        var go = (GameObject) Instantiate(possiblePrefabs[enemyIndex], transform.position + rVector, Quaternion.identity);
        if (isSetScale)
            go.transform.localScale = transform.localScale;
    }
}
