using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale : Enemy {

    public OgreFemale_AutoAttack atk1;
    public OgreFemale_Sk2 atk2;
    public OgreFemale_Sk3 atk3;
    public OgreFemale_Sk4 atk4;

    void Start()
    {
        speed_x = 1f;
        speed_y = 1f;
        atk1.SetSkill(this);
        atk2.SetSkill(this);
        atk3.SetSkill(this);
        atk4.SetSkill(this);
    }

    private void Update()
    {
        base.Update();
        if(Input.GetKey("q"))
        {
            atk1.OnCast();
        }
        if(Input.GetKey("w"))
        {
            atk2.OnCast();
            Debug.Log("w");
        }
        if(Input.GetKey("e"))
        {
            atk3.OnCast();
        }
        if(Input.GetKey("x"))
        {
            atk4.OnCast();
        }
        
    }

    public override void ReceiveHeal (int heal)
	{
		base.ReceiveHeal (heal);
		Debug.Log ("Received heal : " + heal);
	}
}
