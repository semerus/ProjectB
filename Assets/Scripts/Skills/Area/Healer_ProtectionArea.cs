using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer_ProtectionArea : MonoBehaviour, ITimeHandler {

	public Character caster;

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
                RemoveArea();
                return;

            default:
                break;
        }

        if(curDuration >= duration)
        {
            RemoveArea();
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
				Buff_Link_ProtectionArea link_Buff = new Buff_Link_ProtectionArea (this, hero, "Healer_ProtectionArea");
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
				RemoveBuff (hero);
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
	protected List<Hero> linkedHeroes = new List<Hero> ();

    protected LinkerState linkerState;

	public void SetValue(int shield, float duration, Character caster) {
		this.caster = caster;
		this.maxHp = shield;
		this.duration = duration;
	}

    protected virtual void Refresh_Value()
    {
        //Attribute
        curHp = maxHp;
        linkerState = LinkerState.OnLink;
		linkedHeroes.Clear ();

        //time
        curDuration = 0f;
    }

    public virtual void Set(Vector3 setPosition)
    {
        Refresh_Value();
        this.transform.root.gameObject.SetActive(true);
        this.transform.root.position = setPosition;
        
		TimeSystem.GetTimeSystem ().AddTimer (this);
    }

	protected void RemoveBuff(Hero hero) {
		for (int i = 0; i < hero.Buffs.Count; i++) {
			Buff_Link_ProtectionArea link = hero.Buffs [i] as Buff_Link_ProtectionArea;
			if (link != null) {
				link.EndBuff ();
			}
		}
		/*
		foreach (Buff buff in hero.Buffs) {
			Buff_Link_ProtectionArea link = buff as Buff_Link_ProtectionArea;
			if (link != null) {
				link.EndBuff();
			}
		}
		*/
	}

    public void RemoveArea() {
		for (int i = 0; i < linkedHeroes.Count; i++) {
			RemoveBuff (linkedHeroes [i]);
		}
        this.transform.root.gameObject.SetActive(false);
		TimeSystem.GetTimeSystem().DeleteTimer(this);
    }
    #endregion

}
