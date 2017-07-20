using UnityEngine;

public interface IBattleHandler {
	Team Team { get;}
	CharacterAction Action { get; }
	void ReceiveDamage(IBattleHandler attacker, int damage);
	void ReceiveHeal(int heal);
}
