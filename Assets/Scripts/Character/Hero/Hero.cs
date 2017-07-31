using UnityEngine;

public abstract class Hero : Character, ITapHandler, IDragDropHandler {

	protected Skill autoAttack;
    protected Skill passiveSkill;
	protected HeroActive[] activeSkills;

    protected HeroUI heroUI; // load it from spawn
	protected int[] masks = new int[3] { 1 << 9, 1 << 8, 1 << 10 }; // enemy -> hero -> control Area

    protected const float speed_x_1Value = 2.57f;
    protected const float speed_y_1Value = 1.4f;


    #region Getters and Setter
    public Skill[] ActiveSkills
    {
        get { return activeSkills; }
    }
    #endregion

    #region implemented abstract members of Character

    protected override void UpdateHpUI ()
	{
		float percent = (float)hp / (float)maxHp;
		heroUI.UpdateHp (percent);
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
		Vector3 p = Camera.main.ScreenToWorldPoint (pixelPos);
		p = new Vector3 (p.x, p.y, 0f);

		Background.GetBackground ().PositionPointer (p, this);
	}

	// receives pixel coordinates
	public void OnDrop (Vector3 pixelPos)
	{
		// move only if it is moveable state
		Vector3 p = new Vector3();
		Ray ray = Camera.main.ScreenPointToRay (pixelPos);
		IBattleHandler b = null;
		for (int i = 0; i < masks.Length; i++) {
			RaycastHit2D hitInfo = Physics2D.GetRayIntersection (ray, Mathf.Infinity, masks[i]);
			if (hitInfo.collider != null) {
				b = hitInfo.collider.transform.root.GetComponent<IBattleHandler> ();
				if (b != null && b.Team != Team.Friendly) {
					this.target = b;
					AutoAttack (b);
				} else {
					p = Camera.main.ScreenToWorldPoint (pixelPos);
					BeginMove(new Vector3(p.x, p.y, 0f));
                    RemoveTarget();
				}
				break;
			}
		}
		if (b == null) {
			p = Camera.main.ScreenToWorldPoint (pixelPos);
			p = CalculatePosition(new Vector3(p.x, p.y, 0f));
			BeginMove (p);
            RemoveTarget();
        }
		Background.GetBackground ().DeactivatePointer (this);
	}

    #endregion
    
	public override void RunTime ()
	{
		base.RunTime ();
        if (autoAttack.CheckSkillStatus(SkillStatus.ReadyMask) && action == CharacterAction.Attacking)
        {
            autoAttack.OnCast();
        }
    }

    // not in use now
    public virtual void SetSkill(Skill[] skills) {
		
	}

    public void RemoveTarget()
    {
        this.target = null;
    }

	public override void Spawn ()
	{
		base.Spawn ();
		heroUI = GetComponentInChildren<HeroUI> ();
	}

	protected override void KillCharacter ()
	{
		CloseSkill ();
		base.KillCharacter ();
	}

	public virtual void AutoAttack(IBattleHandler target) {
		
		if (ChangeAction (CharacterAction.Attacking)) {
			this.target = target;
			//autoAttack.OnCast ();
		}
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
