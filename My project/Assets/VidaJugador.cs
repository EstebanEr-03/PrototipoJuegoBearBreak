using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaJugador : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth; // Cambiado a float para manejar decimales
    public HealthBar healthBar; // Referencia a la barra de salud

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        StartCoroutine(AutoHeal()); // Iniciar la corrutina de curación automática
    }

    public void TakeDamage(float damage) // Cambiado a público
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Asegurarse de que la salud no caiga por debajo de 0
        healthBar.SetHealth((int)currentHealth); // Actualizar la barra de salud
    }

    private IEnumerator AutoHeal()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f); // Esperar 10 segundos

            if (currentHealth < maxHealth)
            {
                float healAmount = maxHealth * 0.20f; // Curar el 20% de la salud máxima
                currentHealth += healAmount;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Asegurarse de que la salud no supere el máximo
                healthBar.SetHealth((int)currentHealth); // Actualizar la barra de salud
            }
        }
    }
}
