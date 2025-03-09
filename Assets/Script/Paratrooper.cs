using UnityEngine;

public class Paratrooper : MonoBehaviour
{
    public float fallSpeed = 2f;
    public float moveSpeed = 1.5f;

    [SerializeField] private float stackSpacing = 0.6f;
    [SerializeField] private GameObject explosionPrefab;

    [SerializeField] private AudioClip spawnSound;
    [SerializeField] private AudioClip walkingSound;
    [SerializeField] private AudioClip deathSound;

    private Transform baseTarget;
    private bool hasLanded = false;
    private bool isStacked = false;
    private Vector3 stackPosition;

    public static int leftStackCount = 0;
    public static int rightStackCount = 0;
    public int maxStackLimit = 4;

    private bool movingLeft;
    private Transform leftStackStart;
    private Transform rightStackStart;

    private static bool isGameRestarted = false;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (spawnSound != null)
        {
            audioSource.PlayOneShot(spawnSound);
        }

        if (!isGameRestarted)
        {
            leftStackCount = 0;
            rightStackCount = 0;
            isGameRestarted = true;
        }

        GameObject baseObject = GameObject.FindGameObjectWithTag("Base");
        if (baseObject != null)
        {
            baseTarget = baseObject.transform;
            movingLeft = transform.position.x < baseTarget.position.x;

            leftStackStart = baseTarget.transform.Find("LeftStackStart");
            rightStackStart = baseTarget.transform.Find("RightStackStart");

            if (leftStackStart == null || rightStackStart == null)
            {
                Debug.LogError("LeftStackStart or RightStackStart not found under Base!");
            }
        }
        else
        {
            Debug.LogError("Base not found! Make sure an object has the 'Base' tag.");
        }
    }

    void Update()
    {
        if (!hasLanded)
        {
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
        }
        else if (!isStacked && baseTarget != null)
        {
            Vector2 targetPosition = new Vector2(baseTarget.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (walkingSound != null && !audioSource.isPlaying)
            {
                audioSource.clip = walkingSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            hasLanded = true;
        }
        else if (collision.gameObject.CompareTag("Base") && !isStacked)
        {
            StickToBase();
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            DestroyParatrooper();
        }
    }

    void StickToBase()
    {
        if (!isStacked)
        {
            isStacked = true;

            if (audioSource.isPlaying && audioSource.clip == walkingSound)
            {
                audioSource.Stop();
            }

            if (movingLeft && leftStackStart != null)
            {
                stackPosition = new Vector3(leftStackStart.position.x,
                                            leftStackStart.position.y + (leftStackCount * stackSpacing),
                                            0);
                transform.position = stackPosition;
                leftStackCount++;

                if (leftStackCount >= maxStackLimit)
                {
                    GameOver();
                }
            }
            else if (!movingLeft && rightStackStart != null)
            {
                stackPosition = new Vector3(rightStackStart.position.x,
                                            rightStackStart.position.y + (rightStackCount * stackSpacing),
                                            0);
                transform.position = stackPosition;
                rightStackCount++;

                if (rightStackCount >= maxStackLimit)
                {
                    GameOver();
                }
            }
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over! Too many paratroopers stacked on one side.");
        UIManager.Instance.GameOver();
        Time.timeScale = 0;
        isGameRestarted = false;
    }

    public static void ClearStack()
    {
        leftStackCount = 0;
        rightStackCount = 0;
    }

    public void DestroyParatrooper()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        Destroy(gameObject);
    }
}
