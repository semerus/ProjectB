using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer_ProtectionArea_MoreDuration : Healer_ProtectionArea
{
    protected override void Refresh_Value()
    {
        base.Refresh_Value();
        duration = 4f;
    }
}
