using System.Collections;
using System.Collections.Generic;
using Cement;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public CementGun CementGun;
    public RectTransform InnerBar;

    void OnGUI()
    {
        var amax = InnerBar.anchorMax;
        amax.x = CementGun.Storage / CementGun.MaxStorage;
        InnerBar.anchorMax = amax;

        var omax = InnerBar.offsetMax;
        omax.x = 0;
        InnerBar.offsetMax = omax;
    }
}
