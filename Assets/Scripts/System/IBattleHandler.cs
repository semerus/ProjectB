using UnityEngine;

public interface IBattleHandler {
	Team Team { get;}
	int Status { get; }
	void ReceiveDamage(int damage);
	void ReceiveHeal(int heal);
}
