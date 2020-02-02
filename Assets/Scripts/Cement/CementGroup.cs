using UnityEngine;

namespace Cement
{
    [RequireComponent(typeof(Rigidbody))]
    public class CementGroup : MonoBehaviour
    {
        [HideInInspector] public Rigidbody Rigidbody;

        private bool dead = false;

        public bool Dead
        {
            get => dead;
            set
            {
                dead = value;
                if (value)
                {
                    name += " DEAD";
                }
            }
        }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.rigidbody == null || collision.rigidbody.isKinematic)
            {
                return;
            }

            if (Dead)
            {
                print("im dead :< doing nothing");
                return;
            }

            var contacts = new ContactPoint[collision.contactCount];
            collision.GetContacts(contacts);
            foreach (var contact in contacts)
            {
                var cementBall = contact.thisCollider.GetComponent<CementBall>();
                if (cementBall != null)
                {
                    //    Check if we collided with a CementGroup or a free rigidbody
                    var cementGroup = collision.rigidbody.GetComponent<CementGroup>();
                    if (cementGroup != null)
                    {
                        AddAndDestroy(cementGroup);
                    }
                    else
                    {
                        AddAndDestroy(collision.rigidbody);
                    }

                    break;
                }
            }
        }

        public void AddAndDestroy(Rigidbody rBody)
        {
            print($"{name} adding {rBody.name}");
//        rBody.velocity = Vector3.zero;

            Rigidbody.mass += rBody.mass;
            var t = rBody.transform;
            Destroy(rBody);

            t.SetParent(transform);
        }

        public void AddAndDestroy(CementGroup cementGroup)
        {
            print(
                $"{name}[{transform.childCount}] will add {cementGroup.gameObject.name}[{cementGroup.transform.childCount}]");

            cementGroup.Dead = true;

            //    Stop recalculating center of mass
            Rigidbody.centerOfMass = Vector3.zero;

            var t = transform;
            foreach (var child in cementGroup.GetComponentsOnlyInChildren<Transform>())
            {
//            print($"Merging {child.gameObject.name} into {name}");
                child.SetParent(t);
            }

            //    Recalculate center of mass at the end
            Rigidbody.ResetCenterOfMass();

            var timer = cementGroup.gameObject.AddComponent<DestroyTimer>();
            timer.SecondsToDestroy = 3;
        }
    }
}