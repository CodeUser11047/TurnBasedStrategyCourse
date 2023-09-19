using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;
    [SerializeField] private float explosionForce = 5f;
    // [SerializeField] private Transform debugPrefab;

    public void SetUp(Transform originRootBone, Transform damageSourceTransform = null)
    {
        MathAllChildTransform(originRootBone, ragdollRootBone);
        // Debug.Log(damageSourceTransform);
        //如果可以获取伤害源就实现第二类 第一类留给爆炸类
        if (damageSourceTransform == null)
        {
            ApplyExplosionToRagdoll(ragdollRootBone, explosionForce * 200f, transform.position, 10f);
        }
        else
        {
            var dmgDirectionOffest = (damageSourceTransform.position - transform.position).normalized;
            var dmgDirection = transform.position + dmgDirectionOffest + new Vector3(0, 1, 0);
            // Instantiate(debugPrefab, dmgDirection, Quaternion.identity);Debug用的
            ApplyExplosionToRagdoll(ragdollRootBone, explosionForce * 200f, dmgDirection, 100f);
        }
    }

    private void MathAllChildTransform(Transform origin, Transform clone)
    {
        foreach (Transform child in origin)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.SetPositionAndRotation(child.position, child.rotation);
                // Debug.Log("2");这里没问题
                MathAllChildTransform(child, cloneChild);
            }
        }
    }

    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent(out Rigidbody childRigidBody))
            {
                childRigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
                // Debug.Log("1");
            }

            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
