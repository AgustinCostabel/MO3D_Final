using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseUI : MonoBehaviour
{
    public static LoseUI Instance { get; private set; }

    // Start is called before the first frame update
    [SerializeField] private GameObject loseActivation;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button restart;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("ERROR: MORE THAN ONE MENU UI");
        } else {
            Instance = this;
        }
        mainMenuButton.onClick.AddListener(() => {
            S_Loader.Load(S_Loader.Scene.MainMenu);
        });
    }

    private void Start() {
        Player.Instance.OnDeathPlayer += Instance_OnDeathPlayer;
    }

    private void Instance_OnDeathPlayer(object sender, System.EventArgs e) {
        Show();
    }

    private void Hide() {
        loseActivation.gameObject.SetActive(false);
    }

    private void Show() {
        loseActivation.gameObject.SetActive(true);
    }
}
