using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 200f;
    [SerializeField] private Vector3 targetOffest = new(0, 1f, 0);
    [SerializeField] private TrailRenderer trailRenderer;
    private Vector3 targetPosition;

    private void Update()
    {
        //哥哥我只飞一帧！
        Vector3 moveDir = ((targetPosition + targetOffest) - transform.position).normalized;
        float distanceBeforeMoving = Vector3.Distance(targetPosition, transform.position);

        transform.position += moveSpeed * Time.deltaTime * moveDir;

        float distanceAfterMoving = Vector3.Distance(targetPosition, transform.position);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            trailRenderer.transform.parent = null;
            Destroy(gameObject);
        }
    }
    public void SetUp(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
