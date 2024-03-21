using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMgr : MonoBehaviour
{

    //動畫結束後自己關掉自己
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LineRenderer getPriceLineRenderer;
    public List<Price> priceGroup;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Start()
    {
        //接收List內數量並等同於線的點
        lineRenderer.positionCount = priceGroup.Count;
        getPriceLineRenderer.positionCount = priceGroup.Count;
        //設置原有的點
        for (int i = 0; i < priceGroup.Count; i++)
        {
            lineRenderer.SetPosition(i, priceGroup[i].transform.position);
            getPriceLineRenderer.SetPosition(i, priceGroup[i].transform.position);
        }
        //間距
        float x = Mathf.Abs(lineRenderer.GetPosition(0).x - lineRenderer.GetPosition(1).x);
        //若無五連線就新增點直到五個點都填滿
        for (int i = priceGroup.Count; i < 5; i++)
        {
            getPriceLineRenderer.positionCount++;
            getPriceLineRenderer.SetPosition(getPriceLineRenderer.positionCount - 1, 
                new Vector3(getPriceLineRenderer.GetPosition(i - 1).x + x, 
                getPriceLineRenderer.GetPosition(i - 1).y));
        }
    }

}
