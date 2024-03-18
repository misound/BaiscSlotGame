using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSystem : MonoBehaviour
{
    [Header("��P�C�A�첾���Z��")]
    [Tooltip("�I���A�ͦ�����m")]
    public RectTransform BGRect;
    public int rowCount = 5;
    public int columnCount = 6;
    private Vector2 addPos = new Vector2(120, 120);
    [Tooltip("�x�s�Ҧ��]�l�}��")]
    public List<Orb> orbs = new List<Orb>();
    [Tooltip("�x�s�Ҧ��]�l�s��")]
    public List<List<Orb>> orbGroups = new List<List<Orb>>();
    [Tooltip("�������]�l�s�ռƶq")]
    public int removeCount = 3;
    [System.Serializable]
    public struct Combo //�]�l�����ή����]�l���`�ƶq
    {
        public Orb.OrbsType type;
        public int count;
    };
    public Combo[] orbCombo;////(3)
    void Start()
    {
        InitGrid();
        //OrbCreate();
        OrbGroup();
        OrbCombo();
    }
    /// <summary>
    /// �s�W��l�ƽL�����禡InitGrid�A�禡���ϥΨ�h���j��A�b�L�����C�Ӧ�m�s�W�]�l�C
    /// </summary>
    void InitGrid()
    {
        addPos.y = BGRect.sizeDelta.y / rowCount;
        addPos.x = BGRect.sizeDelta.x / columnCount;

        for (int r = 0; r < rowCount; r++)
        {
            for (int c = 0; c < columnCount; c++)
            {
                //���ͯ]�l����
                GameObject orbObj = Instantiate(Resources.Load("Prefabs/Orb")) as GameObject;
                RectTransform orbRect = orbObj.GetComponent<RectTransform>();
                //���w������
                orbRect.SetParent(BGRect);
                //�]�w�j�p
                orbRect.localScale = Vector2.one;
                //�]�w��m
                orbRect.anchoredPosition = new Vector2(c * addPos.x, r * addPos.y);
                //����]�l�}���A�ק��P�C
                Orb orb = orbObj.GetComponent<Orb>();
                orb.row = r;
                orb.column = c;
                OrbCreate();
                orb.height = BGRect.sizeDelta.y / rowCount;
                orb.width = BGRect.sizeDelta.x / columnCount;
                //��e�]�l�[�JList
                orbs.Add(orb);
            }
        }
        //�s���W�U���k���]�l
        for (int index = 0; index < orbs.Count; index++)////(3)
        {
            //if���J���|��Ψ������]�l�s��
            if (orbs[index].column != columnCount - 1)
                orbs[index].linkOrbs.Add(orbs[index + 1]);
            if (orbs[index].row != rowCount - 1)
                orbs[index].linkOrbs.Add(orbs[index + columnCount]);
            if (orbs[index].column != 0)
                orbs[index].linkOrbs.Add(orbs[index - 1]);
            if (orbs[index].row != 0)
                orbs[index].linkOrbs.Add(orbs[index - columnCount]);
        }
    }
    /// <summary>
    /// �����H���]�l�s���A�ñN�s���a�J�]�l������
    /// </summary>
    void OrbCreate()
    {
        foreach (Orb orb in orbs)
        {
            if (orb.type == Orb.OrbsType.Null)
            {
                int typeNum = Random.Range(0, (int)Orb.OrbsType.Null);
                orb.type = (Orb.OrbsType)typeNum;
            }

        }
    }
    void FindMembers(Orb orb, int groupNum)
    {
        //�j�M�۳s�]�l
        foreach (Orb linkOrb in orb.linkOrbs)
        {
            //��e�]�l�P�۾F�]�l�����ۦP�ɡA�B���Q�[�J��s�ծ�
            if (linkOrb.type == orb.type && linkOrb.group == false)
            {
                //�[�J��e�s�աA�B����Group��
                orbGroups[groupNum].Add(linkOrb);
                linkOrb.group = true;
                linkOrb.groupNum.text = groupNum.ToString();
                //�A���I�sFindMembers�A�o���n�䪺�O�o�Ӭ۾F�]�l��linkOrbs�A����Ҧ��۳s�P�ݩʪ��]�l���[�J�s�լ���C
                FindMembers(linkOrb, groupNum);
            }
        }
    }
    /// <summary>
    /// �bInitGrid�������
    /// </summary>
    void OrbGroup()
    {
        //��l�Ƹs��
        orbGroups.Clear();
        //�[�J�S���s�ժ��]�l
        foreach (Orb orb in orbs)
        {
            //�p�G���S�s�ժ��]�l
            if (!orb.group)
            {
                //�إ߷s���]�l�s�աA�÷s�W��orbGroups��
                orbGroups.Add(new List<Orb>());
                //�ŧi�s�սs���A�ñN�s���s�W��List���s��
                int groupNum = orbGroups.Count - 1;
                //�N�ثe�o���S���s�ժ��]�l�[�J�o�ӷs�W��List�C
                orbGroups[groupNum].Add(orb);
                //�N��e���]�lgroup�]��true�A��ܥ��w�g�Q�[��Y�Ӹs�դ��C
                orb.group = true;
                //�]�wText������ܤ�r�A��r���e���s�սs���C
                orb.groupNum.text = (groupNum).ToString();
                //�I�sFindMembers�禡�A�a�J�ѼƬ���e���]�l�M�s���C
                FindMembers(orb, groupNum);////(10)
            }
        }
    }
    /// <summary>
    /// �ŧiFindRemoveOrb�禡
    /// </summary>
    /// <param name="orb">�]�l</param>
    /// <param name="dir">�j�M��V</param>
    /// <param name="length">�j�M����</param>
    /// <param name="comboIndex">combo�s��</param>
    void FindRemoveOrb(Orb orb, int dir, int length, int comboIndex)
    {
        //�ŧi�Ψӭp��۳s�]�l�ƶq�����orbCount�A��l�Ȭ�0�C
        int orbCount = 0;
        //�ŧi�]�l�s��orbIndex�C
        int orbIndex = orbs.IndexOf(orb);
        //����j��A�q�ǤJ���]�l�s���}�l�B��ǤJ���j�M���׵����B�W�[�ƭȬ��j�M��V�C
        for (int index = orbIndex; index <= length; index += dir)
        {
            //��ǤJ���]�l�ݩʻP��U���]�l�ݩʬۦP�ɡBorbCount�[1�C
            if (orbs[index].type == orb.type)
                orbCount++;
            //�Y(4)�����󤣦��ߡA��ܤw�g�䧹�۳s���]�l�A�N�i�H�����j��C
            else
                break;
        }
        //)��orbCount�j��������eremoveCount�ɡC
        if (orbCount >= removeCount)
        {
            //����for�j��A�_�I�P��V�ۦP�A���o���j�M�u��orbCount�ƶq����C
            for (int index = orbIndex; index < orbIndex + (orbCount * dir); index += dir)
            {
                //)��e�]�l��removed��false�ɡA�N����combo�U��count�[1�A�H�p���combo�`�ơC
                if (orbs[index].removed == false)
                    orbCombo[comboIndex].count += 1;
                //�N��e�]�l��removed�]��true�A�o�ɸ����]�l�N�|�Q�����ݭn�������]�l�C
                orbs[index].removed = true;
                //�ק�Text�A�|��ܩ���浲�G���C
                orbs[index].removeText.text = "C";
            }
        }
    }
    /// <summary>
    /// �]�l�զX
    /// </summary>
    void OrbCombo()
    {
        //��l�Ƹs�ռƶqorbCombo�}�C�A�j�p�P�`�s�ռƬۦP
        orbCombo = new Combo[orbGroups.Count];
        //�w��]�l�`�s�հ���foreach�j��C
        foreach (List<Orb> orbGroup in orbGroups)
        {
            //�ŧi����ܼ�comboIndex�A�ƭȬ���e�s�սs���C
            int comboIndex = orbGroups.IndexOf(orbGroup);
            //�N��ecombo���]�l�ݩʳ]�w����e�s���ݩʡC
            orbCombo[comboIndex].type = orbGroup[0].type;
            //combo���]�l�ƶq��0�C
            orbCombo[comboIndex].count = 0;
            //���۰w���e�s�հ���foreach�j��C
            foreach (Orb orb in orbGroup)
            {
                //���k�j�M�A�I�sFindRemoveOrb�禡�ѼƬ��]�lorb�B�j�M��V���k��1�B�j�M���׬��ӦC�̥k�䪺�]�l�Bcombo�s����comboIndex�C
                FindRemoveOrb(orb, 1, columnCount * (orb.row + 1) - 1, comboIndex);
                //���W�j�M�A�I�sFindRemoveOrb�禡�ѼƬ��]�lorb�B�j�M��V���W��columnCount�B�j�M���׬��Ӧ�̤W�����]�l�Bcombo�s����comboIndex�C
                FindRemoveOrb(orb, columnCount, columnCount * (rowCount - 1) + orb.column, comboIndex);
            }
        }
    }
}
