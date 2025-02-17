﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using TMPro;
using Sirenix.OdinInspector;

public class SettingsMenu : MonoBehaviour{      public static SettingsMenu instance;
    [SerializeField] int panelActive=0;
    [SerializeField] GameObject[] panels;
    [Title("Game")]
    [SceneObjectsOnly][SerializeField]GameObject steeringButton;
    [SceneObjectsOnly][SerializeField]Toggle vibrationsToggle;
    [SceneObjectsOnly][SerializeField]Toggle horizPlayfieldToggle;
    [SceneObjectsOnly][SerializeField]Toggle dtapMouseShootToggle;
    [SceneObjectsOnly][SerializeField]Toggle lefthandToggle;
    [SceneObjectsOnly][SerializeField]Toggle scbuttonsToggle;
    [SceneObjectsOnly][SerializeField]Toggle discordRPCToggle;
    [SceneObjectsOnly][SerializeField]Toggle autoselectNewItemToggle;
    [SceneObjectsOnly][SerializeField]Toggle alwaysReplaceCurrentSlotToggle;
    [SceneObjectsOnly][SerializeField]Toggle autoUseMedkitsIfLowToggle;
    [SceneObjectsOnly][SerializeField]Toggle allowSelectingEmptySlotsToggle;
    [SceneObjectsOnly][SerializeField]Toggle allowScrollingEmptySlotsToggle;
    [SceneObjectsOnly][SerializeField]Toggle cheatToggle;
    [Title("Sound")]
    public AudioMixer audioMixer;
    [SceneObjectsOnly][SerializeField]Slider masterSlider;
    [SceneObjectsOnly][SerializeField]Slider masterOOFSlider;
    [SceneObjectsOnly][SerializeField]Slider soundSlider;
    [SceneObjectsOnly][SerializeField]Slider ambienceSlider;
    [SceneObjectsOnly][SerializeField]Slider musicSlider;
    [SceneObjectsOnly][SerializeField]Toggle musicWinddownToggle;
    [SceneObjectsOnly][SerializeField]Toggle turnUpBossMusicToggle;
    
    [Title("Graphics")]
    [SceneObjectsOnly][SerializeField]TMP_Dropdown windowModeDropdown;
    [SceneObjectsOnly][SerializeField]TMP_Dropdown resolutionDropdown;
    //[SceneObjectsOnly][SerializeField]Toggle fullscreenToggle;
    [SceneObjectsOnly][SerializeField]Toggle vSyncToggle;
    [SceneObjectsOnly][SerializeField]Toggle lockCursorToggle;
    [SceneObjectsOnly][SerializeField]TMP_Dropdown qualityDropdopwn;
    [SceneObjectsOnly][SerializeField]Toggle pprocessingToggle;
    [SceneObjectsOnly][SerializeField]Toggle screenshakeToggle;
    [SceneObjectsOnly][SerializeField]Toggle dmgPopupsToggle;
    [SceneObjectsOnly][SerializeField]Toggle particlesToggle;
    [SceneObjectsOnly][SerializeField]Toggle screenflashToggle;
    [SceneObjectsOnly][SerializeField]Toggle playerWeaponsFadeToggle;

    [SceneObjectsOnly][SerializeField]Toggle classicHudToggle;
    [SceneObjectsOnly][SerializeField]Slider hudVis_graphicsSlider;
    [SceneObjectsOnly][SerializeField]Slider hudVis_textSlider;
    [SceneObjectsOnly][SerializeField]Slider hudVis_barTextSlider;
    [SceneObjectsOnly][SerializeField]Slider hudVis_barsSlider;
    [SceneObjectsOnly][SerializeField]Slider hudVis_absorpSlider;
    [SceneObjectsOnly][SerializeField]Slider hudVis_popupsSlider;
    [SceneObjectsOnly][SerializeField]Slider hudVis_notifsSlider;

