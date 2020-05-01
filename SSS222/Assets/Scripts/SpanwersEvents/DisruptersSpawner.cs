﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisruptersSpawner : MonoBehaviour{
    //[SerializeField] int[] waveConfigsWeights;
    //[SerializeField] int startingWave = 0;
    [HeaderAttribute("Mecha Leech")]
    public bool spawnLeech=true;
    [SerializeField] WaveConfig cfgLeech;
    [SerializeField] float mSTimeSpawnsLeech = 55f;
    [SerializeField] float mETimeSpawnsLeech = 80f;
    public float timeSpawnsLeech = 0f;
    [HeaderAttribute("Homing Laser")]
    public bool spawnHlaser=true;
    [SerializeField] WaveConfig cfgHlaser;
    [SerializeField] float mSTimeSpawnsHlaser = 30f;
    [SerializeField] float mETimeSpawnsHlaser = 60f;
    public float timeSpawnsHlaser = 0f;
    [HeaderAttribute("Goblin Thief")]
    public bool spawnGoblin=true;
    [SerializeField] WaveConfig cfgGoblin;
    [SerializeField] float mSTimeSpawnsGoblin = 40f;
    [SerializeField] float mESTimeSpawnsGoblin = 50f;
    public float timeSpawnsGoblin = 0f;
    //public int waveIndex = 0;
    //WaveConfig currentWave;
    bool looping = true;
    //[SerializeField] bool progressiveWaves = false;

    //WaveDisplay waveDisplay;
    GameSession gameSession;
    Player player;
    // Start is called before the first frame update
    #region//GetRandomWeightedIndex
        /*public int GetRandomWeightedIndex(int[] weights)
        {
            if (weights == null || weights.Length == 0) return -1;

            int w=0;
            int i;
            for (i = 0; i < weights.Length; i++)
            {
                if (weights[i] >= 0) w += weights[i];
            }

            float r = Random.value;
            float s = 0f;

            for (i = 0; i < weights.Length; i++)
            {
                if (weights[i] <= 0f) continue;

                s += (float)weights[i] / waveConfigsWeights.Length;
                if (s >= r) return i;
            }

            return -1;
        }*/
    #endregion
    IEnumerator Start()
    {
        //waveDisplay = FindObjectOfType<WaveDisplay>();
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<Player>();
        if(spawnLeech==true)timeSpawnsLeech = Random.Range(mSTimeSpawnsLeech,mETimeSpawnsLeech);
        if(spawnHlaser==true)timeSpawnsHlaser = Random.Range(mSTimeSpawnsHlaser, mETimeSpawnsHlaser);
        if(spawnGoblin==true)timeSpawnsGoblin = Random.Range(mSTimeSpawnsGoblin, mESTimeSpawnsGoblin);
        do
        {
            yield return StartCoroutine(SpawnWaves());
        }
        while (looping);
    }

    public IEnumerator SpawnWaves()
    {
        if(spawnLeech==true){
        if (timeSpawnsLeech<=0 && timeSpawnsLeech>-4){
            //currentWave = cfgLeech;
            yield return StartCoroutine(SpawnAllEnemiesInWave(cfgLeech));
            timeSpawnsLeech = -4;
        }
        //if (progressiveWaves == true){if (waveIndex<waveConfigs.Count){ waveIndex++; } }
        //else{if(gameSession.EVscore>=50){ /*WaveRandomize();*/
        //waveIndex = Random.Range(0, waveConfigs.Count); gameSession.EVscore = 0; } }
        }if(spawnHlaser==true){
            if (timeSpawnsHlaser <= 0 && timeSpawnsHlaser > -4){
                //currentWave = cfgHlaser;
                yield return StartCoroutine(SpawnAllEnemiesInWave(cfgHlaser));
                timeSpawnsHlaser = -4;
            }
        }
        if(spawnGoblin==true){
            if(GameObject.FindGameObjectWithTag("Powerups")!=null){
                if (timeSpawnsGoblin <= 0 && timeSpawnsGoblin > -4){
                    //currentWave = cfgGoblin;
                    yield return StartCoroutine(SpawnAllEnemiesInWave(cfgGoblin));
                    timeSpawnsGoblin = -4;
                }
            }
        }
    }

    public IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        var RpathIndex = Random.Range(0, waveConfig.pathsRandom.Count);
        if (waveConfig.randomPath == false && waveConfig.between2PtsPath==false && waveConfig.shipPlace==false && waveConfig.randomPoint==false){
            for (int enCount = 0; enCount < waveConfig.GetNumberOfEnemies(); enCount++)
            {
                var newEnemy = Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    waveConfig.GetWaypoints()[enCount].transform.position,
                    Quaternion.identity);
                newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                newEnemy.GetComponent<EnemyPathing>().enemyIndex = enCount;
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
        }else if (waveConfig.randomPath == true || waveConfig.between2PtsPath == true) {
            if(waveConfig.between2PtsPath==true){
                for (int enCount = 0; enCount < waveConfig.GetNumberOfEnemies(); enCount++)
                {
                    var newEnemy = Instantiate(
                        waveConfig.GetEnemyPrefab(),
                        waveConfig.GetWaypoints()[0].transform.position,
                        Quaternion.identity);
                    newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                   // newEnemy.GetComponent<EnemyPathing>().enemyIndex = enCount;
                    yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
                }
            }
            if(waveConfig.randomPath == true){
                for (int enCount = 0; enCount < waveConfig.GetNumberOfEnemies(); enCount++)
                {
                    var newEnemy = Instantiate(
                        waveConfig.GetEnemyPrefab(),
                        waveConfig.GetWaypointsRandomPath(RpathIndex)[0].transform.position,
                        Quaternion.identity);
                    newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                    newEnemy.GetComponent<EnemyPathing>().enemyIndex = RpathIndex;
                    yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
                }
            }
        }else if(waveConfig.randomPoint==true){
            for (int enCount = 0; enCount < waveConfig.GetNumberOfEnemies(); enCount++)
            {
                var waveWaypoints = new List<Transform>();
                foreach (Transform child in waveConfig.pathsRandom[0].transform){waveWaypoints.Add(child);}
                var pointIndex = Random.Range(0, waveWaypoints.Count);
                var newEnemy = Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    waveWaypoints[Random.Range(0, pointIndex)].transform.position,
                    Quaternion.identity);
                newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                newEnemy.GetComponent<EnemyPathing>().waypointIndex = pointIndex;
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
        }else if(waveConfig.shipPlace==true){
            for (int enCount = 0; enCount < waveConfig.GetNumberOfEnemies(); enCount++)
            {
                var newEnemy = Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    new Vector2(player.transform.position.x, 7.2f+waveConfig.shipYY),
                Quaternion.identity);
                newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
                yield return new WaitForSeconds(waveConfig.GetTimeSpawn());
            }
        }
        else { yield return new WaitForSeconds(waveConfig.GetTimeSpawn()); }
    }

    /*public void WaveRandomize()
    {
        var weights = new Dictionary<WaveConfig, int>();
        for (int index = 0; index < waveConfigs.Count; index++){
            weights.Add(waveConfigs[index], waveConfigsWeights[index]);
        }

        WaveConfig selected = WeightedRandomizer.From(weights).TakeOne(); // Strongly-typed object returned. No casting necessary.
    }*/
    //public string GetWaveName(){return currentWave.waveName;}
    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale>0.0001f){
            if(spawnLeech==true){
                if(timeSpawnsLeech>-0.01f){timeSpawnsLeech -= Time.deltaTime; }
                else if(timeSpawnsLeech==-4){ timeSpawnsLeech = Random.Range(mSTimeSpawnsLeech, mETimeSpawnsLeech); }
            }
            if(spawnHlaser==true){
                if(timeSpawnsHlaser>-0.01f){ timeSpawnsHlaser -= Time.deltaTime; }
                else if(timeSpawnsHlaser == -4){ Random.Range(mSTimeSpawnsHlaser, mETimeSpawnsHlaser); }
            }
            if(spawnGoblin==true){
                if(timeSpawnsGoblin > -0.01f){ timeSpawnsGoblin -= Time.deltaTime; }
                else if(timeSpawnsGoblin == -4){ timeSpawnsGoblin = Random.Range(mSTimeSpawnsGoblin, mESTimeSpawnsGoblin); }
            }
            /*if(progressiveWaves==true){if (waveIndex >= waveConfigs.Count) { waveIndex = startingWave; } }
            else{if (gameSession.EVscore >= 50) { waveDisplay.enableText = true; waveDisplay.timer = waveDisplay.showTime;
                    timeSpawns = 0; waveIndex = Random.Range(0, waveConfigs.Count); currentWave = waveConfigs[waveIndex];
                    gameSession.EVscore = 0; } }*/
            //if (timeSpawns <= 0) {timeSpawns = mTimeSpawns; }
            //Debug.Log(timeSpawns);
        }
    }
}
