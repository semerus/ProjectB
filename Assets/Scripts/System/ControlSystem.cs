using UnityEngine;

public sealed class ControlSystem : MonoBehaviour {
	#if UNITY_EDITOR
	// use mouse click
	#endif

	Transform[] target = new Transform[2];
	int[] touchId = new int[2]{ -1, -1 };
	Vector3[] startPos = new Vector3[2];

	float lastClickTime = 0f;
	bool oneClick = false;
	float clickDelay = 0.2f;
	int mask_area = 1 << 8;
	int mask_char = 2 << 8;

	#if UNITY_ANDROID
	// use touch
		
	#endif
	// singleton
	// active during battlescene

	// touch time analysis

	// Tap

	// DoubleTap

	// Drag and drop

	// Disable input


	void Update() {
		#if UNITY_EDITOR
		// use mouse click
		ProcessClick ();
		#endif



		#if UNITY_ANDROID
		ProcessTouch();
		#endif
	}

	#if UNITY_EDITOR
	// use mouse click
	#endif

	private void ProcessClick() {
		if (Input.GetMouseButtonDown (0)) {
			startPos [0] = Input.mousePosition;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D hitInfo = Physics2D.GetRayIntersection (ray, Mathf.Infinity, mask_char);

			if (hitInfo.collider != null) {
				target [0] = hitInfo.collider.transform.root;
			} else {
				hitInfo = Physics2D.GetRayIntersection (ray, Mathf.Infinity, mask_area);
				if (hitInfo.collider != null) {
					target [0] = hitInfo.collider.transform.root;
				}
			}
		} else if (Input.GetMouseButtonUp (0)) {
			if (target [0] == null)
				return;
			// tap
			if (Vector3.Distance (Input.mousePosition, startPos [0]) < 0.1f) {
				if (!oneClick) {
					oneClick = true;
					lastClickTime = Time.time;
				} else {
					// double tap
					IDoubleTapHandler doubleTap = target [0].GetComponent<IDoubleTapHandler> ();
					if (doubleTap != null) {
						doubleTap.OnDoubleTap (Input.mousePosition);
					}
					oneClick = false;
				}
			} else { // drop
				IDragDropHandler drop = target [0].GetComponent<IDragDropHandler> ();
				if (drop != null) {
					drop.OnDrop (Input.mousePosition);
				}
			}
		} else if (Input.GetMouseButton (0)) {
			// on drag
			if (target[0] != null) {
				if(Vector3.Distance(Input.mousePosition, startPos[0]) > 0.1f) {
					IDragDropHandler drag = target [0].GetComponent<IDragDropHandler> ();
					if (drag != null) {
						drag.OnDrag (Input.mousePosition);
					}
				}
			}
		}
		if (oneClick) {
			if (Time.time - lastClickTime > clickDelay) {
				// tap
				ITapHandler tap = target [0].GetComponent<ITapHandler> ();
				if (tap != null) {
					tap.OnTap ();
				}
				oneClick = false;
				target [0] = null;
			}
		}
	}

	// need fixing on touch
	private void ProcessTouch() {
		for (int i = 0; i < Input.touchCount; i++) {
			switch (Input.GetTouch (i).phase) {
			case TouchPhase.Began:
				SaveFirstTouch (i);
				break;
			case TouchPhase.Moved:
				// drag
				break;
			case TouchPhase.Ended:
				ResetTouch (Input.GetTouch(i).fingerId, i);
				break;
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
				RaycastHit2D hitInfo = Physics2D.GetRayIntersection (ray);
				if (hitInfo.collider != null) {
					target[i] = hitInfo.collider.transform.root;
				}
				break;
			}
		}
	}

	private void ResetTouch(int fingerId, int index) {
		for (int i = 0; i < touchId.Length; i++) {
			if (touchId [i] == fingerId) {
				// reset touch and give drop
				if (Vector3.Distance(Input.GetTouch(index).position, startPos [i]) < 0.1f) {
					ITapHandler tap = target [i].GetComponent<ITapHandler> ();
					if (tap != null) {
						tap.OnTap ();
					}
				} else {
					IDragDropHandler drop = target [i].GetComponent<IDragDropHandler> ();
					if (drop != null) {
						drop.OnDrop (Input.GetTouch(index).position);
					}
				}
				target [i] = null;
				startPos [i] = new Vector3();
				touchId [i] = -1;
				break;
			}
		}
	}
}
