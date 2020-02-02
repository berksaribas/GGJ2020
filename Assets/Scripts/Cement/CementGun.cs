using UnityEngine;

namespace Cement
{
    public class CementGun : MonoBehaviour
    {
        public CementManager CementManager;
        public Transform SpawnPoint;
        public float ShotForce;

        public float SecondsToCooldown;
        private float lastShotTime = 0;

        void Update()
        {
            if (Input.GetMouseButton(0) && Time.time - lastShotTime >= SecondsToCooldown)
            {
                var cementRigidbody = CementManager.InstantiateCement(SpawnPoint.position);
                cementRigidbody.AddForce(
                    transform.rotation * (Vector3.forward * ShotForce),
                    ForceMode.VelocityChange
                );

                lastShotTime = Time.time;
            }
        }
    }
}