using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemaleSkill : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    void AutoAttavk()
    {

        BattleManager temp;
        temp = BattleManager.GetBattleManager();
        IBattleHandler[] friendlyNum = temp.GetEntities(Team.Friendly);

        float[] distance = new float[friendlyNum.Length];
        float x;
        float y;
        float min = 1000000000000;
        IBattleHandler minNum = null;

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

    }
}
