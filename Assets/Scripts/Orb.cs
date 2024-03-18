using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Orb : MonoBehaviour
{
    [Tooltip("宣告珠子屬性")]
    public enum OrbsType { Fire, Ice, Shine, Null };

    [Header("珠子類型、行列及長寬")]
    [Tooltip("類型")]
    public OrbsType type;
    private Image image;
    [Tooltip("列")]
    public int row;
    [Tooltip("行")]
    public int column;
    public float width = 120;
    public float height = 120;

    [Header("珠子群組")]
    [Tooltip("相鄰的珠子")]
    public List<Orb> linkOrbs = new List<Orb>();
    [Tooltip("是否已經被分配群組")]
    public bool group = false;
    public Text groupNum;
    [Header("消除")]
    public bool removed = false;
    public Text removeText;
    /// <summary>
    /// 換圖片
    /// </summary>
    void ChangeImage()
    {
        //載入圖檔
        image.sprite = Resources.Load<Sprite>("Image/" + type);
    }
    void Start()
    {
        image = GetComponent<Image>();
        //初始化
        ChangeImage();
        transform.name = "orb" + row + column;
        //初始化珠子大小
        GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }
}
