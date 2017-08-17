/*
 * Written by Insung Kim
 * Updated: 2017.08.16
 */
using UnityEngine;

public abstract class HeroActive : Skill, IHeroActiveUI {

	protected Sprite button;

<<<<<<< HEAD
	#region implemented abstract members of Skill

	public override void Activate ()
	{

	}

	#endregion

=======
>>>>>>> 96a441a56d03b4f6eda8cbf73eb63b00e7d93ad2
	#region implemented IHeroActiveUI

	public Sprite InGameUI {
		get {
			return button;
		}
	}

	public float CurCoolDown {
		get {
			return timer_cooldown;
		}
	}

	public float MaxCoolDown {
		get {
			return cooldown;
		}
	}

    #endregion

    #region MonoBehaviours
    // State
    /// <summary>
    /// index 0 = prepareTime, 1 = invokeTime, 2 = finishTime
    /// </summary>
    protected float[] motionTimes; 
    protected float curMotionTime;
    protected bool isCameraTriggered;
    #endregion
}
