using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Healer_Summon_Totem : HeroActive {

    #region Implement abstract member
    public override void Activate(IBattleHandler target)
    {
        if (CheckSkillStatus(SkillStatus.ReadyMask))
        {
            if(caster.CheckCharacterStatus(CharacterStatus.IsSilencedMask) == false)
            {
                Vector3 spawnPos = GetSpawnPos();
                healer_Totem_Script.Set(vector 3);
            }
        }
    }
    #endregion

    #region ITimeHandler override
    public override void RunTime()
    {
        ;
    }
    #endregion

    #region MonoBehaviours
    protected void Awake()
    {
        //set initial Value
        skillStatus = SkillStatus.ReadyOn;
        cooldown = 20f;
        timer_cooldown = 0f;
        
        //Prefab
        GameObject potion_ref = Resources.Load("Skills\\Area\\Healer_Totem", typeof(GameObject)) as GameObject;
        healer_Totem_Object = Instantiate(potion_ref, Vector3.zero, Quaternion.identity);
        healer_Totem_Script = healer_Totem_Object.GetComponentInChildren<Healer_Totem>();
    }
    #endregion

    #region Field & Method
    GameObject healer_Totem_Object;
    Healer_Totem healer_Totem_Script;

    Vector3 Get_SpawnPos()
    {
        Vector3 returnVector = caster.transform.position;
        Vector3 summonPos = new Vector3(1.5f, 0, 0);

        BoxCollider2D controlArea = Background.GetBackground().gameObject.GetComponentInChildren<BoxCollider2D>();
        float damping = 0.5f;
        float xLeft = -controlArea.size.x / 2 + controlArea.offset.x + damping;
        float xRight = controlArea.size.x / 2 + controlArea.offset.x - damping;
        
        if(caster.IsFacingLeft == true)
        {
            
        }
        else
        {

        }

        return returnVector;
    }
    #endregion

}
