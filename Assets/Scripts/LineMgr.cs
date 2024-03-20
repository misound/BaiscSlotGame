using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMgr : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    public List<Price> priceGroup;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Start()
    {
        //float priceX = Mathf.Abs(priceGroup[0].transform.position.x - priceGroup[1].transform.position.x);
        lineRenderer.positionCount = priceGroup.Count;

        for (int i = 0; i < priceGroup.Count; i++)
        {
            lineRenderer.SetPosition(i, priceGroup[i].transform.position);
        }
        float x = Mathf.Abs(lineRenderer.GetPosition(0).x - lineRenderer.GetPosition(1).x);
        for (int i = priceGroup.Count; i < 5; i++)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, new Vector3(lineRenderer.GetPosition(i - 1).x + x, lineRenderer.GetPosition(i - 1).y));

        }


        /*
         * to do: 
         * �Y����5���I�A�h�ɨ�5���I
         * �ӷs�W���I����m = �쥻�I�P�I���Z���������ܶq + �̫��I����m
         * �B����N�̫�@���I�s�W���Ʀܸɺ�
        */

    }

}
