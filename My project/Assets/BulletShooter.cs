using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    public GameObject bulletPrefab;   // Arrastra aquí el prefab de la bala en el Inspector
    public Transform bulletOrigin;    // Arrastra aquí el Transform de BulletOrigin en el Inspector
    public Transform player;          // Referencia al jugador
    public float shootingInterval = 3f; // Intervalo entre disparos
    public float shootingRange = 20f; // Rango de disparo
    private float timer; // Temporizador para contar hasta el próximo disparo

    void Start()
    {
        timer = shootingInterval; // Inicializa el temporizador
    }

    void Update()
    {
        timer -= Time.deltaTime; // Reduce el temporizador cada frame
        if (timer <= 0 && PlayerInSight())
        {
            Shoot(); // Dispara cuando el temporizador llega a 0 y el jugador está a la vista
            timer = shootingInterval; // Restablece el temporizador
        }
    }

    bool PlayerInSight()
    {
        if (Vector3.Distance(player.position, transform.position) <= shootingRange)
        {
            Vector3 directionToPlayer = (player.position - bulletOrigin.position).normalized;
            RaycastHit hit;
            if (Physics.Raycast(bulletOrigin.position, directionToPlayer, out hit, shootingRange))
            {
                // Comprueba si el raycast golpea al jugador
                if (hit.transform == player)
                {
                    return true; // El jugador está visible y dentro del rango
                }
            }
        }
        return false; // El jugador no está visible o está fuera de rango
    }

    void Shoot()
    {
        if (bulletPrefab && bulletOrigin)
        {
            Instantiate(bulletPrefab, bulletOrigin.position, bulletOrigin.rotation);
        }
        else
        {
            Debug.LogError("Bullet prefab or origin not assigned!");
        }
    }
}
