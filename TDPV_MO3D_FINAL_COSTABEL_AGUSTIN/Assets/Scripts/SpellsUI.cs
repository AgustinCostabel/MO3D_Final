using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellsUI : MonoBehaviour
{
    public static SpellsUI Instance { get; private set; }

    [SerializeField] private Image spell1;
    [SerializeField] private Image spell2;
    [SerializeField] private Image spell3;
    [SerializeField] private Image spell4;
    [SerializeField] private Image[] spellsImage;
    [SerializeField] private SpellsSO[] spellsSO;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("ERROR: MORE THAN ONE SPELL UI");
        }
        Instance = this;

        spell1.type = Image.Type.Filled;
        spell1.fillOrigin = (int)Image.Origin360.Top;
        spell2.type = Image.Type.Filled;
        spell2.fillOrigin = (int)Image.Origin360.Top;
        spell3.type = Image.Type.Filled;
        spell3.fillOrigin = (int)Image.Origin360.Top;
        spell4.type = Image.Type.Filled;
        spell4.fillOrigin = (int)Image.Origin360.Top;
        spellsImage = new Image[] { spell1, spell2, spell3, spell4 };
    }

    private void Start() {
        spellsSO = Spells.Instance.GetSpellsSO();
    }

    private void Update() {
        CooldownsUI();
    }

    public void ActiveSpellSprite(int index, Sprite newSprite) {

        spellsImage[index].sprite = newSprite;
        spellsImage[index].gameObject.SetActive(true);
    }

    public void SelectSpellSprite(int index) {
        for (int i = 0; i < spellsImage.Length; i++) {
            if (index == i) {
                spellsImage[i].color = new Color(255f, 255f, 255f, 1f);
            } else {
                spellsImage[i].color = new Color(255f, 255f, 255f, 0.3f);
            }
        }
    }

    public void CooldownsUI() {
        for(int i = 0; i < spellsImage.Length; i++) {
            if (spellsSO[i] != null) {
                if (spellsSO[i].cooldown >= 0) {
                    spellsImage[i].fillAmount = 1 - (spellsSO[i].cooldown / spellsSO[i].cooldownMax);
                }
            }
        }
    }
}
