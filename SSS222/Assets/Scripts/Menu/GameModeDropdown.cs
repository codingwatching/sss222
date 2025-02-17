﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static TMPro.TMP_Dropdown;
using System.Linq;

public class GameModeDropdown : MonoBehaviour{
    [SerializeField]List<string> skip=new List<string>(0);
    TMP_Dropdown dd;
    void Start(){
        dd=GetComponent<TMP_Dropdown>();
        
        List<OptionData> options=new List<OptionData>();
        for(var i=0;i<CoreSetup.instance.gamerulesetsPrefabs.Length;i++){
            if(skip.Count==0){
                options.Add(new OptionData(CoreSetup.instance.gamerulesetsPrefabs[i].cfgName,dd.itemImage.sprite));
            }else{for(var j=0;j<skip.Count;j++){if(!CoreSetup.instance.gamerulesetsPrefabs[i].cfgName.Contains(skip[j])){
                    options.Add(new OptionData(CoreSetup.instance.gamerulesetsPrefabs[i].cfgName,dd.itemImage.sprite));
            }}}
        }
        dd.ClearOptions();
        dd.AddOptions(options);
        dd.value=dd.options.FindIndex(d=>d.text.Contains(GameManager.instance.GetCurrentGamemodeName()));//GameManager.instance.GetGamemodeID(dd.options[dd.value].text);
    }
    public void SetGamemode(){
        GameManager.instance.SetGamemodeSelectedStr(dd.options[dd.value].text);
        if(FindObjectOfType<DisplayLeaderboard>()!=null){FindObjectOfType<DisplayLeaderboard>().DisplayCurrentUserHighscore();}
    }
}