using UnityEngine;

namespace Cement
{
    [RequireComponent(typeof(Collider))]
    public class CementBall : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.rigidbody == null || collision.rigidbody.isKinematic)
            {
                return;
            }
        
            //    Remove DestroyTimer
            Destroy(GetComponent<DestroyTimer>());
        
            //    Check if we collided with a CementGroup or a free rigidbody
            var cementGroup = collision.rigidbody.GetComponent<CementGroup>();
            if (cementGroup != null)
            {
                cementGroup.AddAndDestroy(GetComponent<Rigidbody>());
            }
            else
            {
                print("CementBall created");
                var newCementGroup = CementManager.Instance.InstantiateCementGroup(transform.position).GetComponent<CementGroup>();
                newCementGroup.AddAndDestroy(GetComponent<Rigidbody>());
                newCementGroup.AddAndDestroy(collision.rigidbody);
            }
        }
    }
}