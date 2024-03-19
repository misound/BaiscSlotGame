using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Price : MonoBehaviour
{
    [SerializeField] public PriceSObj priceSObj;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int priceScore;
    public int PriceScore
    {
        get { return priceScore; }
    }
    [SerializeField] PriceMgr priceMgr;
    public List<Price> prices;
    //[SerializeField] private string myName;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        UpdateSObj();
    }
    void UpdateSObj()
    {
        //myName = priceSObj.priceName;
        spriteRenderer.sprite = priceSObj.priceSprite;
        priceScore = priceSObj.priceScore;
    }
}
