using UnityEngine;

public class OgreSetFire : Skill {
	
	#region implemented abstract members of Skill
	public override void Activate (IBattleHandler target)
	{
		OgreMale m = caster as OgreMale;
		if (m != null) {
			OgreFemale f = m.partner as OgreFemale;
			if (f != null) {
				f.atk4.OnCast ();
			} else {
				Debug.LogError("Wrong OgreSetFire implementation");
			}	
		}
	}
	#endregion
}