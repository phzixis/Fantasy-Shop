using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemFrame : MonoBehaviour
{
    [SerializeField] List<Sprite> frameTypes = new List<Sprite>();
    [SerializeField] Image item;
    [SerializeField] TextMeshProUGUI quantityText;
    [SerializeField] float ratio;
    [SerializeField] GameObject noneText;
    Image image;

    void Awake() {
        image = GetComponent<Image>();
    }

    void Start() {
        StartCoroutine(SetIconSize());
    }

    IEnumerator SetIconSize() {
        yield return 0;
        Vector2 parentSize = GetComponent<RectTransform>().sizeDelta;
        item.gameObject.GetComponent<RectTransform>().sizeDelta = parentSize * ratio;
    }

    public void SetProduct(Rarity rarity) {
        noneText.SetActive(true);
        item.gameObject.SetActive(false);
        if(rarity == Rarity.none) {
            image.sprite = frameTypes[0];
        } else {
            image.sprite = frameTypes[((int)rarity)+1];
            quantityText.text = "";
        }
    }

    public void SetProduct(ProductObject product, Rarity rarity) {
        noneText.SetActive(false);
        item.gameObject.SetActive(true);
        item.sprite = product.img;
        if(rarity == Rarity.none) {
            image.sprite = frameTypes[0];
        } else {
            image.sprite = frameTypes[((int)rarity)+1];
            quantityText.text = product.inventory[rarity] > 1 ? product.inventory[rarity].ToString() : "";
        }
    }
}
