using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // El objetivo que la cámara debe seguir
    public Vector3 offset = new Vector3(0f, 1f, -1f); // Offset de la cámara respecto al target
    public float smoothSpeed = 0.125f; // Velocidad de interpolación para suavizar el movimiento de la cámara
    public float minDistance = 1.0f; // Mínima distancia a mantener de cualquier objeto
    public GameObject noTargetUI; // Referencia al UI que se activará cuando no haya target

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.rotation;
        // Asegúrate de que la interfaz no esté activa al inicio si el target existe
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
            // Comprobación de colisiones para ajustar la posición de la cámara si encuentra un obstáculo
            if (Physics.Raycast(target.position, smoothedPosition - target.position, out hit, offset.magnitude))
            {
                if (hit.distance < minDistance)
                {
                    smoothedPosition = hit.point - (smoothedPosition - target.position).normalized * minDistance;
                }
            }

            transform.position = smoothedPosition;
            transform.rotation = initialRotation;

            // Asegúrate de desactivar la interfaz si el target está presente
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
