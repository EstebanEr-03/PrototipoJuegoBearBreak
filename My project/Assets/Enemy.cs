using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;
    private bool wasThrown = false;
    public GameObject destructionParticlePrefab; // Prefab de part�culas para la destrucci�n
    public AudioClip collisionSound;  // Clip de audio para la colisi�n

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Cargar el prefab de part�culas desde Resources si no est� asignado
        if (destructionParticlePrefab == null)
        {
            destructionParticlePrefab = Resources.Load<GameObject>("DustExplosion");
            if (destructionParticlePrefab == null)
            {
                Debug.LogError("No se encontr� el prefab de part�culas 'DustExplosion' en la carpeta Resources.");
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colisi�n detectada con: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Instanciar el prefab de part�culas en el punto de colisi�n
            if (destructionParticlePrefab != null)
            {
                Debug.Log("Instanciando part�culas en: " + collision.contacts[0].point);
                Instantiate(destructionParticlePrefab, collision.contacts[0].point, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Destruction Particle Prefab no est� asignado en el Inspector y no se encontr� en Resources.");
            }

            // Reproducir el sonido de colisi�n
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
                Debug.LogError("Destruction Particle Prefab no est� asignado en el Inspector y no se encontr� en Resources.");
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
            Destroy(soundObject, collisionSound.length); // Destruye el objeto despu�s de que el sonido termine
        }
        else
        {
            Debug.LogError("Collision sound no est� asignado en el Inspector.");
        }
    }
}
