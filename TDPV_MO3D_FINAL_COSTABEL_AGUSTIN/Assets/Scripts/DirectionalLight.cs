using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class DirectionalLight : MonoBehaviour
{

    public static DirectionalLight Instance { get; private set; }
   
    private new Light light;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float rotationSpeedSkybox;
    float rotationX = 45f;
    float rotationY = 45f;
    public bool direction = true;

    private float lerpTime;
    [SerializeField] private Color[] skyColors;
    private int nextColor;
    private float t = 0f;

    private void Awake() {
        Instance = this;
        light = GetComponent<Light>();
    }

    private void Start() {

        lerpTime = (1/GameManager.Instance.GetGameTimerMax())*10;

        GameManager.Instance.OnTimeLapsed += GameManager_OnTimeLapsed;

        RenderSettings.skybox.SetColor("_Tint", new Color32(10, 10, 10, 1));

        light.color = skyColors[4];
        nextColor = 4;
    }

    void Update() {
        //transform.localEulerAngles = new Vector3(RotationX(), 0, 0);
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeedSkybox);

        if (!GameManager.Instance.GetTimer()) {
            light.color = Color.Lerp(light.color, skyColors[nextColor], lerpTime * Time.deltaTime);

            t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);
            if (t > .9f) {
                t = 0f;
                if (nextColor == 4) {
                    nextColor = 0;
                } else {
                    nextColor++;
                }
            }
        }

    }

    private void GameManager_OnTimeLapsed(object sender, EventArgs e) {
        RenderSettings.skybox.SetColor("_Tint", new Color32(120, 120, 120, 1));
        light.color = new Color32(255, 240, 200, 1);
        nextColor = 1;
        t = 0f;
        rotationX = 45f;
        rotationY = 45f;
    }

    float RotationX() {
        rotationX += rotateSpeed * Time.deltaTime;
        if (rotationX >= 140f)
            rotationX -= 120f;
        return direction ? rotationX : -rotationX;
    }

    float RotationY() {
        rotationY += rotateSpeed * Time.deltaTime;
        if (rotationY >= 360f)
            rotationY -= 360f;
        return direction ? rotationY : -rotationY;
    }

    public Light GetLight() {
        return light;
    }
}

