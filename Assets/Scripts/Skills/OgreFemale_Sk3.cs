using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale_Sk3 : Skill {

    IBattleHandler[] friendlyNum;
    int startNum = 0;
    private bool Sk3Acess = false;
    private float ctime = 0;

	
	// Update is called once per frame
	void Update () {
        if(startNum==0)
        {
            friendlyNum = BattleManager.GetBattleManager().GetEntities(Team.Friendly);
            Debug.Log(friendlyNum.Length);
            startNum = 1;
        }

        Sk3On();
        if(Input.GetKeyDown("e"))
        {
            Debug.Log("e");
            Activate(null);
        }
	}


    private void Sk3On()
    {
        if(Sk3Acess==true)
        {
            ctime += Time.deltaTime;

            if(ctime<=2)
            {
                //캐스팅
            }
            if(ctime>=2 && ctime<=6)
            {
                for(int i=0; i<=friendlyNum.Length-1; i++)
                {
                    Character c = friendlyNum[i] as Character;
                    Debug.Log(c.transform.name);
                    bool hitCheck = EllipseScanner(3.5f, 1.3f, this.gameObject.transform.position, c.gameObject.transform.position);

                    if(hitCheck==true)
                    {
                        Debug.Log("stun "+c.transform.name);
                        //기절
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
            bool targetOn = EllipseScanner(4, 1.5f, this.gameObject.transform.position, c.gameObject.transform.position);

            if(targetOn == true)
            {
                targetingNum++;
            }
        }
        if(targetingNum>=2)
        {
            Debug.Log("e skill on");
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
