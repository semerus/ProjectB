using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale_Sk3 : Skill {

    IBattleHandler[] friendlyNum;
    bool[] hitCheck;
    bool[] targetOn;
    bool[] stunCheck;
    private bool Sk3Acess = false;
    private float ctime = 0;

	// Use this for initialization
	void Start () {
        friendlyNum = BattleManager.GetBattleManager().GetEntities(Team.Friendly);
        for (int i = 0; i<= friendlyNum.Length - 1; i++)
        {
            stunCheck[i] = false;
            hitCheck[i] = false;
            targetOn[i] = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        Sk3On();
	}


    private void Sk3On()
    {
        if(Sk3Acess==true)
        {
            ctime = +Time.deltaTime;

            if(ctime<=2)
            {
                //캐스팅
            }
            if(ctime>=2 && ctime<=6)
            {
                for(int i=0; i<=friendlyNum.Length-1; i++)
                {
                    Character c = friendlyNum[i] as Character;
                    hitCheck[i] = EllipseScanner(3.5f, 1.3f, this.gameObject.transform.position, c.gameObject.transform.position);

                    if(hitCheck[i]==true && stunCheck[i]==false)
                    {
                        //기절
                        stunCheck[i] = true;
                    }

                }
            }
            if(ctime>=6 && ctime<=7.5)
            {
                // 마무리 동작
            }
            if(ctime>7.5)
            {
                Sk3Acess = false;
                ctime = 0;
            }
        }
    }


    private void Sk3Targgetting()
    {
        int targetingNum = 0;

        for(int i=0; i<=friendlyNum.Length-1; i++)
        {
            Character c = friendlyNum[i] as Character;
            targetOn[i] = EllipseScanner(4, 1.5f, this.gameObject.transform.position, c.gameObject.transform.position);

            if(targetOn[i] == true)
            {
                targetingNum++;
            }
        }
        //bool targetOn = EllipseScanner(4, 1.5f, this.gameObject.transform.position, GameObject.Find("Fighter").gameObject.transform.position);
        if(targetingNum>=2)
        {
            Sk3Acess = true;
            state = SkillState.OnCoolDown;
        }
    }

    private bool EllipseScanner(float a, float b, Vector3 center,Vector3 targetPosition)
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
        if(Sk3Acess==false)
        {
            Sk3Targgetting();
        }
    }
}
