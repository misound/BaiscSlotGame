using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit : MonoBehaviour
{
    [SerializeField] private int creditPoint;
    public int CreditPoint
    {
        get { return creditPoint; }
    }
    public void DecreasePlayerCredit(int Bets)
    {
        creditPoint -= Bets;
        if (creditPoint < 0)
        {
            creditPoint = 0;
        }
    }
    public void IncreasePlayerCredit(int Price)
    {
        creditPoint += Price;
    }
}
