using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSystem : MonoBehaviour
{
    [Header("��P�C�A�첾���Z��")]
    [Tooltip("�I���A�ͦ�����m")]
    public RectTransform BGRect;
    [Tooltip("�ݦC��")]
    [SerializeField] private int rowCount = 5;
    [Tooltip("����")]
    [SerializeField] private int columnCount = 6;
    private Vector2 addPos = new Vector2(120, 120);
    [Tooltip("�x�s�Ҧ��]�l�}��")]
    [SerializeField] private List<Orb> orbs = new List<Orb>();
    [Tooltip("�x�s�Ҧ��]�l�s��")]
    [SerializeField] private List<List<Orb>> orbGroups = new List<List<Orb>>();
    [Tooltip("�������]�l�s�ռƶq")]
    [SerializeField] private int removeCount = 3;
    public bool hasRemove = false;
    [System.Serializable]
    public struct Combo //�]�l�����ή����]�l���`�ƶq
    {
        public Orb.OrbsType type;
        public int count;
    };
    //�ŧiCombo�}�CorbCombo
    public Combo[] orbCombo;
    void Start()
    {
        InitGrid();
        //�s�W���L�ܼ�hasRemove�A�ΨӧP�_�O�_�n���ư����s�աBcombo�����ʧ@�C
        do
        {
            hasRemove = false;
            OrbCreate();
            OrbGroup();
            OrbCombo();
            OrbRemove();
        } while (hasRemove);
    }
    #region �Ͳ��]�l����
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
                orb.type = Orb.OrbsType.Null;
                //OrbCreate();
                orb.row = r;
                orb.column = c;
                orb.height = BGRect.sizeDelta.y / rowCount;
                orb.width = BGRect.sizeDelta.x / columnCount;
                //��e�]�l�[�JList
                orbs.Add(orb);
            }
        }
        //�s���W�U���k���]�l
        for (int index = 0; index < orbs.Count; index++)
        {
            //if���J���|��Ψ������]�l�s��

            //�p�G��e�]�l�Ҧb���� != �����̤j�� - 1(���O�̥k�䪺���p)
            if (orbs[index].column != columnCount - 1)
                //�h�[�J�L�k��(��)���F��]�l
                orbs[index].linkOrbs.Add(orbs[index + 1]);

            //�p�G��e�]�l�Ҧb���C != �����̤j�C - 1(���O�̤W�������p)
            if (orbs[index].row != rowCount - 1)
                //�h�[�J�L�W��(�C)���F��]�l
                orbs[index].linkOrbs.Add(orbs[index + columnCount]);

            //�p�G��e�]�l�Ҧb���� != �����̤p��(���O�̥��䪺���p)
            if (orbs[index].column != 0)
                //�h�[�J�L����(��)���F��]�l
                orbs[index].linkOrbs.Add(orbs[index - 1]);

            //�p�G��e�]�l�Ҧb���C != �����̤p�C(���O�̤U�������p)
            if (orbs[index].row != 0)
                //�h�[�J�L�U��(�C)���F��]�l
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
            //��l��
            orb.group = false;
            //��l��
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
    /// �M����񪺯]�l
    /// </summary>
    /// <param name="orb">��e�]�l</param>
    /// <param name="groupNum">��e�s�սs��</param>
    void FindMembers(Orb orb, int groupNum)
    {
        //�j�M�۳s�]�l
        foreach (Orb linkOrb in orb.linkOrbs)
        {
            //��e�]�l�P�۾F�]�l�����ۦP�ɡA�B���Q�[�J��s�ծ�
            if (linkOrb.type == orb.type && !linkOrb.group)
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
    /// �bInitGrid�������A�إ߯]�l�s��
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
                FindMembers(orb, groupNum);
            }
        }
    }
    /// <summary>
    /// �ŧiFindRemoveOrb�禡
    /// </summary>
    /// <param name="orb">�]�l</param>
    /// <param name="dir">�j�M��V(��P�C)</param>
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
            //�Y�W�誺���󤣦��ߡA��ܤw�g�䧹�۳s���]�l�A�N�i�H�����j��C
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
                if (!orbs[index].removed)
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
    void OrbRemove()
    {
        //�w��C�@�ӯ]�l���]�ݩʡC
        foreach (Orb orb in orbs)
        {
            if (orb.removed == true)
            {
                //����]�l���ݭn�Q���������A�A��]�l���ݩʭקאּNull�C
                orb.type = Orb.OrbsType.Null;
                //�P�ɱNhasRemove�]��true�A�o��Start����while�j��N�|�A������C
                hasRemove = true;
                //���]�]�l�s�ժ�text�Atext����P�{���X�b��Ӭy�{�T�w�S���D��N�i�H�R���C
                orb.removeText.text = "";
            }
            //�P�W
            orb.groupNum.text = "";
        }
        //�w��C�@�ӯ]�l�������P�w
        foreach (Orb orb in orbs)
        {
            //�����ݩʬ�Null�ɡC
            if (orb.type == Orb.OrbsType.Null)
            {
                //���]�l���W��j�M�C
                for (int index = orbs.IndexOf(orb); index <= columnCount * (rowCount - 1) + orb.column; index += columnCount)////(8)
                {
                    //����DNull�ݩʪ��]�l�ɡC
                    if (orbs[index].type != Orb.OrbsType.Null)
                    {
                        //���e�]�l�ݩʳ]�w���ӯ]�l���ݩʡC
                        orb.type = orbs[index].type;
                        //�N�ӯ]�l�]�w��Null�ݩʡC
                        orbs[index].type = Orb.OrbsType.Null;
                        //�]�w���ݩʴN�N��ӯ]�l���������A�������W�j�M���j��C
                        break;
                    }
                }
            }
        }
    }
}
