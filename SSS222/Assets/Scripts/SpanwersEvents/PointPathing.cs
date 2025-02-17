﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PointPathing : MonoBehaviour{
    [SerializeField] public bool off=false;
    [SerializeField] public WaveConfig waveConfig;
    [DisableInEditorMode]List<Transform> waypointsS;
    [DisableInEditorMode]List<Transform> waypointsE;
    [DisableInEditorMode]List<Transform> waypointsR;
    [DisableInEditorMode]public List<Transform> waypointsL;
    public int waypointIndex = 0;
    public int enemyIndex = 0;
    Rigidbody2D rb;
    Vector2 velPaused;
    //bool CheckWavesPath(wavePathType wavePathType){if(waveConfig.wavePathType==wavePathType){return true;}else{return false;}}
    void Start(){
        rb=GetComponent<Rigidbody2D>();
        SetPathingInfo();
    }
    void SetPathingInfo(){
        if(!off){
            if(waveConfig!=null){
                if(waveConfig.wavePathType==wavePathType.startToEnd){
                    waypointsS=waveConfig.GetWaypointsStart();
                    waypointsE=waveConfig.GetWaypointsEnd();
                    waypointIndex=enemyIndex;
                }else{
                    if(waveConfig.wavePathType==wavePathType.btwn2Pts){waypointsS=waveConfig.GetWaypointsSingle();}
                    else if(waveConfig.wavePathType==wavePathType.randomPath){waypointsR=waveConfig.GetWaypointsRandomPath(enemyIndex);}
                    else if(waveConfig.wavePathType==wavePathType.randomPathEach){waypointsR=waveConfig.GetWaypointsRandomPathEach();}
                    else if(waveConfig.wavePathType==wavePathType.randomPoint){waypointsR=waveConfig.GetWaypointsRandomPoint();}
                    //}else if(waveConfig.loopPath==true){waypointsL=waveConfig.GetWaypointsLoop();
                    else if(waveConfig.wavePathType==wavePathType.loopPath||waveConfig.wavePathType==wavePathType.loopPathRandom){waypointsL=waveConfig.GetWaypointsSingle();}
                    else{if(waveConfig.wavePathType!=wavePathType.shipPlace){
                        waypointsS=waveConfig.GetWaypointsStart();
                    }}
                }
                if(waveConfig.wavePathType!=wavePathType.loopPath){
                if(waveConfig.wavePathType==wavePathType.randomPath||waveConfig.wavePathType==wavePathType.randomPathEach||waveConfig.wavePathType==wavePathType.randomPoint){transform.position=waypointsR[waypointIndex].transform.position;
                    if(waveConfig.wavePathType==wavePathType.randomPoint&&(waveConfig.GetMoveSpeed()!=0||waveConfig.randomSpeed==true)){
                        if(waveConfig.randomSpeed==false){rb.velocity=new Vector2(0f, -waveConfig.GetMoveSpeed());}
                        else{rb.velocity=new Vector2(0f, Random.Range(-waveConfig.GetMoveSpeedS(), -waveConfig.GetMoveSpeedE()));}
                    }
                }
                else if(waveConfig.wavePathType==wavePathType.btwn2Pts){var p0=waypointsS[0].transform.position; var p1=waypointsS[1].transform.position;
                    Vector3 v=p1-p0;
                    transform.position=p0+Random.value*v;
                    if(waveConfig.randomSpeed==false){rb.velocity = new Vector2(0f, -waveConfig.GetMoveSpeed());}
                    else{rb.velocity=new Vector2(0f, Random.Range(-waveConfig.GetMoveSpeedS(), -waveConfig.GetMoveSpeedE()));}
                }
                else if(waveConfig.wavePathType==wavePathType.shipPlace){transform.position = new Vector2(Player.instance.transform.position.x, 7.2f);
                    if(waveConfig.randomSpeed==false){rb.velocity = new Vector2(0f, -waveConfig.GetMoveSpeed());}
                    else{rb.velocity=new Vector2(0f, Random.Range(-waveConfig.GetMoveSpeedS(), -waveConfig.GetMoveSpeedE()));}
                }
                else{transform.position=waypointsS[waypointIndex].transform.position;}
                }
            }else{Debug.LogWarning(gameObject.name+" WaveConfig not found.");}
        }
    }
    public void ChangePathingInfo(WaveConfig wc){
        waveConfig=wc;
        if(!off){
            if(waveConfig.wavePathType==wavePathType.startToEnd){
                waypointsS=waveConfig.GetWaypointsStart();
                waypointsE=waveConfig.GetWaypointsEnd();
                waypointIndex=enemyIndex;
            }else{
                if(waveConfig.wavePathType==wavePathType.btwn2Pts){waypointsS=waveConfig.GetWaypointsSingle();}
                else if(waveConfig.wavePathType==wavePathType.randomPath){waypointsR=waveConfig.GetWaypointsRandomPath(enemyIndex);}
                else if(waveConfig.wavePathType==wavePathType.randomPathEach){waypointsR=waveConfig.GetWaypointsRandomPathEach();}
                else if(waveConfig.wavePathType==wavePathType.randomPoint){waypointsR=waveConfig.GetWaypointsRandomPoint();}
                else if(waveConfig.wavePathType==wavePathType.loopPath||waveConfig.wavePathType==wavePathType.loopPathRandom){waypointsL=waveConfig.GetWaypointsSingle();}
                else{if(waveConfig.wavePathType!=wavePathType.shipPlace){
                    waypointsS=waveConfig.GetWaypointsStart();
                }}
            }
        }
    }
    
    void Update(){  if(!GameManager.GlobalTimeIsPaused){
        if(!off)Move();
    }}
    public void SetWaveConfig(WaveConfig waveConfig){this.waveConfig = waveConfig;}
    void Move(){    if(waveConfig!=null){
        if(waveConfig.wavePathType==wavePathType.startToEnd){//Start-End Path
            if (transform.position!= waypointsE[waypointIndex].transform.position){
                var targetPos = waypointsE[waypointIndex].transform.position;
                var step = waveConfig.GetMoveSpeed()*Time.deltaTime;
                transform.position=Vector2.MoveTowards(transform.position, targetPos, step);
                //if (transform.position == targetPos)waypointIndex++;
            }else{Destroy(gameObject);}
        }else if(waveConfig.wavePathType==wavePathType.btwn2Pts){//Between 2 points
            if(transform.position.y<-7.5f){Destroy(gameObject);}
        }else if(waveConfig.wavePathType!=wavePathType.randomPoint){
            if(waveConfig.wavePathType==wavePathType.randomPath||waveConfig.wavePathType==wavePathType.randomPathEach){//Random Path
                if (waypointIndex<waypointsR.Count){
                    var targetPos=waypointsR[waypointIndex].transform.position;
                    var step=waveConfig.GetMoveSpeed()*Time.deltaTime;
                    transform.position=Vector2.MoveTowards(transform.position, targetPos, step);
                    if(transform.position==targetPos)waypointIndex++;
                }else{Destroy(gameObject);}
            }
            else{
                if(waveConfig.wavePathType!=wavePathType.shipPlace&&waveConfig.wavePathType!=wavePathType.loopPath&&waveConfig.wavePathType!=wavePathType.loopPathRandom){//Random Point
                    if(waypointIndex<waypointsR.Count){
                        var targetPos=waypointsR[waypointIndex].transform.position;
                        var step=waveConfig.GetMoveSpeed()*Time.deltaTime;
                        transform.position=Vector2.MoveTowards(transform.position, targetPos, step);
                        if (transform.position==targetPos)waypointIndex++;
                    }
                    else{Destroy(gameObject);}
                }if(waveConfig.wavePathType==wavePathType.loopPath){//Loop Path
                    if(waypointIndex<waypointsL.Count){
                        var targetPos=waypointsL[waypointIndex].transform.position;
                        var step=waveConfig.GetMoveSpeed()*Time.deltaTime;
                        transform.position=Vector2.MoveTowards(transform.position, targetPos, step);
                        //GetComponent<Enemy>().curSpeed=step;
                        if (transform.position==targetPos) waypointIndex++;
                        if(waypointIndex>=waypointsL.Count){waypointIndex=0;}
                    }else{waypointIndex=0;}
                }if(waveConfig.wavePathType==wavePathType.loopPathRandom){//Loop Path Random Point
                    if(waypointIndex<waypointsL.Count){
                        var targetPos=waypointsL[waypointIndex].transform.position;
                        var step=waveConfig.GetMoveSpeed()*Time.deltaTime;
                        transform.position=Vector2.MoveTowards(transform.position, targetPos, step);
                        //GetComponent<Enemy>().curSpeed=step;
                        if (transform.position==targetPos) waypointIndex=Random.Range(0,waypointsL.Count);
                        if(waypointIndex>=waypointsL.Count){waypointIndex=0;}
                    }else{waypointIndex=0;}
                }
            }
        }
    }}
}
