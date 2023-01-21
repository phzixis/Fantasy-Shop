using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualProductObject
{
    public ProductObject info;
    public Rarity rarity;
    public float priceChange = .5f;
    public float priceBound = 0;
    public int quantity;
    public int strength;
    public int durability;
    public int maxDurability;
    public bool special;
    public int marketPrice {
        get {return (int)(info.price * info.multiplier[rarity] * quantity);}
    }
    public int sellPrice {
        get {return (int)((priceBound+priceChange)*info.price * info.multiplier[rarity] * quantity);}
    }

    public IndividualProductObject(ProductObject Info, Rarity Rarity) {
        info = Info;
        rarity = Rarity;
        quantity = Info.quantityToAdd;
        special = false;
    }
}
