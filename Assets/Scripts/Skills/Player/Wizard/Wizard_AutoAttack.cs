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
    public Character target = null;
    public bool start = false;
    public float timer = 0;

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
            GameObject p = Instantiate(Resources.Load<GameObject>("Skills/Heroes/Wizard/AutoAttack/AutoAttack"));
            p.transform.SetParent(GameObject.Find("Projectiles").transform);
            projectiles = p.gameObject.GetComponent<Projectile>();

            projectiles.ProjectileOn(caster, this);
            projectiles.ProjectileMove(target, 3);
        }
        else
        {
            projectileNum.Pop().PopDo(target) ;
        }
    }

<<<<<<< HEAD
    public override void RunTime()
    {
        base.RunTime();
        timer += Time.deltaTime;
        if(timer>=0.5 && caster.Action == CharacterAction.Attacking)
        {
            caster.ChangeAction(CharacterAction.Idle);
        }
    }

    public override void Activate()
    {
        if(!start && caster.Target!=null)
        {
            start = true;
        }
        if(start)
        {
            if (caster.Action == CharacterAction.Idle)
            {
                timer = 0;
                TimeSystem.GetTimeSystem().AddTimer(this);
                caster.ChangeAction(CharacterAction.Attacking);
                if (target == null)
                {
                    target = caster.Target as Character;
                }
                if (caster.Target != null)
                {
                    if (caster.Target.Action == CharacterAction.Dead)
                    {
                        caster.ChangeAction(CharacterAction.Idle);
                    }
                }
                if (target.transform.position.x >= this.gameObject.transform.position.x)
                {
                    caster.IsFacingLeft = false;
                }
                else
                {
                    caster.IsFacingLeft = true;
                }
                ProjectileStack();
                cooldown = 2;
                ProjectileStack();
                StartCoolDown();
            }
        }
=======
    public void AutoAttack()
    {
        IBattleHandler target = caster.Target;
        caster.AttackTarget(target, damage);
>>>>>>> 96a441a56d03b4f6eda8cbf73eb63b00e7d93ad2
    }
}
