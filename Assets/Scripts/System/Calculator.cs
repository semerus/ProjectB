using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Calculate for modifed Dmg, Heal, Move, LifeSteal etc
/// </summary>
public class Calculator {
   
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mover"> it is for </param>
    /// <param name="baseVector"></param>
    /// <returns></returns>
    public static float MoveSpeed(Character mover)
    {
        float returnSpeed = 1f;

        float sum_MoveSpeed_Ratio = 0f;
        
        for(int i = 0; i < mover.Buffs.Count; i++)
        {
            IBuff_MoveSpeed_Ratio buff = mover.Buffs[i] as IBuff_MoveSpeed_Ratio;
            if (buff != null)
                sum_MoveSpeed_Ratio += buff.MoveSpeed_Ratio;
        }

        returnSpeed += sum_MoveSpeed_Ratio;

        return returnSpeed;
    }

    public static int AttackDamage(Character caster, int baseDmg)
    {
        int returnValue = baseDmg;

        int sum_Attack_Abs = 0;
        float sum_Attack_Ratio = 0f;
        float sum_AllDmg_Ratio = 0f;
        
        for(int i = 0; i < caster.Buffs.Count; i++)
        {
            IBuff_AttackDmg_Abs buff = caster.Buffs[i] as IBuff_AttackDmg_Abs;
            if (buff != null)
                sum_Attack_Abs += buff.AttackDmg_Abs;

            IBuff_AttackDmg_Ratio buff2 = caster.Buffs[i] as IBuff_AttackDmg_Ratio;
            if (buff2 != null)
                sum_Attack_Ratio += buff2.AttackDmg_Ratio;

            IBuff_AllDmg_Ratio buff3 = caster.Buffs[i] as IBuff_AllDmg_Ratio;
            if (buff3 != null)
                sum_AllDmg_Ratio += buff3.AllDmg_Ratio;
        }

        returnValue = (int)((baseDmg + sum_Attack_Abs) * (1 + sum_Attack_Ratio) * (1 + sum_AllDmg_Ratio));
        
        return returnValue;
    }

    public static int SkillDamage(Character caster, int baseDmg)
    {
        int returnValue = baseDmg;
        
        float sum_Skill_Ratio = 0f;
        float sum_AllDmg_Ratio = 0f;

        for (int i = 0; i < caster.Buffs.Count; i++)
        {
            IBuff_SkillDmg_Ratio buff = caster.Buffs[i] as IBuff_SkillDmg_Ratio;
            if (buff != null)
                sum_Skill_Ratio += buff.SkillDmg_Ratio;

            IBuff_AllDmg_Ratio buff2 = caster.Buffs[i] as IBuff_AllDmg_Ratio;
            if (buff2 != null)
                sum_AllDmg_Ratio += buff2.AllDmg_Ratio;
        }

        returnValue = (int)(baseDmg * (1 + sum_Skill_Ratio) * (1 + sum_AllDmg_Ratio));

        return returnValue;
    }

    public static int AllDamage(Character caster, int baseDmg)
    {
        int returnValue = baseDmg;
        
        float sum_AllDmg_Ratio = 0f;

        for (int i = 0; i < caster.Buffs.Count; i++)
        {
            IBuff_AllDmg_Ratio buff = caster.Buffs[i] as IBuff_AllDmg_Ratio;
            if (buff != null)
                sum_AllDmg_Ratio += buff.AllDmg_Ratio;
        }

        returnValue = (int)(baseDmg * (1 + sum_AllDmg_Ratio));

        return returnValue;
    }


    public static int ReflectionDamage(Character target, int baseDmg)
    {
        int returnValue = baseDmg;

        float sum_Reflection_Ratio = 0f;

        for (int i = 0; i < target.Buffs.Count; i++)
        {
            IBuff_ReflectionDmg_Ratio buff = target.Buffs[i] as IBuff_ReflectionDmg_Ratio;
            if (buff != null)
                sum_Reflection_Ratio += buff.ReflectionDmg_Ratio;
        }

        returnValue = (int)(returnValue * sum_Reflection_Ratio);

        return returnValue;
    }

    public static int ReceiveDamage(Character target, int baseDmg)
    {
        int returnValue = baseDmg;

        float sum_Receive_Ratio = 0f;
        int sum_Receive_Abs = 0;

        for (int i = 0; i < target.Buffs.Count; i++)
        {
            IBuff_ReceiveDmg_Abs buff = target.Buffs[i] as IBuff_ReceiveDmg_Abs;
            if (buff != null)
                sum_Receive_Abs += buff.ReceiveDmg_Abs;

            IBuff_ReceiveDmg_Ratio buff2 = target.Buffs[i] as IBuff_ReceiveDmg_Ratio;
            if (buff2 != null)
                sum_Receive_Ratio += buff2.ReceiveDmg_Ratio;
        }

        returnValue = (int)((returnValue + sum_Receive_Abs) * (1 + sum_Receive_Ratio));

        return returnValue;
    }

    public static int LifeSteal(Character caster, int baseDmg)
    {
        int returnValue = 0;

        float sum_LifeSteal_Ratio = 0;
        int sum_LifeSteal_Abs = 0;

        for (int i = 0; i < caster.Buffs.Count; i++)
        {
            IBuff_LifeSteal_Ratio buff = caster.Buffs[i] as IBuff_LifeSteal_Ratio;
            if (buff != null)
                sum_LifeSteal_Ratio += buff.LifeSteal_Ratio;

            IBuff_LifeSteal_Abs buff2 = caster.Buffs[i] as IBuff_LifeSteal_Abs;
            if (buff2 != null)
                sum_LifeSteal_Abs  += buff2.LifeSteal_Abs;
        }

        returnValue = (int)(baseDmg * (0 + sum_LifeSteal_Ratio)) + sum_LifeSteal_Abs;

        return returnValue;
    }

    public static int HealReceive(Character target, int baseHeal)
    {
        int returnValue = baseHeal;

        float sum_Heal_Ratio = 0;

        for (int i = 0; i < target.Buffs.Count; i++)
        {
            IBuff_Heal_Ratio buff = target.Buffs[i] as IBuff_Heal_Ratio;
            if (buff != null)
                sum_Heal_Ratio += buff.Heal_Ratio;
        }

        returnValue = (int)(returnValue * sum_Heal_Ratio);

        return returnValue;
    }
}
