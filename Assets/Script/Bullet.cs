using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public GameObject explosionPrefab;

    public AudioClip helicopterDestroySound;
    public AudioClip paratrooperDestroySound;

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 spawnPosition = collision.transform.position;

        if (collision.CompareTag("Helicopter"))
        {
            PlaySoundAtPosition(helicopterDestroySound, spawnPosition);
            InstantiateExplosion(spawnPosition);
            Destroy(collision.gameObject);
            UIManager.Instance.AddScore(10);
            Destroy(gameObject, 0.1f);
        }
        else if (collision.CompareTag("Paratrooper"))
        {
            PlaySoundAtPosition(paratrooperDestroySound, spawnPosition);
            InstantiateExplosion(spawnPosition);
            Destroy(collision.gameObject);
            UIManager.Instance.AddScore(20);
            Destroy(gameObject, 0.1f);
        }
    }

    void PlaySoundAtPosition(AudioClip clip, Vector3 position)
    {
        if (clip != null)
        {
            GameObject audioObject = new GameObject("TempAudio");
            audioObject.transform.position = position;
            AudioSource tempSource = audioObject.AddComponent<AudioSource>();
            tempSource.clip = clip;
            tempSource.spatialBlend = 1f;
            tempSource.Play();
            Destroy(audioObject, clip.length);
        }
    }

    void InstantiateExplosion(Vector3 position)
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, position, Quaternion.identity);
        }
    }
}
