using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;
    private bool wasThrown = false;
    public GameObject destructionParticlePrefab; // Prefab de partículas para la destrucción
    public AudioClip collisionSound;  // Clip de audio para la colisión

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Cargar el prefab de partículas desde Resources si no está asignado
        if (destructionParticlePrefab == null)
        {
            destructionParticlePrefab = Resources.Load<GameObject>("DustExplosion");
            if (destructionParticlePrefab == null)
            {
                Debug.LogError("No se encontró el prefab de partículas 'DustExplosion' en la carpeta Resources.");
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colisión detectada con: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Instanciar el prefab de partículas en el punto de colisión
            if (destructionParticlePrefab != null)
            {
                Debug.Log("Instanciando partículas en: " + collision.contacts[0].point);
                Instantiate(destructionParticlePrefab, collision.contacts[0].point, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Destruction Particle Prefab no está asignado en el Inspector y no se encontró en Resources.");
            }

            // Reproducir el sonido de colisión
            PlayCollisionSound();

            Debug.Log("Destruyendo: " + gameObject.name + " y " + collision.gameObject.name);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Comprueba si ha sido lanzado y si se ha detenido
        if (wasThrown && rb.velocity.magnitude < 0.1f)
        {
            if (destructionParticlePrefab != null)
            {
                Instantiate(destructionParticlePrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Destruction Particle Prefab no está asignado en el Inspector y no se encontró en Resources.");
            }
            PlayCollisionSound();
            Destroy(gameObject); // Destruye el objeto inmediatamente sin retardo
        }
    }

    public void MarkAsThrown()
    {
        wasThrown = true; // Marca al enemigo como lanzado
    }

    private void PlayCollisionSound()
    {
        if (collisionSound != null)
        {
            GameObject soundObject = new GameObject("CollisionSound");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = collisionSound;
            audioSource.volume = 0.111f;
            audioSource.Play();
            Destroy(soundObject, collisionSound.length); // Destruye el objeto después de que el sonido termine
        }
        else
        {
            Debug.LogError("Collision sound no está asignado en el Inspector.");
        }
    }
}
