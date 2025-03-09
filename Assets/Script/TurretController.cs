using UnityEngine;

public class TurretController : MonoBehaviour
{
    [Header("Turret Settings")]
    public Transform barrel;  
    public GameObject bulletPrefab;  
    public Transform firePoint;  
    private float currentAngle = 0f;
    private float nextFireTime = 0f;

    [Header("Difficulty Settings")]
    public float easyRotationSpeed = 80f;
    public float mediumRotationSpeed = 100f;
    public float hardRotationSpeed = 120f;
    public float easyFireRate = 0.7f;
    public float mediumFireRate = 0.5f;
    public float hardFireRate = 0.3f;
    private float rotationSpeed;
    private float fireRate;

    [Header("Movement Sound Settings")]
    public AudioSource turretAudioSource;  
    public AudioClip turretMoveSound;  
    [Range(0f, 1f)] public float maxVolume = 1f;  
    private float targetVolume = 0f;
    private float fadeSpeed = 2f;  

    [Header("Firing Sound Settings")]
    public AudioClip fireSound;  
    private AudioSource fireAudioSource;  
    [Range(0f, 1f)] public float fireMaxVolume = 1f;  

    void Start()
    {
    string difficulty = PlayerPrefs.GetString("SelectedDifficulty", "Medium");
    ApplyDifficultySettings(difficulty);

        if (turretAudioSource == null)
        {
            turretAudioSource = gameObject.AddComponent<AudioSource>();  
        }
        turretAudioSource.clip = turretMoveSound;
        turretAudioSource.loop = true;
        turretAudioSource.volume = 0f;  
        turretAudioSource.Play();  

        fireAudioSource = gameObject.AddComponent<AudioSource>();  
        fireAudioSource.playOnAwake = false;
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        if (Mathf.Abs(moveInput) > 0.01f) 
        {
            targetVolume = maxVolume;  
        }
        else
        {
            targetVolume = 0f;
        }

        currentAngle -= moveInput * rotationSpeed * Time.deltaTime;
        currentAngle = Mathf.Clamp(currentAngle, -60f, 60f);
        barrel.rotation = Quaternion.Euler(0, 0, currentAngle);

        if (Input.GetKey(KeyCode.Space) && Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        AdjustSoundVolume();
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, barrel.rotation);
        PlayFireSound();  
    }

    void PlayFireSound()
    {
        if (fireSound != null && fireAudioSource != null)
        {
            fireAudioSource.volume = fireMaxVolume;  
            fireAudioSource.PlayOneShot(fireSound);
        }
    }

    void AdjustSoundVolume()
    {
        if (turretAudioSource != null)
        {
            turretAudioSource.volume = Mathf.Lerp(turretAudioSource.volume, targetVolume, Time.deltaTime * fadeSpeed);
        }
    }
public void ApplyDifficultySettings(string difficulty)
{
    if (difficulty == "Easy")
    {
        rotationSpeed = easyRotationSpeed;
        fireRate = easyFireRate;
    }
    else if (difficulty == "Hard")
    {
        rotationSpeed = hardRotationSpeed;
        fireRate = hardFireRate;
    }
    else  
    {
        rotationSpeed = mediumRotationSpeed;
        fireRate = mediumFireRate;
    }
}

}
