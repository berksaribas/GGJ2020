using System;
using UnityEngine;

public class ForceCommand : ActionCommand
{
    public Vector3 ForceVector;
    private Rigidbody ObjectRigidbody;

    public void Awake()
    {
        ObjectRigidbody = GetComponent<Rigidbody>();
    }

    public override void OnCommand()
    {
        ObjectRigidbody.isKinematic = false;
        ObjectRigidbody.AddForce(ForceVector);
    }
}