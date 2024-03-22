using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SlotController : MonoBehaviour
{
    /* to do:
     * �s���s
     * �]��`�B��ӡB���ګh�t�~�}
     * 
    */
    [Header("UI")]
    [Header("Buttons")]
    [SerializeField] private Button startSpin;
    [SerializeField] private Button BetOneBtn;
    [SerializeField] private Button BetBaxBtn;
    [SerializeField] private Button DoubleBtn;
    [SerializeField] private Button NoMoneyBtn;
    [Header("TextMeshUGUI")]
    [SerializeField] private TextMeshProUGUI playerCreditTMP;
    [SerializeField] private TextMeshProUGUI NumOfTicketTMP;
    [SerializeField] private TextMeshProUGUI SumOfTicketTMP;
    [SerializeField] private GameObject poorGuyImage;
    public Credit credit;
    public Bet bet;
    public PriceMgr priceMgr;
    private bool isDirty = false;
    public AudioSource audioSource;
    private void Awake()
    {
        BetOneBtn.onClick.AddListener(BetOneBtn_OnClick);
        startSpin.onClick.AddListener(StartBtn_OnClick);
        BetBaxBtn.onClick.AddListener(BetBaxBtn_OnClick);
        DoubleBtn.onClick.AddListener(DoubleBtn_OnClick);
        NoMoneyBtn.onClick.AddListener(NoMoneyBtn_OnClick);
        SumOfTicketTMP.text = priceMgr.PriceComboSum().ToString();
        isDirty = true;
    }
    private void Update()
    {
        playerCreditTMP.text = credit.CreditPoint.ToString();
        NumOfTicketTMP.text = bet.BetPrize.ToString();
    }
    #region ���s�s��
    /// <summary>
    /// �}�l�e�A���i���I�ƪ��޿�B�z
    /// </summary>
    private void StartBtn_OnClick()
    {
        if (credit.CreditPoint >= bet.BetPrize)
        {
            credit.DecreasePlayerCredit(bet.BetPrize);
            priceMgr.PressStart();
        }
        else if (credit.CreditPoint < bet.BetPrize && credit.CreditPoint > 0)
        {
            bet.IncreaseBetToMax(credit.CreditPoint);
            credit.DecreasePlayerCredit(bet.BetPrize);
            priceMgr.PressStart();
        }
        else if (credit.CreditPoint > 0)
        {
            credit.DecreasePlayerCredit(bet.BetPrize);
            priceMgr.PressStart();
        }
        else
        {
            poorGuyImage.SetActive(true);
            audioSource.Play();
        }

        isDirty = true;
    }
    private void BetOneBtn_OnClick()
    {
        bet.IncreaseBet(credit.CreditPoint);
        isDirty = true;
    }
    private void BetBaxBtn_OnClick()
    {
        bet.IncreaseBetToMax(credit.CreditPoint);
        isDirty = true;
    }
    private void DoubleBtn_OnClick()
    {
        bet.LetBetDouble(credit.CreditPoint);
        isDirty = true;
    }
    private void NoMoneyBtn_OnClick()
    {
        SceneManager.LoadScene(0);
    }
    public void SumTheScore()
    {
        credit.IncreasePlayerCredit(priceMgr.PriceComboSum());
        SumOfTicketTMP.text = priceMgr.PriceComboSum().ToString();
    }
    #endregion
    /* to do
     * �Y�O���a�I�Ƥ���minBet
     * �h����A�ާ@�A�Ҧp�[�J�@�i�Ϥ��צ�Ҧ��F��
     * �A����Restart�~�୫�s�֦��w�]�I��
     */
}
