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
    /// �W�[��`�A�Y�W�L�̤j�ȴN�ܦ��̧C��`
    /// </summary>
    /// <param name="maxBet">�̤j��`(�q�`�����a�`�I��)</param>
    public void IncreaseBet(int maxBet)
    {
        betPrize += betIncrease;
        if (betPrize > maxBet)
        {
            betPrize = minBet;
        }
    }
    /// <summary>
    /// �W�[��`�̤ܳj�ȡA�Y��` < 0 �h�^��̤p��
    /// </summary>
    /// <param name="maxBet">�̤j��`(�q�`�����a�`�I��)</param>
    public void IncreaseBetToMax(int maxBet)
    {
        betPrize = maxBet;
        if (maxBet <= 0)
        {
            betPrize = minBet;
        }
    }
    /// <summary>
    /// �U�`�����A�Y�W�L�̤j�ȴN�ܦ��̧C��`
    /// </summary>
    /// <param name="maxBet">�̤j��`(�q�`�����a�`�I��)</param>
    public void LetBetDouble(int maxBet)
    {
        betPrize *= 2;
        if (betPrize > maxBet)
        {
            betPrize = minBet;
        }
    }
    /*to do :
     * �̧C��`
     * �[�`
     * ��`
     * all in
     * ���a�����C���`�����
    */
}
