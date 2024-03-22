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
    /// <summary>
    /// 減少玩家點數
    /// </summary>
    /// <param name="Bets">賭注</param>
    public void DecreasePlayerCredit(int Bets)
    {
        creditPoint -= Bets;
        if (creditPoint < 0)
        {
            creditPoint = 0;
        }
    }
    /// <summary>
    /// 增加玩家點數
    /// </summary>
    /// <param name="Price">連線獎勵</param>
    public void IncreasePlayerCredit(int Price)
    {
        creditPoint += Price;
    }
}