    [AssetsOnly][SerializeField]GameObject pprocessingPrefab;
    [SceneObjectsOnly]public PostProcessVolume postProcessVolume;
    SaveSerial.SettingsData settingsData;
    void Start(){   instance=this;if(SaveSerial.instance!=null){settingsData=SaveSerial.instance.settingsData;}

        scbuttonsToggle.isOn=settingsData.scbuttons;
        dtapMouseShootToggle.isOn=settingsData.dtapMouseShoot;
        lefthandToggle.isOn=settingsData.lefthand;
        vibrationsToggle.isOn=settingsData.vibrations;
        discordRPCToggle.isOn=settingsData.discordRPC;
        autoselectNewItemToggle.isOn=settingsData.autoselectNewItem;
        alwaysReplaceCurrentSlotToggle.isOn=settingsData.alwaysReplaceCurrentSlot;
        autoUseMedkitsIfLowToggle.isOn=settingsData.autoUseMedkitsIfLow;
        allowSelectingEmptySlotsToggle.isOn=settingsData.allowSelectingEmptySlots;
        allowScrollingEmptySlotsToggle.isOn=settingsData.allowScrollingEmptySlots;
        cheatToggle.isOn=GameManager.instance.cheatmode;
        bool h=false;if(settingsData.playfieldRot==PlaneDir.horiz){h=true;}horizPlayfieldToggle.isOn=h;


        masterSlider.value=settingsData.masterVolume;
        masterOOFSlider.value=settingsData.masterOOFVolume;
        soundSlider.value=settingsData.soundVolume;
        ambienceSlider.value=settingsData.ambienceVolume;
        musicSlider.value=settingsData.musicVolume;
        musicWinddownToggle.isOn=settingsData.windDownMusic;
        turnUpBossMusicToggle.isOn=settingsData.bossVolumeTurnUp;

        SetAvailableResolutions();
        windowModeDropdown.value=settingsData.windowMode;
        //fullscreenToggle.isOn=settingsData.fullscreen;
        vSyncToggle.isOn=settingsData.vSync;
        lockCursorToggle.isOn=settingsData.lockCursor;
        qualityDropdopwn.value=settingsData.quality;
        pprocessingToggle.isOn=settingsData.pprocessing;
        screenshakeToggle.isOn=settingsData.screenshake;
        dmgPopupsToggle.isOn=settingsData.dmgPopups;
        particlesToggle.isOn=settingsData.particles;
        screenflashToggle.isOn=settingsData.screenflash;
        playerWeaponsFadeToggle.isOn=settingsData.playerWeaponsFade;

        classicHudToggle.isOn=settingsData.classicHUD;
        hudVis_graphicsSlider.value=settingsData.hudVis_graphics;
        hudVis_textSlider.value=settingsData.hudVis_text;
        hudVis_barTextSlider.value=settingsData.hudVis_barText;
        hudVis_barsSlider.value=settingsData.hudVis_barFill;
        hudVis_absorpSlider.value=settingsData.hudVis_absorpFill;
        hudVis_popupsSlider.value=settingsData.hudVis_popups;
        hudVis_notifsSlider.value=settingsData.hudVis_notif;
        if(SceneManager.GetActiveScene().name=="Options")OpenSettings();
        SetPanelActive(0);
        foreach(Scrollbar sc in GetComponentsInChildren<Scrollbar>()){
            sc.value=1;
        }
    }
    void Update(){postProcessVolume=FindObjectOfType<PostProcessVolume>();
        if(settingsData.pprocessing==true&&postProcessVolume==null){postProcessVolume=Instantiate(pprocessingPrefab,Camera.main.transform).GetComponent<PostProcessVolume>();}
        if(settingsData.pprocessing==true&&FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume.enabled=true;}
        if(settingsData.pprocessing==false&&FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume=FindObjectOfType<PostProcessVolume>();postProcessVolume.enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
        if(settingsData.masterVolume<=-50){settingsData.masterVolume=-80;}
        if(settingsData.soundVolume<=-50){settingsData.soundVolume=-80;}
        if(settingsData.ambienceVolume<=-50){settingsData.ambienceVolume=-80;}
        if(settingsData.musicVolume<=-50){settingsData.musicVolume=-80;}

        if(GameCanvas.instance!=null){
            if(!GameCanvas._isPossibleToUpscaleHud()){classicHudToggle.interactable=false;classicHudToggle.isOn=true;}
            else{if(SaveSerial.instance!=null)if(SaveSerial.instance.settingsData!=null)classicHudToggle.interactable=true;classicHudToggle.isOn=SaveSerial.instance.settingsData.classicHUD;}
        }
        if(GSceneManager.EscPressed()){Back();}
    }
    public void SetPanelActive(int i){panelActive=i;foreach(GameObject p in panels){p.SetActive(false);}panels[panelActive].SetActive(true);}
    public void OpenSettings(){transform.GetChild(0).gameObject.SetActive(true);transform.GetChild(1).gameObject.SetActive(false);}
    public void OpenDeleteAll(){transform.GetChild(1).gameObject.SetActive(true);transform.GetChild(0).gameObject.SetActive(false);}
    public void Close(){SaveSerial.instance.SaveSettings();transform.GetChild(0).gameObject.SetActive(false);transform.GetChild(1).gameObject.SetActive(false);}
    public void Back(){
        if(transform.GetChild(1).gameObject.activeSelf){OpenSettings();return;}
        else{
            if(SceneManager.GetActiveScene().name=="Options"){GSceneManager.instance.LoadStartMenu();SaveSerial.instance.SaveSettings();}
            else if(SceneManager.GetActiveScene().name=="Game"&&PauseMenu.GameIsPaused){Close();PauseMenu.instance.Pause();}
        }
    }


    #region//Game
    public void SetOnScreenButtons(bool isOn){settingsData.scbuttons=isOn;}
    public void SetVibrations(bool isOn){settingsData.vibrations=isOn;}
    public void SetDiscordRPC(bool isOn){settingsData.discordRPC=isOn;}
    public void SetAutoselectNewItem(bool isOn){settingsData.autoselectNewItem=isOn;
        if(isOn){
            if(settingsData.alwaysReplaceCurrentSlot){settingsData.alwaysReplaceCurrentSlot=false;alwaysReplaceCurrentSlotToggle.isOn=false;}
        }
    }
    public void SetAlwaysReplaceCurrentSlot(bool isOn){settingsData.alwaysReplaceCurrentSlot=isOn;
        if(isOn){
            if(settingsData.autoselectNewItem){settingsData.autoselectNewItem=false;autoselectNewItemToggle.isOn=false;}
        }
    }   
    public void SetAutouseMedkitsIfLow(bool isOn){settingsData.autoUseMedkitsIfLow=isOn;}
    public void SetAllowSelectingEmptySlots(bool isOn){settingsData.allowSelectingEmptySlots=isOn;
        if(isOn){
            if(settingsData.allowScrollingEmptySlots&&settingsData.alwaysReplaceCurrentSlot){settingsData.alwaysReplaceCurrentSlot=false;alwaysReplaceCurrentSlotToggle.isOn=false;}
        }
    }
    public void SetAllowScrollingEmptySlots(bool isOn){settingsData.allowScrollingEmptySlots=isOn;
        if(isOn){
            if(settingsData.allowSelectingEmptySlots&&settingsData.alwaysReplaceCurrentSlot){settingsData.alwaysReplaceCurrentSlot=false;alwaysReplaceCurrentSlotToggle.isOn=false;}
        }
    }
    public void SetPlayfieldRot(bool horiz){
        PlaneDir h=PlaneDir.vert;if(horiz==true){h=PlaneDir.horiz;}settingsData.playfieldRot=h;
        if(SceneManager.GetActiveScene().name=="Game"&&Application.isPlaying){
            if(h==PlaneDir.horiz){FindObjectOfType<Camera>().transform.localEulerAngles=new Vector3(0,0,90);FindObjectOfType<Camera>().orthographicSize=GameManager.instance.horizCameraSize;}
            else{FindObjectOfType<Camera>().transform.localEulerAngles=new Vector3(0,0,0);FindObjectOfType<Camera>().orthographicSize=GameManager.instance.vertCameraSize;}
        }
    }
    public void SetSteering(){
        switch(settingsData.inputType){
            case (InputType)0:
                settingsData.inputType=(InputType)1;
                break;
            case (InputType)1:
                if(GameManager.instance.cheatmode)settingsData.inputType=(InputType)2;
                else settingsData.inputType=(InputType)0;
                break;
            case (InputType)2:
                if(GameManager.instance.cheatmode)settingsData.inputType=(InputType)3;
                else settingsData.inputType=(InputType)0;
                break;
            case (InputType)3:
                settingsData.inputType=(InputType)0;
                break;
        }
    }
    public void SetJoystick(){
        switch(settingsData.joystickType){
            case (JoystickType)0:
                settingsData.joystickType=(JoystickType)1;
                break;
            case (JoystickType)1:
                settingsData.joystickType=(JoystickType)2;
                break;
            case (JoystickType)2:
                settingsData.joystickType=(JoystickType)0;
                break;
        }
        if(FindObjectOfType<Tag_Joystick>()!=null)FindObjectOfType<Tag_Joystick>().StartCoroutine(FindObjectOfType<Tag_Joystick>().ChangeType());
    }
    public void SetJoystickSize(float size){settingsData.joystickSize=size;}
    public void SetLefthand(bool isOn){
        settingsData.lefthand=isOn;
        if(FindObjectOfType<SwitchPlacesCanvas>()!=null)FindObjectOfType<SwitchPlacesCanvas>().Set();
    }
    public void SetDTapMouse(bool isOn){settingsData.dtapMouseShoot=isOn;}
    public void SetCheatmode(bool isOn){if(GameManager.instance!=null)GameManager.instance.cheatmode=isOn;}
    #endregion


    #region//Sound
    public void SetMasterVolume(float val){settingsData.masterVolume=(float)System.Math.Round(val,2);if(val>1.15f)StatsAchievsManager.instance.DeepFried();}
    public void SetMasterOOFVolume(float val){settingsData.masterOOFVolume=(float)System.Math.Round(val,2);}
    public void SetSoundVolume(float val){settingsData.soundVolume=(float)System.Math.Round(val,2);if(val>1.15f)StatsAchievsManager.instance.DeepFried();}
    public void SetAmbienceVolume(float val){settingsData.ambienceVolume=(float)System.Math.Round(val,2);if(val>1.15f)StatsAchievsManager.instance.DeepFried();}
    public void SetMusicVolume(float val){settingsData.musicVolume=(float)System.Math.Round(val,2);if(val>1.15f)StatsAchievsManager.instance.DeepFried();}
    public void SetMusicWinddown(bool isOn){settingsData.windDownMusic=isOn;}
    public void SetMusicBossTurnUp(bool isOn){settingsData.bossVolumeTurnUp=isOn;}
    #endregion


    #region//Graphics
    public void SetQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
        settingsData.quality=qualityIndex;
        if(qualityIndex<=1){
            settingsData.pprocessing=false;
            settingsData.dmgPopups=false;
            pprocessingToggle.isOn=false;
            dmgPopupsToggle.isOn=false;
        }if(qualityIndex==0){
            settingsData.screenshake=false;
            settingsData.particles=false;
            settingsData.screenflash=false;
            screenshakeToggle.isOn=false;
            particlesToggle.isOn=false;
            screenflashToggle.isOn=false;
        }if(qualityIndex>1){
            settingsData.particles=true;
            particlesToggle.isOn=true;
        }if(qualityIndex>4){
            settingsData.pprocessing=true;
            pprocessingToggle.isOn=true;
        }
    }
    public void SetScreenshake(bool isOn){settingsData.screenshake=isOn;}
    public void SetDmgPopups(bool isOn){settingsData.dmgPopups=isOn;}
    public void SetParticles(bool isOn){settingsData.particles=isOn;}
    public void SetScreenflash(bool isOn){settingsData.screenflash=isOn;}
    public void SetPlayerWeaponsFade(bool isOn){settingsData.playerWeaponsFade=isOn;}
    public static FullScreenMode GetFullScreenMode(int id){
        switch(id){
            case 0:return FullScreenMode.ExclusiveFullScreen;
            case 1:return FullScreenMode.FullScreenWindow;
            case 2:return FullScreenMode.Windowed;
            default:return FullScreenMode.ExclusiveFullScreen;
        }
    }
    public static int GetFullScreenModeID(FullScreenMode fullScreenMode){
        switch(fullScreenMode){
            case FullScreenMode.ExclusiveFullScreen:return 0;
            case FullScreenMode.FullScreenWindow:return 1;
            case FullScreenMode.Windowed:return 2;
            default:return 0;
        }
    }
    public void SetWindowMode(int id){
        Screen.SetResolution(settingsData.resolution.x,settingsData.resolution.y,GetFullScreenMode(settingsData.windowMode));
        settingsData.windowMode=id;
    }
    public void SetVSync(bool isOn){
        QualitySettings.vSyncCount=AssetsManager.BoolToInt(isOn);
        settingsData.vSync=isOn;
    }
    public void SetLockCursor(bool isOn){
        Cursor.lockState=(CursorLockMode)(AssetsManager.BoolToInt(isOn)*2);
        settingsData.lockCursor=isOn;
    }
    public void SetResolution(int resIndex){
        Vector2Int _res=_availableResolutionsList[resIndex];
        Screen.SetResolution(_res.x,_res.y,GetFullScreenMode(settingsData.windowMode));
        settingsData.resolution=_res;
    }
    public void SetPostProcessing(bool isOn){
        postProcessVolume=FindObjectOfType<PostProcessVolume>();
        settingsData.pprocessing=isOn;
        if(isOn==true&&postProcessVolume==null){postProcessVolume=Instantiate(pprocessingPrefab,Camera.main.transform).GetComponent<PostProcessVolume>();}//FindObjectOfType<Level>().RestartScene();}
        if(isOn==true&&postProcessVolume!=null){postProcessVolume.enabled=true;}
        if(isOn==false&&FindObjectOfType<PostProcessVolume>()!=null){FindObjectOfType<PostProcessVolume>().enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
    }

    public void SetClassicHud(bool val){if(GameCanvas._isPossibleToUpscaleHud()){settingsData.classicHUD=val;if(GameCanvas.instance!=null){GameCanvas.instance.ChangeHUDAligment();}}}
    public void SetHudVis_Graphics(float val){settingsData.hudVis_graphics=val;}
    public void SetHudVis_Text(float val){settingsData.hudVis_text=val;}
    public void SetHudVis_BarText(float val){settingsData.hudVis_barText=val;}
    public void SetHudVis_BarFill(float val){settingsData.hudVis_barFill=val;}
    public void SetHudVis_AbsorpFill(float val){settingsData.hudVis_absorpFill=val;}
    public void SetHudVis_Popups(float val){settingsData.hudVis_popups=val;}
    public void SetHudVis_Notif(float val){settingsData.hudVis_notif=val;}
    #endregion
    

    public void PlayDing(){if(Application.isPlaying)GetComponent<AudioSource>().Play();}
    public void PlayDingOOF(){audioMixer.SetFloat("MasterVolume", AssetsManager.InvertNormalizedMin(settingsData.masterOOFVolume,-50));if(Application.isPlaying)GetComponent<AudioSource>().Play();}

    void SetAvailableResolutions(){
        _availableResolutionsList.Clear();
        resolutionDropdown.ClearOptions();
        foreach(Vector2Int r in resolutionsList){
            if(Display.main.systemWidth>Display.main.systemHeight){//Horizontal
                if(r.x>r.y){
                    if(r.x<=Display.main.systemWidth&&r.y<=Display.main.systemHeight){
                        _availableResolutionsList.Add(r);
                        resolutionDropdown.AddOptions(new List<string>(){(r.x+"x"+r.y)});
                    }
                }
            }else{//Vertical
                if(r.y>r.x){
                    if(r.x<=Display.main.systemWidth&&r.y<=Display.main.systemHeight){
                        _availableResolutionsList.Add(r);
                        resolutionDropdown.AddOptions(new List<string>(){(r.x+"x"+r.y)});
                    }
                }
            }
        }
        if(_availableResolutionsList.Count==0){foreach(Vector2Int r in forcedResolutionsList){
            _availableResolutionsList.Add(r);
            resolutionDropdown.AddOptions(new List<string>(){(r.x+"x"+r.y)});
        }}

        Vector2Int _currentRes=SaveSerial.instance.settingsData.resolution;
        if(!_availableResolutionsList.Contains(_currentRes)){
            _availableResolutionsList.Insert(0,_currentRes);
            List<TMP_Dropdown.OptionData> _optionsCache=new List<TMP_Dropdown.OptionData>();
            for(var i=0;i<resolutionDropdown.options.Count;i++){_optionsCache.Add(resolutionDropdown.options[i]);}//Cache old options

            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(new List<string>(){(_currentRes.x+"x"+_currentRes.y)});//Add current on top
            resolutionDropdown.AddOptions(_optionsCache);
        }

        resolutionDropdown.value=_availableResolutionsList.FindIndex(e=>e==settingsData.resolution);
    }
    [DisableInEditorMode]public List<Vector2Int> _availableResolutionsList;
    [HideInEditorMode]public List<Vector2Int> resolutionsList=new List<Vector2Int>(){
        //16:9
        new Vector2Int(3840,2160),
        new Vector2Int(2560,1440),
        new Vector2Int(1920,1080),
        new Vector2Int(1600,900),
        new Vector2Int(1366,768),
        new Vector2Int(1280,720),
        
        //16:10
        new Vector2Int(1920,1200),
        new Vector2Int(1680,1050),
        new Vector2Int(1440,900),
        new Vector2Int(1280,800),

        //4:3
        new Vector2Int(1024,768),
        new Vector2Int(800,600),
        new Vector2Int(640,480),

        //3:2
        new Vector2Int(2160,1440),
        new Vector2Int(1440,960),

        //21:9
        new Vector2Int(2560,1080),

        //9:16
        new Vector2Int(1080,1920),
        new Vector2Int(720,1280),
    };
    [HideInEditorMode]public List<Vector2Int> forcedResolutionsList=new List<Vector2Int>(){
        new Vector2Int(1920,1080),
        new Vector2Int(640,480),
        new Vector2Int(1080,1920),
    };
}
