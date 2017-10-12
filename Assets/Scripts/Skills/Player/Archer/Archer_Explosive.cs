using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Explosive : HeroActive {

    // 채널링 없음

    protected Projectile projectile = new Projectile();
    protected int damage;
    protected float speed;

    #region Setskill

    private void Update()
    {
        if(Input.GetKeyDown("f"))
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
            h.activeSkills[1] = this;
        }
        button = Resources.Load<Sprite>("Skills/Heroes/Archer/Archer_Explosive");
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
        caster.ChangeAction(CharacterAction.Active2);
        SlowMotion();
        caster.Anim.onCue += SetArrowProjectiles;
    }

    protected virtual void SetArrowProjectiles()
    {
        StartCoolDown();
        caster.Anim.onCue -= SetArrowProjectiles;
        projectile.transform.SetParent(GameObject.Find("Projectiles").transform);
        projectile.transform.position = caster.transform.position;
        EffectMove();
        TimeSystem.GetTimeSystem().UnSlowMotion();
        caster.ChangeAction(CharacterAction.Idle);
        Debug.Log("ssssggggg");
    }

    public void EffectMove()
    {
        projectile.ProjectileMove(caster.Target as Character);
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
