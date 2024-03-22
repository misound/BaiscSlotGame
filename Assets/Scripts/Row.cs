using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Row : MonoBehaviour
{

    [SerializeField] public bool isRolliing;
    [SerializeField] private float rollSpeed = 5f;
    [SerializeField] private int randValue;
    [SerializeField] private int lastRoll;
    [SerializeField] private int rollAmount;
    [SerializeField] public List<Price> priceInRow;
    System.Random rand;
    void Start()
    {
        rand = new System.Random();

    }
    public void Rolling()
    {
        randValue = rand.Next(1, 5);
        rollAmount = randValue;
        for (int i = 0; i < randValue; i++)
        {
            transform.DOMoveY(-2, rollSpeed).SetSpeedBased();
            rollAmount--;
        }
    }
    IEnumerator LastRoll()
    {
        lastRoll = rand.Next(-2, 1);
        transform.DOMoveY(lastRoll, rollSpeed).SetSpeedBased();
        yield return null;
    }
}
