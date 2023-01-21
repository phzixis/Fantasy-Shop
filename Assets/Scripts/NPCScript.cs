using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Intention {
    move,
    lookAt,
    wait,
    buy,
    talk,
    leave,
    deinstantiate,
}
public enum ClickState {
    none,
    buy
}

public class NPCScript : MonoBehaviour
{
    public ClickState clickState;
    public GameObject intentionBubble;
    GameObject intentionBubbleObject;
    [SerializeField] CustomerObject customerInfo;
    Queue<(Intention, Vector2)> intentionList;
    public (Intention intention, Vector2 v2) currentIntention;
    bool hasIntention;

    ManagerManager managerManager;
    UIManager uiManager;
    RecipeManager recipeManager;
    public NPCManager npcManager;
    NPCMovementScript movementScript;
    public float customPrice = -1;
    public bool haggled = false;

    
    string dialogue;
    public ProductObject productToBuy;
    public int quantity;
    public Rarity rarity;
    IndividualProductObject itemToBuy;

    void Awake() {
        managerManager = GameObject.FindObjectOfType<ManagerManager>();
        uiManager = managerManager.uiManager;
        recipeManager = managerManager.recipeManager;
        movementScript = GetComponent<NPCMovementScript>();
        intentionList = new Queue<(Intention, Vector2)>();
        hasIntention = false;
        clickState = ClickState.none; 
    }

    void Start() {
        if (productToBuy == null) {
            GetItemToBuy();
        } else {
            itemToBuy = new IndividualProductObject(productToBuy, rarity);
            itemToBuy.quantity = quantity;
        }
    }

    void GetItemToBuy() {
        ProductObject po = recipeManager.GetItemToBuy(customerInfo.canBuy);
        if(po == null) {
            npcManager.SpawnNPC(CustomerClass.Adventurer);
            DeInstantiate(); 
        }
        itemToBuy = new IndividualProductObject(po, Rarity.common);
    }

    public void DeInstantiate() {
        npcManager.customerCount--;
        npcManager.npcList.Remove(this);
        Destroy(gameObject);
    }

    public void AddIntention(Intention i, Vector2 v2) {
        intentionList.Enqueue((i,v2));
        if(!hasIntention) {
            hasIntention = true;
            DoNextIntention();
        }
    }

    public void DoNextIntention() {
        if (intentionList.Count == 0) {
            hasIntention = false;
            return;
        }

        currentIntention = intentionList.Dequeue();
        switch(currentIntention.intention) {
            case Intention.buy:
                intentionBubbleObject = Instantiate(intentionBubble,transform.position + Vector3.up, transform.rotation, transform);
                clickState = ClickState.buy;
                break;
            case Intention.move:
                movementScript.PathFindToLocation(currentIntention.v2);
                break;
            case Intention.lookAt:
                //lookAt
                break;
            case Intention.wait:
                StartCoroutine(WaitForSeconds(currentIntention.v2.x));
                break;
            case Intention.talk:
                //talk
                break;
            case Intention.leave:
                movementScript.Leave();
                clickState = ClickState.none;
                break;
            case Intention.deinstantiate:
                DeInstantiate();
                break;
        }
    }

    IEnumerator WaitForSeconds(float Seconds) {
        yield return new WaitForSeconds(Seconds);;
        DoNextIntention();
    }
    
    public void Leave() {
        AddIntention(Intention.leave, Vector2.zero);
        AddIntention(Intention.deinstantiate, Vector2.zero);
    }

    void Buy() {
        dialogue = customerInfo.buyDialogue[Random.Range(0,customerInfo.buyDialogue.Count)];
        
        if(customPrice < 0) {
            itemToBuy.priceBound = managerManager.playerManager.GetPriceBound();
            itemToBuy.priceChange = Random.Range(0,1f);
        } else {
            itemToBuy.priceBound = 0;
            itemToBuy.priceChange = customPrice;
        }
        dialogue = dialogue.Replace("<ITEM>", itemToBuy.info.productName);
        dialogue = dialogue.Replace("<AMOUNT>", itemToBuy.sellPrice.ToString());
        uiManager.CreateBuyingMenu(dialogue, itemToBuy, this);
    }

    void OnMouseDown() {
        if(!EventSystem.current.IsPointerOverGameObject()) {
            switch(clickState) {
                case ClickState.none:
                    break;
                case ClickState.buy:
                    Buy();
                    break;
            }
        }
    }
}
