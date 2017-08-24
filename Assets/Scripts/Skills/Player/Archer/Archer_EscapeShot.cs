using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_EscapeShot : HeroActive,IChanneling {

    Vector3 CurrentPosition;
    Vector3 MoveVector;
    Vector3 targetPosition;
    float MoveVectorSize;
    float time = 0;

    #region None

    public float ChannelTime
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public float Timer_Channeling { get ; set ;}

    #endregion

    public void OnChanneling()
    {
        caster.ChangeAction(CharacterAction.Channeling);
        UpdateSkillStatus(SkillStatus.ChannelingOn);
        EscapeMove();
    }

    public void OnInterrupt(IBattleHandler interrupter)
    {
        caster.ChangeAction(CharacterAction.Idle);
        StartCoolDown();
        UpdateSkillStatus(SkillStatus.ChannelingOff);
    }

    protected override void OnProcess()
    {
        caster.ChangeAction(CharacterAction.Idle);
        base.OnProcess();
        Damage();
    }
    
    public void EscapeTarget(int abiliity)
    {
        time = 0;
        StartCoolDown();
        CurrentPosition = this.gameObject.transform.position;
        Character target = Archer_AutoAttack.AttackTarget;
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
        //2회사격
        caster.AttackTarget(caster.Target, 20);
    }


}
