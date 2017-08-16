using UnityEngine;

public class OgreSetFire : Skill {

	#region implemented abstract members of Skill
	public override void Activate ()
	{
		OgreMale m = caster as OgreMale;
		if (m != null) {
			OgreFemale f = m.partner as OgreFemale;
			if (f != null) {
				f.Skills[3].OnCast ();
			} else {
				Debug.LogError("Wrong OgreSetFire implementation");
			}
		}
	}
	#endregion

	void Awake() {
		caster = gameObject.GetComponent<Character> ();
	}
}