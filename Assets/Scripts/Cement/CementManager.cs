using UnityEngine;

namespace Cement
{
    public class CementManager : MonoBehaviour
    {
        public static CementManager Instance;

        public GameObject CementBallPrefab;
        public GameObject CementGroupPrefab;

        void Awake()
        {
            Instance = this;
        }

        public Rigidbody InstantiateCementGroup(Vector3 spawnPosition)
        {
            var cementGroup = Instantiate(CementGroupPrefab, spawnPosition, transform.rotation, transform);
            return cementGroup.GetComponent<Rigidbody>();
        }

        public Rigidbody InstantiateCement(Vector3 spawnPosition)
        {
            var cementBall = Instantiate(CementBallPrefab, spawnPosition, transform.rotation, transform);
            return cementBall.GetComponent<Rigidbody>();
        }
    }
}