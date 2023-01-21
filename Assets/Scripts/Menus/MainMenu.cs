using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject contents;
    [SerializeField] Image fade;
    [SerializeField] float fadeDuration;

    bool fadingIn = false;
    bool fadingOut = false;

    void Update() {
        if (fadingIn) {
            Color fadeColor = fade.color;
            fadeColor.a += 1/fadeDuration * Time.deltaTime;
            fade.color = fadeColor;
            if (fadeColor.a >= 1) {
                fadingIn = false;
                fadingOut = true;
                contents.SetActive(false);
            }
        }

        if (fadingOut) {
            Color fadeColor = fade.color;
            fadeColor.a -= 1/fadeDuration * Time.deltaTime;
            fade.color = fadeColor;
            if (fadeColor.a <= 0) {
                fadingOut = false;
                Destroy(gameObject);
            }
        }
    }


    public void NewGame() {
        GameObject.FindObjectOfType<TimeManager>().StartDayWithoutCustomers();
        fade.gameObject.SetActive(true);
        fadingIn = true;
    }

    public void LoadGame() {
        GameObject.FindObjectOfType<PlayerManager>().LoadGame();
        RemoveScreen();
    }

    public void RemoveScreen() {
        GameObject.FindObjectOfType<TimeManager>().StartDay();
        fade.gameObject.SetActive(true);
        fadingIn = true;
    }
}
