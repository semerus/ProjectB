using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour, IDoubleTapHandler {

	private static Background instance;
	protected GameObject pointerPrefab;

	protected Stack<Pointer> pointers = new Stack<Pointer>();
	protected Dictionary<Character, Pointer> current = new Dictionary<Character, Pointer>();
	protected BoxCollider2D boundary;

	protected string[] paths = {
		"UI/Control/move_basic_character", "UI/Control/move_basic_ground",
		"UI/Control/attack_basic_character", "UI/Control/attack_basic_enemy"
	};

	protected Sprite[] sprites = new Sprite[4];

	public static Background GetBackground() {
		if (!instance) {
			instance = GameObject.FindObjectOfType (typeof(Background)) as Background;
			if (!instance)
				Debug.LogError ("No active Background in the scene");
		}
		return instance;
	}

	public Sprite[] Sprites {
		get {
			return sprites;
		}
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
		GetBackground ();
		boundary = GetComponentInChildren<BoxCollider2D> ();
		pointerPrefab = Resources.Load ("Background/Pointer") as GameObject;

		for (int i = 0; i < paths.Length; i++) {
			sprites [i] = Resources.Load<Sprite> (paths [i]);
		}
	}

	/// <summary>
	/// Checks the boundaries.
	/// </summary>
	/// <returns><c>true</c>, if inside boundaries, <c>false</c> if outside boundaries.</returns>
	/// <param name="pos">Position.</param>
	public bool CheckBoundaries(Vector3 pos) {
		if (pos.x - (boundary.bounds.max.x + 0.01f) > 0 || pos.y - (boundary.bounds.max.y + 0.01f) > 0 ||
			pos.x - (boundary.bounds.min.x - 0.01f) < 0 || pos.y - (boundary.bounds.min.y - 0.01f) < 0) {
			return false;
		} else
			return true;
	}

	public void PositionPointer(Vector3 pos, Character sender, PointerType type) {
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
		p.PositionPointer (pos, sender.transform.position, type);
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
