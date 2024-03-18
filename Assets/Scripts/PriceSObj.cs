using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PriceSObj", menuName = "Price")]

public class PriceSObj : ScriptableObject
{
    public string priceName;
    public Sprite priceSprite;
    public int priceScore;
}
