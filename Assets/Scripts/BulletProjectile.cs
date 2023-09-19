using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 200f;
    [SerializeField] private Vector3 targetOffest = new(0, 1f, 0);
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Transform bulletHitVFXPrefab;
    private Vector3 targetPosition;

    private void Update()
    {
        //哥哥我只飞一帧！
        Vector3 moveDir = (targetPosition + targetOffest - transform.position).normalized;
        float distanceBeforeMoving = Vector3.Distance(targetPosition, transform.position);

        transform.position += moveSpeed * Time.deltaTime * moveDir;

        float distanceAfterMoving = Vector3.Distance(targetPosition, transform.position);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            //并不直接通过碰撞来检测，因为本游戏采用的是回合制，可以直接在对应的敌人脚本上扣除生命
            //因为右trail所以在最后的时刻把position设置在对应位置上以便于trail显示正确
            transform.position = targetPosition + targetOffest;
            Instantiate(bulletHitVFXPrefab, targetPosition + targetOffest, Quaternion.identity);
            trailRenderer.transform.parent = null;

            Destroy(gameObject);
        }
    }
    public void SetUp(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
