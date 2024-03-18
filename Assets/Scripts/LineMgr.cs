using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMgr : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private SlotController slotController;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //slotController = GetComponent<SlotController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < slotController.Rows.Length; i++)
        {
            lineRenderer.SetPosition(i, slotController.Rows[i].transform.position);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
