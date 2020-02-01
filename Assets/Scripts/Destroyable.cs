using System;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public GameObject DestroyedVersion;
    private bool isDone;
    
    private void OnCollisionEnter(Collision other)
    {
        if (isDone)
        {
            return;
        }
        
        if(other.relativeVelocity.magnitude > 15f)
        {
            Instantiate(DestroyedVersion, transform.position, transform.rotation, transform.parent);
            Destroy(gameObject);
            isDone = true;
        }
    }
}