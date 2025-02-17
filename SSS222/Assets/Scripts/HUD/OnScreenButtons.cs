﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnScreenButtons : MonoBehaviour{
    [Sirenix.OdinInspector.HideInPlayMode]public bool lvldUp;
    void Update(){
        if(SaveSerial.instance.settingsData.scbuttons==false&&
        (GetComponent<Animator>()==null||(GetComponent<Animator>()!=null&&!GetComponent<Animator>().GetBool("on")))
        ){
            foreach(Button bt in GetComponentsInChildren<Button>()){if(bt.enabled){
                bt.enabled=false;
                bt.GetComponent<Image>().enabled=false;
            }}
        }else{
            foreach(Button bt in GetComponentsInChildren<Button>()){if(!bt.enabled){
                bt.enabled=true;
                bt.GetComponent<Image>().enabled=true;
            }}
        }
    }
    public void Pause(){
        if(PauseMenu.GameIsPaused!=true){
            if(PauseMenu.instance._isPausable()){PauseMenu.instance.Pause();}
        }else{PauseMenu.instance.Resume();}
    }
    public void OpenUpgrade(){
        if(UpgradeMenu.UpgradeMenuIsOpen!=true){
            if(UpgradeMenu.instance._isOpenable()){UpgradeMenu.instance.Open();}
        }else{UpgradeMenu.instance.Resume();}
    }

    public void UseSkillQ(){if(Player.instance!=null)Player.instance.GetComponent<PlayerModules>().CheckSkillButton(1);}
    public void UseSkillE(){if(Player.instance!=null)Player.instance.GetComponent<PlayerModules>().CheckSkillButton(2);}
}
