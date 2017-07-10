using UnityEngine;

public class Pointer : MonoBehaviour {

	GameObject pointer;
	LineRenderer line;

	void Awake () {
		pointer = GameObject.Find("Oval");
		line = GetComponentInChildren<LineRenderer> ();
		line.sortingLayerName = "UI";
	}

	void Start() {
		gameObject.SetActive (false);
	}

	public void PositionPointer(Vector3 pos, Vector3 sender) {
		if (!gameObject.activeSelf) {
			gameObject.SetActive (true);
		}

		Vector3[] vertice = new Vector3[2];
		pointer.transform.position = pos;
		vertice [0] = pos;
		vertice [1] = sender;
		line.SetPositions (vertice);
	}

	public void DeactivatePointer() {
		gameObject.SetActive (false);
	}
}
