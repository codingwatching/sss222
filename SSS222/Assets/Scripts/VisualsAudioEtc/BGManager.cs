using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class BGManager : MonoBehaviour{
    ShaderMatProps _shaderMatPropsDef=new ShaderMatProps();
    [DisableInEditorMode][SerializeField]ShaderMatProps shaderMatProps;
    [DisableInEditorMode][SerializeField]Material material;
    [SerializeField]Material _materialDef;
    [SerializeField]Texture2D text;
    [SerializeField]Texture2D textMiddle;
    [DisableInPlayMode][SerializeField]bool setOnValidate;
    ShaderMatProps travelingShaderMatProps;
    IEnumerator Start(){
        /*if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name=="Game"){*/yield return new WaitForSecondsRealtime(0.075f);//}else{}

        SetStartingProperties();
        if(SceneManager.GetActiveScene().name!="Loading"){
            if(GameRules.instance!=null){
                if(GameRules.instance.bgMaterial!=null){SetMatProps(GameRules.instance.bgMaterial);}
                else{SetMatProps(_shaderMatPropsDef);}
            }else{SetMatProps(_shaderMatPropsDef);}
        }
        setOnValidate=true;//?

        travelingShaderMatProps=_shaderMatPropsDef;
    }
    void OnValidate(){if(setOnValidate){
        SetStartingProperties();
    }}
    void SetStartingProperties(){
        _shaderMatPropsDef.text=text;
        shaderMatProps.text=text;
        if(_materialDef==null)_materialDef=GetBgMat();
        if(material==null&&_materialDef!=null)material=_materialDef;
        SetMatProps(_shaderMatPropsDef);
    }
    float _deltaTime;
    void Update(){
        if(SceneManager.GetActiveScene().name=="SandboxMode"){if(GameRules.instance.bgMaterial!=null){SetMatProps(GameRules.instance.bgMaterial);}}
        if(SceneManager.GetActiveScene().name=="Game"&&GameManager.instance.zoneToTravelTo!=-1){
            var curMat=CoreSetup.instance.adventureZones[GameManager.instance.zoneSelected].gameRules.bgMaterial;
            var targMat=CoreSetup.instance.adventureZones[GameManager.instance.zoneToTravelTo].gameRules.bgMaterial;
            var vel=0f;var smoothTime=GameManager.instance.NormalizedZoneTravelTimeLeft()/10;
                //(GameManager.instance.CalcZoneTravelTime()/2);
                //Mathf.Round(GameManager.instance.gameTimeLeft)/100;
            if(Time.deltaTime>0){_deltaTime=Time.deltaTime;}var maxSpeed=Mathf.Infinity;

            TransitionBackgroundMats(ref travelingShaderMatProps, curMat, targMat, ref vel, smoothTime, maxSpeed, _deltaTime);

            //travelingShaderMatProps.hue=(CoreSetup.instance.adventureZones[GameManager.instance.zoneToTravelTo].gameRules.bgMaterial.hue-CoreSetup.instance.adventureZones[GameManager.instance.zoneSelected].gameRules.bgMaterial.hue)*Time.deltaTime;
            /*travelingShaderMatProps.hue=AssetsManager.Normalize(
                GameManager.instance.gameTimeLeft/GameManager.instance.CalcZoneTravelTime(),
                CoreSetup.instance.adventureZones[GameManager.instance.zoneSelected].gameRules.bgMaterial.hue,
                CoreSetup.instance.adventureZones[GameManager.instance.zoneToTravelTo].gameRules.bgMaterial.hue
            );*/
            SetMatProps(travelingShaderMatProps);
        }
    }
    public static void TransitionBackgroundMats(ref ShaderMatProps shaderMatProps, ShaderMatProps curMat, ShaderMatProps targMat, ref float vel, float smoothTime, float maxSpeed, float _deltaTime){
        var startHue=curMat.hue;var targHue=targMat.hue;
        if(startHue==0&&targHue>0.5f){startHue=1;}else if(startHue>0.5f&&targHue==0){targHue=1;}
        shaderMatProps.hue=Mathf.SmoothDamp(startHue, targHue, ref vel, smoothTime, maxSpeed, _deltaTime);

        var startSat=curMat.saturation;var targSat=targMat.saturation;
        if(startSat==0&&targSat>0.5f){startSat=1;}else if(startSat>0.5f&&targSat==0){targSat=1;}
        shaderMatProps.saturation=Mathf.SmoothDamp(startSat, targSat, ref vel, smoothTime, maxSpeed, _deltaTime);

        var startVal=curMat.value;var targVal=targMat.value;
        if(startVal==0&&targVal>0.5f){startVal=1;}else if(startVal>0.5f&&targVal==0){targVal=1;}
        shaderMatProps.value=Mathf.SmoothDamp(startVal, targVal, ref vel, smoothTime, maxSpeed, _deltaTime);
    }
    public void SetMatProps(ShaderMatProps mat){shaderMatProps=mat;if(material!=null){material=AssetsManager.instance.UpdateShaderMatProps(material,shaderMatProps);}UpdateMaterials();}
    [Button]public void UpdateMaterials(){foreach(Tag_BGColor t in transform.GetComponentsInChildren<Tag_BGColor>()){if(material!=null)t.GetComponent<Renderer>().sharedMaterial=material;}}
    public Texture2D GetBgTexture(){return text;}
    public Texture2D GetBgTextureMiddle(){return textMiddle;}
    public Material GetBgMat(){return transform.GetComponentInChildren<Tag_BGColor>().GetComponent<Renderer>().sharedMaterial;}
}