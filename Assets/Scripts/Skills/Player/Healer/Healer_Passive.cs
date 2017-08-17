using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer_Passive : Skill {

    #region Implement abstract member
    public override void Activate()
    {
        //Do nothing;
    }
    #endregion

    #region MonoBehaviours
    void Awake()
    {
		Hero h = caster as Hero;
		if (h != null) {
			h.passiveSkill = this;
		}

        //Time system Add
        TimeSystem.GetTimeSystem().AddTimer(this);

        //Prefab
        GameObject potion_ref = Resources.Load("Skills\\Area\\Healer_Potion", typeof(GameObject)) as GameObject;
        healer_Potion_Object = Instantiate(potion_ref, Vector3.zero, Quaternion.identity);
        healer_Potion_Script = healer_Potion_Object.GetComponentInChildren<Healer_Potion>();
    }
    #endregion

    #region ITimeHandler override
    public override void RunTime()
    {
        timer_cooldown += Time.deltaTime;

        if(timer_cooldown >= cooldown)
        {
            Spawn();
            timer_cooldown = 0f;
        }
    }

    #endregion

    #region Field & Method
    GameObject healer_Potion_Object;
    Healer_Potion healer_Potion_Script;


    void Spawn()
    {
        BoxCollider2D controlArea = Background.GetBackground().GetComponentInChildren<BoxCollider2D>();

        float space_X = 2f;
        float space_Y = 1f;

        float xMin = -controlArea.size.x / 2 + controlArea.offset.x + space_X / 2;
        float xMax = controlArea.size.x / 2 + controlArea.offset.x - space_X / 2;

        float yMin = -controlArea.size.y / 2 + controlArea.offset.y + space_Y / 2;
        float yMax = controlArea.size.y / 2 + controlArea.offset.y - space_Y / 2;
        
        int infinityBreaker = 0;
        bool isSpawnAvailable = false;
        Vector3 spawnVector = Vector3.zero;

        while (isSpawnAvailable == false)
        {
            isSpawnAvailable = true;

            float xPos = UnityEngine.Random.Range(xMin, xMax);
            float yPos = UnityEngine.Random.Range(yMin, yMax);

            spawnVector = new Vector3(xPos, yPos, 0);

			IBattleHandler[] friendly = BattleManager.GetBattleManager ().GetEntities (Team.Friendly);
			for (int i = 0; i < friendly.Length; i++) {
				Hero hero = friendly[i] as Hero;
				if (hero != null) {
					if (Mathf.Abs(hero.transform.position.x - xPos) >= space_X)
						isSpawnAvailable = false;
					if (Mathf.Abs(hero.transform.position.y - yPos) >= space_Y)
						isSpawnAvailable = false;
				}
			}

            infinityBreaker++;
            if (infinityBreaker > 20)
                break;
        }
        
        healer_Potion_Script.Set(spawnVector);
    }
    #endregion


}
