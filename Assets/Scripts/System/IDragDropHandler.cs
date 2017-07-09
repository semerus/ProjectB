using UnityEngine;

public interface IDragDropHandler {

	void OnBeginDrag();
	void OnDrag();
	void OnDrop(Vector3 pixelPos);
}
