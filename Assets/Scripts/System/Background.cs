using UnityEngine;

public class Background : MonoBehaviour, IDoubleTapHandler {

	private static Background instance;
	public Pointer pointer;

	public static Background GetBackground() {
		if (!instance) {
			instance = GameObject.FindObjectOfType (typeof(Background)) as Background;
			if (!instance)
				Debug.LogError ("No active Background in the scene");
		}
		return instance;
	}


	#region IDoubleTapHandler implementation

	public void OnDoubleTap (Vector3 pixelPos)
	{
		Vector3 p = Camera.main.ScreenToWorldPoint (pixelPos);
		p = p + new Vector3(0f, 0f, 10f);
		BattleManager.GetBattleManager ().MoveAllFriendly (p);
	}

	#endregion

	void Awake() {
		pointer = GetComponentInChildren<Pointer> ();
	}
}
