using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer_Summon_ProtectionArea : HeroActive {

	protected GameObject protectionArea;
	protected int shield;
	protected float duration;

    #region Implement abstract member
    public override void Activate()
    {
		/*
        if (CheckSkillStatus(SkillStatus.ReadyMask))
        {
            if (caster.CheckCharacterStatus(CharacterStatus.IsSilencedMask) == false)
            {
                healer_ProtectionArea_Script.Set(caster.transform.position);
                StartCoolDown();
            }
        }
        */
		healer_ProtectionArea_Script.Set(caster.transform.position);
		StartCoolDown();
    }
    #endregion

    #region MonoBehaviours
    protected virtual void Awake()
    {
		caster = gameObject.GetComponent<Character> ();
		Hero h = caster as Hero;
		if (h != null) {
			h.activeSkills [1] = this;
		}
        
        //Prefab
		protectionArea = Resources.Load("Skills/Area/Healer_ProtectionArea", typeof(GameObject)) as GameObject;
		healer_ProtectionArea_Object = Instantiate (protectionArea);
        healer_ProtectionArea_Script = healer_ProtectionArea_Object.GetComponentInChildren<Healer_ProtectionArea>();
		healer_ProtectionArea_Object.SetActive (false);

        //UI
        button = Resources.Load<Sprite>("Skills/Heroes/Healer/Healer_Skill2");
    }
    #endregion

    #region Field & Method

	public override void SetSkill (Dictionary<string, object> param)
	{
		base.SetSkill (param);
		this.shield = (int)param ["shield"];
		this.duration = (float)((double)param ["duration"]);
		//healer_ProtectionArea_Script.SetValue (shield, duration, caster);
	}

    protected GameObject healer_ProtectionArea_Object;
    protected Healer_ProtectionArea healer_ProtectionArea_Script;
    #endregion

}
