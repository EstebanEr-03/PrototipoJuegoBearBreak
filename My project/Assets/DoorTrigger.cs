using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Animator Puerta; // Referencia al Animator de la puerta

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Puerta.SetBool("IsOpen", true); // Cambiar el booleano IsOpen a true
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Puerta.SetBool("IsOpen", false); // Cambiar el booleano IsOpen a false
        }
    }
}
