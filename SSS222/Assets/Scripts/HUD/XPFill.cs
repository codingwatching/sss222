﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPFill : MonoBehaviour{
    [SerializeField] bool replaceSprites=true;
    [SerializeField] Sprite emptySprite;
    [SerializeField] Sprite fillSprite;
    [SerializeField] GameObject particlePrefab;
    public bool changed;
    public bool shop;
    public bool upgradeMenu=true;
    [SerializeField] public string valueName;
    [SerializeField] public int valueReq;
    public int value;
    Image img;
    PlayerModules pmodules;
    void Start(){
        img=GetComponent<Image>();
        pmodules=Player.instance.GetComponent<PlayerModules>();
        if(replaceSprites){
            if(transform.root.gameObject.name.Contains("UpgradeCanvas")){
                emptySprite=AssetsManager.instance.Spr("upgradeEmpty");
                fillSprite=AssetsManager.instance.Spr("upgradeFill");
                particlePrefab=AssetsManager.instance.GetVFX("UpgradeVFX");
                /*if(pmodules.shipLvl<GameRules.instance.saveBarsFromLvl){
                    if(fillSprite!=AssetsManager.instance.Spr("upgradeFill_des"))fillSprite=AssetsManager.instance.Spr("upgradeFill_des");
                    if(particlePrefab!=AssetsManager.instance.GetVFX("UpgradeVFX_des"))particlePrefab=AssetsManager.instance.GetVFX("UpgradeVFX_des");
                }*/
            }
        }
    }

    void Update(){
        if(shop){value=Convert.ToInt32(Shop.instance.GetType().GetField(valueName).GetValue(Shop.instance));}
        else if(upgradeMenu){value=Convert.ToInt32(UpgradeMenu.instance.GetType().GetField(valueName).GetValue(UpgradeMenu.instance));}
        else{
            if(valueName=="shipLvlFraction"){value=Player.instance.GetComponent<PlayerModules>().shipLvlFraction;}

            if(valueName.Contains("moduleUnlocked_")){valueReq=1;value=AssetsManager.BoolToInt(Player.instance.GetComponent<PlayerModules>()._isModuleUnlocked(valueName.Split('_')[1]));}
            if(valueName.Contains("skillUnlocked_")){valueReq=1;value=AssetsManager.BoolToInt(Player.instance.GetComponent<PlayerModules>()._isSkillUnlocked(valueName.Split('_')[1]));}

            if(valueName.Contains("moduleMaxed")){valueReq=1;value=AssetsManager.BoolToInt(Player.instance.GetComponent<PlayerModules>()._isModuleMaxed(valueName.Split('_')[1]));}
            if(valueName.Contains("skillMaxed_")){valueReq=1;value=AssetsManager.BoolToInt(Player.instance.GetComponent<PlayerModules>()._isSkillMaxed(valueName.Split('_')[1]));}
            
            if(valueName.Contains("moduleEquipped_")){valueReq=1;value=AssetsManager.BoolToInt(Player.instance.GetComponent<PlayerModules>()._isModuleEquipped(valueName.Split('_')[1]));}
            if(valueName.Contains("skillEquipped_")){valueReq=1;value=AssetsManager.BoolToInt(Player.instance.GetComponent<PlayerModules>()._isSkillEquipped(valueName.Split('_')[1]));}
            
            if(valueName.Contains("moduleEquippedThisSlot_")){valueReq=1;value=AssetsManager.BoolToInt(Player.instance.GetComponent<PlayerModules>().moduleSlots.FindIndex(x=>x==valueName.Split('_')[1])==UpgradeMenu.instance.selectedModuleSlot);}
            if(valueName.Contains("skillEquippedThisSlot_")){valueReq=1;value=AssetsManager.BoolToInt(Player.instance.GetComponent<PlayerModules>().skillsSlots.FindIndex(x=>x==valueName.Split('_')[1])==UpgradeMenu.instance.selectedSkillSlot);}
            if(valueName=="moduleEmptyThisSlot"){valueReq=1;value=AssetsManager.BoolToInt(Player.instance.GetComponent<PlayerModules>().moduleSlots[UpgradeMenu.instance.selectedModuleSlot]=="");}
            if(valueName=="skillEmptyThisSlot"){valueReq=1;value=AssetsManager.BoolToInt(Player.instance.GetComponent<PlayerModules>().skillsSlots[UpgradeMenu.instance.selectedSkillSlot]=="");}
        }

        if(valueReq!=0){
            if(value>=valueReq){
                if(changed==false){
                    img.sprite=fillSprite;
                    UpgradeParticles();
                    //shake.CamShake();
                    changed=true;
                }
            }else{img.sprite=emptySprite;changed=false;}
        }else{
            if(value==1){
                if(changed==false){
                    img.sprite=fillSprite;
                    UpgradeParticles();
                    changed=true;
                }
            }else{img.sprite=emptySprite;changed=false;}
        }
    }
    public void UpgradeParticles(){
        var pt=Instantiate(particlePrefab,transform);
        var ps=pt.GetComponent<ParticleSystem>();
        var sh=ps.shape;
        sh.radius*=GetComponent<RectTransform>().sizeDelta.x/160;
        var pm=ps.main;
        pm.maxParticles*=Mathf.RoundToInt(GetComponent<RectTransform>().sizeDelta.x/110);
        var pe=ps.emission;
        pe.rateOverTime=Mathf.RoundToInt(GetComponent<RectTransform>().sizeDelta.x);
        Destroy(pt,0.5f);
    }
}
