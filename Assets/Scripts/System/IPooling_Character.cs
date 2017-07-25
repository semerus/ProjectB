using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooling_Character {

    Stack<IPooledItem_Character> Pool { get; }
}
