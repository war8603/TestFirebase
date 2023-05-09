using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : IDetailedStoreListener
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

    bool _isInitialized = false;
    private IStoreController _controller;
    private IExtensionProvider _extension;

    public Action OnSuccessPurchased;

    public Action OnSuccessInitializ;
    public Action OnFailedInitializ;
    public Action OnFailedDisconnect;

    public Action OnSuccessPurcahse;
    public Action OnFailedPurchase;

    void Start()
    {

    }

    public void Init(string[] productIds)
    {
        var module = StandardPurchasingModule.Instance();
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);
        for (int i = 0; i < productIds.Length; i++)
        {
            builder.AddProduct(productIds[i], ProductType.NonConsumable);
        }
        UnityPurchasing.Initialize(this, builder);
        Debug.Log("[IAP] Start Successs");
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        //초기화 성공
        Debug.Log("[IAP][Success] Initialize Successs");
        _isInitialized = controller != null && extensions != null;
        _controller = controller;
        _extension = extensions;
        OnSuccessInitializ?.Invoke();
    }

    public void Purchase(string productID, Action onSuccessPurchased)
    {
        //상품 구매.
        if (!_isInitialized)
        {
            Debug.Log("[IAP][Failed] Purchase Failed. => OnPurchase // The store has not been initialized.");
            OnFailedInitializ?.Invoke();
            return;
        }

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("[IAP][Failed] Purchase Failed. => OnPurchase // You are not connected to the Internet.");
            OnFailedDisconnect?.Invoke();
            return;
        }

        try
        {
            Product product = _controller.products.WithID(productID);
            if (product != null && product.availableToPurchase)
            {
                OnSuccessPurchased = onSuccessPurchased;
                _controller.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("[IAP][Failed] Purchase Failed. => OnPurchase // Product not found or cannot be purchased");
                OnFailedPurchase?.Invoke();
            }
        }
        catch (Exception e)
        {
            Debug.Log(string.Format("[IAP][Failed] Purchase Failed. => OnPurchase // Critical Error : {0}", e));
            OnFailedPurchase?.Invoke();
        }
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        //결제 성공 콜백. 앱이 내려갔다 올라올 때도 호출된다.
        //결제된 상품 확인.
        Debug.Log("[IAP] ProcessPurchase");
        Product resultProduct = purchaseEvent.purchasedProduct;

        if (_extension.GetExtension<IGooglePlayStoreExtensions>().IsPurchasedProductDeferred(resultProduct) == false)
        {
            OnValidateReceipt(resultProduct);
        }
        return PurchaseProcessingResult.Pending;
    }

    private void OnValidateReceipt(Product product)
    {
        //영수증 체크
        OnComfirmPendingPurchase(product);
    }

    private void OnComfirmPendingPurchase(Product product)
    {
        Debug.Log($"[IAP] OnConfirm : {product.definition.id}");
        _controller.ConfirmPendingPurchase(product);

        OnSuccessPurcahse?.Invoke();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log($"[IAP][Failed] Initialized error = {error.ToString()}");
        OnFailedInitializ?.Invoke();
    }

    public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log($"[IAP][Failed] product = {product.definition.id}, message ={failureDescription.message}");
        OnFailedInitializ?.Invoke();
    }

    public void OnPurchaseFailed(UnityEngine.Purchasing.Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"[IAP][Failed] product = {product.definition.id}, message ={failureReason.ToString()}");
        OnFailedInitializ?.Invoke();
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log($"[IAP][Failed] Initialized error = {error.ToString()} // message = {message}");
        OnFailedInitializ?.Invoke();
    }
}
