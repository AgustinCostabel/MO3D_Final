using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;

    private void Awake() {
        mainMenuButton.onClick.AddListener(() => {
            S_Loader.Load(S_Loader.Scene.MainMenu);
        });
    }
 
}
