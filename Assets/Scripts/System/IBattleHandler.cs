using UnityEngine;

public interface IBattleHandler {
	Team Team { get;}
	CharacterAction Action { get; }
	Transform Transform { get; }
	void ReceiveDamage(IBattleHandler attacker, int damage);
	void ReceiveHeal(int heal);
}
