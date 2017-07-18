using UnityEngine;
using UnityEngine.UI;

public sealed class UIManager : MonoBehaviour {

	private static UIManager instance;
	public GameObject timer;

	public static UIManager GetUIManager() {
		if (!instance) {
			instance = GameObject.FindObjectOfType (typeof(UIManager)) as UIManager;
			if (!instance)
				Debug.LogError ("No active UIManager in the scene");
		}
		return instance; 
	}

	void Awake() {
		GetUIManager ();
	}

	public void UpdateTime(float time) {
		Text text = timer.GetComponentInChildren<Text> ();
		string minutes = ((int)(time / 60f)).ToString("D2");
		string seconds = ((int)(time % 60f)).ToString("D2");
		string temp = minutes + ":" + seconds;
		text.text = temp;
    }

    public void ResizeUI(bool isFixed, int sizeRatio)
    {
        ResizeUI_ItemButton(isFixed, sizeRatio);
        ResizeUI_SkillButton(isFixed, sizeRatio);
        ResizeUI_Timer(isFixed, sizeRatio);
        ResizeUI_PauseButton(isFixed, sizeRatio);
    }

    public void ResizeUI_Timer(bool isFixed, int ratio)
    {
        
    }

    public void ResizeUI_PauseButton(bool isFixed, int ratio)
    {

    }

    public void ResizeUI_SkillButton(bool isFixed, int ratio)
    {

    }

    public void ResizeUI_ItemButton(bool isFixed, int ratio)
    {
    }
}
