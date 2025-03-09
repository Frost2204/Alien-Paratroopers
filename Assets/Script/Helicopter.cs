using UnityEngine;
using System.Collections;

public class Helicopter : MonoBehaviour
{
    public float speed = 2f;
    public float ascentSpeed = 3f;
    private int direction = 1;
    private bool ascending = false;
    public float stopDistance = 3f;
    private Vector3 basePosition;

    public GameObject paratrooperPrefab;
    public Transform dropPoint;
    public float dropRandomness = 0.5f;

    private bool hasDropped = false;
    private float dropDelay;
    private float timer = 0f;
    private bool isPaused = false;

    [Header("Drop Timing Settings")]
    public float minDropTime = 0f;
    public float maxDropTime = 2f;

    public float pauseTime = 2f;
    void Start()
    {
        dropDelay = Random.Range(minDropTime, maxDropTime);
    }

    public void SetDirection(int dir, Vector3 basePos)
    {
        direction = dir;
        basePosition = basePos;
        transform.localScale = new Vector3(dir == 1 ? -1 : 1, 1, 1);
    }

    void Update()
    {
        if (!ascending && !isPaused)
        {
            transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

            timer += Time.deltaTime;

            if (!hasDropped && timer >= dropDelay)
            {
                StartCoroutine(DropSequence());
                hasDropped = true;
            }

            if (Mathf.Abs(transform.position.x - basePosition.x) < stopDistance)
            {
                ascending = true;
            }
        }
        else if (ascending)
        {
            transform.Translate(Vector2.up * ascentSpeed * Time.deltaTime);
            if (transform.position.y > 10)
            {
                Destroy(gameObject);
            }
        }
    }

    IEnumerator DropSequence()
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseTime); 

        DropParatrooper();

        yield return new WaitForSeconds(1f);

        isPaused = false;
    }

    void DropParatrooper()
    {
        if (paratrooperPrefab && dropPoint)
        {
            Vector3 randomDropPosition = dropPoint.position + new Vector3(Random.Range(-dropRandomness, dropRandomness), 0, 0);
            Instantiate(paratrooperPrefab, randomDropPosition, Quaternion.identity);
        }
    }
}
