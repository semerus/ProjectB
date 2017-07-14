using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale_Sk4 : Skill
{

    int count = 0;
    int mod = 1;
    int startNum = 0;
    bool sk4_On = false;
    IBattleHandler[] friendlyNum;


    public override void Activate(IBattleHandler target)
    {
        sk4_On = true;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (startNum == 0)
        {
            friendlyNum = BattleManager.GetBattleManager().GetEntities(Team.Friendly);
            Debug.Log(friendlyNum.Length);
            startNum = 1;
        }

        if (Input.GetKeyDown("r"))
        {
            Activate(null);
        }

        Sk4();
    }

    private void Sk4()
    {
        if (sk4_On == true)
        {
            WayMod();
            Debug.Log("001");
            EdgeChange();
            Debug.Log("002");
            BurnBurn();
            Debug.Log("003");
        }
    }

    private void BurnBurn()
    {
        for (int i = 0; i < friendlyNum.Length; i++)
        {
            Character c = friendlyNum[i] as Character;
            bool burnCheck = EllipseScanner(2.6f, 0.9f, this.gameObject.transform.position, c.transform.position);

            if (burnCheck == true)
            {
                Debug.Log("Burn " + c.transform.name);
            }
        }
    }

    private void WayMod()
    {
        float alpha = (3 / 2 )* Mathf.Sqrt(2);
        switch (mod)
        {
            case 1:
                this.gameObject.transform.position += new Vector3(alpha, alpha, 0) * Time.deltaTime;
                break;

            case 2:
                this.gameObject.transform.position += new Vector3(alpha, -1 * alpha, 0) * Time.deltaTime;
                break;

            case 3:
                this.gameObject.transform.position += new Vector3(-1 * alpha, -1 * alpha, 0) * Time.deltaTime;
                break;

            case 4:
                this.gameObject.transform.position += new Vector3(-1 * alpha, alpha, 0) * Time.deltaTime;
                break;
        }
        Debug.Log(mod);
    }

    private void EdgeChange()
    {
        Vector3 myPosition = this.gameObject.transform.position;
        if (count <= 6)
        {
            if (myPosition.y >= 3.4)
            {
                if (mod == 1)
                {
                    mod = 2;
                }
                else
                {
                    mod = 1;
                }
                count++;
            }
            if (myPosition.y <= -3.4)
            {
                if (mod == 3)
                {
                    mod = 4;
                }
                else
                {
                    mod = 3;
                }
                count++;
            }
            if (myPosition.x >= 9)
            {
                if (mod == 2)
                {
                    mod = 3;
                }
                else
                {
                    mod = 2;
                }
                count++;
            }
            if (myPosition.x <= -9)
            {
                if (mod == 4)
                {
                    mod = 1;
                }
                else
                {
                    mod = 4;
                }
                count++;
            }
        }
        else
        {
            sk4_On = false;
            count = 0;
        }
        Debug.Log(count);
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
}
