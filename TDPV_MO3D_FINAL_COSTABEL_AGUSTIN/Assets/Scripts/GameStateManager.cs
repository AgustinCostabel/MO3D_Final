using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
    }

    private void Update() {
        
    }

}
