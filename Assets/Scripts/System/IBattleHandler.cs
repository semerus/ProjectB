using UnityEngine;

public interface IBattleHandler {
	Team Team { get;}
	CharacterState State { get;}
	void ReceiveDamage(int damage);
	void ReceiveHeal(int heal);
}
