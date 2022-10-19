using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class User : MonoBehaviour
{
    enum State
    {
        Move, JumpLeft = -1, JumpRight = 1, LoseEmoji, WinEmoji
    }
    [SerializeField] private Transform[] transforms;
    [SerializeField] private Transform posFlyScore;
    [SerializeField] private int heart;
    [SerializeField] private float speedJump;
    [SerializeField] private GameObject shieldGO;


    private State curState;
    private Animator animator;
    private GameManager instanceGM;
    private AudioManager instanceAM;
    private Icon icon;
    private int curIndexPos;
    private int curHeart;
    private Vector3 origin;
    private bool touch, shield = false;
    private int from, to;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        instanceGM = GameManager.instance;
        instanceAM = AudioManager.instance;
        curHeart = heart;
        curIndexPos = Mathf.RoundToInt(Random.Range(0f, (transforms.Length - 1) * 1f));
        icon = GetComponentInChildren<Icon>();
        icon.gameObject.SetActive(false);
        transform.position = transforms[curIndexPos].position;
        shieldGO.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transforms[curIndexPos].position, speedJump * Time.deltaTime);
        //vì lí do lỗi vị trí liên quan đến recttransform nên dùng 2 biến float from và to để check
        from = Mathf.FloorToInt(transform.position.x * 100);
        to = Mathf.FloorToInt(transforms[curIndexPos].position.x * 100);
        if (from == to)
        {
            SetAnimation(State.Move);
        }
        if (instanceGM.IsPause)
        {
            return;
        }
        else
        {
            GetTouchMove();
        }


    }
    private void GetTouchMove()
    {
        //handle swipe main character
        //nhận chạm đầu tiên
        if (Input.GetMouseButtonDown(0))
        {
            origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touch = true;
        }
        //nhận chạm đầu tiên sau đó vuốt màn hình
        if (Input.GetMouseButton(0))
        {

            Vector3 swipe = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dirSwipe = origin - swipe;
            Vector3 dirVector = swipe - origin;
            float angleSwipe = Mathf.Atan2(dirVector.y, dirVector.x) * Mathf.Rad2Deg;
            if (dirSwipe.x < 0f && touch && (angleSwipe <= 45 && angleSwipe >= -45))
            {

                touch = false;
                GetPos(1);
            }
            else if (dirSwipe.x > 0f && touch && (angleSwipe <= -135 || angleSwipe >= 135))
            {
                touch = false;
                GetPos(-1);
            }
        }
        //trường hợp này khi chạm không vuốt touch return false
        if (Input.GetMouseButtonUp(0))
        {
            touch = false;
        }
    }
    private void GetPos(int index)
    {
        // điều kiện để nhảy sang ô tiếp theo chỉ được khi vị trí của player đã vào đúng chỗ
        if (from != to)
        {
            return;
        }
        curIndexPos += index;
        if (curIndexPos == transforms.Length || curIndexPos < 0)
        {
            curIndexPos -= index;
            return;
        }
        SetDirection(index);
    }
    private void SetDirection(int dir)
    {
        //left
        if (dir < 0)
        {
            SetAnimation(State.JumpLeft);
        }
        //right
        else if (dir > 0)
        {
            SetAnimation(State.JumpRight);
        }

        //audio
        instanceAM.JumpFx();
    }
    private void GetIcon(bool type)
    {
        icon.gameObject.SetActive(true);
        icon.Init(type);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            Item item = collision.GetComponent<Item>();
            instanceAM.GetItemFx();
            item.DieByPlayer();
        }
        else if (collision.gameObject.tag == "Answer")
        {
            Answer answer = collision.GetComponent<Answer>();
            //nếu bằng true thì mới cho phép tương tác xử lý
            if (answer.Interact)
            {
                bool isRight = answer.GetAnswer();
                Result(isRight);
                instanceGM.RemoveAnswers(answer, curHeart, isRight);
            }
        }
    }
    /// <summary>
    /// Kiểm tra đáp án người chơi vừa chọn là sai hay đúng
    /// </summary>
    /// <param name="rightAnswer"></param>
    private void Result(bool rightAnswer)
    {
        if (rightAnswer == true)
        {
            instanceGM.SetScoreGamePlay();
            instanceGM.ShowFlyScore(posFlyScore);
            //audio
            instanceAM.RightAnswerFx();
            GetIcon(rightAnswer);
            //đáp án đúng nhưng có khiên thì vẫn vỡ
            if (shield)
            {
                shield = false;
                shieldGO.SetActive(false);
            }
        }
        else
        {
            if (shield == false)
            {
                // Không có khiên chắn thì sẽ mất mạng
                curHeart--;
                instanceGM.IncreaseHeartPlayer(curHeart);
                //audio
                if (curHeart > 0)
                    instanceAM.WrongAnswerFx();

                GetIcon(rightAnswer);
            }
            else
            {
                // Có khiên chắn sẽ không làm mất mạng
                shield = false;
                shieldGO.SetActive(false);
                //audio
                instanceAM.BreakShieldFx();
            }
        }

    }

    // Truyền khiên bảo vệ từ item
    public void Shield(bool haveShield)
    {
        shield = haveShield;
        shieldGO.SetActive(true);
    }
    public void GetHeartFromItem()
    {
        //nếu mạng hiện tại của player đã đầy thì không tăng mạng cho player
        if (curHeart == heart)
            return;
        curHeart++;
        instanceGM.IncreaseHeartPlayer(curHeart);
    }
    /// <summary>
    /// Khi tương tác với item sau đó mất khiên
    /// </summary>
    private void SetAnimation(State state)
    {
        if (curState == state)
            return;

        animator.SetInteger("State", (int)state);
        animator.SetTrigger("Change");
        curState = state;
    }

}
