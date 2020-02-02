using UnityEngine;

namespace Cement
{
    

    [RequireComponent(typeof(SphereCollider))]
    public class CementBall : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.rigidbody == null || (collision.rigidbody.isKinematic && collision.collider.GetComponent<CementBall>() == null))
            {
                return;
            }
            
            var _rigidbody = GetComponent<Rigidbody>();

            if (_rigidbody.isKinematic)
            {
                return;
            }
        
            //    Remove DestroyTimer
            Destroy(GetComponent<DestroyTimer>());
            
            //    Increase collider size
            transform.localScale *= Random.Range(1.8f, 2.6f);

            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.mass = 0;
            _rigidbody.isKinematic = true;
        
            //    Check if we collided with a CementGroup or a free rigidbody
            var cementGroup = collision.rigidbody.GetComponent<CementGroup>();
            if (cementGroup != null)
            {
                cementGroup.AddAndDestroy(_rigidbody);
            }
            else
            {
                print("CementBall created");
                var newCementGroup = CementManager.Instance.InstantiateCementGroup(transform.position).GetComponent<CementGroup>();
                newCementGroup.AddAndDestroy(_rigidbody);
                newCementGroup.AddAndDestroy(collision.rigidbody);
            }
        }
    }
}