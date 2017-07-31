using UnityEngine;

public abstract class Buff {
	protected Character caster;
	protected Character target;
    protected bool isBuff;
    public bool Isbuff
    {
        get { return isBuff; }
    }
    public Character Caster
    {
        get { return caster; }
    }

    #region Field for Debugging in Hierarchy
    protected string buffName;
    public string BuffName
    {
        get { return buffName; }
    }
    #endregion

    /// <summary>
    /// Starts the buff. Please base.StartBuff() when overriding
    /// </summary>
    /// <param name="caster">Caster.</param>
    /// <param name="target">Target.</param>
    /// 
    public virtual void StartBuff(Character caster, Character target) {
        // check for counterBuff
        CheckCounterBuff();

		// add to Buff list to target
		this.target = target; // can do this at construct method
		target.Buffs.Add(this);
        
		// check for status changes to the character
		IStatusBuff buff = this as IStatusBuff;
		if (buff != null) {
			if ((buff.Status & CharacterStatus.IsSilencedMask) > 0) {
				// interrupt all channeling
			}
			if ((buff.Status & CharacterStatus.IsRootedMask) > 0) {
				// stop all moving
				target.StopMove();
			}
		}

		target.RefreshBuff ();
        // add timer to TimeSystem if it implements ITimeHandler
	}

    public abstract void CheckCounterBuff();
    
	public virtual void EndBuff() {
		// delete timer to TimeSystem if it implements ITimeHandler
		ITimeHandler t = this as ITimeHandler;
		if (t != null) {
			TimeSystem.GetTimeSystem ().DeleteTimer (t);
		}
		// delete from Buff list
		if (target.Buffs.Remove (this)) {
			target.Anim.UpdateAnimation ();
			target.RefreshBuff ();
		}

		target.UpdateBuffList ();
	}

	/*
	private void AddBuff(Character target) {
		if (target.Buffs.Contains (this))
			return;
		target.Buffs.Add (this);
		target.RefreshBuff ();
	}
	*/
}
