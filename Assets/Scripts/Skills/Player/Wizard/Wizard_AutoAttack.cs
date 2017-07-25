using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_AutoAttack : Skill, IPooling_Character
{
    public Stack<IPooledItem_Character> projectileNum = new Stack<IPooledItem_Character>();
    public Projectile projectiles= new Projectile();

    public Stack<IPooledItem_Character> Pool
    {
        get
        {
            return projectileNum;
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

    public override void Activate(IBattleHandler target)
    {
        ProjectileStack();
        cooldown = 2;
        StartCoolDown();
        AuttoAttack();
    }

    public void AuttoAttack()
    {
        int damage = 20;
        IBattleHandler target = caster.Target;
        caster.AttackTarget(target, damage);
    }
}
