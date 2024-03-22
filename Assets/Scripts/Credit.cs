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
    /// ��֪��a�I��
    /// </summary>
    /// <param name="Bets">��`</param>
    public void DecreasePlayerCredit(int Bets)
    {
        creditPoint -= Bets;
        if (creditPoint < 0)
        {
            creditPoint = 0;
        }
    }
    /// <summary>
    /// �W�[���a�I��
    /// </summary>
    /// <param name="Price">�s�u���y</param>
    public void IncreasePlayerCredit(int Price)
    {
        creditPoint += Price;
    }
}
