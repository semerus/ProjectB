using UnityEngine;

public class GameManager : MonoBehaviour {
	// singleton

	// active throughout the whole game

    void Awake()
    {
        //Set Scene view as horizontal
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
}
