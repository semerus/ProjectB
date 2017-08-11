using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyInfo
{
    #region Contructor
    public PartyInfo()
    {
        heroIDs = new int[4];
        heroSkillIDs = new int[4, 3];
    }
    #endregion

    #region Field
    public int[] heroIDs;
    public int[,] heroSkillIDs;
    #endregion

    #region Method
    public void Reset_Hero(int index)
    {
        heroIDs[index] = 0;
        for (int i = 0; i < 3; i++)
        {
            heroSkillIDs[index, i] = 0;
        }
    }

    public void Reset_All_Hero()
    {
        for (int i = 0; i < 4; i++)
        {
            heroIDs[i] = 0;

            for (int j = 0; j < 3; j++)
            {
                heroSkillIDs[i, j] = 0;
            }
        }
    }

    public void Set_HeroID(int index, int ID)
    {
        heroIDs[index] = ID;
    }

    public void Set_HeroSkillID(int heroIndex, int skillIndex, int ID)
    {
        heroSkillIDs[heroIndex, skillIndex] = ID;
    }
    #endregion




}
