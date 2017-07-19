using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BuffBit {

    // None Buff  == 0
    // Buff should be 1 <<(2n-1)
    // Debuff should be 1<<(2n)

    public const int None = 0;
    public const int MaxHp_Buff = 1 << 1;
    public const int MaxHP_Debuff = 1 << 2;
    public const int AttackDmg_Buff = 1 << 3;
    public const int AttackDmg_Debuff = 1 << 4;
    public const int SkillDmg_Buff = 1 << 5;
    public const int SkillDmg_Debuff = 1 << 6;
    public const int AllDmg_Buff = 1 << 7;
    public const int AllDmg_Debuff = 1 << 8;
    public const int MovementSpeed_Buff = 1 << 9;
    public const int MovementSpeed_Debuff = 1 << 10;
    public const int AttackSpeed_Buff = 1 << 11;
    public const int AttackSpeed_Debuff = 1 << 12;
    public const int ReceiveDmg_Ratio_Buff = 1 << 13;
    public const int ReceiveDmg_Ratio_Debuff = 1 << 14;
    public const int ReceiveDmg_Abs_Buff = 1 << 15;
    public const int ReceiveDmg_Abs_Debuff = 1 << 16;
    public const int CoolTime_Buff = 1 << 17;
    public const int CoolTime_Debuff = 1 << 18;
    public const int ReflectionDmg_Buff = 1 << 19;
    public const int ReflectionDmg_Debuff = 1 << 20;


    // Mask
    public const int IsValueBuff = 2147483646; // all 1 except  0*2^0
    public const int IsBuff = 715827882;
    public const int IsDebuff = 1431655764;


}
