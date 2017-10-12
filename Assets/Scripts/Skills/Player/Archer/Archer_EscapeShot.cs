using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_EscapeShot : HeroActive {

    Vector3 CurrentPosition;
    Vector3 MoveVector;
    Vector3 targetPosition;
    float MoveVectorSize;
    float time = 0;
    protected int damage;
    protected float speed;

    #region SetSkill

    void Awake()
    {
        caster = gameObject.GetComponent<Character>();
        Hero h = caster as Hero;
        if (h != null)
        {
            h.activeSkills[2] = this;
        }
    }

    public override void SetSkill(Dictionary<string, object> param)
    {
        base.SetSkill(param);
        damage = (int)param["damage"];
        speed = (float)((double)param["speed"]);
    }

    #endregion

    public override void Activate()
    {
        StartCoolDown();
    }

    public void EscapeTarget(int abiliity)
    {
        time = 0;
        
        CurrentPosition = this.gameObject.transform.position;
        //Character target = Archer_AutoAttack.AtkTarget;
        Character target = caster.Target as Character;
        MoveVector = this.gameObject.transform.position - target.transform.position;
        MoveVectorSize= Mathf.Sqrt(MoveVector.x * MoveVector.x + MoveVector.y * MoveVector.y);
        switch(abiliity)
        {
            case 1:
                MoveVector.x = MoveVector.x * 3 / MoveVectorSize;
                MoveVector.y = MoveVector.y * 3 / MoveVectorSize;
                break;

            case 2:
                MoveVector.x = MoveVector.x * 4 / MoveVectorSize;
                MoveVector.y = MoveVector.y * 4 / MoveVectorSize;
                break;
        }
    }

    public bool CheckBoundary(Character target)
    {
        Vector3 position = target.transform.position;
        float cpx = position.x;
        float cpy = position.y;
        float lx = GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>().size.x / 2;
        float ly = (GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>().size.y / 2) + GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>().offset.y;

        if ((cpx <= lx && cpx >= -lx) && (cpy < ly && cpy > -ly))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void EscapeMove()
    {
        if(CheckBoundary(caster as Character))
        {
            this.gameObject.transform.position = Vector3.MoveTowards(CurrentPosition, CurrentPosition + MoveVector, 3 * Time.deltaTime);
        }
        else
        {
            UpdateSkillStatus(SkillStatus.ChannelingOff);
            UpdateSkillStatus(SkillStatus.ProcessOn);
        }
        
        if(this.gameObject.transform.position== CurrentPosition+ MoveVector)
        {
            UpdateSkillStatus(SkillStatus.ChannelingOff);
            UpdateSkillStatus(SkillStatus.ProcessOn);
        }
    }

    public void Damage()
    {
        caster.AttackTarget(caster.Target, 20);
    }


}
