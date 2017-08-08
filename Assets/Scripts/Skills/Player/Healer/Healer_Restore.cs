using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer_Restore : HeroActive {
    
    #region Implement abstract member
    public override void Activate()
    {
        if (CheckSkillStatus(SkillStatus.ReadyMask))
        {
            if (caster.CheckCharacterStatus(CharacterStatus.IsSilencedMask) == false)
            {
                    
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

        //UI
        button = Resources.Load<Sprite>("Skills/Heroes/Healer/Healer_Skill3");
    }
    #endregion

    #region Field & Method
    // GetFriendly

    // 상태이상이 해제된 캐릭터 읽어들이기
    // 50회복 or 반사버프 시전
    #endregion



}
