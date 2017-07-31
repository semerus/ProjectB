using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer_Potion : HitScanner, ITimeHandler {

    #region Implement ITimeHandler
    public void RunTime()
    {
        curDuration += Time.deltaTime;
        
        if(curDuration >= duration)
        {
            Remove();
        }
    }
    #endregion

    #region MonoBehaviours
    protected override void Awake()
    {
        // Collider Setting
        scanCharacter = GameObject.FindObjectOfType<Healer>();
        scanCollider = GetComponentInChildren<Collider2D>();

        friendlyBattleHandler = new List<IBattleHandler>();
        hostileBattleHandler = new List<IBattleHandler>();
        neutralBattleHandler = new List<IBattleHandler>();

        SetScanMask("Battle");

        // Time system setting
        duration = 5f;
        curDuration = 0f;

        // Skill value Setting
        healingPower = 50;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ScanColliders();
        if(friendlyBattleHandler.Count != 0)
        {
            foreach(IBattleHandler eachBattler in friendlyBattleHandler)
            {
                if(eachBattler is Hero)
                {
                    Activate(eachBattler as Hero);
                    return;
                }
            }
        }

    }
    #endregion

    #region Field & Method

    int healingPower;
    float duration;
    [SerializeField]
    float curDuration;

    public void Activate(Hero target)
    {
        target.ReceiveHeal(healingPower);
        Remove();
    }

    public void Set(Vector3 setPosition)
    {
        this.transform.root.position = setPosition;

        this.transform.root.gameObject.SetActive(true);

        curDuration = 0f;

        if (TimeSystem.GetTimeSystem().CheckTimer(this) == false)
            TimeSystem.GetTimeSystem().AddTimer(this);
    }

    public void Remove()
    {
        curDuration = 0f;

        if (TimeSystem.GetTimeSystem().CheckTimer(this) == true)
            TimeSystem.GetTimeSystem().DeleteTimer(this);

        this.transform.root.gameObject.SetActive(false);
    }
    

    #endregion

}
