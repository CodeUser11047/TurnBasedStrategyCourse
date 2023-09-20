using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool invert;
    [SerializeField] private float disVisualDiantace = 4f;
    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    [System.Obsolete]
    private void LateUpdate()
    {
        transform.forward = invert ? cameraTransform.forward * -1 : cameraTransform.forward;
        foreach (Transform child in transform)
        {
            // child.gameObject.active = Vector3.Distance(cameraTransform.position, transform.position) >= disVisualDiantace;
            child.gameObject.active = CameraController.Instance.GetCameraTargetFollowOffset().y >= disVisualDiantace;
        }

        // if (!invert)
        // {
        //     Vector3 dirTocamera = (cameraTransform.position - transform.position).normalized;
        //     transform.LookAt(transform.position + dirTocamera * -1);

        // }
        // else
        // {
        //     transform.LookAt(cameraTransform);
        // }
    }
}
