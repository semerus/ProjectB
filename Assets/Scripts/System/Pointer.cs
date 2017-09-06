using UnityEngine;

public class Pointer : MonoBehaviour {

	Color[] colors = new Color[2];

	GameObject pointer;
	GameObject selected;
	LineRenderer line;

	void Awake () {
		colors [0] = new Color (0.137f, 0.769f, 0.424f);
		colors [1] = new Color (0.937f, 0.235f, 0.439f);

		pointer = transform.Find("Oval").gameObject;
		selected = transform.Find ("Selected").gameObject;
		line = GetComponentInChildren<LineRenderer> ();

		selected.GetComponent<SpriteRenderer> ().sortingLayerName = "Selected";
		pointer.GetComponent<SpriteRenderer> ().sortingLayerName = "UI";
		line.sortingLayerName = "UI";
	}

	void Start() {
		gameObject.SetActive (false);
	}

	public void PositionPointer(Vector3 pos, Vector3 sender, PointerType type) {
		if (!gameObject.activeSelf) {
			gameObject.SetActive (true);
		}

		// change sprites and color to the corresponding situation
		switch (type) {
		case PointerType.Move:
			selected.GetComponent<SpriteRenderer> ().sprite = Background.GetBackground ().Sprites [0];
			pointer.GetComponent<SpriteRenderer> ().sprite = Background.GetBackground ().Sprites [1];
			line.startColor = colors [0];
			line.endColor = colors [0];
			break;
		case PointerType.Attack:
			selected.GetComponent<SpriteRenderer> ().sprite = Background.GetBackground ().Sprites [2];
			pointer.GetComponent<SpriteRenderer> ().sprite = Background.GetBackground ().Sprites [3];
			line.startColor = colors [1];
			line.endColor = colors [1];
			break;
		}

		Vector3[] vertice = new Vector3[2];
		selected.transform.position = sender;
		pointer.transform.position = pos;
		vertice [0] = pos;
		vertice [1] = sender;
		line.SetPositions (vertice);
	}

	public void DeactivatePointer() {
		gameObject.SetActive (false);
	}
}

public enum PointerType {
	Move,
	Attack
}
