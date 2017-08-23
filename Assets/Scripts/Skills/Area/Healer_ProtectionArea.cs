using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer_ProtectionArea : MonoBehaviour, ITimeHandler {

    #region ITimeHandler
    public void RunTime()
    {
        curDuration += Time.deltaTime;
        
        switch(linkerState)
        {
            case LinkerState.willBreak:
                linkerState = LinkerState.OffLink;
                break;

            case LinkerState.OffLink:
                Remove();
                return;

            default:
                break;
        }

        if(curDuration >= duration)
        {
            Remove();
        }
    }
    #endregion

    #region implement part of IBattleHandler
    public void ReceiveDamage(IBattleHandler attacker, int damage)
    {
        curHp -= damage;

        if (curHp <= 0)
            linkerState = LinkerState.willBreak;
    }
    #endregion

    #region MonoBehaviours
    protected virtual void Awake()
    {
        // hit scanner check
        if (hitScanner == null)
            hitScanner = transform.root.GetComponentInChildren<HitScanner>();
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Battle"))
        {
            // middle point check!

            Hero hero = other.transform.root.GetComponent<Hero>();

            if(hero != null)
            {
                Buff_Link_ProtectionArea link_Buff = new Buff_Link_ProtectionArea(this, hero, "Helaer_ProtectionArea");
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Battle"))
        {
            Hero hero = other.transform.root.GetComponent<Hero>();

            if (hero != null)
            {
                foreach (Buff_Link_ProtectionArea link_buff in hero.Buffs)
                    link_buff.EndBuff();
            }
        }
    }
    #endregion

    #region Getters & Setters
    public LinkerState LinkerState
    {
        get { return linkerState; }
    }

    public List<Hero> LinkedHeroes
    {
        get { return linkedHeroes; }
    }

    #endregion


    #region Field & Method
    [SerializeField]
    protected HitScanner hitScanner;

    protected int maxHp;
    protected int curHp;
    
    protected float duration;
    
    protected float curDuration;
    protected List<Hero> linkedHeroes;

    protected LinkerState linkerState;

    protected virtual void Refresh_Value()
    {
        //Attribute
        maxHp = 300;
        curHp = maxHp;
        linkerState = LinkerState.OnLink;

        //time
        duration = 2f;
        curDuration = 0f;
    }

    public virtual void Set(Vector3 setPosition)
    {
        Refresh_Value();

        this.transform.root.gameObject.SetActive(true);
        this.transform.root.position = setPosition;
        

        if (TimeSystem.GetTimeSystem().CheckTimer(this) == false)
            TimeSystem.GetTimeSystem().AddTimer(this);
    }

    public void Remove()
    {
        if (TimeSystem.GetTimeSystem().CheckTimer(this) == true)
            TimeSystem.GetTimeSystem().DeleteTimer(this);

        this.transform.root.gameObject.SetActive(false);
    }
    #endregion

}
