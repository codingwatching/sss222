using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class CstmzTypeElement : MonoBehaviour, IPointerClickHandler{
    [Header("Object References")]
    [SceneObjectsOnly][SerializeField] public GameObject selectedBg;
    [SceneObjectsOnly][SerializeField] public GameObject elementPv;
    [SceneObjectsOnly][SerializeField] public GameObject overlayImg;
    [Header("Properties")]
    [SerializeField] public CstmzType elementType=CstmzType.skin;
    [SerializeField] public string elementName="Mk.22";
    //[SerializeField] public CstmzRarity rarity;
    void Update(){
        //GetComponent<Image>().color=CustomizationInventory.instance.GetRarityColor(rarity);
        if(elementType==CstmzType.skin){
            elementName=CustomizationInventory.instance.skinName;
            if(ShipCustomizationManager.instance.GetOverlaySprite(elementName)!=null){overlayImg.GetComponent<Image>().color=CustomizationInventory.instance.overlayColor;}
            else{overlayImg.GetComponent<Image>().color=Color.clear;}
        }//else{if(transform.childCount>1)Destroy(overlayImg.gameObject);}
        else if(elementType==CstmzType.trail){
            elementName=CustomizationInventory.instance.trailName;
        }else if(elementType==CstmzType.flares){
            elementName=CustomizationInventory.instance.flaresName;
        }else if(elementType==CstmzType.deathFx){
            elementName=CustomizationInventory.instance.deathFxName;
        }else if(elementType==CstmzType.music){
            elementName=CustomizationInventory.instance.musicName;
        }
    }
    public void SetType(){CustomizationInventory.instance.SetType(elementType);}
    public void OnPointerClick(PointerEventData eventData){
        if(eventData.button==PointerEventData.InputButton.Left){
            SetType();
        }
    }
}