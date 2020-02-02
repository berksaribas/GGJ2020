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
            var targetTransform = transform;
            var layer = 0;
            if (TimeManager.Instance != null)
            {
                if(TimeManager.Instance.GetCurrentTimespaceTransform() != null)
                {
                    targetTransform = TimeManager.Instance.GetCurrentTimespaceTransform();
                }

                layer = TimeManager.Instance.GetCurrentTimespaceLayer();
            }
            var cementGroup = Instantiate(CementGroupPrefab, spawnPosition, transform.rotation, targetTransform);
            Utils.SetLayerRecursively(cementGroup, layer);
            return cementGroup.GetComponent<Rigidbody>();
        }

        public Rigidbody InstantiateCement(Vector3 spawnPosition)
        {
            var targetTransform = transform;
            var layer = 0;
            if (TimeManager.Instance != null)
            {
                if(TimeManager.Instance.GetCurrentTimespaceTransform() != null)
                {
                    targetTransform = TimeManager.Instance.GetCurrentTimespaceTransform();
                }

                layer = TimeManager.Instance.GetCurrentTimespaceLayer();
            }
            var cementBall = Instantiate(CementBallPrefab, spawnPosition, transform.rotation, targetTransform);
            Utils.SetLayerRecursively(cementBall, layer);
            return cementBall.GetComponent<Rigidbody>();
        }
    }
}