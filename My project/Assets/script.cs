using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HideUI : MonoBehaviour
{
    public GameObject canvas; // Referencia al objeto del canvas
    void Start()
    {
        StartCoroutine(DisableUIAfterDelay(1f)); // Llama a la coroutine para desactivar el UI después de 1 segundo
    }

    IEnumerator DisableUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Espera durante el tiempo especificado
        canvas.SetActive(false); // Desactiva el canvas
    }
}
