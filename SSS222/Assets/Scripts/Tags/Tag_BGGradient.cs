using UnityEngine;
using UnityEngine.SceneManagement;

public class Tag_BGGradient : MonoBehaviour{
    const bool disableInClassicMode = true;
    void Start(){
        if(false == SaveSerial.instance.settingsData.gradientBackgroundSides){
            SetEnabled(false);
        }
        if(GameManager.instance != null){
            if((SceneManager.GetActiveScene().name == "Game" || SceneManager.GetActiveScene().name == "InfoGameMode")
                && GameManager.instance.CheckGamemodeSelected("Classic") && disableInClassicMode){
                SetEnabled(false);
            }
        }
    }
    public void SetEnabled(bool enabled){
        transform.GetChild(0).gameObject.SetActive(enabled);
    }
}
