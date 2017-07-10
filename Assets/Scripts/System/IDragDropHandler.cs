using UnityEngine;

public interface IDragDropHandler {

	void OnBeginDrag();
	void OnDrag(Vector3 pixelPos);
	void OnDrop(Vector3 pixelPos);
}
