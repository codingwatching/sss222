﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

public class Enemy : MonoBehaviour{
    [Header("Enemy")]
    [SerializeField] public string Name;
    [SerializeField] public enemyType type;
    [SerializeField] public Vector2 size=Vector2.one;
    [DisableInEditorMode] public float sizeAvg=1;
    [SerializeField] public string sprAsset;
    [DisableInEditorMode] public Sprite spr;
    [SerializeField] public ShaderMatProps sprMatProps;
    public float health=100f;
    public float healthMax=100f;
    public int defense=0;
    public bool defenseOnPhase=true;
    public bool healthBySize=false;
    [SerializeField] public bool shooting=false;
    [SerializeField] public string bulletAssetName;
    [SerializeField] public Vector2 shootTime=new Vector2(1.75f,2.8f);
    [ReadOnly] public float shootTimer;
    [SerializeField] public float bulletSpeed=8f;
    [SerializeField] public bool DBullets=false;
    [SerializeField] public float bulletDist=0.35f;
    [SerializeField] public bool randomizeWaveDeath=false;
    [SerializeField] public bool flyOff=false;
    [SerializeField] public bool killOnDash=true;
    [Header("Drops & Points")]
    [SerializeField] public bool giveScore=true;
    [ShowIf("giveScore")][SerializeField] public Vector2 scoreValue=new Vector2(1,10);
    [SerializeField] public float xpChance=100f;
    [EnableIf("@this.xpChance>0")][SerializeField] public float xpAmnt=0f;
    [EnableIf("@this.xpChance>0")][SerializeField] public bool accumulateXp=true;
    [SerializeField] public List<LootTableEntryDrops> drops;
    [ReadOnly]public List<float> dropValues;

    [Header("Others")]
    [SerializeField] public bool destroyOut=true;
    [ReadOnly]public bool yeeted=false;
    [ReadOnly]public bool dmgCounted;
    [ReadOnly]public float dmgCount;
    [ReadOnly]public bool _dmgHeals;
    GameObject dmgCountPopup;

    [ReadOnly]public GameObject bullet;
    Rigidbody2D rb;
    SpriteRenderer sprRender;

    void Awake(){
        if(GetComponent<SpriteRenderer>()!=null){sprRender=GetComponent<SpriteRenderer>();}
        else{if(transform.GetChild(0)!=null){sprRender=transform.GetChild(0).GetComponent<SpriteRenderer>();}}
        if(GameManager.maskMode!=0){
            if(sprRender!=null)sprRender.maskInteraction=(SpriteMaskInteraction)GameManager.maskMode;
            else{if(transform.GetChild(0)!=null)transform.GetChild(0).GetComponent<SpriteRenderer>().maskInteraction=(SpriteMaskInteraction)GameManager.maskMode;}
        }
        StartCoroutine(SetValues());
        if(GetComponent<BossAI>()!=null&&GameRules.instance.bossInfo.scaleUpOnSpawn){size=Vector2.zero;}
    }
    IEnumerator SetValues(){
        //drops=(LootTableDrops)gameObject.AddComponent(typeof(LootTableDrops));
        yield return new WaitForSeconds(0.01f);
        var i=GameRules.instance;
        if(i!=null){
        EnemyClass e=null;
        e=System.Array.Find(i.enemies,x=>x.name==Name);
        if(e!=null){
            type=e.type;
            size=e.size;
            sprAsset=e.sprAsset;sprMatProps=e.sprMatProps;
            health=e.healthStart;healthMax=e.healthMax;
            healthBySize=e.healthBySize;
            defense=e.defense;
            shooting=e.shooting;
            shootTime=e.shootTime;
            bulletAssetName=e.bulletAssetName;
            bulletSpeed=e.bulletSpeed;
            DBullets=e.DBullets;
            bulletDist=e.bulletDist;
            randomizeWaveDeath=e.randomizeWaveDeath;
            flyOff=e.flyOff;
            killOnDash=e.killOnDash;
            destroyOut=e.destroyOut;
            
            giveScore=e.giveScore;
            scoreValue=e.scoreValue;
            xpChance=e.xpChance;
            xpAmnt=e.xpAmnt;
            accumulateXp=e.accumulateXp;
            drops=e.drops;
        }
        if(GetComponent<Goblin>()!=null||GetComponent<VortexWheel>()!=null/*||GetComponent<HealingDrone>()!=null*/)shooting=false;
        if(GetComponent<HealingDrone>()!=null){if(!GetComponent<HealingDrone>().enabled)GetComponent<HealingDrone>().enabled=true;}
    
            yield return new WaitForSeconds(0.04f);
            if(drops!=null&&drops.Count>0){
                for(var d=0;d<drops.Count;d++){dropValues.Add(drops[d].dropChance);}
                dropValues[0]*=GameManager.instance.enballDropMulti;
                dropValues[1]*=GameManager.instance.coinDropMulti;
                dropValues[2]*=GameManager.instance.coreDropMulti;

                for(var d=0;d<dropValues.Count;d++){
                    if(Random.Range(1,101)<=dropValues[d]&&dropValues[d]!=0){dropValues[d]=101;}
                }
                if(!GameRules.instance.energyOnPlayer)dropValues[0]=0;
                if(!GameRules.instance.crystalsOn)dropValues[1]=0;
                if(!GameRules.instance.coresOn&&!GameManager.instance.CheckGamemodeSelected("Classic"))dropValues[2]=0;
                if(GameManager.instance.CheckGamemodeSelected("Classic")){
                    drops[2].name="Starshard";drops[2].ammount=Vector2.one;drops[2].dropChance=15f;
                    if(!drops.Exists(x=>x.name=="Starshard")){drops.Add(new LootTableEntryDrops{name="Starshard",ammount=Vector2.one,dropChance=15f});}
                }
            }
        }

        bullet=AssetsManager.instance.GetEnemyBullet(bulletAssetName);
    }
    IEnumerator Start(){
        rb=GetComponent<Rigidbody2D>();
        if(GetComponent<Tag_PauseVelocity>()==null){gameObject.AddComponent<Tag_PauseVelocity>();}
        if(GetComponent<CometRandomProperties>()==null)if(spr!=AssetsManager.instance.SprAnyReverse(sprAsset))spr=AssetsManager.instance.SprAnyReverse(sprAsset);

        if(shooting)shootTimer=Random.Range(shootTime.x,shootTime.y);
        if(healthBySize){healthMax=Mathf.RoundToInt(healthMax*sizeAvg);health=Mathf.RoundToInt(health*sizeAvg);}

        yield return new WaitForSeconds(0.02f);
        if(GetComponent<CometRandomProperties>()==null)if(spr!=AssetsManager.instance.SprAnyReverse(sprAsset))spr=AssetsManager.instance.SprAnyReverse(sprAsset);
    }
    void Update(){
        if(shooting){Shoot();}
        if(flyOff){FlyOff();}
        Die();
        if(destroyOut)DestroyOutside();
        DispDmgCountUp();

        health=Mathf.Clamp(health,-1000,healthMax);

        if((Vector2)transform.localScale!=size)transform.localScale=size;
        if(sizeAvg!=(size.x+size.y)/2)sizeAvg=(size.x+size.y)/2;
        if(sprRender.sprite!=spr)sprRender.sprite=spr;
        if(sprMatProps!=null){sprRender.material=AssetsManager.instance.UpdateShaderMatProps(sprRender.material,sprMatProps);}
    }
    
