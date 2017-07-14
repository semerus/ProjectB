using System;
using UnityEngine;

public class MoveEventArgs : EventArgs {
	public bool result;
	public Vector3 currentPosition;

	public MoveEventArgs(bool result, Vector3 currentPosition) {
		this.result = result;
		this.currentPosition = currentPosition;
	}
}
