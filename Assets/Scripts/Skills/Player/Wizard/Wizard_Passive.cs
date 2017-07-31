using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Passive : Skill {

    public static float mag=1;
    public static int skillCount = 0;
    public static bool skillResfresh = false;
    public static bool stackOff = false;
    private float buffTime = 0;

    public override void Activate()
    {
        TimeSystem.GetTimeSystem().AddTimer(this);
    }

    public override void RunTime()
    {
        base.RunTime();
        CheckBuff();
    }

    private void CheckBuff()
    {
        CheckMag();

        if(Input.GetKeyDown("."))
        {
            Debug.Log("mag = " + mag + "  skillCount = " + skillCount);
        }

        if (skillCount >= 1)
        {
            buffTime += Time.deltaTime;

            if (skillResfresh)
            {
                buffTime = 0;
                skillResfresh = false;
            }
            if (stackOff)
            {
                buffTime = 0;
                skillCount = 0;
                stackOff = false;
            }
            if (buffTime >= 20)
            {
                buffTime = 0;
                skillCount = 0;
            }
        }
    }

    private void CheckMag()
    {
        switch(skillCount)
        {
            case 0:
                mag = 1;
                break;

            case 1:
                mag = 1.05f;
                break;

            case 2:
                mag = 1.1f;
                break;

            case 3:
                mag = 1.15f;
                break;

            case 4:
                mag = 1.2f;
                break;

            case 5:
                mag = 1.25f;
                break;

            default:
                mag = 1.25f;
                break;
        }
    }
}
