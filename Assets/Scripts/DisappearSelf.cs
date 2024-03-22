using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearSelf : MonoBehaviour
{
    [SerializeField] float DisappearTimer = 2f;
    float timer;
    private void OnEnable()
    {
        timer = 0;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > DisappearTimer) 
        {
            gameObject.SetActive(false);
        }
    }
}
