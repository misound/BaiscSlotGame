using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NagativeUntilSpinEnd : MonoBehaviour
{
    public GameObject NegativeImage;

    public void ImageActive()
    {
        NegativeImage.SetActive(true);
    }
    public void ImageNegative()
    {
        NegativeImage.SetActive(false);
    }
}
