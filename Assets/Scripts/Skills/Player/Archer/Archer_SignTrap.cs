using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_SignTrap : HeroActive {

    protected int damage;
    protected float speed;
    protected GameObject trap;

    #region SetSkill

    private void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            Activate();
        }
    }

    void Awake()
    {
        caster = gameObject.GetComponent<Character>();
        Hero h = caster as Hero;
        if (h != null)
        {
            h.activeSkills[0] = this;
        }
        button = Resources.Load<Sprite>("Skills/Heroes/Archer/Archer_SignTrap");
    }

    public override void SetSkill(Dictionary<string, object> param)
    {
        base.SetSkill(param);
        damage = (int)param["damage"];
        speed = (float)((double)param["speed"]);
    }

    #endregion

    public override void Activate()
    {
        Debug.Log(caster);
        caster.ChangeAction(CharacterAction.Active1);
        caster.Anim.onCue += SetTrap;
        SlowMotion();
    }

    protected virtual void SetTrap()
    {
        TimeSystem.GetTimeSystem().UnSlowMotion();
        caster.Anim.onCue -= SetTrap;
        trap.SetActive(true);
        trap.transform.position = caster.transform.position;
    }

    protected void SlowMotion()
    {
        List<ITimeHandler> temp = new List<ITimeHandler>();
        temp.Add(this);
        temp.Add(this.caster);

        ITimeHandler[] nonSlow = new ITimeHandler[temp.Count];
        temp.CopyTo(nonSlow);
        TimeSystem.GetTimeSystem().SlowMotion(nonSlow);
    }
}
