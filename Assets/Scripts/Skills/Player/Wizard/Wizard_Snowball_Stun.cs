using UnityEngine;

public class Wizard_Snowball_Stun : Wizard_Snowball {

	float stunTime;

	public override void OnInterrupt (IBattleHandler interrupter)
	{
		base.OnInterrupt (interrupter);
		DeleteProjectile();
		caster.ChangeAction(CharacterAction.Idle);
	}

	public override void SetSkill (System.Collections.Generic.Dictionary<string, object> param)
	{
		base.SetSkill (param);
		this.stunTime = (float)((double)param ["stun_time"]);
	}

	protected override void SetSnowProjectile ()
	{
		base.SetSnowProjectile ();
		GameObject gm = projectile [4].gameObject;
		Destroy (projectile[4]);
		projectile[4] = gm.AddComponent<SnowBall_Stun> ();
		gm.SetActive (false);
	}

	protected override void CreateSnowBall ()
	{
		if (ballCount < 4) {
			SetSnowBall (projectile [ballCount++]);
		} else {
			// if count is 5 shoot(last ball is stun)
			SnowBall_Stun stunBall = projectile[ballCount] as SnowBall_Stun;
			if (stunBall != null) {
				SetStunBall (stunBall);
			} else {
				SetSnowBall (projectile [ballCount]);
			}

			StartCoolDown();
			UpdateSkillStatus(SkillStatus.ChannelingOff);
            caster.ChangeAction(CharacterAction.Idle);
            UpdateSkillStatus(SkillStatus.ProcessOn);
			SlowMotion();
		}
	}

	protected void SetStunBall(SnowBall_Stun ball) {
		ball.ProjectileOn (caster);
		ball.SetProjectile (target, damage, speed, stunTime);
	}

	protected void DeleteProjectile() {
		for (int i = 0; i < projectile.Length; i++)
		{
			projectile[i].DeleteProjectile();
		}
	}
}
