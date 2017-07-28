using System;
using UnityEngine;

public class Projectile : MonoBehaviour,ITimeHandler,IPooledItem_Character {

    
    Character caster;
    Character target=null;
    float speed;
    IPooling_Character pool =null;
    private bool Pmoving = false;
    private bool arriving = false;

    public void ProjectileOn(Character caster, IPooling_Character pool)
    {
        arriving = false;
        this.caster = caster;
        this.pool = pool;
        Pmoving = false;
        this.gameObject.SetActive(true);
        transform.position = caster.transform.position;
        
    }

    public void ProjectileOn()
    {
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
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col != null&&target!=null)
        {
            if (col.transform.root.transform == target.transform)
            {
                OnArrival();
            }
        }
    }

    public void OnArrival()
    {
        arriving = true;
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
        transform.position = caster.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }
}
