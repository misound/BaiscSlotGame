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
    /// <summary>
    /// 增加賭注，若超過最大值就變成最低賭注
    /// </summary>
    /// <param name="maxBet">最大賭注(通常為玩家總點數)</param>
    public void IncreaseBet(int maxBet)
    {
        betPrize += betIncrease;
        if (betPrize > maxBet)
        {
            betPrize = minBet;
        }
    }
    /// <summary>
    /// 增加賭注至最大值，若賭注 < 0 則回到最小值
    /// </summary>
    /// <param name="maxBet">最大賭注(通常為玩家總點數)</param>
    public void IncreaseBetToMax(int maxBet)
    {
        betPrize = maxBet;
        if (maxBet <= 0)
        {
            betPrize = minBet;
        }
    }
    /// <summary>
    /// 下注雙倍，若超過最大值就變成最低賭注
    /// </summary>
    /// <param name="maxBet">最大賭注(通常為玩家總點數)</param>
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
