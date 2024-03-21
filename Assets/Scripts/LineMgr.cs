using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMgr : MonoBehaviour
{

    //�ʵe������ۤv�����ۤv
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LineRenderer getPriceLineRenderer;
    public List<Price> priceGroup;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Start()
    {
        //����List���ƶq�õ��P��u���I
        lineRenderer.positionCount = priceGroup.Count;
        getPriceLineRenderer.positionCount = priceGroup.Count;
        //�]�m�즳���I
        for (int i = 0; i < priceGroup.Count; i++)
        {
            lineRenderer.SetPosition(i, priceGroup[i].transform.position);
            getPriceLineRenderer.SetPosition(i, priceGroup[i].transform.position);
        }
        //���Z
        float x = Mathf.Abs(lineRenderer.GetPosition(0).x - lineRenderer.GetPosition(1).x);
        //�Y�L���s�u�N�s�W�I���줭���I����
        for (int i = priceGroup.Count; i < 5; i++)
        {
            getPriceLineRenderer.positionCount++;
            getPriceLineRenderer.SetPosition(getPriceLineRenderer.positionCount - 1, 
                new Vector3(getPriceLineRenderer.GetPosition(i - 1).x + x, 
                getPriceLineRenderer.GetPosition(i - 1).y));
        }
    }

}
