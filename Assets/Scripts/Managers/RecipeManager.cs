using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public List<ProductObject> unlockedRecipes = new List<ProductObject>();
    public List<ProductObject> canResearchRecipes;
    [SerializeField] List<ProductObject> initialRecipes;

    void Start() {
        InitializeBeginningRecipes();
    }

    void InitializeBeginningRecipes() {
        unlockedRecipes = new List<ProductObject>(initialRecipes);
    }

    public void RemoveForResearch(ProductObject product) {
        canResearchRecipes.Remove(product);
    }

    public void UnlockRecipe(ProductObject product) {
        unlockedRecipes.Add(product);
        foreach(ProductObject p in product.unlockNext) {
            canResearchRecipes.Add(p);
        }
    }

    public List<ProductObject> GetRecipesOfType(string type) {
        List<ProductObject> list = new List<ProductObject>();
        foreach (ProductObject p in unlockedRecipes) {
            if (p.productType == type) {
                list.Add(p);
            }
        }
        return list;
    }

    public List<ProductObject> GetRecipesOfType(List<string> types) {
        List<ProductObject> list = new List<ProductObject>();
        foreach (ProductObject p in unlockedRecipes) {
            if (types.Contains(p.productType)) {
                list.Add(p);
            }
        }
        return list;
    }

    public List<ProductObject> GetUnknownRecipesOfType(string type) {
        List<ProductObject> list = new List<ProductObject>();
        foreach (ProductObject p in canResearchRecipes) {
            if (p.productType == type) {
                list.Add(p);
            }
        }
        return list;
    }

    public List<ProductObject> GetUnknownRecipesOfType(List<string> types) {
        List<ProductObject> list = new List<ProductObject>();
        foreach (ProductObject p in canResearchRecipes) {
            if (types.Contains(p.productType)) {
                list.Add(p);
            }
        }
        return list;
    }

    public ProductObject GetItemToBuy(List<string> types) {
        List<ProductObject> candidateProducts = new List<ProductObject>();
        if (Random.Range(0,100) < 5) {
            candidateProducts = GetUnknownRecipesOfType(types);
        }
        if (candidateProducts.Count == 0) {
            candidateProducts = GetRecipesOfType(types);
        }
        if(candidateProducts.Count == 0) {
            return null;
        }
        return Instantiate(candidateProducts[Random.Range(0,candidateProducts.Count)]);
    }

    public ProductObject GetRandomItemToBuy() {
        List<ProductObject> candidateProducts = new List<ProductObject>();
        if (Random.Range(0,100) < 5) {
            candidateProducts = canResearchRecipes;
        }
        if (candidateProducts.Count == 0) {
            candidateProducts = unlockedRecipes;
        }
        if(candidateProducts.Count == 0) {
            return null;
        }
        return Instantiate(candidateProducts[Random.Range(0,candidateProducts.Count)]);
    }

    public int[] SaveRecipesUnlocked() {
        int[] Recipes = new int[unlockedRecipes.Count];
        for (int i = 0; i < Recipes.Length; i++) {
            Recipes[i] = unlockedRecipes[i].ID;
        }
        return Recipes;
    }

    public int[] SaveCanResearchRecipes() {
        int[] Recipes =  new int[canResearchRecipes.Count];
        for (int i = 0; i < Recipes.Length; i++) {
            Recipes[i] = canResearchRecipes[i].ID;
        }
        return Recipes;
    }

    public void LoadRecipes(int[] recipes, int[] canResearch) {
        unlockedRecipes = new List<ProductObject>();
        canResearchRecipes = new List<ProductObject>();
        ManagerManager managerManager = GameObject.Find("Managers").GetComponent<ManagerManager>();
        Dictionary<int,ProductObject> idDict = managerManager.productDictionary.productIdDict;
        for(int i = 0; i < recipes.Length; i++) {
            unlockedRecipes.Add(idDict[recipes[i]]);
        }
        for(int i = 0; i < canResearch.Length; i++) {
            canResearchRecipes.Add(idDict[canResearch[i]]);
        }
    }
}
