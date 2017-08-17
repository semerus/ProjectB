using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Passive : HeroActive {

    public static Archer_Passive instance;
    List<Character> TargetList = new List<Character>();

    public static Archer_Passive GetArcher_Passive()
    {
        if(!instance)
        {
            instance = GameObject.FindObjectOfType<Archer_Passive>();
        }
        return instance;
    }

    public void AddTargetList(Character target)
    {
        if(TargetList.Count>=1)
        {
            if(TargetList[TargetList.Count - 1] != target)
            {
                TargetList.Clear();
                TargetList.Add(target);
            }
            else
            {
                TargetList.Add(target);
            }
        }
        else
        {
            TargetList.Add(target);
        }
    }

    public void CheckTargetList()
    {
        if(TargetList.Count>=4)
        {
            int damage = 20;
            caster.AttackTarget(TargetList[0] as IBattleHandler, damage);
            TargetList.Clear();
        }
    }

    private void Awake()
    {
        TimeSystem.GetTimeSystem().AddTimer(this);
    }

    public override void RunTime()
    {
        base.RunTime();
        CheckTargetList();
    }

}
