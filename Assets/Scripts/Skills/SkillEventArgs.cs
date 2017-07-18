using System;
using UnityEngine;

public class SkillEventArgs : EventArgs{
	public string name;
    public bool result;

	public SkillEventArgs(string name, bool result) {
		this.name = name;
        this.result = result;
	}
}
