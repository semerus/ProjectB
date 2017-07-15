using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour, IDoubleTapHandler {

	private static Background instance;
	protected GameObject pointerPrefab;

	protected Stack<Pointer> pointers = new Stack<Pointer>();
	protected Dictionary<Character, Pointer> current = new Dictionary<Character, Pointer>();

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
		pointerPrefab = Resources.Load ("Background/Pointer") as GameObject;
	}

	public void PositionPointer(Vector3 pos, Character sender) {
		Pointer p;
		current.TryGetValue (sender, out p);
		if (p == null) {
			if (pointers.Count < 1) {
				GameObject gm = Instantiate (pointerPrefab);
				gm.transform.SetParent (this.transform);
				p = gm.GetComponent<Pointer> ();
			} else {
				p = pointers.Pop ();
			}
			current.Add (sender, p);
		}
		p.PositionPointer (pos, sender.transform.position);
	}

	public void DeactivatePointer(Character sender) {
		Pointer p;
		current.TryGetValue (sender, out p);
		if (p != null) {
			current.Remove (sender);
			p.DeactivatePointer ();
			pointers.Push (p);
		}
	}
}
