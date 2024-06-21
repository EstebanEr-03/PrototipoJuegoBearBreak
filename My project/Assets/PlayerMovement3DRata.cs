using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement3DRata : MonoBehaviour
{
    public float speed = 5.0f;          // Velocidad de movimiento normal
    public float dashSpeed = 20.0f;     // Velocidad de dash
    public float dashDuration = 0.2f;   // Duraci�n del dash en segundos
    public float rotationSpeed = 360f;  // Velocidad de rotaci�n en grados por segundo
    public GameObject dashCollisionParticlePrefab;  // Prefab de part�culas para la colisi�n durante el dash
    public AudioClip dashSound;         // Clip de audio para el dash
    public AudioClip destructionSound;  // Clip de audio para la destrucci�n del enemigo

    private Rigidbody rb;
    private Animator animator;
    private bool isDashing = false;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Aseg�rate de congelar la rotaci�n en los ejes X y Z para evitar vueltas incontroladas
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
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

        // Movimiento normal
        if (!isDashing && movement.magnitude > 0.1f)
        {
            MoveCharacter(movement);

            // Configurar par�metros del Animator
            animator.SetFloat("Xvelocidad", horizontal);
            animator.SetFloat("Yvelocidad", vertical);
            animator.SetBool("IsRunning", true);
        }
        else
        {
            // Detener la animaci�n de caminar
            animator.SetBool("IsRunning", false);
            animator.SetFloat("Xvelocidad", 0);
            animator.SetFloat("Yvelocidad", 0);
        }
    }

    private void MoveCharacter(Vector3 movement)
    {
        // Movimiento normal en la direcci�n en la que el personaje est� mirando
        Vector3 forwardMovement = transform.forward * movement.z * speed * Time.fixedDeltaTime;
        Vector3 rightMovement = transform.right * movement.x * speed * Time.fixedDeltaTime;
        Vector3 newPosition = rb.position + forwardMovement + rightMovement;
        rb.MovePosition(newPosition);
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
            Debug.LogError("dashSound no est� asignado en el Inspector.");
        }

        // Activar el trigger de ataque (dash cuenta como ataque)
        animator.SetTrigger("IsAttacking");

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
            // Instanciar el prefab de part�culas en el punto de colisi�n
            if (dashCollisionParticlePrefab != null)
            {
                Instantiate(dashCollisionParticlePrefab, collision.contacts[0].point, Quaternion.identity);
            }
            else
            {
                Debug.LogError("dashCollisionParticlePrefab no est� asignado en el Inspector.");
            }

            // Reproducir el sonido de destrucci�n
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
            Destroy(soundObject, destructionSound.length); // Destruye el objeto despu�s de que el sonido termine
        }
        else
        {
            Debug.LogError("Destruction sound no est� asignado en el Inspector.");
        }
    }
}
