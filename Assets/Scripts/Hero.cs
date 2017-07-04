using UnityEngine;

public abstract class Hero : Character, ITapHandler, IDragDropHandler {

	protected IBattleHandler target;
	protected Skill autoAttack;

	#region implemented abstract members of Character
	public override void Spawn ()
	{
		throw new System.NotImplementedException ();
	}
	#endregion

	#region ITapHandler implementation
	public void OnTap ()
	{
		Debugging.DebugWindow ("ontap");
		DisplaySkill ();
	}
	#endregion

	#region IDragDropHandler implementation

	public void OnBeginDrag ()
	{
		throw new System.NotImplementedException ();
	}

	public void OnDrag ()
	{
		throw new System.NotImplementedException ();
	}

	// receives pixel coordinates
	public void OnDrop (Vector3 position)
	{
		Ray ray = Camera.main.ScreenPointToRay (position);
		RaycastHit2D hitInfo = Physics2D.GetRayIntersection (ray);
		if (hitInfo.collider != null) {
			IBattleHandler target = hitInfo.collider.transform.root.GetComponent<IBattleHandler> ();
			if (target != null) {
				AutoAttack (target);
			} else {
				Move (Camera.main.ScreenToWorldPoint (position));
			}
		} else {
			Move (Camera.main.ScreenToWorldPoint(position));
		}
	}

	#endregion

	public void SetSkill(Skill[] skills) {
		// set skill on load
	}
	public virtual void AutoAttack (IBattleHandler target) {
		Debug.Log ("autoattack");
		// use skill inside
		// set target
		this.target = target;
		autoAttack.Activate (this.target);
	}
	protected void DisplaySkill(){
		// display skill hud
	}
}
