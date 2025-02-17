﻿using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class LootTableEntryWaves{
    [HideInInspector]public string name;
    public WaveConfig lootItem;
    public float dropChance=0f;
    public int levelReq=0;
}
public class LootTableWaves : MonoBehaviour{
    [SerializeField]public List<LootTableEntryWaves> itemList;
    private Dictionary<WaveConfig, float> itemTable;
    [SerializeField] int currentLvl;
    [ReadOnly][SerializeField] List<float> dropList;
    [ReadOnly][SerializeField] ItemPercentageLable[] itemsPercentage;
    [ReadOnly]public float sum;
    
    void OnValidate(){SumUp();}
    private void Update() {
        if(Player.instance!=null)if(GameRules.instance.modulesOn)currentLvl=Player.instance.GetComponent<PlayerModules>().shipLvl;
        else currentLvl=-4;
        SumUp();
        SumUpAfter();
    }
    public WaveConfig GetItem(){
        float randomWeight = 0;
        do{//No weight on any number?
            if(sum==0)return null;
            randomWeight=Random.Range(0,sum);
        }while(randomWeight==sum);
        var i=-1;
        foreach(LootTableEntryWaves entry in itemList){
            i++;
            if(randomWeight<dropList[i])return entry.lootItem;
            randomWeight-=dropList[i];
        }
        return null;
    }
    /*void SumUp(){
        dropList.Clear();
        itemTable = new Dictionary<WaveConfig, float>();
        var i=-1;
        foreach(LootTableEntryWaves entry in itemList){
            i++;
            dropList.Add(entry.dropChance);
            if(currentLvl<entry.levelReq&&currentLvl!=-4)dropList[i]=0;
            entry.name=entry.lootItem.name;
            itemTable.Add(entry.lootItem, (float)dropList[i]);
            var value=System.Convert.ToSingle(System.Math.Round((dropList[i]/sum*100),2));
                if(i>=0&&i<itemsPercentage.Length)
                itemsPercentage[i].name=
                entry.name+
                "("+entry.levelReq+")"
                +" - "+value+"%"+" - "+
                dropList[i]+"/"+
                (sum-dropList[i]);
        }
        sum=itemTable.Values.Sum();
        System.Array.Resize(ref itemsPercentage, itemList.Count);
    }*/

    void SumUp(){
        if(dropList.Count<itemList.Count){
        dropList=new List<float>(itemList.Count);
        itemTable=new Dictionary<WaveConfig,float>();
        var i=-1;
        System.Array.Resize(ref itemsPercentage, itemList.Count);
        foreach(LootTableEntryWaves entry in itemList){
            i++;
            dropList.Add(entry.dropChance);
            if(!GameRules.instance.levelingOn){entry.levelReq=0;}
            if(currentLvl<entry.levelReq&&currentLvl!=-4&&GameRules.instance.levelingOn)dropList[i]=0;
            entry.name=entry.lootItem.name;
            
            itemTable.Add(
                entry.lootItem,
            (float)dropList[i]);
            var value=System.Convert.ToSingle(System.Math.Round((dropList[i]/sum*100),2));
                if(entry!=null&&itemsPercentage!=null&&itemsPercentage[i]!=null){
                    itemsPercentage[i].name=
                    entry.name+
                    "("+entry.levelReq+")"
                    +" - "+value+"%"+" - "+
                    dropList[i]+"/"+
                    (sum-dropList[i]);
                }
        }
        sum=dropList.Sum();
        System.Array.Resize(ref itemsPercentage, itemList.Count);
        }
    }
    void SumUpAfter(){
        if(dropList.Count<itemList.Count){dropList.Capacity=itemList.Capacity;}
        var i=-1;
        foreach(LootTableEntryWaves entry in itemList){
            i++;
            if(!GameRules.instance.levelingOn){entry.levelReq=0;}
            if(currentLvl<entry.levelReq&&GameRules.instance.levelingOn)dropList[i]=0;
            
            var value=System.Convert.ToSingle(System.Math.Round((dropList[i]/sum*100),2));
                if(entry!=null&&itemsPercentage!=null&&itemsPercentage[i]!=null){
                    itemsPercentage[i].name=
                    entry.name+
                    "("+entry.levelReq+")"
                    +" - "+value+"%"+" - "+
                    dropList[i]+"/"+
                    (sum-dropList[i]);
                }
        }
        sum=dropList.Sum();
    }
}
