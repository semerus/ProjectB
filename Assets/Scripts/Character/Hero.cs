using UnityEngine;

public abstract class Hero : Character, ITapHandler, IDragDropHandler {

	protected IBattleHandler target;
	protected Skill autoAttack;
    protected Skill passiveSkill;
    protected Skill[] activeSkills;
    protected Transform attackTarget;

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
				AutoAttack (target);
			} else {
				p = Camera.main.ScreenToWorldPoint (pixelPos);
				Move (new Vector3(p.x, p.y, 0f));
			}
		} else {
			p = Camera.main.ScreenToWorldPoint (pixelPos);
			p = CalculatePosition(new Vector3(p.x, p.y, 0f));
			Move (p);
		}
	}

	#endregion

	public void SetSkill(Skill[] skills) {
		// set skill on load
	}

	public virtual void AutoAttack (IBattleHandler target) {
		Debug.Log ("autoattack by "+transform.root.name);
		// use skill inside
		// set target
		this.target = target;
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
