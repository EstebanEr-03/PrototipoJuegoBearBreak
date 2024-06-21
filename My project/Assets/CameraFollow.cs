using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // El objetivo que la c�mara debe seguir
    public Vector3 offset = new Vector3(0f, 1f, -1f); // Offset de la c�mara respecto al target
    public float smoothSpeed = 0.125f; // Velocidad de interpolaci�n para suavizar el movimiento de la c�mara
    public float minDistance = 1.0f; // M�nima distancia a mantener de cualquier objeto
    public GameObject noTargetUI; // Referencia al UI que se activar� cuando no haya target

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.rotation;
        // Aseg�rate de que la interfaz no est� activa al inicio si el target existe
        if (noTargetUI != null && target != null)
            noTargetUI.SetActive(false);
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            RaycastHit hit;
            // Comprobaci�n de colisiones para ajustar la posici�n de la c�mara si encuentra un obst�culo
            if (Physics.Raycast(target.position, smoothedPosition - target.position, out hit, offset.magnitude))
            {
                if (hit.distance < minDistance)
                {
                    smoothedPosition = hit.point - (smoothedPosition - target.position).normalized * minDistance;
                }
            }

            transform.position = smoothedPosition;
            transform.rotation = initialRotation;

            // Aseg�rate de desactivar la interfaz si el target est� presente
            if (noTargetUI != null)
                noTargetUI.SetActive(false);
        }
        else
        {
            // Activa la interfaz si no hay target
            if (noTargetUI != null)
                noTargetUI.SetActive(true);
        }
    }
}
