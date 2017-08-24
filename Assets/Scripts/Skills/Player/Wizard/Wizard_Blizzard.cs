using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Blizzard : HeroActive,IChanneling
{
    IBattleHandler[] enemyNum;
    Vector3 targetPosition=new Vector3();
    float channeling = 0;
    protected float time;
    protected int damage;
    protected float maxChannelingTime;
    GameObject blizzard;

    public override void SetSkill(Dictionary<string, object> param)
    {
        base.SetSkill(param);
        caster = gameObject.GetComponent<Character>();
        Hero h = caster as Hero;
        if (h != null)
        {
            h.activeSkills[2] = this;
        }
        time = (float)((double)param["exist_time"]);
        damage = (int)param["damage"];
        button = Resources.Load<Sprite>("Skills/Heroes/Wizard/Wizard_Skill3");
        maxChannelingTime = (float)((double)param["channel_time"]);
        blizzard = Instantiate(Resources.Load<GameObject>(param["path_prefab"].ToString()));
        blizzard.SetActive(false);
    }

    public virtual void OnChanneling()
    {
        caster.ChangeAction(CharacterAction.Channeling);
        UpdateSkillStatus(SkillStatus.ChannelingOn);
        SkillChanneling();
    }

    public virtual void OnInterrupt(IBattleHandler interrupter)
    {
        Wizard_Passive.stackOff = true;
        ResetSetting();
        StartCoolDown();
        UpdateSkillStatus(SkillStatus.ChannelingOff);
    }

    public virtual void SkillChanneling()
    {
        channeling += Time.deltaTime;

        if (channeling >= maxChannelingTime)
        {
            Activate();
           // BlizzardPrefabOn(abillity);
        }
    }

    public override void Activate()
    {
        ResetSetting();
        StartCoolDown();
        UpdateSkillStatus(SkillStatus.ChannelingOff);
        blizzard.SetActive(true);
        blizzard.GetComponent<Wizzard_BlizzardArea>().SetBlizzard(time, damage, caster);
        blizzard.transform.position = targetPosition;
        caster.ChangeAction(CharacterAction.Idle);
    }

    public void ResetSetting()
    {
        FindPosition();
        channeling = 0;
    }

    public void FindPosition()
    {
        float tx;

        if (caster.IsFacingLeft)
        {
            tx = this.gameObject.transform.position.x - 3f;
        }
        else
        {
            tx = this.gameObject.transform.position.x + 3f;
        }
        targetPosition = new Vector3(tx, this.gameObject.transform.position.y, 0);
    }

    #region None
    public float ChannelTime
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public float Timer_Channeling { get; set; }
#endregion

}
