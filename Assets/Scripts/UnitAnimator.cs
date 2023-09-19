using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator myAnimator;
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private Transform shootPointTranform;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }
        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;

        }
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        myAnimator.SetBool("IsWalking", true);
    }
    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        myAnimator.SetBool("IsWalking", false);
    }
    private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
    {
        myAnimator.SetTrigger("Shoot");

        Transform bulletProjectileTransform =
            Instantiate(bulletPrefab, shootPointTranform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorlPosition();
        bulletProjectile.SetUp(targetUnitShootAtPosition);
    }
}
