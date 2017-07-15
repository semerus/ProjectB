using System;
using UnityEngine;

public class SkillEventArgs : EventArgs{
	Type skill;

	public SkillEventArgs(Type skill) {
		this.skill = skill;
	}
}
