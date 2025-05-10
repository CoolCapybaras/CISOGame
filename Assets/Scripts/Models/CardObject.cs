using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CISO/New Card Object", fileName = "CardObject")]
public class CardObject: ScriptableObject
{
    public CardType type;
    public Sprite cardFront;
}
