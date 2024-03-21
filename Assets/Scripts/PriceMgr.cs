using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

//�⥦��W��PrizeSystem�|����n?
public class PriceMgr : MonoBehaviour
{
    [Tooltip("���~�������X")]
    [SerializeField] private List<PriceSObj> priceSObjs;
    [Tooltip("�Ҧ�Price�����X")]
    [SerializeField] private List<Price> Prices = new List<Price>();
    [Tooltip("���O�N�Ҧ����P�������X�����X��")]
    [SerializeField] private List<List<Price>> priceGroup = new List<List<Price>>();
    //[Tooltip("�C������")]
    //[SerializeField] private List<GameObject> prices;
    [Tooltip("�C�����X")]
    [SerializeField] private List<Row> Rows;
    System.Random rand;
    [System.Serializable]
    public struct priceCombo
    {
        public Price.priceName type;
        public int count;
    }
    public priceCombo[] combo;
    [Tooltip("�n�浹LineMgr��List<Price>")]
    public List<List<Price>> LinePriceGroup = new List<List<Price>>();

    public GameObject linePrefab;

    public SlotController slotController;
    [Tooltip("��è���s�M�w�ܼ�")]
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
    #region �]�w���~��T�γ]�w�M��
    /// <summary>
    /// �]�w���~��T�γ]�w�M��
    /// </summary>
    public void InstanPrice()
    {
        rand = new System.Random();
        //���JSObj
        priceSObjs = Resources.LoadAll<PriceSObj>("Prices/").ToList();
        for (int i = 0; i < Rows.Count; i++)
        {
            for (int j = 0; j < Rows[i].priceInRow.Count; j++)
            {
                //�]�H����
                int next = rand.Next(0, priceSObjs.Count);
                //����SObj��Price��
                Rows[i].priceInRow[j].priceSObj = priceSObjs[next];
                //�ץ�int�ܼƦ�type
                int typeNum = next + 1;
                //���w����
                Rows[i].priceInRow[j].PriceName = (Price.priceName)typeNum - 1;
                //�̶��ǥ[�JList
                Prices.Add(Rows[i].priceInRow[j]);
            }

        }
        //�H���@�C�@�����w��H
        for (int i = 0; i < Rows[0].priceInRow.Count; i++)
        {
            //���j�U�C����Price
            foreach (Price price in Rows[i].priceInRow)
            {
                //���m�bprice�����k�C���M��
                price.prices.Clear();
                //�[�J�k�C���Ҧ�����
                price.prices.AddRange(Rows[i + 1].priceInRow);
            }

        }
    }
    #endregion
    #region ���~�s��
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
    #region �s�u����
    /// <summary>
    /// �Ĥ@�C�������]��List�}�Y�A�æV�k�d��O�_���ۦP����
    /// </summary>
    public void PriceCombo()
    {
        /* �Hstruct�Ӥ��s�դ��������@�k
        combo = new priceCombo[priceGroup.Count];
        foreach (List<Price> priceGrop in priceGroup)
        {
            int comboIndex = priceGroup.IndexOf(priceGrop);
            combo[comboIndex].type = priceGrop[0].PriceName;
            combo[comboIndex].count = 0;
        }
        */

        //��l�s�u�M��
        LinePriceGroup.Clear();
        //���j�Ĥ@�C����
        foreach (Price price in Rows[0].priceInRow)
        {
            if (!price.linked)
            {

            }
            //�C���s�WList<Price>
            LinePriceGroup.Add(new List<Price>());
            //�]�wIndex�A�Ȭ�LinePriceGroup���̤j��
            int groupNum = LinePriceGroup.Count - 1;
            //�N�Ĥ@�C��Price�]���s�ն}�Y
            LinePriceGroup[groupNum].Add(price);
            //�N��Price�аO���w�s��
            price.linked = true;
            //�ìd���Price�k�C�������O�_�ۦP
            checkRightRow(price, groupNum);
        }
    }
    /// <summary>
    /// �ˬd�k�C���������ˬd�O�_�s�u
    /// </summary>
    /// <param name="price"></param>
    /// <param name="groupNum"></param>
    void checkRightRow(Price price, int groupNum)
    {
        //�]�w��쪺����
        int indexCount = 0;
        //���j��Price�k�C������
        foreach (Price pricee in price.prices)
        {
            //�Y�����ۦP�B�S�Q���L�����A
            if (price.PriceName == pricee.PriceName && indexCount == 0)
            {
                //�N�P�ˤ�����Price�[�JLinePriceGroup[groupNum]
                LinePriceGroup[groupNum].Add(pricee);
                //�P�˼аO���w�s��
                pricee.linked = true;
                //�ӦC�Q��쪺�����ƶq+1(���P��w���@�Ӥ���)
                indexCount++;
                for (int i = 0; i < LinePriceGroup[groupNum].Count; i++)
                {
                    Debug.Log(LinePriceGroup[groupNum][i].name);
                }

                //�A���M�䦹Price�k�C�������A����k�C���s�b�άO�k�C�S�����������A
                checkRightRow(pricee, groupNum);
                break;
            }
        }
    }
    /// <summary>
    /// �Ͳ��ߥI�u
    /// </summary>
    public void InsLine()
    {
        //�s�ؤ@��List<List>
        List<List<Price>> pricess = new List<List<Price>>();
        //���j�Ҧ��bLinePriceGroup����List<Price>
        for (int i = 0; i < LinePriceGroup.Count; i++)
        {
            //�Y�̭��������j��i�s�u��(�̤j5��)
            if (LinePriceGroup[i].Count > 2)
            {
                //�[�J��List<Price>��Ȧs��List<List<Price>>��
                pricess.Add(LinePriceGroup[i]);
                //�åͲ��ߥI�u�C������
                GameObject temp = Instantiate(linePrefab);
                LineMgr line = temp.GetComponent<LineMgr>();
                //���tList<Price>��C������W
                line.priceGroup = LinePriceGroup[i];
            }
        }
        Debug.Log($"����:{PriceComboSum()}");

    }

    #endregion
    #region �H������
    /// <summary>
    /// �H������
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
    #region �ǩǪ����s
    /// <summary>
    /// ��è���s�M�w�k
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
    /// �B����y�`�M
    /// </summary>
    /// <returns>�T�s�u�H�W���Ʀr</returns>
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
     * �P�B���y�[�`�Ʀr
     */
}
