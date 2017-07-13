using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemaleSkill : Skill {

    bool attackColliderOn = false;
    bool isTargetInMeleeRange;
    Vector3 positionToMeleeAttack;
    IBattleHandler[] friendlyNum = BattleManager.GetBattleManager().GetEntities(Team.Friendly);
    float[] distance;
    float x;
    float y;
    float min = 1000000000000;
    float ctime = 0f;
    bool attackOn = false;
    IBattleHandler minNum = null;
    Character minC;


    // Use this for initialization
    void Start()
    {
        float[] distance = new float[friendlyNum.Length];
    }

    // Update is called once per frame
    void Update()
    {
        AutoAttacking();
    }

    private void AutoAttacking()
    {
        if (attackOn == true)
        {
            ctime += Time.deltaTime;

            if (ctime < 1)
            {
                // 준비동작
            }
            if (ctime == 1)
            {
                EdgeCollider2D col = this.gameObject.transform.GetComponentInChildren<EdgeCollider2D>();
                col.offset = new Vector2(minC.transform.position.x - this.gameObject.transform.position.x, minC.transform.position.y - this.gameObject.transform.position.y);
                attackColliderOn = true;

               
            }
            if (ctime > 1 & ctime <= 1.5)
            {
                // 마무리 동작
            }
            if (ctime > 1.5)
            {
                attackOn = false;
                ctime = 0;
            }
        }
        else
        {
            //Move(positionToMeleeAttack);
        }
    }

    public void AutoAttack() // 일반공격
    {
        if (attackOn == false)
        {
            AutoAttackTargetting();

            CheckTargetRange();
        }    
    }

    private void AutoAttackTargetting()
    {
        for (int i = 0; i < friendlyNum.Length; i++)
        {
            Character c = friendlyNum[i] as Character;
            x = this.gameObject.transform.position.x - c.transform.position.x;
            y = this.gameObject.transform.position.y - c.transform.position.y;
            distance[i] = x * x + y * y;

            if (distance[i] <= min)
            {
                min = distance[i];
                minNum = friendlyNum[i];
            }
        }
        minC = minNum as Character;
    }

    private void CheckTargetRange()
    {
        Vector3 enemyPosition = minC.transform.position;
        float deltaX = enemyPosition.x - this.transform.position.x;
        float deltaY = enemyPosition.y - this.transform.position.y;

        float outerX = 0.5f;
        float innerX = 0.4f;
        float outerY = 0.3f;

        float avgX = (outerX + innerX) / 2;
        float halfY = outerY / 2;

        if (deltaX > outerX)
            deltaX -= avgX;
        else if (deltaX > innerX)
            deltaX = 0;
        else if (deltaX >= 0)
            deltaX = -avgX + deltaX;

        else if (deltaX < -outerX)
            deltaX += avgX;
        else if (deltaX < -innerX)
            deltaX = 0;
        else if (deltaX < 0)
            deltaX = avgX + deltaX;

        if (deltaY > outerY)
            deltaY -= halfY;
        else if (deltaY < -outerY)
            deltaY += halfY;
        else
            deltaY = 0;

        if (deltaX == 0 && deltaY == 0)
        {
            attackOn = true;
        }
        else
        {
            attackOn = false;
            positionToMeleeAttack = transform.position + new Vector3(deltaX, deltaY, 0);
        }
    }

    public override void Activate(IBattleHandler target)
    {
        AutoAttack();
    }
}
