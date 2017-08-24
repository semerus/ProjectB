using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizzard_BlizzardArea_Ability1 : Wizzard_BlizzardArea {

    protected float explosionTime;
    bool isExploding = false;

    protected override void Awake()
    {
        base.Awake();
        range = new Vector2(3f, 1.3f);
    }

    protected override void Blizzard()
    {
        base.Blizzard();
        if(isExploding)
        {
            Explosion();
        }
    }

    protected override void OnEnd()
    {
        isExploding = true;
    }

    protected override void ResetSetting()
    {
        base.ResetSetting();
        explosionTime = 0;
    }

    public void ExplosionCheck()
    {
        for (int i = 0; i < enemyNum.Length; i++)
        {
            Character c = enemyNum[i] as Character;

            bool hitcheck = false;

            hitcheck = Scanner.EllipseScanner(3f, 1.3f, this.gameObject.transform.position, c.transform.position);

            if (hitcheck)
            {
                target[count] = c;

                if (c.IsFacingLeft)
                {
                    FacingLeft[count] = true;
                }
                else
                {
                    FacingLeft[count] = false;
                }
                count++;
            }
        }
    }

    public void Explosion()
    {
        if (explosionTime == 0)
        {
            LastDamage();
        }

        explosionTime += Time.deltaTime;
        if (explosionTime <= 0.2)
        {
            for (int i = 0; i < count; i++)
            {
                target[i].StopMove();
                if (FacingLeft[i])
                {
                    if (CheckBoundary(target[i]))
                    {
                        target[i].transform.position += new Vector3(20, 0, 0) * Time.deltaTime;
                    }
                }
                else
                {
                    if (CheckBoundary(target[i]))
                    {
                        target[i].transform.position -= new Vector3(20, 0, 0) * Time.deltaTime;
                    }
                }
            }
        }
        else
        {
            base.OnEnd();
            isExploding = false;
        }
    }

    protected void LastDamage()
    {
        for (int i = 0; i < enemyNum.Length; i++)
        {
            Character c = enemyNum[i] as Character;

            bool hitcheck = false;
            hitcheck = Scanner.EllipseScanner(3f, 1.3f, this.gameObject.transform.position, c.transform.position);
            ExplosionCheck();

            if (hitcheck)
            {
                caster.AttackTarget(enemyNum[i], damage);
            }
        }
    }
}
