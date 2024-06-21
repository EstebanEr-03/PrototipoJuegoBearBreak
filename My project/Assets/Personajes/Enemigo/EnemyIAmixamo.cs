using UnityEngine;
using UnityEngine.AI;
using System.Collections; // Necesario para IEnumerator

public class EnemyAImixamo : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public GameObject bulletPrefab; // Prefab de la bala
    public Transform bulletSpawn; // Punto de origen de la bala
    public float detectionRadius = 10.0f;
    public float shootingInterval = 3.0f; // Intervalo de tiempo entre disparos en segundos
    private float shootingTimer; // Temporizador para controlar los disparos
    private NavMeshAgent agent;
    private AudioSource audioSource; // Componente AudioSource para el sonido del disparo
    public AudioClip shootingSound; // Clip de audio para el disparo
    public GameObject shootingParticlePrefab; // Prefab del sistema de partículas para el disparo
    private Animator animator; // Referencia al Animator

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        shootingTimer = shootingInterval; // Inicializa el temporizador de disparo
        SphereCollider detectionCollider = gameObject.AddComponent<SphereCollider>();
        detectionCollider.isTrigger = true;
        detectionCollider.radius = detectionRadius;
    }

    void Update()
    {
        // Si el jugador está dentro del rango de detección, seguir y disparar
        if (player && Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            // Rotar para mirar hacia el jugador
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            // Moverse hacia el jugador
            agent.SetDestination(player.position);

            // Controlar las animaciones de movimiento
            animator.SetFloat("XSpeed", direction.x);
            animator.SetFloat("YSpeed", direction.z);

            // Manejo del disparo
            if (shootingTimer <= 0)
            {
                Shoot();
                shootingTimer = shootingInterval; // Restablece el temporizador de disparo
            }
            shootingTimer -= Time.deltaTime;
        }
        else
        {
            // Resetear las animaciones de movimiento cuando no esté persiguiendo al jugador
            animator.SetFloat("XSpeed", 0);
            animator.SetFloat("YSpeed", 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            agent.ResetPath(); // Detiene el movimiento del enemigo
        }
    }

    // Método para disparar la bala
    void Shoot()
    {
        if (bulletPrefab && bulletSpawn)
        {
            Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            PlayShootingSound();
            PlayShootingParticles();

            // Activar la animación de disparo
            animator.SetBool("IsShooting", true);

            // Desactivar la animación de disparo después de un corto tiempo
            StartCoroutine(ResetShootingTrigger());
        }
        else
        {
            Debug.LogError("Bullet prefab or spawn point not set!");
        }
    }

    private IEnumerator ResetShootingTrigger()
    {
        yield return null; // Esperar un frame
        animator.SetBool("IsShooting", false);
    }

    // Método para reproducir el sonido de disparo
    void PlayShootingSound()
    {
        if (shootingSound != null)
        {
            audioSource.PlayOneShot(shootingSound);
        }
        else
        {
            Debug.LogError("Shooting sound not set!");
        }
    }

    // Método para reproducir las partículas de disparo
    void PlayShootingParticles()
    {
        if (shootingParticlePrefab != null)
        {
            Instantiate(shootingParticlePrefab, bulletSpawn.position, bulletSpawn.rotation);
        }
        else
        {
            Debug.LogError("Shooting particle prefab not set!");
        }
    }
}
