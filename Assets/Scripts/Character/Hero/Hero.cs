using UnityEngine;

public abstract class Hero : Character, ITapHandler, IDragDropHandler {

	protected Skill autoAttack;
    protected Skill passiveSkill;
    protected Skill[] activeSkills;

    protected HeroUI heroUI; // load it from spawn
	protected int[] masks = new int[3] { 1 << 8, 1 << 9, 1 << 10 };

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
					AutoAttack (b);
				} else {
					p = Camera.main.ScreenToWorldPoint (pixelPos);
					Move(new Vector3(p.x, p.y, 0f));
                    RemoveAttackTarget();
				}
				break;
			}
		}
		if (b == null) {
			p = Camera.main.ScreenToWorldPoint (pixelPos);
			p = CalculatePosition(new Vector3(p.x, p.y, 0f));
			Move (p);
            RemoveAttackTarget();
		}
		Background.GetBackground ().DeactivatePointer (this);
	}

    #endregion
    
    // not in use now
    public virtual void SetSkill(Skill[] skills) {
		
	}

	public override void Spawn ()
	{
		base.Spawn ();
		heroUI = GetComponentInChildren<HeroUI> ();
	}

    public abstract void AutoAttack(IBattleHandler target);

    public void RemoveAttackTarget()
    {
        this.target = null;
    }

    protected void DisplaySkill(){
		// display skill hud
	}

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
