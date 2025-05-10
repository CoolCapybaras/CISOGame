using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<CardObject> cards;

    public CardObject GetCard(CardType type) => cards.First(x => x.type == type);
}
