using UnityEngine;

public class HelicopterSpawner : MonoBehaviour
{
    public GameObject helicopterPrefab;
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;
    public Transform basePosition;
    public float spawnInterval = 5f;

    void Start()
    {
        InvokeRepeating("SpawnHelicopter", 2f, spawnInterval);
    }

    void SpawnHelicopter()
    {
        bool spawnFromLeft = Random.value > 0.5f;
        Transform spawnPoint = spawnFromLeft ? leftSpawnPoint : rightSpawnPoint;

        GameObject helicopter = Instantiate(helicopterPrefab, spawnPoint.position, Quaternion.identity);
        Helicopter heliScript = helicopter.GetComponent<Helicopter>();

        if (heliScript != null)
        {
            int direction = spawnFromLeft ? 1 : -1;
            heliScript.SetDirection(direction, basePosition.position);
        }
    }
}
