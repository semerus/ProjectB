using System.Collections;
using System.Collections.Generic;

public class Loadinfo
{
    #region constructor
    public Loadinfo()
    {
        partyInfo = new PartyInfo();
        itemID = 0;
        mapID = 0;
        bossID = new List<int>();
    }
    #endregion

    #region Field
    public PartyInfo partyInfo;
    public int itemID;
    public int mapID;
    public IList<int> bossID;
    #endregion

    #region Method
    public void Set_MapID(int ID)
    {
        mapID = ID;
        bossID.Clear();
    }

    public void Reset_MapID()
    {
        mapID = 0;
        bossID.Clear();
    }

    public void Set_ItemID(int ID)
    {
        itemID = ID;
    }

    public void Reset_ItemID()
    {
        itemID = 0;
    }
    #endregion
}