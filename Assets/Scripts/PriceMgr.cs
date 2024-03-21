using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

//把它更名為PrizeSystem會比較好?
public class PriceMgr : MonoBehaviour
{
    [Tooltip("獎品元素集合")]
    [SerializeField] private List<PriceSObj> priceSObjs;
    [Tooltip("所有Price的集合")]
    [SerializeField] private List<Price> Prices = new List<Price>();
    [Tooltip("本是將所有不同元素集合的集合體")]
    [SerializeField] private List<List<Price>> priceGroup = new List<List<Price>>();
    //[Tooltip("遊戲物件")]
    //[SerializeField] private List<GameObject> prices;
    [Tooltip("列的集合")]
    [SerializeField] private List<Row> Rows;
    System.Random rand;
    [System.Serializable]
    public struct priceCombo
    {
        public Price.priceName type;
        public int count;
    }
    public priceCombo[] combo;
    [Tooltip("要交給LineMgr的List<Price>")]
    public List<List<Price>> LinePriceGroup = new List<List<Price>>();

    public GameObject linePrefab;

    public SlotController slotController;
    [Tooltip("白癡按鈕決定變數")]
    private int pause = 0;
    private bool go;
    [SerializeField] private int pricePrize = 10;
    public Bet bet;

    float slotTimer = 2f;
    float timer = 0;
    private void Awake()
    {
        InstanPrice();
        //PriceGroup();
        //PriceCombo();
        //InsLine();
    }
    private void Update()
    {
        DumbBtnSwitch();
    }
    #region 設定獎品資訊及設定清單
    /// <summary>
    /// 設定獎品資訊及設定清單
    /// </summary>
    public void InstanPrice()
    {
        rand = new System.Random();
        //載入SObj
        priceSObjs = Resources.LoadAll<PriceSObj>("Prices/").ToList();
        for (int i = 0; i < Rows.Count; i++)
        {
            for (int j = 0; j < Rows[i].priceInRow.Count; j++)
            {
                //設隨機數
                int next = rand.Next(0, priceSObjs.Count);
                //指派SObj到Price內
                Rows[i].priceInRow[j].priceSObj = priceSObjs[next];
                //修正int變數至type
                int typeNum = next + 1;
                //指定類型
                Rows[i].priceInRow[j].PriceName = (Price.priceName)typeNum - 1;
                //依順序加入List
                Prices.Add(Rows[i].priceInRow[j]);
            }

        }
        //以任一列作為指定對象
        for (int i = 0; i < Rows[0].priceInRow.Count; i++)
        {
            //巡迴各列中的Price
            foreach (Price price in Rows[i].priceInRow)
            {
                //重置在price內的右列的清單
                price.prices.Clear();
                //加入右列的所有元素
                price.prices.AddRange(Rows[i + 1].priceInRow);
            }

        }
    }
    #endregion
    #region 獎品群組
    public void PriceGroup()
    {
        priceGroup.Clear();
        foreach (Price price in Prices)
        {
            if (!price.group)
            {
                priceGroup.Add(new List<Price>());
                int groupNum = priceGroup.Count - 1;
                priceGroup[groupNum].Add(price);
                price.group = true;
                FindMembers(price, groupNum);
            }
        }
    }
    void FindMembers(Price price, int groupNum)
    {
        foreach (Price linkPrice in price.prices)
        {
            if (linkPrice.PriceScore == price.PriceScore && linkPrice.group)
            {
                priceGroup[groupNum].Add(linkPrice);
                linkPrice.group = true;
                FindMembers(linkPrice, groupNum);
            }
        }
    }
    #endregion
    #region 連線相關
    /// <summary>
    /// 第一列的元素設為List開頭，並向右查找是否有相同元素
    /// </summary>
    public void PriceCombo()
    {
        /* 以struct來分群組及類型的作法
        combo = new priceCombo[priceGroup.Count];
        foreach (List<Price> priceGrop in priceGroup)
        {
            int comboIndex = priceGroup.IndexOf(priceGrop);
            combo[comboIndex].type = priceGrop[0].PriceName;
            combo[comboIndex].count = 0;
        }
        */

        //初始連線清單
        LinePriceGroup.Clear();
        //巡迴第一列元素
        foreach (Price price in Rows[0].priceInRow)
        {
            if (!price.linked)
            {

            }
            //每次新增List<Price>
            LinePriceGroup.Add(new List<Price>());
            //設定Index，值為LinePriceGroup的最大值
            int groupNum = LinePriceGroup.Count - 1;
            //將第一列的Price設為群組開頭
            LinePriceGroup[groupNum].Add(price);
            //將該Price標記為已連結
            price.linked = true;
            //並查找該Price右列的元素是否相同
            checkRightRow(price, groupNum);
        }
    }
    /// <summary>
    /// 檢查右列的元素並檢查是否連線
    /// </summary>
    /// <param name="price"></param>
    /// <param name="groupNum"></param>
    void checkRightRow(Price price, int groupNum)
    {
        //設定找到的次數
        int indexCount = 0;
        //巡迴該Price右列的元素
        foreach (Price pricee in price.prices)
        {
            //若元素相同且沒被找到過的狀態
            if (price.PriceName == pricee.PriceName && indexCount == 0)
            {
                //將同樣元素的Price加入LinePriceGroup[groupNum]
                LinePriceGroup[groupNum].Add(pricee);
                //同樣標記為已連結
                pricee.linked = true;
                //該列被找到的元素數量+1(等同於已找到一個元素)
                indexCount++;
                for (int i = 0; i < LinePriceGroup[groupNum].Count; i++)
                {
                    Debug.Log(LinePriceGroup[groupNum][i].name);
                }

                //再次尋找此Price右列的元素，直到右列不存在或是右列沒有元素的狀態
                checkRightRow(pricee, groupNum);
                break;
            }
        }
    }
    /// <summary>
    /// 生產賠付線
    /// </summary>
    public void InsLine()
    {
        //新建一個List<List>
        List<List<Price>> pricess = new List<List<Price>>();
        //巡迴所有在LinePriceGroup內的List<Price>
        for (int i = 0; i < LinePriceGroup.Count; i++)
        {
            //若裡面的元素大於可連線數(最大5個)
            if (LinePriceGroup[i].Count > 2)
            {
                //加入該List<Price>到暫存的List<List<Price>>內
                pricess.Add(LinePriceGroup[i]);
                //並生產賠付線遊戲物件
                GameObject temp = Instantiate(linePrefab);
                LineMgr line = temp.GetComponent<LineMgr>();
                //分配List<Price>到遊戲物件上
                line.priceGroup = LinePriceGroup[i];
            }
        }
        Debug.Log($"分數:{PriceComboSum()}");

    }

