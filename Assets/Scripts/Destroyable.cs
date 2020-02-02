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

        var colliderMass = other.rigidbody != null ? other.rigidbody.mass : 1;
        var thisMass = GetComponent<Rigidbody>().mass;

        var impact = 0.5f*colliderMass/thisMass*Mathf.Pow(other.relativeVelocity.magnitude,2);
        
        //Debug.Log($"{other.gameObject.name} collided to {gameObject.name} with an impact value of {impact}" );
        
        if(impact > 7.5f)
        {
            var newObj = Instantiate(DestroyedVersion, transform.position, transform.rotation, transform.parent);
            newObj.transform.localScale = transform.localScale;
            var layer = 0;
            if (TimeManager.Instance != null)
            {
                layer = TimeManager.Instance.GetCurrentTimespaceLayer();
            }
            Utils.SetLayerRecursively(newObj, layer);
            gameObject.SetActive(false);
            Destroy(gameObject);
            isDone = true;
        }
    }
}