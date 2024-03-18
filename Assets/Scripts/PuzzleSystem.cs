using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSystem : MonoBehaviour
{
    [Header("行與列，位移的距離")]
    [Tooltip("背景，生成的位置")]
    public RectTransform BGRect;
    [Tooltip("豎列數")]
    [SerializeField] private int rowCount = 5;
    [Tooltip("橫行數")]
    [SerializeField] private int columnCount = 6;
    private Vector2 addPos = new Vector2(120, 120);
    [Tooltip("儲存所有珠子腳本")]
    [SerializeField] private List<Orb> orbs = new List<Orb>();
    [Tooltip("儲存所有珠子群組")]
    [SerializeField] private List<List<Orb>> orbGroups = new List<List<Orb>>();
    [Tooltip("消除的珠子群組數量")]
    [SerializeField] private int removeCount = 3;
    public bool hasRemove = false;
    [System.Serializable]
    public struct Combo //珠子類型及消除珠子的總數量
    {
        public Orb.OrbsType type;
        public int count;
    };
    //宣告Combo陣列orbCombo
    public Combo[] orbCombo;
    void Start()
    {
        InitGrid();
        //新增布林變數hasRemove，用來判斷是否要重複執行找群組、combo等等動作。
        do
        {
            hasRemove = false;
            OrbCreate();
            OrbGroup();
            OrbCombo();
            OrbRemove();
        } while (hasRemove);
    }
    #region 生產珠子相關
    /// <summary>
    /// 新增初始化盤面的函式InitGrid，函式當中使用兩層的迴圈，在盤面中每個位置新增珠子。
    /// </summary>
    void InitGrid()
    {
        addPos.y = BGRect.sizeDelta.y / rowCount;
        addPos.x = BGRect.sizeDelta.x / columnCount;

        for (int r = 0; r < rowCount; r++)
        {
            for (int c = 0; c < columnCount; c++)
            {
                //產生珠子物件
                GameObject orbObj = Instantiate(Resources.Load("Prefabs/Orb")) as GameObject;
                RectTransform orbRect = orbObj.GetComponent<RectTransform>();
                //指定父物件
                orbRect.SetParent(BGRect);
                //設定大小
                orbRect.localScale = Vector2.one;
                //設定位置
                orbRect.anchoredPosition = new Vector2(c * addPos.x, r * addPos.y);
                //抓取珠子腳本，修改行與列
                Orb orb = orbObj.GetComponent<Orb>();
                orb.type = Orb.OrbsType.Null;
                //OrbCreate();
                orb.row = r;
                orb.column = c;
                orb.height = BGRect.sizeDelta.y / rowCount;
                orb.width = BGRect.sizeDelta.x / columnCount;
                //當前珠子加入List
                orbs.Add(orb);
            }
        }
        //連結上下左右的珠子
        for (int index = 0; index < orbs.Count; index++)
        {
            //if式撇除四邊及角落的珠子連結

            //如果當前珠子所在的行 != 版面最大行 - 1(不是最右邊的情況)
            if (orbs[index].column != columnCount - 1)
                //則加入他右邊(行)的鄰近珠子
                orbs[index].linkOrbs.Add(orbs[index + 1]);

            //如果當前珠子所在的列 != 版面最大列 - 1(不是最上面的情況)
            if (orbs[index].row != rowCount - 1)
                //則加入他上面(列)的鄰近珠子
                orbs[index].linkOrbs.Add(orbs[index + columnCount]);

            //如果當前珠子所在的行 != 版面最小行(不是最左邊的情況)
            if (orbs[index].column != 0)
                //則加入他左邊(行)的鄰近珠子
                orbs[index].linkOrbs.Add(orbs[index - 1]);

            //如果當前珠子所在的列 != 版面最小列(不是最下面的情況)
            if (orbs[index].row != 0)
                //則加入他下面(列)的鄰近珠子
                orbs[index].linkOrbs.Add(orbs[index - columnCount]);
        }
    }
    /// <summary>
    /// 給予隨機珠子編號，並將編號帶入珠子類型中
    /// </summary>
    void OrbCreate()
    {
        foreach (Orb orb in orbs)
        {
            //初始化
            orb.group = false;
            //初始化
            orb.removed = false;
            if (orb.type == Orb.OrbsType.Null)
            {
                int typeNum = Random.Range(0, (int)Orb.OrbsType.Null);
                orb.type = (Orb.OrbsType)typeNum;
            }
        }
    }
    #endregion
    /// <summary>
    /// 尋找附近的珠子
    /// </summary>
    /// <param name="orb">當前珠子</param>
    /// <param name="groupNum">當前群組編號</param>
    void FindMembers(Orb orb, int groupNum)
    {
        //搜尋相連珠子
        foreach (Orb linkOrb in orb.linkOrbs)
        {
            //當前珠子與相鄰珠子類型相同時，且未被加入到群組時
            if (linkOrb.type == orb.type && !linkOrb.group)
            {
                //加入當前群組，且給予Group值
                orbGroups[groupNum].Add(linkOrb);
                linkOrb.group = true;
                linkOrb.groupNum.text = groupNum.ToString();
                //再次呼叫FindMembers，這次要找的是這個相鄰珠子的linkOrbs，直到所有相連同屬性的珠子都加入群組為止。
                FindMembers(linkOrb, groupNum);
            }
        }
    }
    /// <summary>
    /// 在InitGrid之後執行，建立珠子群組
    /// </summary>
    void OrbGroup()
    {
        //初始化群組
        orbGroups.Clear();
        //加入沒有群組的珠子
        foreach (Orb orb in orbs)
        {
            //如果找到沒群組的珠子
            if (!orb.group)
            {
                //建立新的珠子群組，並新增到orbGroups中
                orbGroups.Add(new List<Orb>());
                //宣告群組編號，並將編號新增至List的編號
                int groupNum = orbGroups.Count - 1;
                //將目前這顆沒有群組的珠子加入這個新增的List。
                orbGroups[groupNum].Add(orb);
                //將當前的珠子group設為true，表示它已經被加到某個群組中。
                orb.group = true;
                //設定Text元件的顯示文字，文字內容為群組編號。
                orb.groupNum.text = (groupNum).ToString();
                //呼叫FindMembers函式，帶入參數為當前的珠子和編號。
                FindMembers(orb, groupNum);
            }
        }
    }
    /// <summary>
    /// 宣告FindRemoveOrb函式
    /// </summary>
    /// <param name="orb">珠子</param>
    /// <param name="dir">搜尋方向(行與列)</param>
    /// <param name="length">搜尋長度</param>
    /// <param name="comboIndex">combo編號</param>
    void FindRemoveOrb(Orb orb, int dir, int length, int comboIndex)
    {
        //宣告用來計算相連珠子數量的整數orbCount，初始值為0。
        int orbCount = 0;
        //宣告珠子編號orbIndex。
        int orbIndex = orbs.IndexOf(orb);
        //執行迴圈，從傳入的珠子編號開始、到傳入的搜尋長度結束、增加數值為搜尋方向。
        for (int index = orbIndex; index <= length; index += dir)
        {
            //當傳入的珠子屬性與當下的珠子屬性相同時、orbCount加1。
            if (orbs[index].type == orb.type)
                orbCount++;
            //若上方的條件不成立，表示已經找完相連的珠子，就可以結束迴圈。
            else
                break;
        }
        //)當orbCount大於消除門檻removeCount時。
        if (orbCount >= removeCount)
        {
            //執行for迴圈，起點與方向相同，但這次搜尋只到orbCount數量為止。
            for (int index = orbIndex; index < orbIndex + (orbCount * dir); index += dir)
            {
                //)當前珠子的removed為false時，將對應combo下的count加1，以計算該combo總數。
                if (!orbs[index].removed)
                    orbCombo[comboIndex].count += 1;
                //將當前珠子的removed設為true，這時該顆珠子就會被視為需要消除的珠子。
                orbs[index].removed = true;
                //修改Text，會顯示於執行結果中。
                orbs[index].removeText.text = "C";
            }
        }
    }
    /// <summary>
    /// 珠子組合
    /// </summary>
    void OrbCombo()
    {
        //初始化群組數量orbCombo陣列，大小與總群組數相同
        orbCombo = new Combo[orbGroups.Count];
        //針對珠子總群組執行foreach迴圈。
        foreach (List<Orb> orbGroup in orbGroups)
        {
            //宣告整數變數comboIndex，數值為當前群組編號。
            int comboIndex = orbGroups.IndexOf(orbGroup);
            //將當前combo的珠子屬性設定為當前群組屬性。
            orbCombo[comboIndex].type = orbGroup[0].type;
            //combo的珠子數量為0。
            orbCombo[comboIndex].count = 0;
            //接著針對當前群組執行foreach迴圈。
            foreach (Orb orb in orbGroup)
            {
                //往右搜尋，呼叫FindRemoveOrb函式參數為珠子orb、搜尋方向為右方1、搜尋長度為該列最右邊的珠子、combo編號為comboIndex。
                FindRemoveOrb(orb, 1, columnCount * (orb.row + 1) - 1, comboIndex);
                //往上搜尋，呼叫FindRemoveOrb函式參數為珠子orb、搜尋方向為上方columnCount、搜尋長度為該行最上面的珠子、combo編號為comboIndex。
                FindRemoveOrb(orb, columnCount, columnCount * (rowCount - 1) + orb.column, comboIndex);
            }
        }
    }
    void OrbRemove()
    {
        //針對每一個珠子重設屬性。
        foreach (Orb orb in orbs)
        {
            if (orb.removed == true)
            {
                //當找到珠子為需要被移除的狀態，把珠子的屬性修改為Null。
                orb.type = Orb.OrbsType.Null;
                //同時將hasRemove設為true，這時Start中的while迴圈就會再次執行。
                hasRemove = true;
                //重設珠子群組的text，text元件與程式碼在整個流程確定沒問題後就可以刪除。
                orb.removeText.text = "";
            }
            //同上
            orb.groupNum.text = "";
        }
        //針對每一個珠子做掉落判定
        foreach (Orb orb in orbs)
        {
            //當找到屬性為Null時。
            if (orb.type == Orb.OrbsType.Null)
            {
                //往珠子的上方搜尋。
                for (int index = orbs.IndexOf(orb); index <= columnCount * (rowCount - 1) + orb.column; index += columnCount)////(8)
                {
                    //當找到非Null屬性的珠子時。
                    if (orbs[index].type != Orb.OrbsType.Null)
                    {
                        //把當前珠子屬性設定為該珠子的屬性。
                        orb.type = orbs[index].type;
                        //將該珠子設定為Null屬性。
                        orbs[index].type = Orb.OrbsType.Null;
                        //設定完屬性就代表該珠子掉落完成，結束往上搜尋的迴圈。
                        break;
                    }
                }
            }
        }
    }
}
