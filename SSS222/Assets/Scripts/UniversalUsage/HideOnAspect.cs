using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnAspect : MonoBehaviour{
    [SerializeField] float aspectToCompare = 0.56f; // 9/16 (0.5625)
    [SerializeField] bool moreThan = false;
    void Start(){
        float aspect = Screen.width/Screen.height;
        if(aspect <= aspectToCompare && !moreThan){gameObject.SetActive(false);}
        else if(aspect >= aspectToCompare && moreThan){gameObject.SetActive(false);}
    }
}
