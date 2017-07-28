using System;
using UnityEngine;

public class Projectile : MonoBehaviour, ITimeHandler, IPooledItem_Character {

    protected bool moving = false;
    protected Character caster;
    protected Character target;
    float speed;
    IPooling_Character pool;
    
    public void ProjectileOn(Character caster, IPooling_Character pool)
    {
        this.caster = caster;
        this.pool = pool;
        moving = false;
        this.gameObject.SetActive(true);
        transform.position = caster.transform.position;
    }

    public void ProjectileMove(Character target, float speed)
    {
        moving = true;
        TimeSystem.GetTimeSystem().AddTimer(this);
        this.target = target;
        this.speed = speed;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);

		if (Vector3.Distance (transform.position, target.transform.position) < 0.1f) {
			if (target.Action == CharacterAction.Dead) {
				moving = false;
				this.gameObject.SetActive(false);
				TimeSystem.GetTimeSystem().DeleteTimer(this);
				pool.Pool.Push(this);
			}
		}
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.root.transform == target.transform)
        {
            OnArrival();
        }
    }

    public virtual void OnArrival()
    {
        moving = false;
        this.gameObject.SetActive(false);
        TimeSystem.GetTimeSystem().DeleteTimer(this);
        pool.Pool.Push(this);
    }

    public void RunTime()
    {
        if(moving==true)
        {
            ProjectileMove(target, speed);
        }
    }

    public void PopDo(Character target)
    {
        TimeSystem.GetTimeSystem().AddTimer(this);
        moving = true;
        this.gameObject.SetActive(true);
		this.target = target;
        transform.position = caster.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }
}
