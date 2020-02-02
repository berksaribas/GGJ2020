using System;
using Cement;
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

        if (other.collider.GetComponent<CementBall>() != null)
        {
            return;
        }
        
        if(other.relativeVelocity.magnitude > 15f)
        {
            var newObj = Instantiate(DestroyedVersion, transform.position, transform.rotation, transform.parent);
            newObj.transform.localScale = transform.localScale;
            var layer = 0;
            if (TimeManager.Instance != null)
            {
                layer = TimeManager.Instance.GetCurrentTimespaceLayer();
            }
            Utils.SetLayerRecursively(newObj, layer);
            Destroy(gameObject);
            isDone = true;
        }
    }
}