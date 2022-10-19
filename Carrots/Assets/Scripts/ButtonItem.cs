using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonItem : MonoBehaviour
{
    private Image image;
    private CategoryItemSctbObj category;
    private GameManager instanceGM;
    private AudioManager instanceAM;
    private ButtonItemManager instanceBtnItemM;
    private User player;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<User>();
        instanceBtnItemM = ButtonItemManager.instance;
        instanceGM = GameManager.instance;
        instanceAM = AudioManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BtnItemClicked()
    {
        instanceAM.UseItemFx();
        //HandleItem
        switch (category.curCategory)
        {
            case CategoryItemSctbObj.Category.HalfAnswer:
                HalfAnswer();
                break;
            case CategoryItemSctbObj.Category.Shield:
                Shield();
                break;
            case CategoryItemSctbObj.Category.TransAnswer:
                TransAnswer();
                break;
            case CategoryItemSctbObj.Category.DoublePoint:
                DoublePoint();
                break;
            case CategoryItemSctbObj.Category.SlowSpeed:
                SlowSpeed();
                break;
            case CategoryItemSctbObj.Category.Heart:
                Heart();
                break;
        }
        //RemoveButton
        instanceBtnItemM.RemoveObjInPool(this);
    }


    public void Init(CategoryItemSctbObj cate)
    {
        category = cate;
        image.sprite = category.sprite;
    }
    public CategoryItemSctbObj GetCategory()
    {
        return category;
    }
    /// <summary>
    /// Giảm nửa đáp án (cụ thể là 2 đáp án)
    /// </summary>
    /// Thiếu bắt điều kiện nếu trong kho có 2 item 50/50 trở lên thì chỉ cho chọn 1 item 
    private void HalfAnswer()
    {
        //2 biến sẽ loại bỏ phần text của answer
        bool clearAll = false;
        int one, two;
        List<Answer> wrongAnswers = new List<Answer>();
        Answer[] answers = instanceGM.TakeAnswers();
        foreach (var answer in answers)
        {
            if (!answer.GetAnswer())
            {
                wrongAnswers.Add(answer);
            }
        }
        foreach (var answer in wrongAnswers)
        {
            if(answer.GetText() == "")
            {
                clearAll = true;
                break;
            }
        }
        if (clearAll)
        {
            foreach (var answer in wrongAnswers)
            {
                answer.ClearTextFromItem();
            }
            return;
        }
        one = Mathf.RoundToInt(Random.Range(1, wrongAnswers.Count));
        do
        {
            two = Mathf.RoundToInt(Random.Range(1, wrongAnswers.Count));
        } while (two == one);
        wrongAnswers[one].ClearTextFromItem();
        wrongAnswers[two].ClearTextFromItem();
    }
    /// <summary>
    /// Khiên bảo vệ
    /// </summary>
    private void Shield()
    {
        player.Shield(true);
    }
    /// <summary>
    /// Đổi đáp án
    /// </summary>
    /// Chưa làm gì
    private void TransAnswer()
    {
        instanceGM.RemoveAnswers(null,-1,true);
    }
    /// <summary>
    /// Nhân đôi điểm
    /// </summary>
    /// Thiếu thời gian và hình ảnh
    private void DoublePoint()
    {
        //Sẽ có thời gian giới hạn cho nhân đôi điểm   
        //x2
        instanceGM.SetScoreFromItem(2);
    }
    private void Heart()
    {
        player.GetHeartFromItem();
    }

    private void SlowSpeed()
    {
        //slow x2
        instanceGM.SetSlowInGameFromItem(2);
    }

}