    #endregion
    #region 隨機元素
    /// <summary>
    /// 隨機元素
    /// </summary>
    public void randPrice()
    {
        for (int i = 0; i < Rows.Count; i++)
        {
            for (int j = 0; j < Rows[i].priceInRow.Count; j++)
            {
                Rows[i].priceInRow[j].priceSObj = priceSObjs[rand.Next(0, priceSObjs.Count)];
            }
        }
    }
    #endregion
    #region 怪怪的按鈕
    /// <summary>
    /// 白癡按鈕決定法
    /// </summary>
    void DumbBtnSwitch()
    {
        if (pause == 0)
        {
            randPrice();
            timer += Time.deltaTime;
        }
        if (slotTimer < timer)
        {
            pause++;
            go = true;
        }
        else if (pause == 2)
        {
            pause = 0;
        }
        if (go)
        {
            InstanPrice();
            PriceCombo();
            InsLine();
            slotController.SumTheScore();
            timer = 0;
            go = false;
        }
    }

    public void PressStart()
    {
        pause++;
    }
    #endregion
    /// <summary>
    /// 運算獎勵總和
    /// </summary>
    /// <returns>三連線以上的數字</returns>
    public int PriceComboSum()
    {
        int sum = 0;
        foreach(List<Price> prices in LinePriceGroup)
        {

            if (prices.Count > 2)
            {
                int MaxPriceCount = 0;
                int score = 0;
                MaxPriceCount = prices.Count;
                while (MaxPriceCount != 0)
                {
                    MaxPriceCount--;
                    score++;
                }
                sum += bet.BetPrize * (score * pricePrize);
            }
        }
        sum /= 20;
        return sum;
    }
    /* to do
     * 
     * 同步獎勵加總數字
     */
}
