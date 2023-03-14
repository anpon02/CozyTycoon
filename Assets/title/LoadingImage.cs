using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingImage : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] List<Sprite> imgs;
    

    public void NewImage()
    {
        icon.sprite = imgs[Random.Range(0, imgs.Count)];
    }
}
