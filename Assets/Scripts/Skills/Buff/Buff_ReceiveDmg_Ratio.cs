using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_ReceiveDmg_Ratio : Buff, ITimeHandler, IBuff_ReceiveDmg_Ratio
{
    #region Override Buff & Class Constructor
    public Buff_ReceiveDmg_Ratio(Character caster, Character target, bool isBuff, float ratioValue, float durationValue, string name)
    {
        // set initial Value
        this.caster = caster;
        this.target = target;
        this.isBuff = isBuff;
        receiveDmg_Ratio = ratioValue;
        duration = durationValue;

        curDuration = 0f;

        StartBuff(this.caster, this.target);
        
        // for Debugging
        buffName = name;
        target.UpdateBuffList();
    }

    public override void StartBuff(Character caster, Character target)
    {
        base.StartBuff(caster, target);
        TimeSystem.GetTimeSystem().AddTimer(this as ITimeHandler);
    }

    public override void CheckCounterBuff()
    {
        if (target.Buffs.Count != 0)
        {
            foreach (Buff eachBuff in target.Buffs)
            {
                if ((this.isBuff == true && eachBuff.Isbuff == false) || (this.isBuff == false && eachBuff.Isbuff == true))
                {
                    if (eachBuff is IBuff_ReceiveDmg_Ratio)
                    {
                        eachBuff.EndBuff();
                        continue;
                    }
                    // Add more counter Case
                }
            }
        }
    }
    #endregion

    #region IBuff_ReceiveDmg_Ratio Implement
    private float receiveDmg_Ratio;
    public float ReceiveDmg_Ratio
    {
        get { return receiveDmg_Ratio; }
    }
    #endregion

    #region ITimeHandler Implement
    private float duration;
    private float curDuration;

    public void RunTime()
    {
        curDuration += Time.deltaTime;

        if (curDuration >= duration)
            EndBuff();
    }
    #endregion
}
