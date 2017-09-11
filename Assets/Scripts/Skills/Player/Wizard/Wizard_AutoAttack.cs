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
    public Character AtkTarget = null;
	private int damage;
    private float speed;

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

	public override void SetSkill (Dictionary<string, object> param)
	{
		base.SetSkill (param);
		damage = (int)param ["damage"];
        speed = (float)((double)param["speed"]);
	}

    public void ProjectileStack()
    {
        if (projectileNum.Count < 1)
        {
            GameObject p = Instantiate(Resources.Load<GameObject>("Skills/Heroes/Wizard/AutoAttack/AutoAttack"));
            p.transform.SetParent(GameObject.Find("Projectiles").transform);
            projectiles = p.gameObject.GetComponent<Projectile>();
            projectiles.SetProjectile(AtkTarget, damage, speed);
            projectiles.ProjectileOn(caster, this);
            projectiles.ProjectileMove(AtkTarget);
        }
        else
        {
            projectileNum.Pop().PopDo(AtkTarget) ;
        }
    }

    public override bool CheckCondition()
    {
		bool isReady = false;
        if(AtkTarget==null)
        {
            AtkTarget = caster.Target as Character;
        }

        if(AtkTarget==null)
        {
            isReady = false;
        }
        else
        {
            if (AtkTarget.Action == CharacterAction.Dead)
            {
                AtkTarget = null;
                isReady = false;
            }
            else
            {
                isReady = true;
            }
        }
        return isReady && base.CheckCondition();
    }

    public void Facing()
    {
        //Debug.Log("facing");
        if (AtkTarget.transform.position.x >= this.gameObject.transform.position.x)
        {
            caster.IsFacingLeft = false;
            caster.CheckFacing();
        }
        else
        {
            caster.IsFacingLeft = true;
            caster.CheckFacing();
        }
    }

    public override void Activate()
    {
        if (caster.Target != null)
        {
            AtkTarget = caster.Target as Character;
        }
        Facing();
        caster.ChangeAction(CharacterAction.Attacking);
        caster.Anim.onCue += AutoAttack;
    }  

    public void AutoAttack()
    {
        StartCoolDown();
        ProjectileStack();
        caster.ChangeAction(CharacterAction.Idle);
        caster.Anim.onCue -= AutoAttack;
    }
}
