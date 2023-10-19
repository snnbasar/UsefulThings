using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
    private Collider[] colliders;
    private Animator anim;

    public RagdollPart[] ragdollParts;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        colliders = GetComponentsInChildren<Collider>();
        ragdollParts = GetComponentsInChildren<RagdollPart>();
        DoRagdoll(false);
    }

    public void DoRagdoll(bool status)
    {
        anim.enabled = !status;
        foreach (Collider col in colliders)
        {

            //col.enabled = status;
            col.isTrigger = !status;
            col.gameObject.layer = 9;
            if (col.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = !status;
                int randomDirection = UnityEngine.Random.Range(-1, 2);
                //rb.AddForce((Vector3.up * 10 + -transform.forward * 15 + transform.right * 50 * randomDirection), ForceMode.Impulse);
                rb.AddForce((Vector3.up * 10 + -transform.forward * 15 ), ForceMode.Impulse);
            }
        }
    }


    public void DoRagdollWhithoutAnim(bool status)
    {
        anim.enabled = false;
        foreach (Collider col in colliders)
        {

            col.enabled = status;
            col.gameObject.layer = 9;
            if (col.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = !status;
                int randomDirection = UnityEngine.Random.Range(-1, 2);
                rb.AddForce((Vector3.up * 10 + -transform.forward * 15 + transform.right * 50 * randomDirection), ForceMode.Impulse);
            }
        }
    }

    public void DoRagdollFromAnim() //For Animation Purphouses
    {
        DoRagdoll(true);
    }

    public Transform GetRandomPart()
    {
        return ragdollParts.GetRandomItem().transform;
    }
    public Transform GetChestPart()
    {
        return ragdollParts[1].transform;
    }
}
