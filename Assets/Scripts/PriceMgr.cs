using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PriceMgr : MonoBehaviour
{
    [SerializeField] private List<PriceSObj> priceSObjs;
    public List<PriceSObj> PriceSObjs
    {
        get { return priceSObjs; }
    }
    //[Tooltip("遊戲物件")]
    //[SerializeField] private List<GameObject> prices;
    [SerializeField] private List<Row> Rows;
    System.Random rand;
    public int combo;
    private void Awake()
    {
        rand = new System.Random();
        priceSObjs = Resources.LoadAll<PriceSObj>("Prices/").ToList();
        for (int i = 0; i < Rows.Count; i++)
        {
            for (int j = 0; j < Rows[i].priceInRow.Count; j++)
            {
                Rows[i].priceInRow[j].priceSObj = priceSObjs[rand.Next(0, priceSObjs.Count)];
            }

        }

    }
    private void Update()
    {
        //StartCoroutine(randPrice());
        for (int i = 0; i < Rows[0].priceInRow.Count; i++)
        {
            foreach (Price price in Rows[i].priceInRow)
            {
                price.prices.Clear();
                price.prices.AddRange(Rows[i + 1].priceInRow);
            }

        }
    }
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
    public void stopLine()
    {
        //todo:
        //從左邊數起，若是這Row.priceInRow[任意數字].PriceScore = Row+1.priceInRow[任意數字].PriceScore的話
        //combo數就+1
        //如果combo數大於5的話就連成一條線

        //將右方Price加入左方Price清單
        for (int i = 0; i < Rows[0].priceInRow.Count; i++)
        {
            foreach (Price price in Rows[i].priceInRow)
            {
                price.prices.Clear();
                price.prices.AddRange(Rows[i + 1].priceInRow);
            }

        }
        //檢查隔壁是否有相同元素，若有就計算combo數，並強制跳行
        combo = 0;
        for (int i = 0; i < Rows.Count - 1; i++)
        {

            for (int j = 0; j < Rows[i].priceInRow.Count; j++)
            {


                foreach (Price price in Rows[0].priceInRow)
                {
                    if (price.PriceScore == Rows[i + 1].priceInRow[j].PriceScore)
                    {
                        combo++;
                        break;
                    }
                }

            }

        }

    }
}
