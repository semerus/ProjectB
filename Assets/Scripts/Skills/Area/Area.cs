using UnityEngine;

public abstract class Area : MonoBehaviour {
	public Character caster; // <- change to protected later
	protected Collider2D col;

	protected virtual void Awake() {
		col = GetComponentInChildren<Collider2D> ();
	}

	public void SetArea(Character caster) {
		this.caster = caster;
	}

	protected void Activate() {
		
	}
}


