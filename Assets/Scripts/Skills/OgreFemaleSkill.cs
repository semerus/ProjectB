using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemaleSkill : Skill {

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

    }


    public void AutoAttak() // 일반공격
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

        float dx = this.gameObject.transform.position.x - minC.gameObject.transform.position.x;
        float dy = this.gameObject.transform.position.y - minC.gameObject.transform.position.y;
        float cdistance = Mathf.Sqrt(dx * dx + dy * dy);

        if (0.4f < cdistance && cdistance < 0.5f)
        {
            attackOn = true;
        }
        else
        {
            // 움직인다
        }

        if (attackOn == true)
        {
            ctime += Time.deltaTime;

            if (ctime < 1)
            {
                // 준비동작
            }
            if (ctime == 1)
            {
                // 범위데미지
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
    }
}
