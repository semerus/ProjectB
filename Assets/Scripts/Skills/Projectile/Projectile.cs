using System;
using UnityEngine;

public class Projectile : MonoBehaviour, ITimeHandler, IPooledItem_Character {

    protected bool moving = false;
    protected Character caster;
    protected Character target=null;
    int damage;
    float speed;
    IPooling_Character pool =null;
    private bool Pmoving = false;
    int abillity;

    public void SetProjectile(int damage, float speed)
    {
        this.damage = damage;
        this.speed = speed;
    }

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

    public void DeleteProjectile()
    {
        Destroy(this.gameObject);
    }

    public void ProjectileOn(Character caster)
    {
        pool = null;
        this.caster = caster;
        Pmoving = false;
        this.gameObject.SetActive(true);
        transform.position = caster.transform.position;
    }

    public void ProjectileMove(Character target)
    {
        this.target = target;
        Pmoving = true;
        TimeSystem.GetTimeSystem().AddTimer(this);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
        ProjectileRotation();

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

    public void ProjectileRotation()
    {
        float dx = target.transform.position.x - this.gameObject.transform.position.x;
        float dy = target.transform.position.y - this.gameObject.transform.position.y;
        float ax = Mathf.Abs(dx);
        float ay = Mathf.Abs(dy);
        float seta = Mathf.Atan(ay / ax);
        seta = seta * 180 / Mathf.PI;

        if (dx>=0 && dy>=0)
        {
            transform.rotation = Quaternion.Euler(0, 0, seta);
        }
        else if(dx < 0 && dy >= 0)
        {
            seta = 180 - seta;
            transform.rotation = Quaternion.Euler(0, 0, seta);
        }
        else if (dx < 0 && dy < 0)
        {
            seta = seta - 180;
            transform.rotation = Quaternion.Euler(0, 0, seta);
        }
        else if (dx >= 0 && dy < 0)
        {
            seta = -seta;
            transform.rotation = Quaternion.Euler(0, 0, seta);
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
            ProjectileMove(target);
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
        ProjectileRotation();
    }
}
