using UnityEngine;

public class Bet : MonoBehaviour
{
    [SerializeField] private int betPrize;
    public int BetPrize
    {
        get { return betPrize; }

    }
    [SerializeField] private int betIncrease;
    [SerializeField] private int maxBet;
    [SerializeField] private int minBet;

    public void IncreaseBet(int maxBet)
    {
        betPrize += betIncrease;
        if (betPrize > maxBet)
        {
            betPrize = minBet;
        }
    }

    public void IncreaseBetToMax(int maxBet)
    {
        betPrize = maxBet;
    }
    public void LetBetDouble(int maxBet)
    {
        betPrize *= 2;
        if (betPrize > maxBet)
        {
            betPrize = minBet;
        }
    }
    /*to do :
     * 最低賭注
     * 加注
     * 減注
     * all in
     * 玩家金錢低於賭注不能賭
    */
}
