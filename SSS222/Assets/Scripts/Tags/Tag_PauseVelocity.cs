using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_PauseVelocity : MonoBehaviour{
    public Vector2 velPaused;
    Rigidbody2D rb;
    void Start() {
        rb=GetComponent<Rigidbody2D>();
    }
    void Update(){
        if(rb.linearVelocity!=Vector2.zero){velPaused=rb.linearVelocity;}
        if(GameManager.GlobalTimeIsPaused){if(rb.linearVelocity!=Vector2.zero){velPaused=rb.linearVelocity;rb.linearVelocity=Vector2.zero;}}else{if(velPaused!=Vector2.zero)rb.linearVelocity=velPaused;}
    }
    public static Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDistanceDelta){if(!GameManager.GlobalTimeIsPaused){return Vector2.MoveTowards(current,target,maxDistanceDelta);}else{return current;}}
}
