using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    IAPManager _iapManager;
    string[] _productItemNames = new string[] { "test_product_01", "test_product_02" };

    public void Start()
    {
        _iapManager = new IAPManager();
        _iapManager.Init(_productItemNames);
    }

    public void OnClickPurchase(int itemIndex)
    {
        _iapManager.Purchase(_productItemNames[itemIndex], OnSuccessPurchase);
    }

    public void OnSuccessPurchase()
    {
        Debug.Log($"OnSuccess Purcahse item");

    }
}
