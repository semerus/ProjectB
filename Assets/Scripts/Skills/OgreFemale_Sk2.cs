using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale_Sk2 : MonoBehaviour {

    private float gametime = 0;
    IBattleHandler[] friendlyNum = BattleManager.GetBattleManager().GetEntities(Team.Friendly);
    float[] distance;
    float x;
    float y;
    float max = 0;
    float sk2coolTime = 10;
    bool jumpCoolDownAcess = false;
    bool jumpTargettingOn = false;
    int jumpNum = 0;
    Vector3 adjustpoint;
    Vector3 jumpdistance;
    float jumptime = 1;
    Character maxC;
    IBattleHandler maxNum = null;

    // Use this for initialization
    void Start () {
        float[] distance = new float[friendlyNum.Length];
        adjustpoint = this.gameObject.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        gametime += Time.deltaTime;

        if (gametime >= 5)
        {

        }
    }

    public void JumpCoolDown()
    {
        if (jumpCoolDownAcess == true)
        {
            sk2coolTime -= Time.deltaTime;
        }

        if (sk2coolTime == 0)
        {
            sk2coolTime = 10;
            jumpCoolDownAcess = false;
            jumpTargettingOn = false;
        }
    }

    public void JumpAttack()
    {
        if (jumpTargettingOn == false)
        {
            JumpAttackTargetting();
        }

        if (this.gameObject.transform.position != adjustpoint)
        {
            this.gameObject.transform.position += Time.deltaTime * jumpdistance / jumptime;
        }
        if (this.gameObject.transform.position == adjustpoint && jumpNum == 0)
        {
           //기절
           // AutoAttak();
            jumpNum = 1;
        }
    }

    public void JumpAttackTargetting()
    {
        for (int i = 0; i < friendlyNum.Length; i++)
        {
            jumpCoolDownAcess = true;
            jumpNum = 0;

            Character c = friendlyNum[i] as Character;
            x = this.gameObject.transform.position.x - c.transform.position.x;
            y = this.gameObject.transform.position.y - c.transform.position.y;
            distance[i] = x * x + y * y;

            if (distance[i] >= max)
            {
                max = distance[i];
                maxNum = friendlyNum[i];
            }
        }
        maxC = maxNum as Character;

        if (this.gameObject.transform.position.x >= maxC.transform.position.x)
        {
            adjustpoint = maxC.transform.position;
            adjustpoint.x = maxC.transform.position.x + 1.25f;
            jumpdistance = adjustpoint - this.gameObject.transform.position;
            jumpTargettingOn = true;
        }
        else
        {
            adjustpoint = maxC.transform.position;
            adjustpoint.x = maxC.transform.position.x - 1.25f;
            jumpdistance = adjustpoint - this.gameObject.transform.position;
            jumpTargettingOn = true;
        }

        jumpTargettingOn = true;
    }


}
