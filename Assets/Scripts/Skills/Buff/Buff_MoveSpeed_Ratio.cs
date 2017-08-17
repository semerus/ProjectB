using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_MoveSpeed_Ratio : Buff, ITimeHandler, IBuff_MoveSpeed_Ratio
{
    #region Override Buff & Class Constructor
    public Buff_MoveSpeed_Ratio(Character caster, Character target, bool isBuff, float ratioValue, float durationValue, string name)
    {
        // set initial Value
        this.caster = caster;
        this.target = target;
        this.isBuff = isBuff;
        moveSpeed_Ratio = ratioValue;
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
                    if (eachBuff is IBuff_MoveSpeed_Ratio)
                    {
                        eachBuff.EndBuff();
                        continue;
                    }
                    // Add more counter Case
                }

                //referesh check
                if(eachBuff is Buff_MoveSpeed_Ratio && eachBuff.Caster is Healer)
                {
                    eachBuff.EndBuff();
                }
            }
        }
    }
    #endregion

    #region IBuff_ReceiveDmg_Ratio Implement
    private float moveSpeed_Ratio;
    public float MoveSpeed_Ratio
    {
        get { return moveSpeed_Ratio; }
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
