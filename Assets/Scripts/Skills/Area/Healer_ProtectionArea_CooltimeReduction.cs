using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer_ProtectionArea_CooltimeReduction : Healer_ProtectionArea {
    public override void Set(Vector3 setPosition)
    {
        base.Set(setPosition);

        hitScanner.ScanColliders();
        List<IBattleHandler> battleHandlers = hitScanner.GetFriendly;

        foreach(IBattleHandler b in battleHandlers)
        {
            Hero hero = b as Hero;
            if(hero != null)
            {
                foreach (HeroActive activeSkill in hero.ActiveSkills)
                {
                    activeSkill.AddCooltime(2f);
                }

                hero.PassiveSkill.AddCooltime(2f);
            }
             
        }
    }
}
