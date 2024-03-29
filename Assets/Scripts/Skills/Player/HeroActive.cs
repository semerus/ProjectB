﻿/*
 * Written by Insung Kim
 * Updated: 2017.08.16
 */
using UnityEngine;

public abstract class HeroActive : Skill, IHeroActiveUI {

	protected Sprite button;

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
