using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HomeUI : MonoBehaviour
{
    [SerializeField] private Button[] buttonMaps;
    [SerializeField] private List<CarrotMap> carrotMaps;
    [SerializeField] private TextMeshProUGUI[] txtScoreMap;
    [SerializeField] private TextMeshProUGUI[] txtLevel;
    [SerializeField] private Sprite imgMusicOn, imgMusicOff;
    [SerializeField] private Image imgMusic;
    [SerializeField] private GameObject ExitGamePanel;  
    [SerializeField] private LevelPanelUI levelPanel;

    private List<MapData> maps;
    private DataManager instanceDM;
    private AudioManager instanceAM;
    // Start is called before the first frame update
    void Start()
    {
        instanceDM = DataManager.instance;
        instanceAM = AudioManager.instance;
        //call all map
        maps = instanceDM.GetDataAllMaps(5);
        HandleButtonMap();
        instanceAM.GetMusic();
        SetImageMusic(instanceAM.StatusMusic == 1 ? true : false);
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void HandleButtonMap()
    {
        if (maps.Count > buttonMaps.Length)
        {
            Debug.LogError("Bug logic.Fix please");
            return;
        }
        //Take high score
        for (int i = 0; i < buttonMaps.Length; i++)
        {
            //active button map
            if(maps[i] != null)
            {
                int[] high = maps[i].GetHighScore();
                HandleLevelMap(i, high[0].ToString()); 
                HandleScoreMap(i, high[1].ToString()); 
                HandleCarrotMap(i, high[2] - 1);
                buttonMaps[i].interactable = true;
            }
            //deactive button map
            else
            {
                HandleLevelMap(i, 0.ToString());
                HandleScoreMap(i, 0.ToString());
                HandleCarrotMap(i, -1);
                buttonMaps[i].interactable = false;
            }
        }
    }
    private void HandleScoreMap(int index, string txtScore)
    {
        txtScoreMap[index].text = "High Score : " + txtScore;
    }
    private void HandleLevelMap(int index, string txtLv)
    {
        txtLevel[index].text = "Level : " + txtLv;
    }
    private void HandleCarrotMap(int index, int indexCarrot)
    {
        for (int i = 0; i < carrotMaps[index].imgCarrots.Length; i++)
        {
            if (i <= indexCarrot)
            {
                carrotMaps[index].imgCarrots[i].gameObject.SetActive(true);
            }
            else
            {
                carrotMaps[index].imgCarrots[i].gameObject.SetActive(false);
            }
        }
    }
    public void BtnAudio()
    {
        instanceAM.SetMusicSetting(true);
        SetImageMusic(instanceAM.StatusMusic == 1 ? true : false);
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
    public void BtnExitGame()
    {   
        instanceAM.ClickFx();
        //mở panel exit game
        ExitGamePanel.SetActive(true);
    }
    public void BtnYes_No(bool y_n)
    {
        instanceAM.ClickFx();
        if (y_n)
        {
            //quit
            Application.Quit();
        }
        else
        {
            ExitGamePanel.SetActive(false);
        }
    }
    public void BtnMap(int level)
    {
        instanceAM.ClickFx();
        levelPanel.Init(maps[level - 1], maps[level - 1].GetHighScore()[0]);
        levelPanel.gameObject.SetActive(true);
    }
    [System.Serializable]
    class CarrotMap
    {
        public Image[] imgCarrots;
    }
}
