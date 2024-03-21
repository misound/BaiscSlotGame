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
    private void Awake()
    {
        BetOneBtn.onClick.AddListener(BetOneBtn_OnClick);
        startSpin.onClick.AddListener(StartBtn_OnClick);
        BetBaxBtn.onClick.AddListener(BetBaxBtn_OnClick);
        DoubleBtn.onClick.AddListener(DoubleBtn_OnClick);
        NoMoneyBtn.onClick.AddListener(DoubleBtn_OnClick);
        isDirty = true;
    }
    private void Update()
    {
        playerCreditTMP.text = credit.CreditPoint.ToString();
        NumOfTicketTMP.text = bet.BetPrize.ToString();
        SumOfTicketTMP.text = priceMgr.PriceComboSum().ToString();
    }
    private void StartBtn_OnClick()
    {
        if(credit.CreditPoint >= bet.BetPrize)
        {
            credit.DecreasePlayerCredit(bet.BetPrize);
        }
        else
        {
            poorGuyImage.SetActive(true);
        }

        isDirty = true;
    }
    private void BetOneBtn_OnClick()
    {
        bet.IncreaseBet();
        isDirty = true;
    }
    private void BetBaxBtn_OnClick()
    {
        bet.IncreaseBetToMax();
        isDirty = true;
    }
    private void DoubleBtn_OnClick()
    {
        bet.LetBetDouble();
        isDirty = true;
    }
    private void NoMoneyBtn_OnClick()
    {
        SceneManager.LoadScene(0);
    }
    public void SumTheScore()
    {
        credit.IncreasePlayerCredit(priceMgr.PriceComboSum());
    }
    /* to do
     * �Y�O���a�I�Ƥ���minBet
     * �h����A�ާ@�A�Ҧp�[�J�@�i�Ϥ��צ�Ҧ��F��
     * �A����Restart�~�୫�s�֦��w�]�I��
     */
}
