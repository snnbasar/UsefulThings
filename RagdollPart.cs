using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollPart : MonoBehaviour
{
    private Damageable damageable;
    private Rigidbody rb;

    private void Start()
    {
        damageable = GetComponentInParent<Damageable>();
        rb = GetComponent<Rigidbody>();

    }
    public void MakeMeKinematic()
    {
        rb.isKinematic = true;

    }

    public Damageable GetDamageable() => damageable;
}
