using System;
using UnityEngine;

public class Projectile : MonoBehaviour, ITimeHandler, IPooledItem_Character {

    protected bool moving = false;
    protected Character caster;
    protected Character target=null;
    float speed;
    IPooling_Character pool =null;
    private bool Pmoving = false;
    int abillity;

    public void ProjectileOn(Character caster, IPooling_Character pool)
    {
        this.caster = caster;
        this.pool = pool;
        Pmoving = false;
        this.gameObject.SetActive(true);
        transform.position = caster.transform.position;
    }

    public void ProjectileOn(Character caster, int abillity)
    {
        this.abillity = abillity;
        pool = null;
        this.caster = caster;
        Pmoving = false;
        this.gameObject.SetActive(true);
    }

    public void ProjectileMove(Character target, float speed)
    {
        Pmoving = true;
        TimeSystem.GetTimeSystem().AddTimer(this);
        this.target = target;
        this.speed = speed;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);

		if (Vector3.Distance (transform.position, target.transform.position) < 0.1f) {
			if (target.Action == CharacterAction.Dead) {
				moving = false;
				this.gameObject.SetActive(false);
				TimeSystem.GetTimeSystem().DeleteTimer(this);
                if(pool!=null)
                {
                    pool.Pool.Push(this);
                }
			}
		}
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (target != null&&Pmoving==true)
        {
            if (col.transform.root.transform == target.transform.root.transform)
            {
                OnArrival(abillity);
            }
        }
    }

    public virtual void OnArrival(int abillity)
    {
        Pmoving = false;
        this.gameObject.SetActive(false);
        TimeSystem.GetTimeSystem().DeleteTimer(this);
        if(pool!=null)
        {
            pool.Pool.Push(this);
        }
    }

    public void RunTime()
    {
        if(Pmoving==true)
        {
            ProjectileMove(target, speed);
        }
    }

    public void PopDo(Character target)
    {
        TimeSystem.GetTimeSystem().AddTimer(this);
        Pmoving = true;
        this.gameObject.SetActive(true);
		this.target = target;
        transform.position = caster.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }
}
