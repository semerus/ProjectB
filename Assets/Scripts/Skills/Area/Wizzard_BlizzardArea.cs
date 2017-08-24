using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizzard_BlizzardArea : MonoBehaviour, ITimeHandler {

    protected Character caster;
    protected IBattleHandler[] enemyNum;
    protected int damage;
    protected float blizzardTime;
    protected AnimationController anim;

    float[] inTime = new float[10];
    protected float skilltime = 0;
    protected int count = 0;
    protected Character[] target = new Character[10];
    protected bool[] FacingLeft = new bool[10];
    protected Vector2 range;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<AnimationController>();
    }

    public void SetBlizzard(float time, int damage, Character caster)
    {
        this.caster = caster;
        this.blizzardTime = time;
        this.damage = damage;
        TimeSystem.GetTimeSystem().AddTimer(this);
        ResetSetting();
        anim.onCue += OnEnd;
    }

    protected virtual void ResetSetting()
    {
        enemyNum = BattleManager.GetBattleManager().GetEntities(Team.Hostile);
        for (int i = 0; i < enemyNum.Length; i++)
        {
            inTime[i] = 0;
        }
        skilltime = 0;
        count = 0;
    }

    public void RunTime()
    {
        skilltime += Time.deltaTime;
        Blizzard();
    }

    protected virtual void Blizzard()
    {
        if (skilltime <= blizzardTime)
        {
            Gravity();
            Damage();
        }
    }

    protected virtual void OnEnd()
    {
        ResetSetting();
        TimeSystem.GetTimeSystem().DeleteTimer(this);
        anim.onCue -= OnEnd;
        gameObject.SetActive(false);
    }

    protected void Gravity()
    {
        for (int i = 0; i < enemyNum.Length; i++)
        {
            Character c = enemyNum[i] as Character;
            bool hitcheck = false;

            hitcheck = Scanner.EllipseScanner(range.x, range.y, transform.position, c.transform.position);

            if (hitcheck)
            {
                float dx = this.gameObject.transform.position.x - c.transform.position.x;
                float dy = this.gameObject.transform.position.y - c.transform.position.y;
                c.transform.position += (new Vector3(dx, dy, 0)) * Time.deltaTime;
            }
        }
    }

    public bool CheckBoundary(Character target)
    {
        Vector3 position = target.transform.position;
        float cpx = position.x;
        float cpy = position.y;
        float lx = GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>().size.x / 2;
        float ly = (GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>().size.y / 2) + GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>().offset.y;

        if ((cpx <= lx && cpx >= -lx) && (cpy < ly && cpy > -ly))
        {
            return true;
        }
        else
        {
            return false;
        }
    } 

    public void Damage()
    {
        for (int i = 0; i < enemyNum.Length; i++)
        {
            Character c = enemyNum[i] as Character;
            bool hitcheck = false;

            hitcheck = Scanner.EllipseScanner(range.x, range.y, this.gameObject.transform.position, c.transform.position);

            if (hitcheck)
            {
                inTime[i] += Time.deltaTime;

                if (inTime[i] >= 1)
                {
                    inTime[i] = 0;
                    caster.AttackTarget(enemyNum[i], damage);
                }
            }
        }
    }
}
