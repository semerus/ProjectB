using UnityEngine;

public class Background : MonoBehaviour, IDoubleTapHandler {
	#region IDoubleTapHandler implementation

	public void OnDoubleTap (Vector3 pixelPos)
	{
		Vector3 p = Camera.main.ScreenToWorldPoint (pixelPos);
		p = p + new Vector3(0f, 0f, 10f);
		BattleManager.GetBattleManager ().MoveAllFriendly (p);
	}

	#endregion
}
