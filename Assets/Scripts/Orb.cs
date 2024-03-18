using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Orb : MonoBehaviour
{
    [Tooltip("�ŧi�]�l�ݩ�")]
    public enum OrbsType { Fire, Ice, Shine, Null };

    [Header("�]�l�����B��C�Ϊ��e")]
    [Tooltip("����")]
    public OrbsType type;
    private Image image;
    [Tooltip("�C")]
    public int row;
    [Tooltip("��")]
    public int column;
    public float width = 120;
    public float height = 120;

    [Header("�]�l�s��")]
    [Tooltip("�۾F���]�l")]
    public List<Orb> linkOrbs = new List<Orb>();
    [Tooltip("�O�_�w�g�Q���t�s��")]
    public bool group = false;
    public Text groupNum;
    [Header("����")]
    public bool removed = false;
    public Text removeText;
    /// <summary>
    /// ���Ϥ�
    /// </summary>
    void ChangeImage()
    {
        //���J����
        image.sprite = Resources.Load<Sprite>("Image/" + type);
    }
    void Start()
    {
        image = GetComponent<Image>();
        //��l��
        ChangeImage();
        transform.name = "orb" + row + column;
        //��l�Ư]�l�j�p
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }
}
