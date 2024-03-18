using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    [SerializeField] private Row[] rows;
    public Row[] Rows 
    {
        get { return rows; }
    }
    [SerializeField] private Button startSpin;
    private void Awake()
    {
        startSpin.onClick.AddListener(Spin);
    }
    void Start()
    {
        for (int i = 0; i < rows.Length; i++)
        {
            if (rows[i].transform.localPosition.y == 0)
            {
                Debug.Log(rows[i].PriceName);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void Spin()
    {
        for (int i = 0; i < rows.Length; i++)
        {
            rows[i].isRolliing = true;
        }

    }
}
