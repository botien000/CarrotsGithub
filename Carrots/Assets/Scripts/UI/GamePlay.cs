using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GamePlay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtScore;
    [SerializeField] private Image[] imgCarrotPoint;
    [SerializeField] private TextMeshProUGUI txtQuestion;
    [SerializeField] private Image[] imgHeart;
    [SerializeField] private float timeX2;
    [SerializeField] private Image imgFillX2;
    [SerializeField] private float timeSlow;
    [SerializeField] private Image imgFillSlow;


    private AudioManager instanceAM;
    private GameManager instanceGM;
    private int curIndexCarrotPoint;
    private bool triggerX2;
    private float curTimeX2;
    private bool triggerSlow;
    private float curTimeSlow;
    private void OnEnable()
    {
        ClearUI();
    }
    // Start is called before the first frame update
    void Start()
    {
        curIndexCarrotPoint = 0;
        instanceAM = AudioManager.instance;
        instanceGM = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (triggerX2)
        {
            curTimeX2 -= Time.deltaTime;
            imgFillX2.fillAmount = curTimeX2 * 1 / timeX2;
            if (curTimeX2 <= 0)
            {
                instanceGM.SetScoreFromItem(1);
                triggerX2 = false;
                curTimeX2 = timeX2;
                imgFillX2.gameObject.SetActive(false);
            }
        }
        if (triggerSlow)
        {
            curTimeSlow -= Time.deltaTime;
            imgFillSlow.fillAmount = curTimeSlow * 1 / timeSlow;
            if (curTimeSlow <= 0)
            {
                instanceGM.SetSlowInGameFromItem(1);
                triggerSlow = false;
                curTimeSlow = timeSlow;
                imgFillSlow.gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// Hiện điểm trên UI
    /// </summary>
    /// <param name="score">Score</param>
    public void SetTextScore(int score)
    {
        txtScore.text = "Score:" + score.ToString();
    }
    /// <summary>
    /// Hiện điểm cà rốt trên UI
    /// </summary>
    /// <param name="curPoint">curPoint</param>
    /// <param name="totalPoint">totalPoint</param>
    public void SetImgCarrotPoint(int curTurn, int totalTurn)
    {
        if (curTurn > totalTurn)
        {
            Debug.LogError("Out total.Fix bug");
            return;
        }
        imgCarrotPoint[curIndexCarrotPoint].fillAmount += (float)1 / (totalTurn / imgCarrotPoint.Length);
        if (imgCarrotPoint[curIndexCarrotPoint].fillAmount >= 1f && curIndexCarrotPoint < imgCarrotPoint.Length)
        {
            //fill next img
            curIndexCarrotPoint++;
        }
    }
    public void ShowTextQuestion(string question)
    {
        txtQuestion.text += question;
    }

    public void ClearText()
    {
        txtQuestion.text = "";
    }
    public void BtnSetting()
    {
        instanceAM.ClickFx();
        instanceGM.SetState(GameManager.StateGame.GameSetting);
    }
    private void ClearUI()
    {
        SetTextScore(0);
        foreach (var img in imgCarrotPoint)
        {
            img.fillAmount = 0;
        }
    }
    public void ShowHeartPlayer(int heart)
    {
        if (heart <= imgHeart.Length)
        {
            for (int i = 0; i < imgHeart.Length; i++)
            {
                if (i < heart)
                {
                    imgHeart[i].gameObject.SetActive(true);
                }
                else
                {
                    imgHeart[i].gameObject.SetActive(false);
                }
            }
        }
    }
    public void TimeX2()
    {
        curTimeX2 = timeX2;
        triggerX2 = true;
        imgFillX2.gameObject.SetActive(true);
    }
    public void TimeSlow()
    {
        curTimeSlow = timeSlow;
        triggerSlow = true;
        imgFillSlow.gameObject.SetActive(true);
    }
    public int GetIndexCarrotPoint()
    {
        return curIndexCarrotPoint;
    }
}
