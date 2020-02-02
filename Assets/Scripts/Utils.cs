using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static void SetLayerRecursively(GameObject go, int layerNumber) {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true)) {
            trans.gameObject.layer = layerNumber;
        }
    }

    public static T[] GetComponentsOnlyInChildren<T>(this MonoBehaviour mb)
    {
        var transform = mb.transform;
        var Ts = new List<T>();

        foreach (Transform child in transform)
        {
            Ts.AddRange(child.GetComponents<T>());
        }

        return Ts.ToArray();
    }
}