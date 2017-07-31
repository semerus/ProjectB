using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale_Sk5 : Skill {

    public static bool rageOn = false;
    bool maleDead = false;

    public override void Activate()
    {
        RageMod();
    }

    private void RageMod()
    {
        if(maleDead==true)
        {
            rageOn = true;
        }
    }
        


}
