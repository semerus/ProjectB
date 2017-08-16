using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale_Sk3 : Skill {

    IBattleHandler[] friendlyNum;

	void Awake() {
		caster = gameObject.GetComponent<Character> ();
	}

    public override void Activate()
    {
        cooldown = 10f;
        StartCoolDown();
        friendlyNum = BattleManager.GetBattleManager().GetEntities(Team.Friendly);
		Sk3On ();
    }

    private void Sk3On()
    {
        for (int i = 0; i <= friendlyNum.Length - 1; i++)
        {
            Character c = friendlyNum[i] as Character;
            bool hitCheck = EllipseScanner(3.5f, 1.3f, this.gameObject.transform.position, c.gameObject.transform.position);
            if (hitCheck == true)
            {
                Debug.Log("stun " + c.transform.name);
                //기절
            }
        }
        SkillEventArgs s = new SkillEventArgs(this.name, true);
        OnEndSkill(s);
    }

    private bool EllipseScanner(float a, float b, Vector3 center, Vector3 targetPosition)
    {
        float dx = targetPosition.x - center.x;
        float dy = targetPosition.y - center.y;

        float l1 = Mathf.Sqrt((dx + Mathf.Sqrt(a * a - b * b)) * (dx + Mathf.Sqrt(a * a - b * b)) + (dy * dy));
        float l2 = Mathf.Sqrt((dx - Mathf.Sqrt(a * a - b * b)) * (dx - Mathf.Sqrt(a * a - b * b)) + (dy * dy));

        if (l1 + l2 <= 2 * a)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
