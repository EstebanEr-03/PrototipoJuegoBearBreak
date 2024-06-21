using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabAndThrow : MonoBehaviour
{
    public Transform grabDetect;
    public Transform holdPoint;
    public float rayDist = 0.5f;  // Distancia reducida para simular una mano
    private GameObject grabbedObject;
    public float throwForce = 20.0f;
    public AudioClip grabSound;    // Clip de audio para grabar
    public AudioClip throwSound;   // Clip de audio para lanzar

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.442f;  // Establecer el volumen del audio source
    }

    void Update()
    {
        RaycastHit hit;

        // Dibuja el raycast en la escena para debug
        Debug.DrawRay(grabDetect.position, transform.forward * rayDist, Color.red);

        if (Physics.Raycast(grabDetect.position, transform.forward, out hit, rayDist))
        {
            // Imprime la distancia detectada por el raycast
            Debug.Log("Raycast hit at distance: " + hit.distance);

            if (hit.collider.gameObject.CompareTag("Enemy") && Input.GetMouseButtonDown(1))  // Click derecho para agarrar
            {
                if (grabbedObject == null)
                {
                    grabbedObject = hit.collider.gameObject;
                    grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                    grabbedObject.GetComponent<Collider>().enabled = false;
                    grabbedObject.transform.position = holdPoint.position;
                    grabbedObject.transform.parent = holdPoint;

                    // Reproducir el sonido de grabar
                    PlaySound(grabSound);
                }
            }
        }

        if (grabbedObject != null)
        {
            grabbedObject.transform.position = holdPoint.position;
        }

        // Modificado para usar la tecla Espacio en lugar del click izquierdo del mouse
        if (Input.GetKeyDown(KeyCode.Space) && grabbedObject != null)  // Tecla Espacio para lanzar
        {
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObject.GetComponent<Collider>().enabled = true;  // Reactiva el collider
            grabbedObject.transform.parent = null;
            grabbedObject.GetComponent<Rigidbody>().velocity = transform.forward * throwForce;

            Enemy enemyComponent = grabbedObject.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.MarkAsThrown(); // Marca como lanzado si es un enemigo
            }
            else
            {
                Debug.LogWarning("Lanzado objeto no enemigo: " + grabbedObject.name);
            }

            // Reproducir el sonido de lanzar
            PlaySound(throwSound);

            Debug.Log("Lanzado: " + grabbedObject.name);
            grabbedObject = null;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Audio clip no asignado.");
        }
    }
}
