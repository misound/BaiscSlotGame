using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private void Awake()
    {
        rand = new System.Random();
        priceSObjs = Resources.LoadAll<PriceSObj>("Prices/").ToList();
        for (int i = 0; i < Rows.Count; i++)
        {
            for (int j = 0; j < Rows[i].priceInRow.Length; j++)
            {
                Rows[i].priceInRow[j].priceSObj = priceSObjs[rand.Next(0, priceSObjs.Count)];
            }

        }
    }
    private void FixedUpdate()
    {
        StartCoroutine(randPrice());
    }
    private void Update()
    {
        stopLine();
    }
    IEnumerator randPrice()
    {
        for (int i = 0; i < Rows.Count; i++)
        {
            for (int j = 0; j < Rows[i].priceInRow.Length; j++)
            {
                Rows[i].priceInRow[j].priceSObj = priceSObjs[rand.Next(0, priceSObjs.Count)];
            }
        }
        yield return new WaitForSeconds(.1f);
    }
    private void stopLine()
    {
        if (Rows[0].priceInRow[0].PriceScore == 100
            && Rows[1].priceInRow[0].PriceScore == 100
            && Rows[2].priceInRow[0].PriceScore == 100
            && Rows[3].priceInRow[0].PriceScore == 100
            && Rows[4].priceInRow[0].PriceScore == 100)
        {
            Debug.LogError("有了吧");
        }
    }
}