    void Shoot(){   if(!GameManager.GlobalTimeIsPaused){
        shootTimer-=Time.deltaTime;
        if(shootTimer<=0f){
        if(GetComponent<LaunchRadialBullets>()==null&&GetComponent<LaunchSwarmBullets>()==null&&GetComponent<HealingDrone>()==null){
            if(bullet!=null){
                if(DBullets!=true){
                    var bt=Instantiate(bullet,transform.position,Quaternion.identity) as GameObject;
                    bt.GetComponent<Rigidbody2D>().velocity=new Vector2(0,-bulletSpeed);
                    if(bt.GetComponent<Tag_PauseVelocity>()==null)bt.AddComponent<Tag_PauseVelocity>();
                }else{
                    var pos1=new Vector2(transform.position.x+bulletDist,transform.position.y);
                    var bt1=Instantiate(bullet,pos1,Quaternion.identity) as GameObject;
                    bt1.GetComponent<Rigidbody2D>().velocity=new Vector2(0,-bulletSpeed);
                    if(bt1.GetComponent<Tag_PauseVelocity>()==null)bt1.AddComponent<Tag_PauseVelocity>();
                    var pos2=new Vector2(transform.position.x - bulletDist, transform.position.y);
                    var bt2=Instantiate(bullet,pos2,Quaternion.identity) as GameObject;
                    bt2.GetComponent<Rigidbody2D>().velocity=new Vector2(0,-bulletSpeed);
                    if(bt2.GetComponent<Tag_PauseVelocity>()==null)bt2.AddComponent<Tag_PauseVelocity>();
                }
            }else{Debug.LogWarning("Bullet not asigned");}
        }else if(GetComponent<LaunchRadialBullets>()!=null){GetComponent<LaunchRadialBullets>().Shoot();}
        else if(GetComponent<LaunchSwarmBullets>()!=null){GetComponent<LaunchSwarmBullets>().Shoot();}
        shootTimer=Random.Range(shootTime.x, shootTime.y);
        }
    }}
    void FlyOff(){
        if(Player.instance==null){
            shooting=false;
            rb.velocity=new Vector2(0,3f);
        }
    }
    bool dead;
    public void Die(bool explode=true){if(health<=0&&health!=-1000){if(!dead){
        if(GetComponent<BossAI>()==null){
            GameManager.instance.AddEnemyCount();
            StatsAchievsManager.instance.AddKills(Name,type);
            int score=Random.Range((int)scoreValue.x,(int)scoreValue.y+1);
            if(GetComponent<CometRandomProperties>()!=null){
                var comet=GetComponent<CometRandomProperties>();
                if(comet.scoreBySize){
                    for(var i=0;i<comet.scoreSizes.Length&&(comet.size>comet.scoreSizes[i].size&&(i+1<comet.scoreSizes.Length&&comet.size<comet.scoreSizes[i+1].size));i++){score=comet.scoreSizes[i].score;}
                }
                if(comet.isLunar){
                    var lunarScore=comet.LunarScore();
                    if(lunarScore!=-1)score=lunarScore;
                    comet.LunarDrop();
                }
            }
            if(giveScore==true)GameManager.instance.AddToScore(score);
        }
        if(GameRules.instance.xpOn)if(xpAmnt!=0&&xpChance>=Random.Range(0f,100f)){
            if(accumulateXp){
                if(xpAmnt/5>=1){for(var i=0;i<(int)(xpAmnt/5);i++){AssetsManager.instance.Make("CelestVial",transform.position);}}
                GameManager.instance.DropXP(xpAmnt%5,transform.position);
            }else{GameManager.instance.DropXP(xpAmnt,transform.position);}
        }
        else{GameManager.instance.AddXP(xpAmnt);}
        giveScore=false;
        if(GameManager.instance.zoneToTravelTo!=-1){
            var cutTime=CoreSetup.instance.adventureTravelZonePrefab.killSubTravelTime;
            if(Player.instance!=null){if(Player.instance.GetComponent<PlayerModules>()!=null)if(Player.instance.GetComponent<PlayerModules>()._isModuleEquipped("STraveler"))cutTime=CoreSetup.instance.adventureTravelZonePrefab.killSubTravelTime*1.33f;}
            GameManager.instance.gameTimeLeft-=CoreSetup.instance.adventureTravelZonePrefab.killSubTravelTime;
            if(GameRules.instance.scoreDisplay==scoreDisplay.timeLeft)GameCanvas.instance.ScorePopupSwitch(-CoreSetup.instance.adventureTravelZonePrefab.killSubTravelTime);
        }


        List<LootTableEntryDrops> ld=drops;
        for(var i=0;i<ld.Count;i++){//Drop items
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
        if(GetComponent<BossAI>()==null){
            if(GetComponent<Goblin>()!=null){GetComponent<Goblin>().DropPowerup(true);if(GetComponent<Goblin>().bossForm){GetComponent<Goblin>().GoblinBossDrop();}}
            
            if(explode){AudioManager.instance.Play("Explosion");GameObject explosion=AssetsManager.instance.VFX("Explosion",transform.position,0.5f);Destroy(gameObject);}
            Shake.instance.CamShake(2,1);
        }
        if(GetComponent<BossAI>()!=null){GetComponent<BossAI>().Die();}
        dead=true;
    }}}
    void OnDestroy(){
        if(GetComponent<Goblin>()!=null)GetComponent<Goblin>().DropPowerup(false);
        if(GetComponent<EnCombatant>()!=null)Destroy(GetComponent<EnCombatant>().saber.gameObject);
        if(randomizeWaveDeath==true){spawnReqsMono.AddScore(-5,-1);}
    }
    void DestroyOutside(){
        if((transform.position.x>6.5f || transform.position.x<-6.5f) || (transform.position.y>10f || transform.position.y<-10f)){if(yeeted==true){giveScore=true;health=-1;Die();}else{Destroy(gameObject,0.001f);if(GetComponent<Goblin>()!=null){foreach(GameObject obj in GetComponent<Goblin>().powerups)Destroy(obj);}}}
    }
    public void Kill(bool giveScore=true,bool explode=true){
        this.giveScore=giveScore;
        health=-1;
        Die(explode);
    }
    public void Damage(float dmg){health-=dmg;}
    public void SetSpr(Sprite _spr){spr=_spr;}
    public void SetSprStr(string str){spr=AssetsManager.instance.SprAnyReverse(str);}
    //Collisions in EnemyCollider
    public void DispDmgCount(Vector2 pos){if(SaveSerial.instance.settingsData.dmgPopups)StartCoroutine(DispDmgCountI(pos));}
    IEnumerator DispDmgCountI(Vector2 pos){
        dmgCounted=true;
        //In Update, DispDmgCountUp
        dmgCountPopup=WorldCanvas.instance.DMGPopup(0,pos,ColorInt32.Int2Color(ColorInt32.dmgColor));
        yield return new WaitForSeconds(0.2f);
        dmgCounted=false;
        dmgCount=0;
    }
    void DispDmgCountUp(){if(dmgCountPopup!=null)dmgCountPopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=System.Math.Round(dmgCount,1).ToString();}

    public bool _healable(){if(type==enemyType.living||type==enemyType.mecha){return true;}else{return false;}}
}

public enum enemyType{living,mecha,other}