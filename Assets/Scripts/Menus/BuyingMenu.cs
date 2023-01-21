using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyingMenu : MonoBehaviour
{
    IndividualProductObject productToBuy;
    InventoryManager inventoryManager;
    TimeManager timeManager;
    EventsManager eventsManager;
    NPCScript npcScript;
    public Button sellButton;
    public Button suggestButton;
    public Button refuseButton;
    public Button haggleButton;
    public Button returnButton;

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI price;
    public TextMeshProUGUI marketPrice;
    public TextMeshProUGUI itemQuantity;
    public TextMeshProUGUI message;
    public TextMeshProUGUI hagglePercent;
    public Image itemIcon;

    float haggleChance;

    const string higherColor = "#28CC0B";
    const string lowerColor = "#FA1515";
    const string equalColor = "#FFFFFF";

    void Awake() {
        ManagerManager managerManager = FindObjectOfType<ManagerManager>();
        inventoryManager = managerManager.inventoryManager;
        timeManager = managerManager.timeManager;
        eventsManager = managerManager.eventsManager;
    }

    void Start() {
        timeManager.Pause();
    }

    void UpdateSellButton() {
        ProductObject existing = inventoryManager.SearchProduct(productToBuy);
        bool interactable = false;
        if (existing != null) {
            if(productToBuy.special) {
                //sellspecial
            } else {
                if (existing.inventory[productToBuy.rarity] >= productToBuy.quantity) {
                    interactable = true;
                }
            }
        }
        sellButton.interactable = interactable;
    }

    public void SetProduct(IndividualProductObject product, string text, NPCScript npc) {
        productToBuy = product;
        npcScript = npc;

        UpdatePriceText();
        SetHaggleChance();
        itemName.text = product.info.productName;
        marketPrice.text = "Market Price: " + (product.marketPrice*product.quantity).ToString();
        message.text = text;
        itemIcon.sprite = product.info.img;
        if(product.quantity > 1) {
            itemQuantity.gameObject.SetActive(true);
            itemQuantity.text = product.quantity.ToString();
        }
        UpdateSellButton();
    }

    void SetHaggleChance() {
         if(npcScript.haggled) {
            haggleButton.interactable = false;
            Destroy(hagglePercent.gameObject);
            return;
        }
        float additional = productToBuy.priceBound == 0 ? .5f : 0;
        haggleChance = 1f - (productToBuy.priceChange - additional);
        hagglePercent.text = "("+((int) (haggleChance*100)).ToString()+"%)";
    }

    void UpdatePriceText() {
        string color;
        int sign;
        if (productToBuy.sellPrice > productToBuy.marketPrice * productToBuy.quantity) {
            color = higherColor;
            sign = 0;
        } else if (productToBuy.sellPrice < productToBuy.marketPrice * productToBuy.quantity){
            color = lowerColor;
            sign = 1;
        } else {
            color = equalColor;
            sign = 2;
        }
        price.text = "Price: <color="+color+">"+productToBuy.sellPrice.ToString()+"</color> <sprite="+sign.ToString()+">";
    }


    public void Sell() {
        eventsManager.productsSoldToday.Add(productToBuy);
        inventoryManager.SellProduct(productToBuy);
        npcScript.Leave();
        npcScript.DoNextIntention();
        CloseMenu();
    }

    public void Haggle() {
        haggleButton.interactable = false;
        float newPrice = Random.Range(0,1f);
        npcScript.haggled = true;
        if (newPrice < haggleChance) {
            if (newPrice < .35f) {
                newPrice += .35f;
            }
            productToBuy.priceChange += newPrice;
            UpdatePriceText();
            Destroy(hagglePercent.gameObject);
            message.text = "You're right that's a more reasonable price";
        } else {
            message.text = "I'll take my business elsewhere";
            sellButton.interactable = false;
            suggestButton.interactable = false;
            refuseButton.interactable = false;
            returnButton.interactable = false;
            npcScript.Leave();
            npcScript.DoNextIntention();
            StartCoroutine(DelayedCloseMenu());
        }
    }

    public void Return() {

    }

    public void Suggest() {

    }

    public void Refuse() {
        npcScript.Leave();
        npcScript.DoNextIntention();
        CloseMenu();
    }

    public void CloseMenu() {
        npcScript.customPrice = productToBuy.priceChange + productToBuy.priceBound;
        timeManager.Resume();
        Destroy(gameObject);
    }

    IEnumerator DelayedCloseMenu() {
        yield return new WaitForSeconds(1);
        CloseMenu();
    }
}
