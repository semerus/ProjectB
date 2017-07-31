using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer_Totem : HitScanner, ITimeHandler, IBattleHandler {

    #region ITimeHandler
    public void RunTime()
    {
        curDuration += Time.deltaTime;
        curtime_Between_Heal += Time.deltaTime;

        if(curtime_Between_Heal >= time_Between_Heal)
        {
            List<Hero> targetHeroes = Find_Heroes_InRange();
            if(targetHeroes.Count != 0)
            {
                foreach (Hero hero in targetHeroes)
                {
                    Heal(hero);
                }
            }
            curtime_Between_Heal = 0f;
        }
        
        if(curDuration >= duration)
        {
            Remove();
        }
    }
    #endregion

    #region IBattleHandler
    public Team Team
    {
        get { return team; }
    }
    
    public CharacterAction Action
    {
        get { return action; }
    }

    public void ReceiveDamage(IBattleHandler attacker, int damage)
    {
        curHp_Count--;

        if (curHp_Count <= 0)
            Remove();
    }
    
    public void ReceiveHeal(int heal)
    {
        ;
    }
    #endregion
    
    #region MonoBehaviours
    protected override void Awake()
    {
        summoner = GameObject.FindObjectOfType<Healer>();
        scanCollider = GetComponentInChildren<Collider2D>();

        friendlyBattleHandler = new List<IBattleHandler>();
        hostileBattleHandler = new List<IBattleHandler>();
        neutralBattleHandler = new List<IBattleHandler>();

        SetScanMask("Battle");

        //attribute
        team = Team.Friendly;
        action = CharacterAction.Idle;

        maxHp_Count = 2;
        curHp_Count = maxHp_Count;

        healingPower = 40;

        //time
        duration = 4f;
        curDuration = 0f;
        time_Between_Heal = 1f;
        curtime_Between_Heal = 0f;

        //Debug
        Set(Vector3.zero);
    }
    #endregion

    #region Field & Method
    protected Healer summoner;
    protected Team team;
    protected CharacterAction action;

    protected int maxHp_Count;
    protected int curHp_Count;

    [SerializeField]
    protected float duration;
    [SerializeField]
    protected float curDuration;
    protected float time_Between_Heal;
    protected float curtime_Between_Heal;
    protected int healingPower;

    protected void Heal(Character target)
    {
        target.ReceiveHeal(healingPower);

        if(summoner.ActiveSkills[0] is Healer_Summon_Totem_MovementSpeedBuff)
        {
            Buff_MoveSpeed_Ratio heroBuff = new Buff_MoveSpeed_Ratio(summoner, target, true, 0.2f, 2f, "MoveR+25%2sec");
        }
        else if(summoner.ActiveSkills[0] is Healer_Summon_Totem_AttackSpeedBuff)
        {
            Debug.LogError("You should inplement AttackSpeedBuff");
        }
    }
    
    protected List<Hero> Find_Heroes_InRange()
    {
        List<Hero> returnHeroes = new List<Hero>();

        ScanColliders();
        if(friendlyBattleHandler.Count != 0)
        {
            foreach(IBattleHandler friendly in friendlyBattleHandler)
            {
                if (friendly is Hero)
                {
                    Hero hero = friendly as Hero;
                    Heal(hero);
                }
            }
        }

        return returnHeroes;
    }

    public void Set(Vector3 setPosition)
    {
        this.transform.root.gameObject.SetActive(true);
        this.transform.root.position = setPosition;
        
        curDuration = 0f;
        curtime_Between_Heal = 0f;

        if (TimeSystem.GetTimeSystem().CheckTimer(this) == false)
            TimeSystem.GetTimeSystem().AddTimer(this);
    }

    public void Remove()
    {
        curDuration = 0f;
        curtime_Between_Heal = 0f;

        if (TimeSystem.GetTimeSystem().CheckTimer(this) == true)
            TimeSystem.GetTimeSystem().DeleteTimer(this);

        this.transform.root.gameObject.SetActive(false);
    }
    #endregion

}
