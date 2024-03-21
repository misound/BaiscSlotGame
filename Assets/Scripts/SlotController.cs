using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    /* to do:
     * 連按鈕
     * 設賭注、獲勝、扣款則另外開
     * 
    */
    [SerializeField] private Row[] rows;
    public Row[] Rows 
    {
        get { return rows; }
    }
    [SerializeField] private Button startSpin;
    public PriceMgr PriceMgr;
    private void Awake()
    {
        startSpin.onClick.AddListener(Spin);
    }
    void Spin()
    {
        for (int i = 0; i < rows.Length; i++)
        {
            rows[i].isRolliing = true;
        }
        PriceMgr.randPrice();
    }
}
