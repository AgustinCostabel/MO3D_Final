using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceManager : MonoBehaviour
{
    public event Action<float> OnGainExperience;
    public event EventHandler OnLevelUp;
    [SerializeField] private Image barImage;
    [SerializeField] private float expMax;
    private float experience;

    private void Start() {
        OnGainExperience += Instance_OnGainExperience;
        experience = 0f;
    }

    private void OnDisable() {
        OnGainExperience -= Instance_OnGainExperience;
    }

    public void GainExperience(float amount) {
        if (Player.Instance.GetLevel() < 5) {
            experience += amount;
            if (experience >= expMax) {
                experience = 0;
                LevelUp();
            }

            OnGainExperience?.Invoke(experience / expMax);
        }
    }

    private void Instance_OnGainExperience(float experienceRatio) {
        barImage.fillAmount = experienceRatio;
    }

    private void LevelUp() {
        OnLevelUp?.Invoke(this, EventArgs.Empty);
        expMax *= 2;
    }
}
