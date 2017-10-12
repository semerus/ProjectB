using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignTrap_Ability1 : MonoBehaviour,ITimeHandler {

    float time = 0;

    public void RunTime()
    {
        time += Time.deltaTime;
        if(time>10)
        {
            TimeSystem.GetTimeSystem().DeleteTimer(this);
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        time = 0;
        TimeSystem.GetTimeSystem().AddTimer(this);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.transform.root.transform.GetComponent<Team>() == Team.Hostile)
        {
            // 궁수 4초간 공격속도 20퍼 증가버프
        }
    }
}
