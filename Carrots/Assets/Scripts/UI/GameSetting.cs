using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSetting : MonoBehaviour
{
    [SerializeField] private Sprite imgSoundOn, imgSoundOff;
    [SerializeField] private Image imgSound;
    [SerializeField] private Sprite imgMusicOn, imgMusicOff;
    [SerializeField] private Image imgMusic;
    [SerializeField] private LoadingScreen loadingScreen;

    private AudioManager instanceAM;
    private GameManager instanceGM;
    // Start is called before the first frame update
    void Start()
    {
        instanceAM = AudioManager.instance;
        instanceGM = GameManager.instance;
        instanceAM.GetMusic();
        instanceAM.GetSound();
        SetImageMusic(instanceAM.StatusMusic == 1 ? true : false);
        SetImageSound(instanceAM.StatusSound == 1 ? true : false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BtnHome()
    {
        instanceAM.ClickFx();
        //SceneManager.LoadScene("Home");
        loadingScreen.gameObject.SetActive(true);
        loadingScreen.LoadSceneHome();
    }
    public void BtnRestart()
    {
        instanceAM.ClickFx();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BtnExit()
    {
        instanceAM.ClickFx();
        instanceGM.FromSettingToPlay();
    }
    public void BtnSound()
    {
        instanceAM.SetSoundSetting(true);
        instanceAM.ClickFx();
        SetImageSound(instanceAM.StatusSound == 1 ? true : false);
    }
    public void BtnMusic()
    {
        instanceAM.SetMusicSetting(true);
        instanceAM.ClickFx();
        SetImageMusic(instanceAM.StatusMusic == 1 ? true : false);
    }
    public void SetImageSound(bool on)
    {
        if (on)
        {
            imgSound.sprite = imgSoundOn;
        }
        else
        {
            imgSound.sprite = imgSoundOff;
        }
    }
    public void SetImageMusic(bool on)
    {
        if (on)
        {
            imgMusic.sprite = imgMusicOn;
        }
        else
        {
            imgMusic.sprite = imgMusicOff;
        }
    }
    public void PointUpSetting(BaseEventData baseEvent)
    {
        PointerEventData pointerEvent = baseEvent as PointerEventData;
        if (pointerEvent.pointerEnter == gameObject)
        {
            instanceAM.ClickFx();
            instanceGM.FromSettingToPlay();
        }
    }
}
