using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEffect : MonoBehaviour
{
    public GameObject collisionParticlePrefab; // Asigna tu prefab de partículas en el Inspector

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto con el que colisionamos es un enemigo
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Instancia el sistema de partículas en el punto de colisión
            Instantiate(collisionParticlePrefab, collision.contacts[0].point, Quaternion.identity);

            // (Opcional) Destruye el sistema de partículas después de un tiempo para limpiar
            Destroy(collisionParticlePrefab, 2.0f); // 2 segundos o el tiempo que dure tu efecto
        }
    }
}
