﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ShipCustomizationManager : MonoBehaviour{  public static ShipCustomizationManager instance;
    [HeaderAttribute("Variables")]
    public bool otherUsersData;
    [HeaderAttribute("Properties")]
    public string skinName="def";
    [SceneObjectsOnly][SerializeField] public GameObject overlayObj;
    public Color overlayColor=Color.red;
    public string trailName="def";
    [SceneObjectsOnly][SerializeField] public GameObject trailObj;
    Vector2 trailObjPos=Vector2.zero;
    public string flaresName="def";
    public string deathFxName="def";

    SpriteRenderer overlaySpr;
    Image overlayImg;
    SpriteRenderer spr;
    Image img;
    void Awake(){instance=this;}
    void Start(){
        if(String.IsNullOrEmpty(skinName)){skinName="def";}
        if(String.IsNullOrEmpty(trailName)){trailName="def";}
        if(String.IsNullOrEmpty(flaresName)){flaresName="def";}
        if(String.IsNullOrEmpty(deathFxName)){deathFxName="def";}

        spr=GetComponent<SpriteRenderer>();
        if(spr==null)img=GetComponent<Image>();
        if(!otherUsersData)LoadValues();
        else{LoadValuesFromSelectedUsersData();}
        if(overlayObj!=null){
            overlayObj.transform.position=new Vector3(overlayObj.transform.position.x,overlayObj.transform.position.y,transform.root.position.z-0.01f);
            overlayObj.transform.localScale=Vector3.one;
            overlaySpr=overlayObj.GetComponent<SpriteRenderer>();
            if(overlaySpr==null){overlayImg=overlayObj.GetComponent<Image>();}
            if(GameManager.maskMode!=0&&overlaySpr!=null){overlaySpr.maskInteraction=(SpriteMaskInteraction)GameManager.maskMode;}
        }
    }
    void Update(){
        SetSkin(skinName);
        if(GetOverlaySprite(skinName)!=null)SetOverlay(GetOverlaySprite(skinName),overlayColor);
        else{
            if(overlaySpr!=null)overlaySpr.color=Color.clear;
            if(overlayImg!=null)overlayImg.color=Color.clear;
        }
        SetTrail(trailName);
    }


    //string GetSkinName(){string str=skinName;if(skinName.Contains(" _")){str=skinName.Split('_')[0];}return str;}
    public CstmzSkin GetSkin(string str){string _str=str;if(_str.Contains("_")){_str=_str.Split('_')[0];}return AssetsManager.instance.GetSkin(_str);}
    //public CstmzSkin GetSkinCurrent(){string _str=skinName;if(_str.Contains("_")){_str=_str.Split('_')[0];}return AssetsManager.instance.GetSkin(_str);}
    public CstmzSkinVariant GetSkinVariant(string str,int id){string _str=str;if(_str.Contains("_")){_str=_str.Split('_')[0];}
        return AssetsManager.instance.GetSkinVariant(_str,id);}
    public CstmzSkinVariant GetSkinVariantAuto(string str){return AssetsManager.instance.GetSkinVariant(str.Split('_')[0],int.Parse(str.Split('_')[1]));}
    
    
    public Sprite GetSkinSprite(string str){CstmzSkin skin=null;Sprite spr=null;
        skin=GetSkin(str);
        if(str.Contains("_")){spr=GetSkinVariantAuto(str).spr;}
        else{if(skin.spr!=null)spr=skin.spr;}

        if(skin.animated){
            if(anim==null){animSpr=skin.animVals[0].spr;anim=StartCoroutine(AnimateSkin(skin));}
            if(animSpr!=null)spr=animSpr;
        }
        return spr;
    }
    Coroutine anim;int iAnim=0;Sprite animSpr;
    IEnumerator AnimateSkin(CstmzSkin skin){Sprite spr;
        if(skin.animSpeed>0){yield return new WaitForSeconds(skin.animSpeed);}
        else{yield return new WaitForSeconds(skin.animVals[iAnim].delay);}
        spr=skin.animVals[iAnim].spr;
        if(iAnim==skin.animVals.Count-1)iAnim=0;
        if(iAnim<skin.animVals.Count)iAnim++;
        animSpr=spr;
        if(skinName==skin.name)anim=StartCoroutine(AnimateSkin(skin));
        else{if(anim!=null)StopCoroutine(anim);anim=null;iAnim=0;}
    }
    public Sprite GetOverlaySprite(string str){Sprite spr=null;
        if(str.Contains("_")){spr=AssetsManager.instance.GetSkin(str.Split('_')[0]).variants[int.Parse(str.Split('_')[1])].sprOverlay;}
        else{spr=AssetsManager.instance.GetSkin(str).sprOverlay;}
    return spr;}
    void SetSkin(string str){if(spr!=null){if(spr.sprite!=GetSkinSprite(str))spr.sprite=GetSkinSprite(str);}
        else if(img!=null){if(img.sprite!=GetSkinSprite(str))img.sprite=GetSkinSprite(str);}}
    void SetOverlay(Sprite sprite, Color color){
        Color _color=Color.white;if(color!=Color.clear){_color=color;}
        if(overlaySpr!=null){
            if(overlaySpr.sprite!=sprite)overlaySpr.sprite=sprite;
            if(overlaySpr.color!=_color)overlaySpr.color=_color;
        }else if(overlayImg!=null){
            if(overlayImg.sprite!=sprite)overlayImg.sprite=sprite;
            if(overlayImg.color!=_color)overlayImg.color=_color;
        }
    }
    void SetTrail(string str){
        if(GetComponent<TrailVFX>()!=null){GetComponent<TrailVFX>().SetNewTrail(str,true);}
        /*else{if(trailObj!=null){if(trailObjPos==Vector2.zero){trailObjPos=trailObj.transform.localPosition;}
        if(AssetsManager.instance.GetTrail(str)!=null){if(!trailObj.name.Contains(AssetsManager.instance.GetTrail(str).part.name)){
            var _tempTrailObj=trailObj;trailObj=Instantiate(AssetsManager.instance.GetTrail(str).part,transform);trailObj.transform.localPosition=trailObjPos;Destroy(_tempTrailObj);
            AssetsManager.instance.TransformIntoUIParticle(trailObj);
        }}}}*/
    }
    public GameObject GetFlareVFX(){GameObject go=AssetsManager.instance.GetVFX("FlareShoot");if(AssetsManager.instance.GetFlares(flaresName)!=null){go=AssetsManager.instance.GetFlareRandom(flaresName);}return go;}
    public CstmzDeathFx GetDeathFx(){return AssetsManager.instance.GetDeathFx(deathFxName);}
    public GameObject GetDeathFxObj(){GameObject go=AssetsManager.instance.GetVFX("Explosion");if(AssetsManager.instance.GetDeathFx(deathFxName)!=null){go=AssetsManager.instance.GetDeathFx(deathFxName).obj;}return go;}


    void LoadValues(){
        skinName=SaveSerial.instance.playerData.skinName;
        overlayColor=SaveSerial.instance.playerData.overlayColor;
        trailName=SaveSerial.instance.playerData.trailName;
        flaresName=SaveSerial.instance.playerData.flaresName;
        deathFxName=SaveSerial.instance.playerData.deathFxName;
    }
    async void LoadValuesFromSelectedUsersData(){
        SSSCustomizationData cstmzData=await DBAccess.instance.GetUsersCustomizationData(GameManager.instance.GetSelectedUsersDataName());
        skinName=cstmzData.customizationData[0];
        trailName=cstmzData.customizationData[1];
        flaresName=cstmzData.customizationData[2];
        deathFxName=cstmzData.customizationData[3];
        overlayColor=Color.HSVToRGB(cstmzData.overlayColors[0],cstmzData.overlayColors[1],cstmzData.overlayColors[2]);
    }
}
