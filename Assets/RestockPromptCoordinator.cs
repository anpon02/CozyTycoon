using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestockPromptCoordinator : MonoBehaviour
{
    [SerializeField] SpriteRenderer sourceImg;
    [SerializeField] Color disabledCol;
    [SerializeField] ItemStorage storage;

    private void Start()
    {
        GameManager.instance.OnStoreOpen.AddListener(Hide);
    }

    private void OnEnable()
    {
        sourceImg.color = disabledCol;
    }

    public void Restock()
    {
        AudioManager.instance.PlaySound(8, gameObject);
        sourceImg.color = Color.white;
        storage.Stock();
        Hide();
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
 }
