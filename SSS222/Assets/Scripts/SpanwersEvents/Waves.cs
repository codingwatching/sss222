﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Waves : MonoBehaviour{
    [Header("SpawnReqs")]
    [InlineButton("ValidateWaveSpawnReqs","Validate")]
    [SerializeField] public spawnReqsType waveSpawnReqsType=spawnReqsType.score;
    [SerializeReference] public spawnReqs waveSpawnReqs;
    [Header("Config")]
    [DisableIf("startingWaveRandom")][SerializeField] public int startingWave=0;
    [SerializeField] public bool startingWaveRandom=false;
    public int waveIndex=0;
    public WaveConfig currentWave;
    [SerializeField] public bool uniqueWaves=true;
    float checkSpawns=3f;
    
    [Header("Current Values")]
    public float timeSpawns=0f;
    float checkSpawnsTimer=-4;

    WaveDisplay waveDisplay;
    LootTableWaves lootTable;

    IEnumerator Start(){
        waveDisplay=FindObjectOfType<WaveDisplay>();
        lootTable=GetComponent<LootTableWaves>();
        yield return new WaitForSeconds(0.05f);
        if(startingWaveRandom){currentWave=GetRandomWave();startingWave=waveIndex;}
    }
    /*public IEnumerator CallRandomizeWave(){
        RandomizeWave();
        GameManager.instance.RandomizeWaveScoreMax();
        yield return null;
    }*/

    [Button("Spawn RandomizeWave")][ContextMenu("RandomizeWave")]public void RandomizeWave(){StartCoroutine(RandomizeWaveI());}
    public IEnumerator RandomizeWaveI(){
        if(waveDisplay!=null){waveDisplay.enableText=true;waveDisplay.timer=waveDisplay.showTime;}
        currentWave=GetRandomWave();
        if(GameRules.instance.xpOn){GameManager.instance.DropXP(GameRules.instance.xp_wave,new Vector2(0,7),3f);}else{GameManager.instance.AddXP(GameRules.instance.xp_wave);}
        GameManager.instance.RandomizeWaveScoreMax();
        spawnReqsMono.AddWaves();
        if(BreakEncounter.instance!=null)BreakEncounter.instance.AddWaves();
        yield return null;
    }
    public WaveConfig GetRandomWave(){
    if(lootTable!=null){
        if(!startingWaveRandom&&(currentWave==null&&lootTable.itemList!=null)){if(lootTable.itemList.Count>0){return lootTable.itemList[startingWave].lootItem;}else{return null;}}
        else{
            if(uniqueWaves){
                WaveConfig wave;
                do{
                    wave=lootTable.GetItem();
                    return wave;
                }while(wave==currentWave);
            }else{return lootTable.GetItem();}
        }
    }else{return null;}}
    
    void CheckSpawnReqs(){
        if(waveSpawnReqs!=GameRules.instance.waveSpawnReqs)waveSpawnReqs=GameRules.instance.waveSpawnReqs;
        if(waveSpawnReqsType!=GameRules.instance.waveSpawnReqsType)waveSpawnReqsType=GameRules.instance.waveSpawnReqsType;
        spawnReqs x=waveSpawnReqs;
        spawnReqsType xt=waveSpawnReqsType;
        spawnReqsMono.instance.CheckSpawns(x,xt,this,"RandomizeWave");
    }
    void Update(){
        if(!GameManager.GlobalTimeIsPaused){
            if(GameManager.instance._noBreak()){
                CheckSpawnReqs();

                if(currentWave==null&&lootTable.itemList.Count>0)currentWave=lootTable.itemList[startingWave].lootItem;
                if(timeSpawns>0){timeSpawns-=Time.deltaTime;}
                else if(timeSpawns==-4){timeSpawns=currentWave.timeSpawnWave;}
                else if(timeSpawns<=0&&timeSpawns>-4&&currentWave!=null){SpawnAllEnemiesInCurrentWave();timeSpawns=currentWave.timeSpawnWave;}
                

                //Check if no Enemies for some time, force a wave spawn
                if(lootTable.itemList.Count>0){
                    if(FindObjectsOfType<Tag_WaveEnemy>().Length==0){
                        if(checkSpawnsTimer==-4)checkSpawnsTimer=checkSpawns;
                        if(checkSpawnsTimer>0)checkSpawnsTimer-=Time.deltaTime;
                        else if(checkSpawnsTimer<=0&&checkSpawnsTimer>-4){
                            Debug.LogWarning("No WaveEnemies found, forcing a spawn!");
                            if(waveDisplay!=null){waveDisplay.enableText=true;waveDisplay.timer=waveDisplay.showTime;}
                            currentWave=GetRandomWave();
                            if(timeSpawns==-4){timeSpawns=currentWave.timeSpawnWave;}
                            //StartCoroutine(SpawnWave());
                            checkSpawnsTimer=checkSpawns;
                        }
                    }else{checkSpawnsTimer=-4;}
                }
            }
        }
    }

    #region//SpawnAllEnemiesInWave
    public void SpawnAllEnemiesInWave(WaveConfig waveConfig){StartCoroutine(SpawnAllEnemiesInWaveI(waveConfig));}
    public void SpawnAllEnemiesInCurrentWave(){SpawnAllEnemiesInWave(currentWave);}
    IEnumerator SpawnAllEnemiesInWaveI(WaveConfig waveConfig){
        spawnReqsMono.AddWaveCounts(waveConfig);
    switch(waveConfig.wavePathType){
        case wavePathType.startToEnd:
            for(int enCount=0; enCount<waveConfig.GetNumberOfEnemies(); enCount++){
                GameObject go=Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    waveConfig.GetWaypointsStart()[enCount].position,
                    Quaternion.identity);
                if(go.GetComponent<PointPathing>()==null){go.AddComponent<PointPathing>();}
                go.GetComponent<PointPathing>().SetWaveConfig(waveConfig);
                go.GetComponent<PointPathing>().enemyIndex=enCount;
                go.AddComponent<Tag_WaveEnemy>();
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
            break;
        case wavePathType.btwn2Pts:
            for(int enCount=0; enCount<waveConfig.GetNumberOfEnemies(); enCount++){
                Vector2 pos;
                pos.x=Random.Range(waveConfig.GetWaypointsSingle()[0].position.x,waveConfig.GetWaypointsSingle()[1].position.x);
                pos.y=Random.Range(waveConfig.GetWaypointsSingle()[0].position.y,waveConfig.GetWaypointsSingle()[1].position.y);
                GameObject go=Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    pos,
                    Quaternion.identity);
                if(go.GetComponent<PointPathing>()==null){go.AddComponent<PointPathing>();}
                go.GetComponent<PointPathing>().SetWaveConfig(waveConfig);
                // go.GetComponent<PointPathing>().enemyIndex=enCount;
                go.AddComponent<Tag_WaveEnemy>();
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
            break;
        case wavePathType.randomPath:
            var pR=(WaveConfig.pathRandom)waveConfig.wavePaths;
            var RpathIndex=Random.Range(0, pR.pathsRandom.Count);
            for(int enCount=0; enCount<waveConfig.GetNumberOfEnemies(); enCount++){
                GameObject go=Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    waveConfig.GetWaypointsRandomPath(RpathIndex)[0].position,
                    Quaternion.identity);
                if(go.GetComponent<PointPathing>()==null){go.AddComponent<PointPathing>();}
                go.GetComponent<PointPathing>().SetWaveConfig(waveConfig);
                go.GetComponent<PointPathing>().enemyIndex=RpathIndex;
                go.AddComponent<Tag_WaveEnemy>();
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
            break;
        case wavePathType.randomPoint:
            for(int enCount=0; enCount<waveConfig.GetNumberOfEnemies(); enCount++){
                //var waveWaypoints=new List<Transform>();
                //foreach(Transform child in waveConfig.GetWaypointsRandomPoint()){waveWaypoints.Add(child);}
                //var pointIndex=Random.Range(0, waveWaypoints.Count);
                var waypoints=waveConfig.GetWaypointsRandomPoint();
                var pos=waveConfig.GetWaypointRandomPoint().position;
                var w=(WaveConfig.pathRandomPoint)waveConfig.wavePaths;
                if(w.closestToPlayer&&Player.instance!=null){pos=waveConfig.GetWaypointClosestToPlayer().position;}
                GameObject go=Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    pos,
                    Quaternion.identity);
                if(go.GetComponent<PointPathing>()==null){go.AddComponent<PointPathing>();}
                go.GetComponent<PointPathing>().SetWaveConfig(waveConfig);
                go.GetComponent<PointPathing>().waypointIndex=Random.Range(0,waypoints.Count);
                //go.GetComponent<PointPathing>().waypointIndex=enCount;
                go.AddComponent<Tag_WaveEnemy>();
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
            break;
        case wavePathType.shipPlace:
            if(Player.instance!=null){
                var pS=(WaveConfig.shipPlace)waveConfig.wavePaths;
                for(int enCount=0; enCount<waveConfig.GetNumberOfEnemies(); enCount++){
                    GameObject go=Instantiate(
                        waveConfig.GetEnemyPrefab(),
                        waveConfig.GetShipPlaceCoords(waveConfig),
                    Quaternion.identity);
                if(go.GetComponent<PointPathing>()==null){go.AddComponent<PointPathing>();}
                go.GetComponent<PointPathing>().SetWaveConfig(waveConfig);
                go.AddComponent<Tag_WaveEnemy>();
                    yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
                }
            }
            break;
        case wavePathType.loopPath:
            for(int enCount=0; enCount<waveConfig.GetNumberOfEnemies(); enCount++){
                GameObject go=Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    waveConfig.GetWaypointsSingle()[enCount].position,
                    Quaternion.identity);
                if(go.GetComponent<PointPathing>()==null){go.AddComponent<PointPathing>();}
                go.GetComponent<PointPathing>().SetWaveConfig(waveConfig);
                go.GetComponent<PointPathing>().enemyIndex=enCount;
                go.AddComponent<Tag_WaveEnemy>();
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
            break;
        default: yield return new WaitForSeconds(waveConfig.GetTimeSpawn());break;
    }}
    #endregion
    public string GetWaveName(){return currentWave.name;}

    [ContextMenu("ValidateWaveSpawnReqs")]void ValidateWaveSpawnReqs(){spawnReqsMono.Validate(ref waveSpawnReqs, ref waveSpawnReqsType);}
}
