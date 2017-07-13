using UnityEngine;

public abstract class Hero : Character, ITapHandler, IDragDropHandler {


	public Skill autoAttack;
    public Skill passiveSkill;
    public Skill[] activeSkills;

    protected HeroUI heroUI; // load it from spawn
    
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
		//Debugging.DebugWindow ("ontap");
		Debug.Log("on tap");
		DisplaySkill ();
	}
	#endregion

	#region IDragDropHandler implementation

	public void OnBeginDrag ()
	{
		throw new System.NotImplementedException ();
	}

	public void OnDrag (Vector3 pixelPos)
	{
		Vector3 p = Camera.main.ScreenToWorldPoint (pixelPos);
		p = new Vector3 (p.x, p.y, 0f);

		Background.GetBackground ().pointer.PositionPointer (p, transform.position);
	}

	// receives pixel coordinates
	public void OnDrop (Vector3 pixelPos)
	{
		// if move only one moveable state
		Vector3 p = new Vector3();
		Ray ray = Camera.main.ScreenPointToRay (pixelPos);
		RaycastHit2D hitInfo = Physics2D.GetRayIntersection (ray);
		if (hitInfo.collider != null) {
			IBattleHandler target = hitInfo.collider.transform.root.GetComponent<IBattleHandler> ();
			if (target != null && target.Team != Team.Friendly) {
                print("autoAttack case");
                queueState = CharacterState.AutoAttaking;
                AutoAttack (target);
			} else {
                p = Camera.main.ScreenToWorldPoint (pixelPos);
                queueState = CharacterState.None;
				Move (new Vector3(p.x, p.y, 0f));
			}
		} else {
            p = Camera.main.ScreenToWorldPoint (pixelPos);
			p = CalculatePosition(new Vector3(p.x, p.y, 0f));
            queueState = CharacterState.None;
            Move (p);
		}

		Background.GetBackground ().pointer.DeactivatePointer ();
	}

	#endregion
    
	public virtual void SetSkill(Skill[] skills) {
	}

	public override void Spawn ()
	{
		base.Spawn ();
		heroUI = GetComponentInChildren<HeroUI> ();
	}

	public virtual void AutoAttack (IBattleHandler target) {
        this.target = target;
        state = CharacterState.AutoAttaking;
		//autoAttack.Activate (this.target);
	}

	protected void DisplaySkill(){
		// display skill hud
	}

	private Vector3 CalculatePosition(Vector3 target) {
		int layermask = 1 << 8;
        // i don't know what this mean, explain me lol
		RaycastHit2D hitInfo = Physics2D.Linecast (target, transform.position, layermask);
		Debug.DrawLine (transform.position, target);

		if (hitInfo.collider == null)
			return target;
		else
			return hitInfo.point;
	}
}
