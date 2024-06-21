using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    public float speed = 5.0f;          // Velocidad de movimiento normal
    public float dashSpeed = 20.0f;     // Velocidad de dash
    public float dashDuration = 0.2f;   // Duración del dash en segundos
    public float rotationSpeed = 360f;  // Velocidad de rotación en grados por segundo
    public GameObject dashCollisionParticlePrefab;  // Prefab de partículas para la colisión durante el dash
    public AudioClip dashSound;         // Clip de audio para el dash
    public AudioClip destructionSound;  // Clip de audio para la destrucción del enemigo

    private Rigidbody rb;
    private bool isDashing = false;
    private float dashTimer;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(horizontal, 0.0f, vertical).normalized;

        // Manejar el input para dash
        if (Input.GetMouseButtonDown(0) && !isDashing) // Click izquierdo del mouse
        {
            StartCoroutine(PerformDash(movement));
        }

        if (!isDashing && movement.magnitude > 0.1f)
        {
            // Movimiento normal
            Vector3 newPosition = rb.position + movement * speed * Time.fixedDeltaTime;
            rb.MovePosition(newPosition);
            Quaternion newRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.RotateTowards(rb.rotation, newRotation, rotationSpeed * Time.deltaTime);
        }
    }

    IEnumerator PerformDash(Vector3 direction)
    {
        float startTime = Time.time;
        isDashing = true;

        // Reproducir el sonido del dash
        if (dashSound != null)
        {
            audioSource.volume = 0.42f;
            audioSource.PlayOneShot(dashSound);
        }
        else
        {
            Debug.LogError("dashSound no está asignado en el Inspector.");
        }

        while (Time.time < startTime + dashDuration)
        {
            rb.MovePosition(rb.position + direction * dashSpeed * Time.fixedDeltaTime);
            yield return null;
        }

        isDashing = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDashing && collision.gameObject.CompareTag("Enemy"))
        {
            // Instanciar el prefab de partículas en el punto de colisión
            if (dashCollisionParticlePrefab != null)
            {
                Instantiate(dashCollisionParticlePrefab, collision.contacts[0].point, Quaternion.identity);
            }
            else
            {
                Debug.LogError("dashCollisionParticlePrefab no está asignado en el Inspector.");
            }

            // Reproducir el sonido de destrucción
            PlayDestructionSound();

            Destroy(collision.gameObject);  // Destruye el objeto enemigo
        }
    }

    private void PlayDestructionSound()
    {
        if (destructionSound != null)
        {
            GameObject soundObject = new GameObject("DestructionSound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = destructionSound;
            audioSource.volume = 0.42f; // Establecer el volumen del audio source
            audioSource.Play();
            Destroy(soundObject, destructionSound.length); // Destruye el objeto después de que el sonido termine
        }
        else
        {
            Debug.LogError("Destruction sound no está asignado en el Inspector.");
        }
    }
}
