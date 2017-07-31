using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer_Shield : HitScanner, ITimeHandler, IBattleHandler {

    #region ITimeHandler
    public void RunTime()
    {
        curDuration += Time.deltaTime;
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
        curHp  -= damage;
        if (curHp <= 0)
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
        
        //time
        duration = 2f;
        curDuration = 0f;
    }
    #endregion

    #region Field & Method
    protected Healer summoner;
    protected Team team;
    protected CharacterAction action;

    protected int maxHp;
    protected int curHp;
    
    protected float duration;
    protected float curDuration;

    protected void Link(Character target)
    {
        ;
    }

    protected List<Hero> Find_Heroes_InRange()
    {
        List<Hero> returnHeroes = new List<Hero>();

        ScanColliders();
        if (friendlyBattleHandler.Count != 0)
        {
            foreach (IBattleHandler friendly in friendlyBattleHandler)
            {
                if (friendly is Hero)
                {
                    Hero hero = friendly as Hero;
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
