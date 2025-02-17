﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CometRandomProperties : MonoBehaviour{
    [Header("Basic")]
    [SerializeField] Vector2 sizes=new Vector2(0.4f,1.4f);
    [DisableInEditorMode]public float size=1;
    [SerializeField] public bool scoreBySize=false;
    [SerializeField] public CometScoreSize[] scoreSizes;
    [SerializeField] string[] sprites;
    [Header("Lunar")]
    [SerializeField] Vector2 sizeMultLunar=new Vector2(0.88f,1.55f);
    [SerializeField] int lunarCometChance=10;
    [SerializeField] float lunarHealthMulti=2.5f;
    [SerializeField] float lunarSpeedMulti=0.415f;
    [SerializeField] Vector2 lunarScore;
    [SerializeField] public Vector2 lunarCrystalAmmounts;
    [SerializeField] public List<LootTableEntryDrops> lunarDrops;
    public List<float> dropValues;
    [SerializeField] string[] spritesLunar;
    [SerializeField] string lunarPart="Trail_Lunar";
    [DisableInEditorMode]public int healhitCount;
    [DisableInEditorMode]public bool isLunar;

    Enemy en;
    Rigidbody2D rb;
    TrailVFX trail;
    float rotationSpeed=1;

    void Awake(){StartCoroutine(SetValues());}
    IEnumerator SetValues(){
        //lunarDrops=(LootTableDrops)gameObject.AddComponent(typeof(LootTableDrops));
        yield return new WaitForSeconds(0.02f);
        var i=GameRules.instance;
        if(i!=null){
            var c=i.cometSettings;
            sizes=c.sizes;
            scoreBySize=c.scoreBySize;
            scoreSizes=c.scoreSizes;
            sprites=c.sprites;

            sizeMultLunar=c.sizeMultLunar;
            lunarCometChance=c.lunarCometChance;
            lunarHealthMulti=c.lunarHealthMulti;
            lunarSpeedMulti=c.lunarSpeedMulti;
            lunarScore=c.lunarScore;
            lunarDrops=c.lunarDrops;

            spritesLunar=c.spritesLunar;
            lunarPart=c.lunarPart;
        }
        for(var d=0;d<lunarDrops.Count;d++){dropValues.Add(lunarDrops[d].dropChance);}
        for(var d=0;d<dropValues.Count;d++){if(Random.Range(1,101)<=dropValues[d]&&dropValues[d]!=0){dropValues[d]=101;}}
    }
    IEnumerator Start(){
        en=GetComponent<Enemy>();
        rb=GetComponent<Rigidbody2D>();
        trail=GetComponent<TrailVFX>();

        yield return new WaitForSeconds(0.03f);
        int spriteIndex=Random.Range(0, sprites.Length);
        en.SetSprStr(sprites[spriteIndex]);
        size=(float)System.Math.Round(Random.Range(sizes.x, sizes.y),2);
        en.size=new Vector2(en.size.x*size,en.size.y*size);

        if(Random.Range(0,100)<lunarCometChance)MakeLunar();
        rotationSpeed=Random.Range(2.8f,4.7f)*(GetComponent<Rigidbody2D>().velocity.y*-1);
    }
    public int LunarScore(){return Random.Range((int)lunarScore.x,(int)lunarScore.y);}
    bool _ambiencePlayed=false;
    void Update(){
        if(!GameManager.GlobalTimeIsPaused){
            if(healhitCount>=3&&!isLunar){MakeLunar();}
            if(transform.GetChild(0)!=null){
                float step=rotationSpeed*Time.deltaTime;
                transform.GetChild(0).Rotate(new Vector3(0,0,step));
            }
            if(Player.instance!=null){
                var _dist=(float)System.Math.Round(Vector2.Distance(Player.instance.transform.position,transform.position),2);var _minDist=2.2f;
                if(_dist<=_minDist&&!GetComponent<AudioSource>().isPlaying&&!_ambiencePlayed){var _volume=Mathf.Abs(1-((_minDist-_dist)*2));GetComponent<AudioSource>().volume=_volume;GetComponent<AudioSource>().Play();_ambiencePlayed=true;}//Debug.Log("Playing Comet ambience at volume: "+_volume+" at distance: "+_dist);}
                /*var _distX=(float)System.Math.Round(Vector2.Distance(new Vector2(Player.instance.transform.position.x,0),new Vector2(transform.position.x,0)),2);var _minDistX=2.2f;
                var _distY=(float)System.Math.Round(Vector2.Distance(new Vector2(0,Player.instance.transform.position.y),new Vector2(0,transform.position.y)),2);var _minDistY=2.2f;
                    var _volume=1f;//Mathf.Abs(1-((_minDist-_dist)*2));
                    if(_distX<=_minDistX&&!GetComponent<AudioSource>().isPlaying){_volume=Mathf.Abs(1-((_minDistX-_distX)*2));GetComponent<AudioSource>().Play();Debug.Log("Playing Comet ambience at volume: "+_volume);_ambiencePlayed=true;}else{_volume=0f;}//+" at distance: "+_dist);}
                    if(transform.position.y<Player.instance.transform.position.y){_volume=Mathf.Abs(1-((_minDistY-_distY)*2));}
                    GetComponent<AudioSource>().volume=_volume;*/
            }
        }

        
    }
    [ContextMenu("MakeLunar")][Button("Make Lunar")]
    public void MakeLunar(){isLunar=true;TransformIntoLunar();}
    void TransformIntoLunar(){
        int spriteIndex=Random.Range(0,spritesLunar.Length);
        en.SetSprStr(spritesLunar[spriteIndex]);
        if(trail!=null){trail.SetNewTrail(lunarPart);}

        float sizeL=(float)System.Math.Round(Random.Range(sizeMultLunar.x, sizeMultLunar.y),2);
        en.size=new Vector2(en.size.x*sizeL, en.size.y*sizeL);
        en.healthMax*=lunarHealthMulti;en.health*=lunarHealthMulti;
        rb.velocity*=lunarSpeedMulti;
        if(!GameRules.instance.crystalsOn)dropValues[0]=102;
    }

    public void LunarDrop(){
        List<LootTableEntryDrops> ld=lunarDrops;
        for(var i=0;i<ld.Count;i++){
            string st=ld[i].name;
            if(dropValues.Count>=ld.Count){
            if(dropValues[i]==101){
            var amnt=Random.Range((int)ld[i].ammount.x,(int)ld[i].ammount.y);
            if(amnt!=0){
                if(!st.Contains("Coin")){
                    if(amnt==1)AssetsManager.instance.Make(st,transform.position);
                    else{AssetsManager.instance.MakeSpread(st,transform.position,amnt);}
                }else{//Drop Lunar Crystals
                    if(amnt/GameRules.instance.crystalBigGain>=1){for(var c=0;c<(int)(amnt/GameRules.instance.crystalBigGain);c++){AssetsManager.instance.MakeSpread("CoinB",transform.position,1);}}
                    AssetsManager.instance.MakeSpread("Coin",transform.position,(amnt%GameRules.instance.crystalBigGain)/GameRules.instance.crystalGain);//CrystalB=6, CrystalS=2
                }
            }}}
        }
    }
}
[System.Serializable]public class CometScoreSize{
    public float size;
    public int score;
}