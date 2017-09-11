using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_AutoAttack : HeroActive,IPooling_Character {

    public Stack<IPooledItem_Character> projectileNum = new Stack<IPooledItem_Character>();
    public static Character AttackTarget=null;
    public Projectile projectiles;
    public bool start = false;
    public float timer = 0;

    public Stack<IPooledItem_Character> Pool
    {
        get
        {
            return projectileNum;
        }
    }

    public override void RunTime()
    {
        base.RunTime();
        timer += Time.deltaTime;
        if (timer >= 0.5 && caster.Action == CharacterAction.Attacking)
        {
            caster.ChangeAction(CharacterAction.Idle);
        }
    }

    public override void Activate()
    {
        if (!start && caster.Target != null)
        {
            start = true;
        }
        if (start)
        {
            if (caster.Action == CharacterAction.Idle)
            {
                timer = 0;
                TimeSystem.GetTimeSystem().AddTimer(this);
                caster.ChangeAction(CharacterAction.Attacking);
                if (AttackTarget == null)
                {
                    AttackTarget = caster.Target as Character;
                }
                if (caster.Target != null)
                {
                    if (caster.Target.Action == CharacterAction.Dead)
                    {
                        caster.ChangeAction(CharacterAction.Idle);
                    }
                }
                if (AttackTarget.transform.position.x >= this.gameObject.transform.position.x)
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
    }

    public void ProjectileStack()
    {
        if (projectileNum.Count < 1)
        {
            GameObject p = Instantiate(Resources.Load<GameObject>("Skills/Heroes/Archer/AutoAttack/AutoAttack"));
            p.transform.SetParent(GameObject.Find("Projectiles").transform);
            projectiles = p.gameObject.GetComponent<Projectile>();

            projectiles.ProjectileOn(caster, this);
            //projectiles.SetProjectile(1, 1);
            projectiles.ProjectileMove(AttackTarget);
        }
        else
        {
            projectileNum.Pop().PopDo(AttackTarget);
        }
    }

}
