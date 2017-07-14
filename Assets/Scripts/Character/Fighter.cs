using UnityEngine;

public class Fighter : Hero {

	void Awake() {
		// temporary value given
		id = 1;
		team = Team.Friendly;
        state = CharacterState.Idle;
        queueState = CharacterState.None;
        maxHp = 100000;
		hp = 100000;
		speed_x = 2.57f;
		speed_y = 1.4f;

		// temporary spawning -> should be moved to BattleManager
		Spawn();

        // for Skill debugging
        autoAttack = gameObject.AddComponent<Fighter_Attack>();
        autoAttack.SetSkill(this);

        passiveSkill = gameObject.AddComponent<Fighter_Passive>();
        passiveSkill.SetSkill(this);

        activeSkills = new Skill[3];
        activeSkills[0] = gameObject.AddComponent<Fighter_MeowPunch>();
        activeSkills[1] = gameObject.AddComponent<Fighter_CounterStance>();
        activeSkills[2] = gameObject.AddComponent<Fighter_WarCry>();
        foreach(Skill eachSkill in activeSkills)
        {
            eachSkill.SetSkill(this);
        }
        
    }

    protected override void Update()
    {
        switch(queueState)
        {
            case CharacterState.None:
            case CharacterState.Idle:
            case CharacterState.Moving:
            case CharacterState.Dead:
            case CharacterState.Channeling:
                break;

            case CharacterState.AutoAttaking:
                state = queueState;
                break;

            default:
                Debug.LogError("Set type of queue State!");
                break;
            
        }

        switch (state)
        {
            case CharacterState.Idle:
                // if range hero search auto target
                break;
            case CharacterState.Moving:
                Move(moveTarget);
                break;
            case CharacterState.AutoAttaking:
                autoAttack.Activate(target);
                break;
            default:
                break;
        }
    }
}
