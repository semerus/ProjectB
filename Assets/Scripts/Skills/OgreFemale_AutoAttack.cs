using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale_AutoAttack : Skill {

    IBattleHandler[] friendlyNum;
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
       friendlyNum = BattleManager.GetBattleManager().GetEntities(Team.Friendly);
       Debug.Log(friendlyNum.Length);
    }

    // Update is called once per frame
    void Update()
    {
        AutoAttacking();
        if(Input.GetKeyDown("q"))
        {
            Debug.Log("q");
            Activate(null);
        }
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
            if (ctime <= 1.0&&ctime>=0.99)
            {
                for(int i=0; i< friendlyNum.Length; i++)
                {
                    Character c = friendlyNum[i] as Character;
                    bool hitCheck= EllipseScanner(2, 1.4f, minC.gameObject.transform.position, c.gameObject.transform.position);
                    if(hitCheck==true)
                    {
                        if (OgreFemale_Sk5.rageOn == true)
                        {
                            // 데미지 100
                        }
                        else
                        {
                            Debug.Log("Auto Attack => "+c.gameObject.transform.name);
                            //데미지 50
                        }
                    }
                }
            }
            if (ctime > 1 && ctime <= 1.5)
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

            float distance = x * x + y * y;
            
            if (distance <= min)
            {
                min = distance;
                minNum = friendlyNum[i];
            }
        }
        minC = minNum as Character;
    }

    private void CheckTargetRange()
    {
        // enemyPosition 알아서 수정
        //Vector3 enemyPosition = minC.transform.position;
        Vector3 enemyPosition = GameObject.Find("Fighter").gameObject.transform.position;

        // 공격 시전자의 타원반경 (기획서에 나와았는 AxB 사이즈 넣으면됨)
        float myA = 2.1f;
        float myB = 0.7f;

        // 공격 범위
        float outerX = 0.5f;
        float innerX = 0.2f;
        float outerY = 0.3f;

        float dX = enemyPosition.x - this.transform.position.x;
        float dY = enemyPosition.y - this.transform.position.y;

        float inX = (myA / myB * Mathf.Sqrt(myB * myB - dY * dY)) + innerX;
        float outX = (myA / myB * Mathf.Sqrt(myB * myB - dY * dY)) + outerX;

        float m_inX = -1 * ((myA / myB * Mathf.Sqrt(myB * myB - dY * dY)) + innerX);
        float m_outX = -1 * ((myA / myB * Mathf.Sqrt(myB * myB - dY * dY)) + outerX);

        if (((-1 * outerY) <= dY && dY <= outerY) && ((inX <= dX && dX <= outX) || (m_outX <= dX && dX <= m_inX)))
        {
            // 공격 사정범위내에 들어옴 공격을 실행
            // 필요한대로 알아서 수정
            Debug.Log("attack on");
            attackOn = true;
        }

        else
        {
            //공격 사정범위가 아님
            //필요한대로 수정
            Debug.Log("no");
            attackOn = false;
            //positionToMeleeAttack = transform.position + new Vector3(deltaX, deltaY, 0);
        }

    }

    private bool EllipseScanner(float a, float b, Vector3 center, Vector3 targetPosition)
    {
        // a는 스캐너 장축 b는 스캐너 단축
        // center 는 스캐너의 중심좌표
        // targetPosition 에 타겟의 위치를 넣는다 

        float dx = targetPosition.x - center.x;
        float dy = targetPosition.y - center.y;

        float l1 = Mathf.Sqrt((dx + Mathf.Sqrt(a * a - b * b)) * (dx + Mathf.Sqrt(a * a - b * b)) + (dy * dy));
        float l2 = Mathf.Sqrt((dx - Mathf.Sqrt(a * a - b * b)) * (dx - Mathf.Sqrt(a * a - b * b)) + (dy * dy));

        if (l1 + l2 <= 2 * a)
        {
            // 범위스캐너 안에 타겟이 잡힘
            return true;
        }
        else
        {
            // 범위스캐너 안에 타겟이 없음
            return false;
        }
    }



    public override void Activate(IBattleHandler target)
    {
        AutoAttack();
    }
}
