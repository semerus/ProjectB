using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_AutoAttack : HeroActive, IPooling_Character
{
    public Stack<IPooledItem_Character> projectileNum = new Stack<IPooledItem_Character>();
    public Projectile projectiles= new Projectile();
    public Projectile[] projectile = new Projectile[3];
    int y = 0;

    public Stack<IPooledItem_Character> Pool
    {
        get
        {
            return projectileNum;
        }
    }

    public void SetProjectile()
    {
        projectileNum.

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
        cooldown = 2;
        ProjectileStack();
        StartCoolDown();
    }

    public override void RunTime()
    {
        base.RunTime();
        Debug.Log(projectiles.CheckArrival());
        if(projectiles.CheckArrival())
        {
            AuttoAttack();
            projectiles.EndProjectile();
        }
    }

    public void AuttoAttack()
    {
        int damage = 500;
        IBattleHandler target = caster.Target;
        caster.AttackTarget(target, damage);
    }
}
