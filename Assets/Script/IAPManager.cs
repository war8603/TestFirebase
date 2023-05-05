using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager
{
    private IAPManager _instance;
    public IAPManager Instance
    {
        get 
        {
            if (_instance == null)
                _instance = new IAPManager();
            return _instance; 
        } 
    }

    private IStoreController _controller;
    private IExtensionProvider _extension;

    void Start()
    {
        
    }

    
}
