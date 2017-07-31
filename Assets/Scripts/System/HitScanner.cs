using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanner : MonoBehaviour
{

    #region Monobehaviours
    protected virtual void Awake()
    {
        scanCharacter = transform.root.gameObject.GetComponent<Character>();
        scanCollider = GetComponent<Collider2D>();

        friendlyBattleHandler = new List<IBattleHandler>();
        hostileBattleHandler = new List<IBattleHandler>();
        neutralBattleHandler = new List<IBattleHandler>();

        SetScanMask("Battle");
    }
    #endregion

    #region Getters&Setters
    public List<IBattleHandler> GetFriendly
    {
        get { return friendlyBattleHandler; }
    }
    public List<IBattleHandler> GetNeutral
    {
        get { return neutralBattleHandler; }
    }
    public List<IBattleHandler> GetHostile
    {
        get { return hostileBattleHandler; }
    }
    #endregion

    #region Field & Method
    [SerializeField]
    protected Character scanCharacter;
    [SerializeField]
    protected Collider2D scanCollider;
    
    protected List<IBattleHandler> friendlyBattleHandler;
    protected List<IBattleHandler> neutralBattleHandler;
    protected List<IBattleHandler> hostileBattleHandler;

    protected int scanMask;
    protected void SetScanMask(string layerName)
    {
        scanMask = 1 << LayerMask.NameToLayer(layerName);
    }
    
    public void ScanColliders()
    {
        LayerMask mask = new LayerMask();
        ContactFilter2D filter = new ContactFilter2D();
        Collider2D[] others = new Collider2D[20];

        mask.value = scanMask;
        filter.SetLayerMask(mask);
        filter.useTriggers = true;
        
        // Initialization each List
        friendlyBattleHandler.Clear();
        hostileBattleHandler.Clear();
        neutralBattleHandler.Clear();

        int overlapNum = scanCollider.OverlapCollider(filter, others);
        for (int i = 0; i < overlapNum; i++)
        {
            IBattleHandler scanned = others[i].transform.root.GetComponent<IBattleHandler>();

            // exception Check
            if (scanned == null)
                continue;

            switch(scanned.Team)
            {
                case Team.Friendly:
                    friendlyBattleHandler.Add(scanned);
                    break;

                case Team.Hostile:
                    hostileBattleHandler.Add(scanned);
                    break;

                case Team.Neutral:
                    neutralBattleHandler.Add(scanned);
                    break;
            }
        }
    }
    #endregion
}
