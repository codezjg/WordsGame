using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Purchasing;
using com.moralabs.cube.util;
using UnityEngine.SceneManagement;


// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class Purchaser : MonoBehaviour, IStoreListener
{

    public static Purchaser Instance { get; set; }
    public Text tip50, tip150, tip300;

    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    public static string PRODUCT_50TIP = "tip50";
    public static string PRODUCT_150TIP = "tip150";
    public static string PRODUCT_300TIP = "tip300";

    void Awake(){
        Instance = this;
    }

    void Start()
    {
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(PRODUCT_50TIP, ProductType.Consumable);
        builder.AddProduct(PRODUCT_150TIP, ProductType.Consumable);
        builder.AddProduct(PRODUCT_300TIP, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void Buy50Tip(){
        BuyProductID(PRODUCT_50TIP);
    }

    public void Buy150Tip(){
        BuyProductID(PRODUCT_150TIP);
    }

    public void Buy300Tip(){
        BuyProductID(PRODUCT_300TIP);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }


    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;

        //IAP1Text = GameObject.Find("IAP1Text").GetComponent<Text>();
        tip50.text = m_StoreController.products.WithID(PRODUCT_50TIP).metadata.localizedPriceString;
        tip150.text = m_StoreController.products.WithID(PRODUCT_150TIP).metadata.localizedPriceString;
        tip300.text = m_StoreController.products.WithID(PRODUCT_300TIP).metadata.localizedPriceString;

    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // A consumable product has been purchased by this user.
        if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_50TIP, StringComparison.Ordinal))
        {
            int tempNum = CPlayerPrefs.GetInt("LastHintCount");
            int newNum = tempNum + 10;
            CPlayerPrefs.SetInt("LastHintCount", newNum);
            if(SceneManager.GetActiveScene().ToString() == "GameScene"){

                LevelGenerate levelGenerate = FindObjectOfType<LevelGenerate>();
                levelGenerate.UpdatePaidText();
            }
            Debug.Log("you have just bought 10 tip");
        }
        // Or ... a non-consumable product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_150TIP, StringComparison.Ordinal))
        {
            int tempNum = CPlayerPrefs.GetInt("LastHintCount");
            int newNum = tempNum + 30;
            CPlayerPrefs.SetInt("LastHintCount", newNum);
            if (SceneManager.GetActiveScene().ToString() == "GameScene"){
                LevelGenerate levelGenerate = FindObjectOfType<LevelGenerate>();
                levelGenerate.UpdatePaidText();
            }
            Debug.Log("you have just bought 30 tip");
        }
        // Or ... a subscription product has been purchased by this user.
        else if (String.Equals(args.purchasedProduct.definition.id, PRODUCT_300TIP, StringComparison.Ordinal))
        {
            int tempNum = CPlayerPrefs.GetInt("LastHintCount");
            int newNum = tempNum + 60;
            CPlayerPrefs.SetInt("LastHintCount", newNum);
            if (SceneManager.GetActiveScene().ToString() == "GameScene"){
                LevelGenerate levelGenerate = FindObjectOfType<LevelGenerate>();
                levelGenerate.UpdatePaidText();
            }
            Debug.Log("you have just bought 60 tip");
        }
        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}