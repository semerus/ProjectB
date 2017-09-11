/*
 * Written by Insung Kim
 * Updated: 2017.09.11
 */
using UnityEngine;

public class SnowBall_Stun : SnowBall_Projectile {

	protected float stunTime;

	public void SetProjectile (IBattleHandler target, int damage, float speed, float stun)
	{
		base.SetProjectile (target, damage, speed);
		this.stunTime = stun;
	}

	public override void OnArrival (int abiility)
	{
		base.OnArrival (abiility);
		StunTarget ();
	}

	protected void StunTarget() {
		Character c = target as Character;
		if (c != null) {
			new Buff_Stun (stunTime, caster, c);
		}
	}

}
