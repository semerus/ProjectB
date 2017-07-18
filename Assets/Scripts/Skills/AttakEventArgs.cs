using System;
using UnityEngine;

public class AttakEventArgs : EventArgs {
    public IBattleHandler attacker;
    public IBattleHandler receiver;

    public int attackValue;

    // attack Type()
}
