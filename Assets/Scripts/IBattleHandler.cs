using UnityEngine;

public interface IBattleHandler {
	void ReceiveDamage(int damage);
	void ReceiveHeal(int heal);
}
