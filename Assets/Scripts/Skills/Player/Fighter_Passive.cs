using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter_Passive : Skill {
    #region implemented abstract members of Skill

    // 수치를 받을 수 있는 무언가가 필요하다.
    public override void Activate(IBattleHandler target)
    {
        Character self = target as Character;
        self.ReceiveHeal((int)(dmg * 0.1f));
        print(self.name + " Healed " + (int)(dmg * 0.1f));
    }

    #endregion

    int dmg;
    public void SetDmg(int dmg)
    {
        this.dmg = dmg; 
    }


}