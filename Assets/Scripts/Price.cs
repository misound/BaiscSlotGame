using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Price : MonoBehaviour
{
    [SerializeField] public PriceSObj priceSObj;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private int priceScore;
    public bool group = false;
    public bool linked = false;
    public int PriceScore
    {
        get { return priceScore; }
    }
    [SerializeField] PriceMgr priceMgr;
    public List<Price> prices;
    public enum priceName
    {
        Camp,
        ClimbMan,
        ClimbShoes,
        Hook,
        Pickaxe,
        RockClimber,
        SafeCap,
        Willump
    }
    public priceName PriceName;
    //[SerializeField] private string myName;
    private void Awake()
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
        //PriceName = (priceName)priceScore - 1;
    }
}
