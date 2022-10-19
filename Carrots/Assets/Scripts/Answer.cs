using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Answer : MonoBehaviour
{
    private Animator animator;
    private TextMeshProUGUI txtAnswer;
    private bool rightAnswer;
    private GameManager instanceGM;
    private SpawnManager instanceSM;
    private bool interact;
    private Road road;

    public bool Interact { get => interact; set => interact = value; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        txtAnswer = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        interact = true;
        SetAnimation(false, true);
    }
    // Start is called before the first frame update
    void Start()
    {
        road = FindObjectOfType<Road>();
        instanceGM = GameManager.instance;
        instanceSM = SpawnManager.instance;
        interact = true;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += road.DistanceRoad2;
    }
    public void Init(int text, bool answer)
    {
        txtAnswer.text = text.ToString();
        rightAnswer = answer;
    }
    public void HandleDie(bool collision)
    {
        SetAnimation(collision, false);
    }
    public void Die()
    {
        instanceSM.answersPool.RemoveObjInPool(gameObject);
    }
    public bool GetAnswer()
    {
        return rightAnswer;
    }
    public string GetText()
    {
        return txtAnswer.text;
    }
    /// <summary>
    /// Loại bỏ text đáp án khi iitem được kích hoạt
    /// </summary>
    public void ClearTextFromItem()
    {
        txtAnswer.text = "";
    }
    /// <summary>
    /// Animation
    /// </summary>
    /// <param name="mcInteract">Main tương tác</param>
    /// <param name="interact">Tên biến này dùng để xét nó có thể tương tác không</param>
    private void SetAnimation(bool mcInteract, bool interact)
    {
        animator.SetBool("MCinteract", mcInteract);
        animator.SetBool("Interact", interact);
    }
}
