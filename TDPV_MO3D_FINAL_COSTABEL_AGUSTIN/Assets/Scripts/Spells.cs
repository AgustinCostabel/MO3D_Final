using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spells : MonoBehaviour
{

    public static Spells Instance { get; private set; }
    [SerializeField] private SpellsSO[] spellsSO;
    private int spellsCount = 0;
    private int spellActive = 5;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("ERROR: MORE THAN ONE SPELL CLASS");
        }
        Instance = this;
        spellsSO = new SpellsSO[10];
    }

    private void Start() {
        for (int i = 0; i < spellsCount; i++) {          
            spellsSO[i].cooldown = 0;           
        }
    }

    private void Update() {
        for (int i = 0; i < spellsCount; i++) {
            if (spellsSO[i].cooldown >= 0) {
                spellsSO[i].cooldown -= Time.deltaTime;
            }
        }
    }

    public void AddSpell(SpellsSO spellSO) {
        spellsSO[spellsCount] = spellSO;
        SpellsUI.Instance.ActiveSpellSprite(spellsCount, spellSO.spellSprite);
        spellsCount++;
        spellSO.cooldown = 0;
    }

    public void CastSpell(Player player) {
        if (spellsSO[spellActive] != null && spellsSO[spellActive].cooldown <= 0) {
            if(spellsSO[spellActive].type == SpellsSO.Type.Heal) {
                player.Heal(spellsSO[spellActive].heal);
                spellsSO[spellActive].cooldown = spellsSO[spellActive].cooldownMax;
            }
            if (spellsSO[spellActive].type == SpellsSO.Type.Buff) {
                player.Buff(spellsSO[spellActive].buffDamage, spellsSO[spellActive].buffTime);
                spellsSO[spellActive].cooldown = spellsSO[spellActive].cooldownMax;
            }
            if(spellsSO[spellActive].type == SpellsSO.Type.Melee) {
                if (player.HasWeaponTwoHands()) {
                    player.MeleeSpell(spellsSO[spellActive].damage);
                    spellsSO[spellActive].cooldown = spellsSO[spellActive].cooldownMax;
                }
            }
            if(spellsSO[spellActive].type == SpellsSO.Type.Cast) {
                player.CastSpell(spellsSO[spellActive].damage);
                spellsSO[spellActive].cooldown = spellsSO[spellActive].cooldownMax;
                SoundManager.Instance.Fireball();
            }
        }
    }

    public void SelectSpell(int index) {
        if (spellsSO[index] != null) {
            spellActive = index;
            SpellsUI.Instance.SelectSpellSprite(index);
        }
    }

    public SpellsSO GetSpellActive() {
        return spellsSO[spellActive];
    }

    public SpellsSO[] GetSpellsSO() {
        return spellsSO;
    }

    public void SpellLevelUp(SpellsSO spellSO) {

    }

}
