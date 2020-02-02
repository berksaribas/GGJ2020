using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public float SecondsToDestroy;
    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time - startTime > SecondsToDestroy)
        {
            Destroy(gameObject);
        }
    }
}