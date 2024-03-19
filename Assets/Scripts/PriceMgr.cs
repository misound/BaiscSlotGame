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
    //[Tooltip("�C������")]
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
        //�q����ư_�A�Y�O�oRow.priceInRow[���N�Ʀr].PriceScore = Row+1.priceInRow[���N�Ʀr].PriceScore����
        //combo�ƴN+1
        //�p�Gcombo�Ƥj��5���ܴN�s���@���u

        //�N�k��Price�[�J����Price�M��
        for (int i = 0; i < Rows[0].priceInRow.Count; i++)
        {
            foreach (Price price in Rows[i].priceInRow)
            {
                price.prices.Clear();
                price.prices.AddRange(Rows[i + 1].priceInRow);
            }

        }
        //�ˬd�j���O�_���ۦP�����A�Y���N�p��combo�ơA�ñj�����
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
