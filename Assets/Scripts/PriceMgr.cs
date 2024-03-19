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
    [SerializeField] private List<Price> Prices = new List<Price>();
    [SerializeField] private List<List<Price>> priceGroup = new List<List<Price>>();
    //[Tooltip("�C������")]
    //[SerializeField] private List<GameObject> prices;
    [SerializeField] private List<Row> Rows;
    System.Random rand;
    [System.Serializable]
    public struct priceCombo
    {
        public Price.priceName type;
        public int count;
    }
    public priceCombo[] combo;

    private void Awake()
    {
        InstanPrice();
        PriceGroup();
        PriceCombo();
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
    private void InstanPrice()
    {
        rand = new System.Random();

        priceSObjs = Resources.LoadAll<PriceSObj>("Prices/").ToList();
        for (int i = 0; i < Rows.Count; i++)
        {
            for (int j = 0; j < Rows[i].priceInRow.Count; j++)
            {
                int next = rand.Next(0, priceSObjs.Count);
                Rows[i].priceInRow[j].priceSObj = priceSObjs[next];
                int typeNum = next + 1;
                Rows[i].priceInRow[j].PriceName = (Price.priceName)typeNum - 1;
                Prices.Add(Rows[i].priceInRow[j]);
            }

        }

        for (int i = 0; i < Rows[0].priceInRow.Count; i++)
        {
            foreach (Price price in Rows[i].priceInRow)
            {
                price.prices.Clear();
                price.prices.AddRange(Rows[i + 1].priceInRow);
            }

        }
    }
    private void PriceGroup()
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
    void PriceCombo()
    {

        combo = new priceCombo[priceGroup.Count];
        foreach (List<Price> priceGrop in priceGroup)
        {
            int comboIndex = priceGroup.IndexOf(priceGrop);
            combo[comboIndex].type = priceGrop[0].PriceName;
            combo[comboIndex].count = 0;


        }
        foreach (Price price in Rows[0].priceInRow)
        {
            checkRightRow(price);
        }
    }
    void FindRemovePrice(Price price)
    {

    }
    void checkRightRow(Price price)
    {
        int indexCount = 0;
        int a = 0;
        foreach (Price pricee in price.prices)
        {
            if (price.PriceName == pricee.PriceName && indexCount == 0)
            {
                indexCount++;
                a++;
                checkRightRow(pricee);
                break;
            }
        }
        //�p�G �������P�t�@�M�椺���������ۦP
        //�h ��combo�_��
        //���p�G ���@�����P�t�@�M�椺�����@�����ۦP
        //�h combo++ �B �����M�椺�����j�_��
        /* ���ѤW�Z�ݳo��
         *to do:�p�G���@�ӳ̤j��combo��
         *�B��ѻ��j�Ӷǻ�combo
         *�h�b�̤jcombo�ƪ����j�ӥ�X��Price�O�_���@�ӧ��㪺�s�u
         *�p�G���s�u�A�h�s�W�@��List<Price>
         *�å��LineMgr�Ӳ��ͦ�LineRenderer���󪺹C������
         *����transform�N�|���󤸥󤤪��`�I
        */
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

}
