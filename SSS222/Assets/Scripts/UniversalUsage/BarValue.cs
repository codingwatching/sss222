﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum barType{
    HorizontalR,
    HorizontalL,
    VerticalU,
    VerticalD,
    Fill
}
public class BarValue : MonoBehaviour{
    [SerializeField] barType barType=barType.HorizontalR;
    [SerializeField] string valueName;
    [SerializeField] float value;
    //[SerializeField] string maxValueName;
    [SerializeField] float maxValue;
    void Start(){
        
    }

    void Update(){
        if(valueName.Contains("health")){if(Player.instance!=null){value=Player.instance.health;maxValue=Player.instance.healthMax;}}
        if(valueName.Contains("energy")){if(Player.instance!=null){value=Player.instance.energy;maxValue=Player.instance.energyMax;}}
        if(valueName.Contains("xp")){if(FindObjectOfType<GameSession>()!=null){value=FindObjectOfType<GameSession>().xp;maxValue=FindObjectOfType<GameSession>().xpMax;}}
        if(valueName.Contains("shopTimer")){if(FindObjectOfType<Shop>()!=null){value=FindObjectOfType<Shop>().shopTimer;maxValue=FindObjectOfType<Shop>().shopTimeMax;}}
        if(valueName.Contains("hpAbsorp")){if(Player.instance!=null){value=Player.instance.hpAbsorpAmnt;maxValue=Player.instance.healthMax/4;}}
        if(valueName.Contains("enAbsorp")){if(Player.instance!=null){value=Player.instance.enAbsorpAmnt;maxValue=Player.instance.healthMax/4;}}

        if(barType==barType.HorizontalR){transform.localScale=new Vector2(value/maxValue,transform.localScale.y);}
        if(barType==barType.HorizontalL){transform.localScale=new Vector2(value/maxValue,transform.localScale.y);/*new Vector2(-(value/maxValue),transform.localScale.y);*/}
        if(barType==barType.VerticalU){transform.localScale=new Vector2(transform.localScale.x,-(value/maxValue));}
        if(barType==barType.VerticalD){transform.localScale=new Vector2(transform.localScale.x,value/maxValue);}
        if(barType==barType.Fill){GetComponent<Image>().fillAmount=value/maxValue;}
    }
}