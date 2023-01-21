using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType{
    craft = 0,
    research = 1,
    furniture = 2
}

public class Task
{
    public Clock duration;
    public TaskType taskType;
    public Sprite sprite;
    public int progress;

    ProductObject product;
    FurnitureObject furniture;

    public Task(Clock Duration, TaskType Type, ProductObject Product) {
        duration = Duration;
        taskType = Type;
        product = Product;
        sprite = Product.img;
        progress = 0;
    }

    public Task(Clock Duration, FurnitureObject Furniture) {
        duration = Duration;
        taskType = TaskType.furniture;
        furniture = Furniture;
        sprite = Furniture.sprite;
        progress = 0;
    }

    public Task(Clock Duration, TaskType Type, int Progress, int ProductID) {
        duration = Duration;
        taskType = Type;
        progress = Progress;

        GetProductFromID(Type, ProductID);
    }

    public void Cancel() {
        switch (taskType) {
            case TaskType.craft:
                InventoryManager inventoryManager = GameObject.FindObjectOfType<InventoryManager>();
                for (int i = 0; i < product.materialName.Count; i++) {
                    inventoryManager.AddMaterial(product.materialName[i], product.materialQuantity[i]);
                }
                break;
            case TaskType.research:
                GameObject.FindObjectOfType<RecipeManager>().RemoveForResearch(product);
                break;
            case TaskType.furniture:
                GameObject.FindObjectOfType<PlayerManager>().GainGold(furniture.upgradeCost); 
                furniture.isUpgrading = false;
                break;
        }
    }

    public void Finish() {
        switch (taskType) {
            case TaskType.craft:
                InventoryManager inventoryManager = GameObject.FindObjectOfType<InventoryManager>();
                EventsManager eventsManager = GameObject.FindObjectOfType<EventsManager>();
                inventoryManager.AddProduct(product, Rarity.common);   
                eventsManager.productsMadeToday.Add(product);
                break;
            case TaskType.research:
                RecipeManager recipeManager = GameObject.FindObjectOfType<RecipeManager>();
                recipeManager.UnlockRecipe(product);
                break;
            case TaskType.furniture:
                GameObject.FindObjectOfType<FurnitureManager>().AddFurniture(furniture.id, 1);
                furniture.isUpgrading = false;
                break;
        }
    }

    public int GetTaskID() {
        switch (taskType) {
            case TaskType.furniture:
                return furniture.id;
            default:
                return product.ID;
        }
    }

    void GetProductFromID(TaskType type, int id) {
        switch(taskType) {
            case TaskType.furniture:
                // No implementation
                break;
            default:
                ProductDictionary pd = GameObject.Find("ProductDictionary").GetComponent<ProductDictionary>();
                product = pd.GetProductFromID(id);
                sprite = product.img;
                break;
        }
    }
}
