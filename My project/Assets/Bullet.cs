using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject canvasToActivate; // Referencia al canvas que se activará
    public Camera cameraToActivate;     // Referencia a la cámara que se activará
    public Camera playerCamera;         // Referencia a la cámara del jugador
    public float life = 9;              // Tiempo de vida de la bala antes de autodestruirse

    void Awake()
    {
        Destroy(gameObject, life); // Destruye la bala después de un tiempo 'life' si no colisiona
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            VidaJugador player = collision.gameObject.GetComponent<VidaJugador>(); // Asegúrate de que el nombre de la clase es correcto
            if (player != null)
            {
                player.TakeDamage(33.33f); // Causar daño al jugador
                if (player.currentHealth <= 2)
                {
                    if (canvasToActivate != null)
                    {
                        canvasToActivate.SetActive(true); // Activa el canvas especificado
                    }

                    if (cameraToActivate != null)
                    {
                        cameraToActivate.gameObject.SetActive(true); // Activa la cámara especificada
                    }

                    if (playerCamera != null)
                    {
                        playerCamera.transform.SetParent(null); // Desacoplar la cámara del jugador
                    }

                    Destroy(collision.gameObject); // Destruye el jugador
                }
            }
            Destroy(gameObject); // Destruye la bala
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject); // Destruye la bala inmediatamente tras colisionar con un enemigo
        }
    }
}
