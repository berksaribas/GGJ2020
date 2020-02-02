using System.Collections.Generic;
using UnityEngine;

public class CementManager : MonoBehaviour
{
    public static CementManager Instance;

    public float MinDistance;

    public GameObject CementBallPrefab;
    public GameObject CementGroupPrefab;
    private readonly List<CementGroup> cementGroups = new List<CementGroup>();

    void Awake()
    {
        Instance = this;
    }

//    public void ManageCementBall(CementBall cementBall, Rigidbody other)
//    {
//        var cementGroup = other.GetComponent<CementGroup>();
//
//        //  If other is already a cementGroup
//        if (cementGroup != null)
//        {
//            cementGroup.AddAndDestroy(cementBall.Rigidbody);
//            return;
//        }
//
//        // Create CementGroup
//        cementGroup = Instantiate(CementGroupPrefab, transform).GetComponent<CementGroup>();
//        cementGroups.Add(cementGroup.GetComponent<CementGroup>());
//
//        // Handle CementBall
//        cementGroup.AddAndDestroy(cementBall.Rigidbody);
//
//        // Handle other
//        cementGroup.AddAndDestroy(other);
//    }

    public List<CementBall> GetCementBallsInRange(Vector3 position)
    {
        var cementBallsInRange = new List<CementBall>();

        foreach (var cementGroup in cementGroups)
        {
            foreach (var cementBall in cementGroup.GetComponents<CementBall>())
            {
                var dist = (position - cementBall.transform.position).magnitude;
                if (dist < MinDistance)
                {
                    cementBallsInRange.Add(cementBall);
                }
            }
        }

        return cementBallsInRange;
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