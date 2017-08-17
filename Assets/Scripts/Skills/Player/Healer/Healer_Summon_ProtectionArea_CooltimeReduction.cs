using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer_Summon_ProtectionArea_CooltimeReduction : Healer_Summon_ProtectionArea {

    protected override void Awake()
    {
        base.Awake();
        healer_ProtectionArea_Script = healer_ProtectionArea_Object.AddComponent<Healer_ProtectionArea_CooltimeReduction>();
    }

}
