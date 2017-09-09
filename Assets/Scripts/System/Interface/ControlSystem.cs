using UnityEngine;

public sealed class ControlSystem : MonoBehaviour {

	Transform[] target = new Transform[2];
	int[] touchId = new int[2]{ -1, -1 };
	Vector3[] startPos = new Vector3[2];

	float[] lastClickTime = new float[2]{ 0f, 0f };
	bool[] oneClick = new bool[2]{ false, false };
	float clickDelay = 0.2f;

	int[] masks = new int[3] { 1 << 8, 1 << 9, 1 << 10 };

	void Update() {
		#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
		ProcessClick ();
		#endif

		#if UNITY_ANDROID
		ProcessTouch();
		#endif
	}

	#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
	private void ProcessClick() {
		if (Input.GetMouseButtonDown (0)) {
			startPos [0] = Input.mousePosition;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			for (int i = 0; i < masks.Length; i++) {
				RaycastHit2D hitInfo = Physics2D.GetRayIntersection (ray, Mathf.Infinity, masks[i]);
				if (hitInfo.collider != null) {
					target [0] = hitInfo.collider.transform.root;
					IDragDropHandler beginDrag = target [0].GetComponent<IDragDropHandler> ();
					if (beginDrag != null) {
						beginDrag.OnBeginDrag ();
					}
					break;
				}
			}
		} else if (Input.GetMouseButtonUp (0)) {
			if (target [0] == null)
				return;
			if (Vector3.Distance (Input.mousePosition, startPos [0]) < 0.1f) {
				if (!oneClick[0]) {
					oneClick[0] = true;
					lastClickTime[0] = Time.time;
				} else {
			// double tap
					IDoubleTapHandler doubleTap = target [0].GetComponent<IDoubleTapHandler> ();
					if (doubleTap != null) {
						doubleTap.OnDoubleTap (Input.mousePosition);
					}
					oneClick[0] = false;
				}
			} else { 
			// drop
				IDragDropHandler drop = target [0].GetComponent<IDragDropHandler> ();
				if (drop != null) {
					drop.OnDrop (Input.mousePosition);
					oneClick[0] = false;
					target [0] = null;
				}
			}
		} else if (Input.GetMouseButton (0)) {
			// on drag
			if (target[0] != null) {
				if (Vector3.Distance(Input.mousePosition, startPos[0]) > 0.1f) {
					IDragDropHandler drag = target [0].GetComponent<IDragDropHandler> ();
					if (drag != null) {
						drag.OnDrag (Input.mousePosition);
					}
				}
			}
		}
		if (oneClick[0]) {
			if (Time.time - lastClickTime[0] > clickDelay) {
			// tap
				ITapHandler tap = target [0].GetComponent<ITapHandler> ();
				if (tap != null) {
					tap.OnTap ();
				}
				oneClick[0] = false;
				target [0] = null;
			}
		}
	}
	#endif


	#if UNITY_ANDROID

	private void ProcessTouch() {
		for (int i = 0; i < Input.touchCount; i++) {
			switch (Input.GetTouch (i).phase) {
			case TouchPhase.Began:
				SaveFirstTouch (i);
				break;
			case TouchPhase.Moved:
				OnDrag(Input.GetTouch(i).fingerId, i);
				break;
			case TouchPhase.Ended:
				ResetTouch (Input.GetTouch(i).fingerId, i);
				break;
			default:
				break;
			}
			if (oneClick [i]) {
				if (Time.time - lastClickTime [i] > clickDelay) {
					// tap
					ITapHandler tap = target[i].GetComponent<ITapHandler>();
					if (tap != null) {
						tap.OnTap ();
					}
					oneClick [i] = false;
					target [i] = null;
					touchId [i] = -1;
				}
			}
		}
	}

	private void SaveFirstTouch(int index) {
		for (int i = 0; i < touchId.Length; i++) {
			// unused touchId
			if (touchId [i] == -1) {
				touchId [i] = Input.GetTouch(index).fingerId;
				startPos [i] = Input.GetTouch (index).position;
				Ray ray = Camera.main.ScreenPointToRay (startPos [i]);

				for (int j = 0; j < masks.Length; j++) {
					RaycastHit2D hitInfo = Physics2D.GetRayIntersection (ray, Mathf.Infinity, masks[j]);
					if (hitInfo.collider != null) {
						target [i] = hitInfo.collider.transform.root;
						IDragDropHandler beginDrag = target [i].GetComponent<IDragDropHandler> ();
						if (beginDrag != null) {
							beginDrag.OnBeginDrag ();
						}
						break;
					}
				}
				break;
			}
		}
	}

	private void OnDrag(int fingerId, int index) {
		for (int i = 0; i < touchId.Length; i++) {
			if (target [i] == null) {
				continue;
			}
			if (touchId [i] == fingerId) {
				IDragDropHandler drag = target [i].GetComponent<IDragDropHandler> ();
				if (drag != null) {
					drag.OnDrag (Input.GetTouch (index).position);
				}
			}
		}
	}

	private void ResetTouch(int fingerId, int index) {
		for (int i = 0; i < touchId.Length; i++) {
			if (target [i] == null) {
				touchId [i] = -1;
				oneClick [i] = false;
				continue;
			}
			if (touchId [i] == fingerId) {
				// reset touch and give drop
				if (Vector3.Distance(Input.GetTouch(index).position, startPos [i]) < 0.1f) {
					if (!oneClick [i]) {
						oneClick [i] = true;
						lastClickTime [i] = Time.time;
					} else {
						// double tap
						IDoubleTapHandler doubleTap =  target[i].GetComponent<IDoubleTapHandler>();
						if (doubleTap != null) {
							doubleTap.OnDoubleTap (Input.GetTouch(index).position);
						}
						oneClick [i] = false;
					}
				} else {
					IDragDropHandler drop = target [i].GetComponent<IDragDropHandler> ();
					if (drop != null) {
						drop.OnDrop (Input.GetTouch(index).position);
						oneClick [i] = false;
					}
				}
				target [i] = null;
				startPos [i] = new Vector3();
				touchId [i] = -1;
				break;
			}
		}
	}
	#endif


}