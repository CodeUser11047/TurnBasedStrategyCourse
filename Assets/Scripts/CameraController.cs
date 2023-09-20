using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private bool isFlipRotation;
    [SerializeField] private float zoomAmount = 2f;
    [SerializeField] private float zoomSpeed = 2f;

    private const float MIN_FOLLOW_Y_OFFEST = 2f;
    private const float MAX_FOLLOW_Y_OFFEST = 12f;

    private Vector3 targetFollowOffset;
    private CinemachineTransposer cinemachineTransposer;

    protected override void Awake()
    {
        base.Awake();
        cinemachineVirtualCamera = FindFirstObjectByType<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }
    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement()
    {
        Vector3 inputMoveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDir.z += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDir.z -= 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDir.x -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDir.x += 1f;
        }

        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveSpeed * Time.deltaTime * moveVector;
    }

    private void HandleRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y += 1f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y -= 1f;
        }
        if (!isFlipRotation)
        {
            transform.eulerAngles += rotationSpeed * Time.deltaTime * -rotationVector;
        }
        else
        {
            transform.eulerAngles += rotationSpeed * Time.deltaTime * rotationVector;
        }
    }

    private void HandleZoom()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFollowOffset.y -= zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFollowOffset.y += zoomAmount;
        }
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFEST, MAX_FOLLOW_Y_OFFEST);

        cinemachineTransposer.m_FollowOffset =
          Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, zoomSpeed * Time.deltaTime);
    }

    public Vector3 GetCameraTargetFollowOffset()
    {
        return targetFollowOffset;
    }
}
