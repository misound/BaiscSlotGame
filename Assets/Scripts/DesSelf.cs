using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesSelf : MonoBehaviour
{
    private void Start()
    {
        Destroy();
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
