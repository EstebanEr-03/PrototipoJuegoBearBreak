using UnityEngine;
using UnityEngine.AI; // Utiliza la navegación de malla

public class EnemyAI : MonoBehaviour
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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
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

            // Manejo del disparo
            if (shootingTimer <= 0)
            {
                Shoot();
                shootingTimer = shootingInterval; // Restablece el temporizador de disparo
            }
            shootingTimer -= Time.deltaTime;
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
        }
        else
        {
            Debug.LogError("Bullet prefab or spawn point not set!");
        }
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
