using UnityEngine;

public abstract class Character : MonoBehaviour, IBattleHandler {
	// id
	// team enum
	// hp
	// maxHp

	#region IBattleHandler implementation

	public void ReceiveDamage (int damage)
	{
		throw new System.NotImplementedException ();
	}

	public void ReceiveHeal (int heal)
	{
		throw new System.NotImplementedException ();
	}

	#endregion

	public abstract void Spawn ();
	protected void KillCharacter ();
	public void Move ();
}
