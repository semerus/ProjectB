/*
 * Written by Insung Kim
 * Updated: 2017.08.13
 */
using System.Collections.Generic;
using UnityEngine;

public abstract class Hero : Character, ITapHandler, IDragDropHandler {

	public Skill autoAttack;
	public Skill passiveSkill;
	public HeroActive[] activeSkills = new HeroActive[3];

    protected HeroUI heroUI;
	protected int[] masks = new int[3] { 1 << 9, 1 << 8, 1 << 10 }; // enemy -> hero -> control Area

    //protected const float speed_x_1Value = 2.57f;
    //protected const float speed_y_1Value = 1.4f;


    #region Getters and Setter
    public Skill[] ActiveSkills
    {
        get { return activeSkills; }
    }

    public Skill PassiveSkill
    {
        get { return passiveSkill; }
    }
    #endregion

    #region implemented abstract members of Character

    protected override void UpdateHpUI ()
	{
		float percent = (float)hp / (float)maxHp;
		heroUI.UpdateHp (percent);
	}

	protected override void UpdateCCUI() {
		heroUI.UpdateCC (status);
	}
	#endregion

	#region ITapHandler implementation
	public void OnTap ()
	{
		DisplaySkill ();
	}
	#endregion

	#region IDragDropHandler implementation

	public void OnBeginDrag ()
	{
		OnTap ();
	}

	public void OnDrag (Vector3 pixelPos)
	{
		if (Action == CharacterAction.Channeling) {
			return;
		}

		PointerType type = PointerType.Move;
		Vector3 p = Camera.main.ScreenToWorldPoint (pixelPos);
		p = new Vector3 (p.x, p.y, 0f);

		Ray ray = Camera.main.ScreenPointToRay (pixelPos);
		IBattleHandler b = null;

		for (int i = 0; i < masks.Length; i++) {
			RaycastHit2D hitInfo = Physics2D.GetRayIntersection (ray, Mathf.Infinity, masks[i]);
			if (hitInfo.collider != null) {
				b = hitInfo.collider.transform.root.GetComponent<IBattleHandler> ();
				if (b != null && b.Team != Team.Friendly) {
					this.target = b;
					type = PointerType.Attack;
				} else {
					type = PointerType.Move;
				}
				break;
			}
		}
		if (b == null) {
			type = PointerType.Move;
		}
		p = SetAdjustPosition (p);
		Background.GetBackground ().PositionPointer (p, this, type);
	}

	// receives pixel coordinates
	public void OnDrop (Vector3 pixelPos)
	{
		if (Action == CharacterAction.Channeling) {
			return;
		}

		// move only if it is moveable state
		Vector3 p = new Vector3();
		Ray ray = Camera.main.ScreenPointToRay (pixelPos);
		IBattleHandler b = null;

		MoveEventArgs m = new MoveEventArgs (false, transform.position);
		OnMoveComplete (m);

		for (int i = 0; i < masks.Length; i++) {
			RaycastHit2D hitInfo = Physics2D.GetRayIntersection (ray, Mathf.Infinity, masks[i]);
			if (hitInfo.collider != null) {
				b = hitInfo.collider.transform.root.GetComponent<IBattleHandler> ();
				if (b != null && b.Team != Team.Friendly) {
					this.target = b;
					AutoAttack (b);
				} else {
					p = Camera.main.ScreenToWorldPoint (pixelPos);
					StopMove ();
					BeginMove(new Vector3(p.x, p.y, 0f));
                    RemoveTarget();
				}
				break;
			}
		}
		if (b == null) {
			p = Camera.main.ScreenToWorldPoint (pixelPos);
            p.z = 0f;
			//p = CalculatePosition(new Vector3(p.x, p.y, 0f));
			StopMove ();
			BeginMove (p);
            RemoveTarget();
        }
		Background.GetBackground ().DeactivatePointer (this);
	}

    #endregion
    
	public override void RunTime ()
	{
		base.RunTime ();
		/*
		if (autoAttack != null) {
			if (autoAttack.CheckSkillStatus(SkillStatus.ReadyMask) && action == CharacterAction.Attacking)
			{
				autoAttack.OnCast();
			}
		}
		*/
    }

	public override void ReceiveDamage (IBattleHandler attacker, int damage)
	{
		int receivedDamage = Calculator.ReceiveDamage(this, damage);

		foreach (Buff eachbuff in buffs)
		{
			if (eachbuff is Buff_Link_ProtectionArea)
			{
				Buff_Link_ProtectionArea buff = eachbuff as Buff_Link_ProtectionArea;
				if (buff.healer_ProtectionArea.LinkerState == LinkerState.OnLink || buff.healer_ProtectionArea.LinkerState == LinkerState.willBreak)
				{
					buff.healer_ProtectionArea.ReceiveDamage(attacker, receivedDamage);
					return;
				}
				else
				{
					buff.EndBuff();
				}
			}
		}
		base.ReceiveDamage (attacker, damage);
	}

    public void RemoveTarget()
    {
        this.target = null;
    }

	public override void Spawn (Dictionary<string, object> data, int[] skills)
	{
		team = Team.Friendly;
		heroUI = GetComponentInChildren<HeroUI> ();
		base.Spawn (data, skills);
	}

	protected override void KillCharacter ()
	{
		CloseSkill ();
		base.KillCharacter ();
	}

	public virtual void AutoAttack(IBattleHandler target) {
		this.target = target;
		autoAttack.OnCast ();
	}

	public void CancelCurrentAction() {
		autoAttack.CancelSkill ();
	}


    protected void DisplaySkill(){
		UIManager.GetUIManager ().SkillPanel.OpenSkills (activeSkills, this);
	}

	private void CloseSkill() {
		if (UIManager.GetUIManager ().SkillPanel.CurrentHero == this) {
			UIManager.GetUIManager ().SkillPanel.CloseSkills ();
		}
	}

	/// <summary>
	/// Calculates the correct position inside walking boundaries
	/// </summary>
	/// <returns>The position.</returns>
	/// <param name="target">Target.</param>
	private Vector3 CalculatePosition(Vector3 target) {
		int layermask = 1 << 10;
        
		RaycastHit2D hitInfo = Physics2D.Linecast (target, transform.position, layermask);
		Debug.DrawLine (transform.position, target);

		if (hitInfo.collider == null)
			return target;
		else
			return hitInfo.point;
	}
}
