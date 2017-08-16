/*
 * Written by JeongMin Seo, Insung Kim
 * Updated: 2017.08.13
 */
using System.Collections.Generic;
using UnityEngine;

public class Wizard_AutoAttack : Skill, IPooling_Character
{
    public Stack<IPooledItem_Character> projectileNum = new Stack<IPooledItem_Character>();
	public Projectile projectiles;

	private int damage;
	private GameObject projectile;

    public Stack<IPooledItem_Character> Pool
    {
        get
        {
            return projectileNum;
        }
    }

	void Awake() {
		caster = gameObject.GetComponent<Character> ();
		Hero h = caster as Hero;
		if (h != null) {
			h.autoAttack = this;
		}
	}

	public override void Activate()
	{
		if (caster.Target.Action == CharacterAction.Dead)
		{
			caster.ChangeAction (CharacterAction.Idle);
		}
		ProjectileStack();
		//cooldown = 2f;
		ProjectileStack();
		StartCoolDown();
	}

	public override void SetSkill (Dictionary<string, object> param)
	{
		base.SetSkill (param);
		damage = (int)param ["damage"];
		Hero hero = caster as Hero;
		if (hero != null) {
			hero.autoAttack = this;
		}
	}

    public void ProjectileStack()
    {
        if (projectileNum.Count < 1)
        {
            GameObject p = Instantiate(Resources.Load<GameObject>("aaa"));
            p.transform.SetParent(GameObject.Find("Projectiles").transform);
            projectiles = p.gameObject.GetComponent<Projectile>();

            projectiles.ProjectileOn(caster, this);
            projectiles.ProjectileMove(caster.Target as Character, 3);
        }
        else
        {
            projectileNum.Pop().PopDo(caster.Target as Character) ;
        }
    }

    public void AutoAttack()
    {
        IBattleHandler target = caster.Target;
        caster.AttackTarget(target, damage);
    }
}
