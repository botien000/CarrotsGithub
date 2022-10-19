using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceMusic;
    [SerializeField] private AudioSource audioSourceSFX;

    [SerializeField] private AudioClip home;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip getRightAnswer;
    [SerializeField] private AudioClip getWrongAnswer;
    [SerializeField] private AudioClip getItem;
    [SerializeField] private AudioClip useItem;
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip breakShield;

    private DataManager instanceDM;
    private int statusSound;
    private int statusMusic;

    /// <summary>
    /// Singleton 
    /// </summary>
    public static AudioManager instance;

    public int StatusSound { get => statusSound; set => statusSound = value; }
    public int StatusMusic { get => statusMusic; set => statusMusic = value; }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        instanceDM = DataManager.instance;
        GetSound();
        GetMusic();
        SetSoundSetting(false);
        SetMusicSetting(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void GetMusic()
    {
        statusMusic = DataManager.instance.GetMusic();
    }
    public void GetSound()
    {
        statusSound = instanceDM.GetSound();
    }
    public void SetSoundSetting(bool isPressed)
    {
        //khi nhấn nút
        if (isPressed)
        {
            if (statusSound == 1)
            {
                statusSound = 0;
            }
            else
            {
                statusSound = 1;
            }
        }
        SetMuteorPause(statusSound == 1 ? false : true, 1);
        //save db
        instanceDM.SetSound(statusSound);
    }
    public void SetMusicSetting(bool isPressed)
    {
        //khi nhấn nút
        if (isPressed)
        {
            if (statusMusic == 1)
            {
                statusMusic = 0;
            }
            else
            {
                statusMusic = 1;
            }
        }
        else
        {
        }
        SetMuteorPause(statusMusic == 1 ? false : true, 0);
        //save db
        instanceDM.SetMusic(statusMusic);
    }
    private void SetMuteorPause(bool mute, int type)
    {
        if (type == 0)
        {
            if (mute)
            {
                audioSourceMusic.Pause();
            }
            else
            {
                audioSourceMusic.Play();
            }
            instanceDM.SetMusic(mute ? 0 : 1);
            return;
        }
        audioSourceSFX.mute = mute;
        instanceDM.SetSound(mute ? 0 : 1);
    }
    #region SFx
    public void JumpFx()
    {
        audioSourceSFX.PlayOneShot(jump);
    }
    public void ClickFx()
    {
        audioSourceSFX.PlayOneShot(click);
    }
    public void GetItemFx()
    {
        audioSourceSFX.PlayOneShot(getItem);
    }
    public void UseItemFx()
    {
        audioSourceSFX.PlayOneShot(useItem);
    }
    public void RightAnswerFx()
    {
        audioSourceSFX.PlayOneShot(getRightAnswer);
    }
    public void WrongAnswerFx()
    {
        audioSourceSFX.PlayOneShot(getWrongAnswer);
    }
    public void GameOverFx()
    {
        audioSourceSFX.PlayOneShot(gameOver);
    }
    public void BreakShieldFx()
    {
        audioSourceSFX.PlayOneShot(breakShield);
    }
    #endregion
}
