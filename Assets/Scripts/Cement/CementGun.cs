using UnityEngine;

namespace Cement
{
    public class CementGun : MonoBehaviour
    {
        public float MaxStorage, Storage, CostPerShot;

        public CementManager CementManager;
        public Transform SpawnPoint;
        public float ShotForce;
        public float DirectionFuzz;
        public float CementScale;
        
        public float SecondsToCooldown;
        private float lastShotTime = 0;

        void Update()
        {
            if (Input.GetMouseButton(0)
                && (!TimeManager.Instance || TimeManager.Instance.CanPlayerInteract())
                && Time.time - lastShotTime >= SecondsToCooldown
                && Storage > 0)
            {
                var cementRigidbody = CementManager.InstantiateCement(SpawnPoint.position);
                cementRigidbody.AddForce(
                    transform.rotation
                    * Quaternion.Euler(
                        Random.Range(-DirectionFuzz, DirectionFuzz),
                        Random.Range(-DirectionFuzz, DirectionFuzz),
                        Random.Range(-DirectionFuzz, DirectionFuzz)
                    )
                    * (Vector3.forward * ShotForce),
                    ForceMode.VelocityChange
                );
                cementRigidbody.transform.localScale *= CementScale;

                Storage = Mathf.Max(Storage - CostPerShot, 0f);

                lastShotTime = Time.time;
            }
        }
    }
}