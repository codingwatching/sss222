﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerCollider : MonoBehaviour{
    [Header("Powerups")]
    GameObject CoinPrefab;
    GameObject enBallPrefab;
    GameObject powercorePrefab;
    GameObject armorPwrupPrefab;
    GameObject armorUPwrupPrefab;
    GameObject laser2PwrupPrefab;
    GameObject laser3PwrupPrefab;
    GameObject phaserPwrupPrefab;
    GameObject hrocketPwrupPrefab;
    GameObject mlaserPwrupPrefab;
    GameObject lsaberPwrupPrefab;
    GameObject lclawsPwrupPrefab;
    GameObject flipPwrupPrefab;
    GameObject gcloverPwrupPrefab;
    GameObject shadowPwrupPrefab;
    GameObject shadowBTPwrupPrefab;
    GameObject qrocketPwrupPrefab;
    GameObject procketPwrupPrefab;
    GameObject cstreamPwrupPrefab;
    GameObject inverterPwrupPrefab;
    GameObject magnetPwrupPrefab;
    GameObject scalerPwrupPrefab;
    GameObject matrixPwrupPrefab;
    GameObject pmultiPwrupPrefab;
    GameObject randomizerPwrupPrefab;
    GameObject acceleratorPwrupPrefab;
    GameObject plaserPwrupPrefab;
    #region
    [Header("Damage Dealers")]
    GameObject cometPrefab;
    GameObject batPrefab;
    GameObject enShip1Prefab;
    GameObject enCombatantPrefab;
    GameObject enSaberPrefab;
    GameObject soundwavePrefab;
    GameObject EBtPrefab;
    GameObject leechPrefab;
    GameObject hlaserPrefab;
    GameObject goblinPrefab;
    GameObject hdronePrefab;
    GameObject vortexPrefab;
    GameObject stingerPrefab;
    GameObject goblinbtPrefab;
    GameObject glaredevPrefab;
    [Header("Other")]
    [SerializeField] public GameObject dmgPopupPrefab;
    [SerializeField] float dmgFreq=0.38f;
    public float dmgTimer;
    public string lastHitObj;
    public float lastHitDmg;

    Player player;
    GameSession gameSession;
    // Start is called before the first frame update
    #endregion
    void Start()
    {
        player = FindObjectOfType<Player>();
        gameSession = FindObjectOfType<GameSession>();

        SetPrefabs();
    }

    void SetPrefabs(){
        CoinPrefab=GameAssets.instance.Get("Coin");
        enBallPrefab=GameAssets.instance.Get("EnBall");
        powercorePrefab=GameAssets.instance.Get("PowerCore");
        armorPwrupPrefab=GameAssets.instance.Get("ArmorPwrup");
        armorUPwrupPrefab=GameAssets.instance.Get("ArmorUPwrup");
        laser2PwrupPrefab=GameAssets.instance.Get("Laser2Pwrup");
        laser3PwrupPrefab=GameAssets.instance.Get("Laser3Pwrup");
        phaserPwrupPrefab=GameAssets.instance.Get("PhaserPwrup");
        hrocketPwrupPrefab=GameAssets.instance.Get("HRocketPwrup");
        mlaserPwrupPrefab=GameAssets.instance.Get("MLaserPwrup");
        lsaberPwrupPrefab=GameAssets.instance.Get("LSaberPwrup");
        lclawsPwrupPrefab=GameAssets.instance.Get("LClawsPwrup");
        flipPwrupPrefab=GameAssets.instance.Get("FlipPwrup");
        gcloverPwrupPrefab=GameAssets.instance.Get("GCloverPwrup");
        shadowPwrupPrefab=GameAssets.instance.Get("ShadowPwrup");
        shadowBTPwrupPrefab=GameAssets.instance.Get("ShadowBtPwrup");
        qrocketPwrupPrefab=GameAssets.instance.Get("QRocketPwrup");
        procketPwrupPrefab=GameAssets.instance.Get("PRocketPwrup");
        cstreamPwrupPrefab=GameAssets.instance.Get("CStreamPwrup");
        inverterPwrupPrefab=GameAssets.instance.Get("InverterPwrup");
        magnetPwrupPrefab=GameAssets.instance.Get("MagnetPwrup");
        scalerPwrupPrefab=GameAssets.instance.Get("ScalerPwrup");
        matrixPwrupPrefab=GameAssets.instance.Get("MatrixPwrup");
        pmultiPwrupPrefab=GameAssets.instance.Get("PMultiPwrup");
        randomizerPwrupPrefab=GameAssets.instance.Get("RandomizerPwrup");
        acceleratorPwrupPrefab=GameAssets.instance.Get("AcceleratorPwrup");
        plaserPwrupPrefab=GameAssets.instance.Get("PLaserPwrup");

        cometPrefab=GameAssets.instance.Get("Comet");
        batPrefab=GameAssets.instance.Get("Bat");
        soundwavePrefab=GameAssets.instance.Get("Soundwave");
        enShip1Prefab=GameAssets.instance.Get("EnShip");
        EBtPrefab=GameAssets.instance.Get("EnBt");
        enCombatantPrefab=GameAssets.instance.Get("EnComb");
        enSaberPrefab=GameAssets.instance.Get("EnSaber");
        leechPrefab=GameAssets.instance.Get("Leech");
        hlaserPrefab=GameAssets.instance.Get("HLaser");
        goblinPrefab=GameAssets.instance.Get("Goblin");
        hdronePrefab=GameAssets.instance.Get("HDrone");
        vortexPrefab=GameAssets.instance.Get("Vortex");
        stingerPrefab=GameAssets.instance.Get("Stinger");
        goblinbtPrefab=GameAssets.instance.Get("GoblinBt");
        glaredevPrefab=GameAssets.instance.Get("GlareDevil");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SetPrefabs();
        if (!other.CompareTag(tag))
        {
            DamageDealer damageDealer = other.GetComponent<DamageDealer>();
            DamageValues damageValues=DamageValues.instance;
            //ifif(!damageDealer||!damageValues){Debug.LogWarning("No DamageDealer component or DamageValues instance");return;}

            if(other.GetComponent<Tag_OutsideZone>()!=null){player.Hack(1f);player.Damage(damageValues.GetDmgZone(),dmgType.silent);}
            #region//Enemies
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyBullet"))
            {
                bool en = false;
                bool destroy = true;
                float dmg = 0;

                var cometName = cometPrefab.name; var cometName1 = cometPrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(cometName)) { if(other.GetComponent<CometRandomProperties>()!=null){if(other.GetComponent<CometRandomProperties>().damageBySpeedSize){dmg=(float)System.Math.Round(damageValues.GetDmgComet()*Mathf.Abs(other.GetComponent<Rigidbody2D>().velocity.y)*other.transform.localScale.x,1);}else{dmg=damageValues.GetDmgComet();}}else{dmg=damageValues.GetDmgComet();} en = true; }

                var batName = batPrefab.name; var batName1 = batPrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(batName)) { dmg = damageValues.GetDmgBat(); en = true; }
                var Sname = soundwavePrefab.name; var Sname1 = soundwavePrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(Sname)) { dmg = damageValues.GetDmgSoundwave(); AudioManager.instance.Play("SoundwaveHit"); }

                var enShip1Name = enShip1Prefab.name; var enShip1Name1 = enShip1Prefab.name + "(Clone)";
                if (other.gameObject.name.Contains(enShip1Name)) { dmg = damageValues.GetDmgEnemyShip1(); en = true; }
                var EBtname = EBtPrefab.name; var EBtname1 = EBtPrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(EBtname)) { dmg = damageValues.GetDmgEBt();}

                var enCombatantName = enCombatantPrefab.name; var enCombatantName1 = enCombatantPrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(enCombatantName)) { en = true; destroy=false; }
                var enSaberName = enSaberPrefab.name; var enSaberName1 = enSaberPrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(enSaberName)) { dmg = damageValues.GetDmgEnSaber(); en = false; destroy=false; }

                var goblinName = goblinPrefab.name; var goblinName1 = goblinPrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(goblinName)) { dmg = damageValues.GetDmgGoblin(); en = true; }

                var hdroneName = hdronePrefab.name; var hdroneName1 = hdronePrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(hdroneName)) { dmg = damageValues.GetDmgHealDrone(); en = true; }

                var vortexName = vortexPrefab.name; var vortexName1 = vortexPrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(vortexName)) { dmg = damageValues.GetDmgVortex(); en = true; }
                if(other.gameObject.name.Contains("StickBomb")) { dmg=0; en = false; destroy=false; }

                var stingerName = stingerPrefab.name; var stingerName1 = stingerPrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(stingerName)) { dmg = damageValues.GetDmgStinger(); player.Weaken(damageValues.GetEfxStinger().x,damageValues.GetEfxStinger().y); en = true; }

                var leechName = leechPrefab.name; var leechName1 = leechPrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(leechName)) { en = true;  destroy = false; }
        
                var hlaserName = hlaserPrefab.name; var hlaserName1 = hlaserPrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(hlaserName)) { destroy = false; }

                var goblinbtName = goblinbtPrefab.name; var goblinbtName1 = goblinbtPrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(goblinbtName)) { dmg=damageValues.GetDmgGoblinBt(); /*player.Blind(3,2);*/player.Fragile(damageValues.GetEfxGoblinBt().x,damageValues.GetEfxGoblinBt().y); player.Hack(damageValues.GetEfxGoblinBt().x*0.9f); AudioManager.instance.Play("GoblinBtHit");}
                
                var glaredevName = glaredevPrefab.name; var glaredevName1 = glaredevPrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(glaredevName)) { en=true; dmg=damageValues.GetDmgGoblin(); player.Fragile(damageValues.GetEfxGlareDev().x,damageValues.GetEfxGlareDev().y); player.Weaken(damageValues.GetEfxGlareDev().x,damageValues.GetEfxGlareDev().y); }

                if (!other.gameObject.name.Contains(hlaserName))
                {
                    if (player.dashing == false)
                    {
                        if(dmg!=0&&!player.gclover){player.Damage(dmg,dmgType.normal);}
                        //else if(dmg!=0&&player.gclover){AudioManager.instance.Play("GCloverHit");}
                        if (destroy == true)
                        {
                            if (en != true) { Destroy(other.gameObject, 0.05f); }
                            else { other.GetComponent<Enemy>().givePts = false; other.GetComponent<Enemy>().health = -1; other.GetComponent<Enemy>().Die(); }
                        }
                        else { }
                        var flare = Instantiate(player.flareHitVFX, new Vector2(other.transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
                        Destroy(flare.gameObject, 0.3f);
                    }
                    else if (player.shadow == true && player.dashing == true)
                    {
                        //if (destroy == true){
                        if (en != true) { Destroy(other.gameObject, 0.05f); }
                        else { other.GetComponent<Enemy>().health = -1; other.GetComponent<Enemy>().Die(); }
                        //}else{ }
                    }
                }else{
                    if(dmg!=0&&!player.gclover){player.Damage(dmg,dmgType.normal);}
                    //else if(dmg!=0&&player.gclover){AudioManager.instance.Play("GCloverHit");}
                }
                var name=other.gameObject.name.Split('(')[0];lastHitObj=name;lastHitDmg=dmg;
                if(gameSession.dmgPopups==true&&dmg!=0&&!player.gclover){
                    GameObject dmgpopup=CreateOnUI.CreateOnUIFunc(dmgPopupPrefab,transform.position);
                    dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().color=Color.red;
                    dmgpopup.transform.localScale=new Vector2(2,2);
                    dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=System.Math.Round(dmg/player.armorMulti,2).ToString();
                }
                //DMGPopUpHud(dmg);
            }
            #endregion
            #region//Powerups
            else if (other.gameObject.CompareTag("Powerups"))
            {
                var enBallName = enBallPrefab.name;
                if (other.gameObject.name.Contains(enBallName)) { player.AddSubEnergy(player.energyBallGet,true);}

                var CoinName = CoinPrefab.name;
                if (other.gameObject.name.Contains(CoinName)) { gameSession.coins+=other.GetComponent<LCrystalDrop>().amnt;}//gameSession.coins += 1; }

                var powercoreName = powercorePrefab.name;
                if (other.gameObject.name.Contains(powercoreName)) { gameSession.cores += 1; }

                if((!other.gameObject.name.Contains(enBallName)) && (!other.gameObject.name.Contains(CoinName)) && (!other.gameObject.name.Contains(powercoreName))){
                    gameSession.AddXP(gameSession.xp_powerup);}//XP For powerups

                var armorName = armorPwrupPrefab.name;
                if (other.gameObject.name.Contains(armorName)) { if(player.health>(player.maxHP-25)){gameSession.AddToScoreNoEV(Mathf.RoundToInt(player.maxHP - player.health)*2);} HPAdd(); player.AddSubEnergy(player.medkitEnergyGet,true); }//EnergyPopUpHUDPlus(player.medkitEnergyGet); player.healed = true; }
                var armorUName = armorUPwrupPrefab.name;
                if (other.gameObject.name.Contains(armorUName)) { if (player.health>(player.maxHP-30)) {gameSession.AddToScoreNoEV(Mathf.RoundToInt(player.maxHP - player.health)*2);} HPAddU(); player.AddSubEnergy(player.medkitUEnergyGet,true); }//EnergyPopUpHUDPlus(player.medkitUEnergyGet); player.healed = true; }

                void HPAdd(){
                    player.Damage(player.medkitHpAmnt,dmgType.heal);
                    //player.health += player.medkitHpAmnt;
                    //HPPopUpHUD(player.medkitHpAmnt);
                }void HPAddU(){
                    player.Damage(player.medkitUHpAmnt,dmgType.heal);
                    //player.health += player.medkitUHpAmnt;
                    //HPPopUpHUD(player.medkitUHpAmnt);
                }

                var flipName = flipPwrupPrefab.name;
                if (other.gameObject.name.Contains(flipName)) {
                    if(player.energy<=player.enForPwrupRefill){EnergyAdd();}
                    if(player.flip==true){EnergyAddDupl();}
                    player.SetStatus("flip"); 
                }
                
                var inverterName = inverterPwrupPrefab.name;
                if (other.gameObject.name.Contains(inverterName)){
                if(player.energyOn){
                    var tempHP = player.health; var tempEn = player.energy;
                    player.energy=tempHP; player.health=tempEn;
                }
                player.SetStatus("inverter"); player.inverterTimer = 0; }

                var magnetName = magnetPwrupPrefab.name;
                if (other.gameObject.name.Contains(magnetName)) {
                    if(player.energy<=player.enForPwrupRefill){EnergyAdd();}
                    if(player.magnet==true){EnergyAddDupl();}
                    player.SetStatus("magnet");
                    }
                
                var scalerName = scalerPwrupPrefab.name;
                if (other.gameObject.name.Contains(scalerName)) {
                    if(player.energy<=player.enForPwrupRefill){EnergyAdd();}
                    if(player.scaler==true){EnergyAddDupl();}
                    player.SetStatus("scaler");
                    player.shipScale=UnityEngine.Random.Range(player.shipScaleMin,player.shipScaleMax);
                    }

                var gcloverName = gcloverPwrupPrefab.name;
                if (other.gameObject.name.Contains(gcloverName))
                {
                    player.SetStatus("gclover");
                    gameSession.MultiplyScore(1.25f);
                    player.energy = player.maxEnergy;
                    //GameObject gcloverexVFX = Instantiate(gcloverVFX, new Vector2(0, 0), Quaternion.identity);
                    GameObject gcloverexOVFX = Instantiate(player.gcloverOVFX, new Vector2(0, 0), Quaternion.identity);
                    //Destroy(gcloverexVFX, 1f);
                    Destroy(gcloverexOVFX, 1f);
                }
                var shadowName = shadowPwrupPrefab.name;
                if (other.gameObject.name.Contains(shadowName))
                {
                    if(player.energy<=player.enForPwrupRefill){player.AddSubEnergy(player.pwrupEnergyGet,true);}
                    if(player.shadow==true){EnergyAddDupl();}
                    player.SetStatus("shadow");
                    player.shadowed = true;
                    //GameObject gcloverexVFX = Instantiate(gcloverVFX, new Vector2(0, 0), Quaternion.identity);
                    //GameObject gcloverexOVFX = Instantiate(shadowEVFX, new Vector2(0, 0), Quaternion.identity);
                    //Destroy(gcloverexVFX, 1f);
                    //Destroy(gcloverexOVFX, 1f);
                }
                var matrixName = matrixPwrupPrefab.name;
                if (other.gameObject.name.Contains(matrixName)) { player.SetStatus("matrix"); }
                var accelName = acceleratorPwrupPrefab.name;
                if (other.gameObject.name.Contains(accelName)) { player.SetStatus("accel"); }
                var pmultiName = pmultiPwrupPrefab.name;
                if (other.gameObject.name.Contains(pmultiName)) { player.SetStatus("pmulti");if(player.pmultiTimer<0){player.pmultiTimer=0;}player.pmultiTimer += player.pmultiTime; gameSession.scoreMulti=2f;}
                
                var randomizerName = randomizerPwrupPrefab.name;
                if (other.gameObject.name.Contains(randomizerName)) { 
                    var item = other.GetComponent<LootTable>().GetItem();
                    Instantiate(item.gameObject,new Vector2(other.transform.position.x,other.transform.position.y),Quaternion.identity);
                    Destroy(other.gameObject,0.01f);
                 }

                var laser2Name = laser2PwrupPrefab.name;
                if (other.gameObject.name.Contains(laser2Name)) { PowerupCollect("laser2"); }

                var laser3Name = laser3PwrupPrefab.name;
                if (other.gameObject.name.Contains(laser3Name)) { PowerupCollect("laser3"); }

                var phaserName = phaserPwrupPrefab.name;
                if (other.gameObject.name.Contains(phaserName)) { PowerupCollect("phaser"); }

                var hrocketName = hrocketPwrupPrefab.name;
                if (other.gameObject.name.Contains(hrocketName)) { PowerupCollect("hrocket"); }

                var mlaserName = mlaserPwrupPrefab.name;
                if (other.gameObject.name.Contains(mlaserName)) { PowerupCollect("mlaser"); }

                var lsaberWName1 = player.lsaberPrefab.name;
                var lclawsWName1 = player.lclawsPrefab.name;
                var lsaberName = lsaberPwrupPrefab.name; var lsaberName1 = lsaberPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(lsaberName)) { PowerupCollect("lsaber"); GameObject.Find(lclawsWName1); }
                
                var lclawsName = lclawsPwrupPrefab.name; var lclawsName1 = lclawsPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(lclawsName)) { PowerupCollect("lclaws"); GameObject.Find(lsaberWName1); }

                var shadowbtName = shadowBTPwrupPrefab.name; var shadowbtName1 = shadowBTPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(shadowbtName)) { PowerupCollect("shadowbt"); }

                var qrocketName = qrocketPwrupPrefab.name; var qrocketName1 = qrocketPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(qrocketName)) { PowerupCollect("qrocket"); }
                var procketName = procketPwrupPrefab.name; var procketName1 = procketPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(procketName)) { PowerupCollect("procket"); }

                var cstreamName = cstreamPwrupPrefab.name; var cstreamName1 = cstreamPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(cstreamName)) { PowerupCollect("cstream"); }
                var plaserName = plaserPwrupPrefab.name; var plaserName1 = plaserPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name.Contains(plaserName)) { PowerupCollect("plaser"); }

                var laser1Name = GameAssets.instance.Get("Laser1Pwrup").name;
                if (other.gameObject.name.Contains(laser1Name)) { PowerupCollect("laser"); }
                var fireName = GameAssets.instance.Get("FirePwrup").name;
                if (other.gameObject.name.Contains(fireName)) { player.OnFire(10,1); }
                var decayName = GameAssets.instance.Get("DecayPwrup").name;
                if (other.gameObject.name.Contains(decayName)) { player.Decay(10,1); }
                var blindName = GameAssets.instance.Get("BlindPwrup").name;
                if (other.gameObject.name.Contains(blindName)) { player.Blind(10,4); }
                var electrcName = GameAssets.instance.Get("ElectrcPwrup").name;
                if (other.gameObject.name.Contains(electrcName)) { player.Electrc(10); }
                var frozenName = GameAssets.instance.Get("FrozenPwrup").name;
                if (other.gameObject.name.Contains(frozenName)) { player.Freeze(10); }
                var fragileName = GameAssets.instance.Get("FragilePwrup").name;
                if (other.gameObject.name.Contains(fragileName)) { player.Fragile(10,1); }
                var armoredName = GameAssets.instance.Get("ArmoredPwrup").name;
                if (other.gameObject.name.Contains(armoredName)) { player.Armor(10,1); }
                var poweredName = GameAssets.instance.Get("PowerPwrup").name;
                if (other.gameObject.name.Contains(poweredName)) { player.Power(10,1); }
                var weaknsName = GameAssets.instance.Get("WeaknsPwrup").name;
                if (other.gameObject.name.Contains(weaknsName)) { player.Weaken(10,1); }
                var hackedName = GameAssets.instance.Get("HackedPwrup").name;
                if (other.gameObject.name.Contains(hackedName)) { player.Hack(10); }
                var infenName = GameAssets.instance.Get("InfEnergyPwrup").name;
                if (other.gameObject.name.Contains(infenName)) { player.InfEnergy(10); }


                if (other.gameObject.name.Contains(enBallName))
                {
                    AudioManager.instance.Play("EnergyBall");
                }
                else if (other.gameObject.name.Contains(CoinName))
                {
                    AudioManager.instance.Play("Coin");
                }else if (other.gameObject.name.Contains(powercoreName))
                {
                    AudioManager.instance.Play("LvlUp");
                }
                else if (other.gameObject.name.Contains(gcloverName))
                {
                    AudioManager.instance.Play("GClover");
                }
                else if (other.gameObject.name.Contains(shadowbtName))
                {
                    AudioManager.instance.Play("ShadowGet");
                }else if (other.gameObject.name.Contains(matrixName))
                {
                    AudioManager.instance.Play("MatrixGet");
                }else if (other.gameObject.name.Contains(accelName))
                {
                    AudioManager.instance.Play("AccelGet");
                }
                else
                {
                    //SoundManager.PlaySound(SoundManager.Sound.powerupSFX);
                    AudioManager.instance.Play("Powerup");
                }
                Destroy(other.gameObject, 0.05f);
            }
            #endregion
        }
    }
    void PowerupCollect(string name){
        if(player.energy<=player.enForPwrupRefill){EnergyAdd();} if(player.powerup==name){EnergyAddDupl();} player.SetPowerup(name);
    }
    void EnergyAdd(){
        player.AddSubEnergy(player.pwrupEnergyGet,true);//EnergyPopUpHUDPlus(player.pwrupEnergyGet);
    }void EnergyAddDupl(){
        player.AddSubEnergy(player.enPwrupDuplicate,true);//EnergyPopUpHUDPlus(player.enPwrupDuplicate);
    }
    private void OnTriggerStay2D(Collider2D other){
        if (!other.CompareTag(tag))
        {
        if (dmgTimer<=0){
            DamageDealer damageDealer=other.GetComponent<DamageDealer>();
            DamageValues damageValues=DamageValues.instance;
            //if(!damageDealer||!damageValues){Debug.LogWarning("No DamageDealer component or DamageValues instance");return;}
            //bool en = false;
            float dmg = 0;
            if(other.GetComponent<Tag_OutsideZone>()!=null){player.Hack(1f);dmg=damageValues.GetDmgZone();}
            var leechName=leechPrefab.name;
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyBullet")){
            
            leechName = leechPrefab.name; var leechName1 = leechPrefab.name + "(Clone)";
            if (other.gameObject.name.Contains(leechName)) { dmg = damageValues.GetDmgLeech(); AudioManager.instance.Play("LeechBite");}

            var hlaserName = hlaserPrefab.name; var hlaserName1 = hlaserPrefab.name + "(Clone)";
            if (other.gameObject.name.Contains(hlaserName)) { dmg = damageValues.GetDmgHLaser(); }

            var enSaberName = enSaberPrefab.name; var enSaberName1 = enSaberPrefab.name + "(Clone)";
            if (other.gameObject.name.Contains(enSaberName)) { dmg = damageValues.GetDmgEnSaber(); }

            if(other.gameObject.name.Contains("StickBomb")) { dmg=0; }
            }
        //if (dmgTimer<=0){
                //if (other.gameObject.name.Contains(leechName)){}
                //var flare = Instantiate(player.flareHitVFX, new Vector2(other.transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
                //Destroy(flare.gameObject, 0.3f);
                if(gameSession.dmgPopups==true&&dmg!=0){
                    GameObject dmgpopup=CreateOnUI.CreateOnUIFunc(dmgPopupPrefab,transform.position);
                    dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().color=Color.red;
                    dmgpopup.transform.localScale=new Vector2(2,2);
                    dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=System.Math.Round(dmg/player.armorMulti,2).ToString();
                }
                if(dmg!=0)player.Damage(dmg,dmgType.silent);
                dmgTimer = dmgFreq;
            }else{ dmgTimer -= Time.deltaTime; }
        }
    }
    private void OnTriggerExit2D(Collider2D other){
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyBullet"))
        {
            dmgTimer = dmgFreq;
        }
        //GameObject dmgpopupHud=GameObject.Find("HPDiffParrent");
        //dmgpopupHud.GetComponent<AnimationOn>().AnimationSet(true);
    }
    public GameObject GetRandomizerPwrup(){return randomizerPwrupPrefab;}

    /*public void DMGPopUpHud(float dmg){
        GameObject dmgpopupHud=GameObject.Find("HPDiffParrent");
        dmgpopupHud.GetComponent<AnimationOn>().AnimationSet(true);
        //dmgpupupHud.GetComponent<Animator>().SetTrigger(0);
        dmgpopupHud.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="-"+dmg.ToString();
    }public void HPPopUpHUD(float dmg){
        GameObject dmgpopupHud=GameObject.Find("HPDiffParrent");
        dmgpopupHud.GetComponent<AnimationOn>().AnimationSet(true);
        //dmgpupupHud.GetComponent<Animator>().SetTrigger(0);
        dmgpopupHud.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="+"+dmg.ToString();
    }
    public void EnergyPopUpHUD(float en){
        GameObject enpopupHud=GameObject.Find("EnergyDiffParrent");
        enpopupHud.GetComponent<AnimationOn>().AnimationSet(true);
        //enpupupHud.GetComponent<Animator>().SetTrigger(0);
        enpopupHud.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="-"+en.ToString();
    }public void EnergyPopUpHUDPlus(float en){
        GameObject enpopupHud=GameObject.Find("EnergyDiffParrent");
        enpopupHud.GetComponent<AnimationOn>().AnimationSet(true);
        //enpupupHud.GetComponent<Animator>().SetTrigger(0);
        enpopupHud.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="+"+en.ToString();
    }*/
}
