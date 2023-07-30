using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator Animator;
    [SerializeField] private CharacterController Controller;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    private Collider[] allColliders;
    private Rigidbody[] allRigidBodies;
    void Start()
    {
        allColliders = GetComponentsInChildren<Collider>(true);
        allRigidBodies = GetComponentsInChildren<Rigidbody>(true);
        ToggleRagdoll(false);
    }

    public void ToggleRagdoll(bool isRagdoll)
    {
        foreach(Collider collider in allColliders)
        {
            if (collider.gameObject.CompareTag("Ragdoll"))
            {
                collider.enabled = isRagdoll;
            }
        }
        foreach(Rigidbody rigidbody in allRigidBodies)
        {
            if (rigidbody.gameObject.CompareTag("Ragdoll"))
            {
                rigidbody.isKinematic = !isRagdoll;
                rigidbody.useGravity = isRagdoll;
            }
        }

        Controller.enabled = !isRagdoll;
        Animator.enabled = !isRagdoll;
        _navMeshAgent.enabled = !isRagdoll;

    }
}
