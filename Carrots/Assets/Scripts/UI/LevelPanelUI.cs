using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtLv, txtscore;
    [SerializeField] private Image[] imgCarrotStar;
    [SerializeField] private RectTransform rectScrollbar;
    [SerializeField] private LoadingScreen loadingScreen;

    private AudioManager instanceAM;
    private Vector2 anchor;
    private MapData map;
    // Start is called before the first frame update
    void Start()
    {
        instanceAM = AudioManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        anchor = rectScrollbar.anchorMax;
        HandleLevel((int)(anchor.x * 10f));
    }
    public void Init(MapData mapData,int levelHighScore)
    {
        map = mapData;
        //levelhigh = 0 là trường hợp mới mở khoá map chưa có điểm từ lv nào
        if (levelHighScore == 0)
        {
            rectScrollbar.anchorMax = new Vector2(1 / 10f, rectScrollbar.anchorMax.y);
            rectScrollbar.anchorMin = new Vector2(1 / 10f - 0.1f, rectScrollbar.anchorMin.y);
            return;
        }
        rectScrollbar.anchorMax = new Vector2(levelHighScore / 10f, rectScrollbar.anchorMax.y);
        rectScrollbar.anchorMin = new Vector2(levelHighScore / 10f - 0.1f, rectScrollbar.anchorMin.y);
    }
    private void HandleLevel(int lv)
    {
        txtLv.text = "Level : " + (lv);
        txtscore.text = "Score : " + map.GetScore(lv);
        //imgcarrotstar
        for (int i = 0; i < imgCarrotStar.Length; i++)
        {
            if (i <= map.GetCarrotStar(lv) - 1)
            {
                imgCarrotStar[i].gameObject.SetActive(true);
            }
            else
            {
                imgCarrotStar[i].gameObject.SetActive(false);
            }
        }
    }
    public  void Btn(bool exit)
    {
        instanceAM.ClickFx();
        if (exit)
        {
            gameObject.SetActive(false);
        }
        else
        {
            //loading screen
            PlayerPrefs.SetInt("LevelGame", (int)(anchor.x * 10f));
            loadingScreen.gameObject.SetActive(true);
            loadingScreen.LoadSceneGamePlay(map.typeMap);
        }
    }
}
