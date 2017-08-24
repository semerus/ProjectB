using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_SignTrap : HeroActive {

    GameObject trap;

    public void TrapOn(int abillity)
    {
        switch(abillity)
        {
            case 1:
                trap = Instantiate(Resources.Load<GameObject>("Skills / Heroes / Archer / SignTrap / SignTrap_Abillity1"));
                break;

            case 2:
                trap = Instantiate(Resources.Load<GameObject>("Skills / Heroes / Archer / SignTrap / SignTrap_Abillity2"));
                break;
        }
        trap.SetActive(true);
        trap.transform.position = caster.transform.position;
    }
}
