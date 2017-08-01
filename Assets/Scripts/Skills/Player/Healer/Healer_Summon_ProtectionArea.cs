using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer_Summon_ProtectionArea : HeroActive {

    #region Implement abstract member
    public override void Activate()
    {
        if (CheckSkillStatus(SkillStatus.ReadyMask))
        {
            if (caster.CheckCharacterStatus(CharacterStatus.IsSilencedMask) == false)
            {
                healer_ProtectionArea_Script.Set(caster.transform.position);
                StartCoolDown();
            }
        }
    }
    #endregion

    #region MonoBehaviours
    protected virtual void Awake()
    {
        //set initial Value
        skillStatus = SkillStatus.ReadyOn;
        cooldown = 20f;
        timer_cooldown = 0f;

        //Prefab
        GameObject potion_ref = Resources.Load("Skills\\Area\\Healer_ProtectionArea", typeof(GameObject)) as GameObject;
        healer_ProtectionArea_Object = Instantiate(potion_ref, Vector3.zero, Quaternion.identity);

        //UI
        button = Resources.Load<Sprite>("Skills/Heroes/Healer/Healer_Skill2");
    }
    #endregion

    #region Field & Method
    protected GameObject healer_ProtectionArea_Object;
    protected Healer_ProtectionArea healer_ProtectionArea_Script;
    #endregion

}
