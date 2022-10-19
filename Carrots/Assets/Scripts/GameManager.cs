using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public enum StateGame
    {
        GameOver, GamePlay, GameWait, GameSetting
    }
    [SerializeField] private int typeMap;
    [SerializeField] private GamePlay gamePlayUI;
    [SerializeField] private GameOver gameOverUI;
    [SerializeField] private GameWait gameWaitUI;
    [SerializeField] private GameSetting gameSettingUI;
    [SerializeField] private Transform[] posSpawnAnswer_Item;
    [SerializeField] private LevelScptObj levelScptObj;
    [SerializeField] private float speedInGame;
    [SerializeField] private int scoreAchieve;
    [SerializeField] private CategoryItemSctbObj[] categoryItem;
    [SerializeField] private Button btnGameSetting;

    private int level;
    private readonly int lowestLimit = 1;
    private int highestLimit;
    //toàn bộ phép tính để show text
    private List<int> longOperation = new List<int>();
    //phép tính thu gọn để tính tìm rightAnswer
    private List<int> shortOperation = new List<int>();
    //tham số thu gọn để tính tìm rightAnswer
    private List<int> shortParams = new List<int>();
    //toàn bộ tham số để show text
    private List<int> longParams = new List<int>();
    //groups để gộp các phép nhân chia thành 1 nhóm
    private List<Group> groups = new List<Group>();
    private int rightAnswer;
    private int indexGroups;

    private Answer[] answers;
    private int curTurnInGame;
    private int curIndexTurnLvScptObj;
    private int curTurnItem;

    private int firstScoreAchieve;
    private float valueSlowInGame;

    private bool isPause;
    public bool IsPause => isPause;

    private int totalTurn;
    private int curScore;
    private float curSpeed;
    private SpawnManager instanceSM;
    private AudioManager instanceAM;
    private DataManager instanceDM;
    /// <summary>
    /// Singleton
    /// </summary>
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        groups = new List<Group>();
        answers = new Answer[posSpawnAnswer_Item.Length];
        totalTurn = 0;
        curTurnInGame = 0;
        curIndexTurnLvScptObj = 0;
        curScore = 0;
        level = PlayerPrefs.GetInt("LevelGame", 1);
        highestLimit = SwitchToHighestLimit(level) - 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        SetState(StateGame.GameWait);
        instanceAM = AudioManager.instance;
        instanceSM = SpawnManager.instance;
        instanceDM = DataManager.instance;
        curSpeed = speedInGame;
        firstScoreAchieve = scoreAchieve;
        totalTurn = levelScptObj.turns[levelScptObj.turns.Count - 1].turnEnd;
        curSpeed += levelScptObj.turns[curIndexTurnLvScptObj].speedUp;
    }

    // Update is called once per frame
    void Update()
    {
    }
    private int SwitchToHighestLimit(int lv)
    {
        int high;
        switch (lv)
        {
            case 1:
                high = 100;
                break;
            case 2:
                high = 200;
                break;
            case 3:
                high = 300;
                break;
            case 4:
                high = 400;
                break;
            case 5:
                high = 500;
                break;
            case 6:
                high = 600;
                break;
            case 7:
                high = 700;
                break;
            case 8:
                high = 800;
                break;
            case 9:
                high = 900;
                break;
            case 10:
                high = 1000;
                break;
            default:
                high = 100;
                break;
        }
        return high;
    }
    public void SetCarrot()
    {
        gamePlayUI.SetImgCarrotPoint(curTurnInGame, totalTurn);
    }
    #region Turn
    public void NextTurn()
    {
        try
        {
            curTurnInGame++;
            while (curIndexTurnLvScptObj < levelScptObj.turns.Count)
            {
                if (curTurnInGame <= levelScptObj.turns[curIndexTurnLvScptObj].turnEnd)
                {
                    //loop kiểm tra turn hiện tại có item không,không có thì thoát loop
                    for (int i = 0; i < levelScptObj.turns[curIndexTurnLvScptObj].turnItem.Length; i++)
                    {
                        if (curTurnInGame == levelScptObj.turns[curIndexTurnLvScptObj].turnItem[i])
                        {
                            curTurnItem = curTurnInGame;
                            //spawn item
                            Item item = instanceSM.itemPool.SpawnObjInPool(posSpawnAnswer_Item[RandomNumber(0, posSpawnAnswer_Item.Length - 1)]).GetComponent<Item>();
                            item.Init(categoryItem[RandomNumber(0, categoryItem.Length - 1)]);
                            return;
                        }
                    }
                    //Initial Question
                    gamePlayUI.ClearText();
                    //spawn question
                    HandleQuestion(levelScptObj.turns[curIndexTurnLvScptObj].numberOfOpeSptObj, levelScptObj.turns[curIndexTurnLvScptObj].GetOperations());
                    break;
                }
                else
                {
                    if (curIndexTurnLvScptObj == levelScptObj.turns.Count - 1)
                    {
                        //end turn 
                        Debug.Log("End turn");
                        for (int i = 0; i < 4; i++)
                        {
                            instanceSM.stopSignalPool.SpawnObjInPool(posSpawnAnswer_Item[i]);
                        }
                        return;
                    }
                    //increase turn level 
                    curIndexTurnLvScptObj++;
                    //increase Speed
                    curSpeed += levelScptObj.turns[curIndexTurnLvScptObj].speedUp;
                }
            }
            //Initial Answer
            int indexRight = RandomNumber(0, posSpawnAnswer_Item.Length - 1);
            //tạo list để kiểm tra đã tồn tại số đó chưa
            List<int> wrongNumbers = new List<int>();
            int wrongNumber;
            for (int i = 0; i < posSpawnAnswer_Item.Length; i++)
            {
                if (i != indexRight)
                {
                    do
                    {
                        wrongNumber = RandomWrongAnswer(rightAnswer, highestLimit);
                    } while (wrongNumbers.Contains(wrongNumber));
                    wrongNumbers.Add(wrongNumber);
                    //spawn answer wrong
                    Answer answer = instanceSM.answersPool.SpawnObjInPool(posSpawnAnswer_Item[i]).GetComponent<Answer>();
                    answer.Init(wrongNumber, false);
                    answers[i] = answer;
                }
                else
                {
                    //spawn answer right
                    Answer answer = instanceSM.answersPool.SpawnObjInPool(posSpawnAnswer_Item[i]).GetComponent<Answer>();
                    answer.Init(rightAnswer, true);
                    answers[i] = answer;
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Application.Quit();
        }
    }
    private int RandomWrongAnswer(int rightAnswer, int high)
    {
        string s = rightAnswer.ToString();
        char[] charArray = s.ToCharArray();
        int lastRightNumber = int.Parse(charArray[charArray.Length - 1].ToString());
        int wrongAnswer;
        //int i = 900;
        do
        {
            wrongAnswer = RandomNumber(1, high);
            int lastWrongNumber = wrongAnswer % 10;
            wrongAnswer = wrongAnswer + (lastRightNumber - lastWrongNumber);
        } while (wrongAnswer == rightAnswer);
        //while (i > 0)
        //{
        //    if (rightAnswer > i)
        //    {
        //        wrongAns = RandomNumber(i, i + 99);
        //        return wrongAns;
        //    }
        //    i -= 100;
        //}
        //if (rightAnswer >= 10)
        //{
        //    wrongAns = RandomNumber(10, 99);
        //}
        //else
        //{
        //    wrongAns = RandomNumber(1, 9);
        //}
        return wrongAnswer;
    }

    /// <summary>
    /// Khi player tương tác với câu trả lời thì xử lý huỷ bỏ
    /// </summary>
    /// <param name="_answer">Answer</param>
    public void RemoveAnswers(Answer _answer, int heartPlayer, bool rightAnswer)
    {
        //check nếu turn item thì không xử lý nút transAnswer
        if (curTurnInGame == curTurnItem)
            return;

        if (heartPlayer == 0)
        {
            //gameover
            instanceSM.answersPool.RemoveObjInPool(_answer.gameObject);
            SetState(StateGame.GameOver);
        }
        foreach (var answer in answers)
        {
            answer.Interact = false;
            if (_answer == answer)
            {
                //hande remove answer is collide
                answer.HandleDie(true);
            }
            else
            {
                //hande remove answer isn't collide
                answer.HandleDie(false);
            }
        }
        if (heartPlayer == 0)
            return;
        if (rightAnswer)
        {
            SetCarrot();
        }
        if (_answer == null)
        {
            Invoke("NextTurn", 2f);
        }
        else
        {
            NextTurn();
        }
    }
    public void IncreaseHeartPlayer(int heart)
    {
        gamePlayUI.ShowHeartPlayer(heart);
    }
    private int RandomNumber(int first, int second)
    {
        return Mathf.RoundToInt(Random.Range((float)first, (float)second));
    }
    private int RandomOperation(int[] _operation)
    {
        int index = RandomNumber(1, _operation.Length);
        return _operation[index - 1];
    }
    //random từng phép tính trong loại phép tính toàn bộ => có operation full
    //xử lý gộp => có operation after
    //lưu vị trí index gộp nhân chia vào indexgroup
    //vòng lặp operation tìm tham số từng index,sau đó xét vị trí index đó trùng với indexgroup => nếu false : add vào longparams và shortparams
    //  true: xử lý group lấy các giá trị tham số bên trong add vào longparams,sau đó các giá trị sẽ được tính tổng lại vào add vào shortparams
    //tính tổng tìm right answer
    private void HandleQuestion(int numberOfOperation, int[] operations)
    {
        ResetValueInQuestion();
        for (int i = 0; i < numberOfOperation; i++)
        {
            //random từng phép tính trong loại phép tính toàn bộ => có operation full
            int randomOperaion = RandomOperation(operations);
            longOperation.Add(randomOperaion);
            //xử lý gộp => có operation after
            //lưu vị trí index gộp nhân chia vào indexgroup
            if (i > 0)
            {
                if ((longOperation[i] == 1 || longOperation[i] == 2))
                {
                    //vế riêng (là phép + -)
                    shortOperation.Add(longOperation[i]);
                }
                else if ((longOperation[i] == 3 || longOperation[i] == 4) && (longOperation[i - 1] == 3 || longOperation[i - 1] == 4))
                {
                    //gộp
                    groups[indexGroups].operationsInGroup.Add(longOperation[i]);
                }
                else
                {
                    //tách
                    indexGroups++;
                    Group group = new Group(this);
                    group.operationsInGroup.Add(longOperation[i]);
                    group.indexGroup = shortOperation.Count;
                    groups.Add(group);
                }
            }
            // i = 0
            else
            {
                // + -
                if (longOperation[i] == 1 || longOperation[i] == 2)
                {
                    shortOperation.Add(longOperation[i]);
                }
                // * /                    
                else
                {
                    indexGroups++;
                    Group group = new Group(this);
                    group.operationsInGroup.Add(longOperation[i]);
                    group.indexGroup = shortOperation.Count;
                    groups.Add(group);
                }
            }
        }
        ////////////tiep tuc
        //vòng lặp operation tìm tham số từng index,sau đó xét vị trí index đó trùng với indexgroup => nếu false : add vào longparams và shortparams
        //  true: xử lý group lấy các giá trị tham số bên trong add vào longparams,sau đó các giá trị sẽ được tính tổng để lấy giá trị mới rồi add vào shortparams
        //chỉ có nhân chia trong phép toán
        if (shortOperation.Count == 0)
        {
            CheckInGroup(0, highestLimit);
            rightAnswer = shortParams[0];
        }
        else
        {
            int numberOne = 0, numberTwo = 0;
            for (int i = 0; i < shortOperation.Count + 1; i++)
            {
                if (i == 0)
                {
                    switch (shortOperation[0])
                    {
                        case 1:
                            numberOne = RandomNumber(lowestLimit, highestLimit - 1);
                            break;
                        case 2:
                            numberOne = RandomNumber(lowestLimit + 1, highestLimit);
                            break;
                        case 3:
                            numberOne = RandomNumber(lowestLimit + 1, highestLimit / 4);
                            break;
                        default:
                            numberOne = RandomNumber(lowestLimit + 1, highestLimit);
                            break;
                    }
                    if (!CheckInGroup(i, numberOne))
                    {
                        shortParams.Add(numberOne);
                        longParams.Add(numberOne);
                    }
                    numberOne = shortParams[i];
                }
                else
                {
                    numberTwo = FindSecondParameter(numberOne, shortOperation[i - 1], lowestLimit, highestLimit);
                    if (!CheckInGroup(i, numberTwo))
                    {
                        shortParams.Add(numberTwo);
                        longParams.Add(numberTwo);
                    }
                    numberOne = Calculate(numberOne, shortParams[i], shortOperation[i - 1]);
                }
            }
            //tính tổng tìm right answer
            rightAnswer = numberOne;
        }
        for (int i = 0; i < longParams.Count; i++)
        {
            if (i == longParams.Count - 1)
            {
                ShowQuestionTextGamePlay(longParams[i], 0);
            }
            else
            {
                ShowQuestionTextGamePlay(longParams[i], longOperation[i]);
            }
        }

    }
    private void ResetValueInQuestion()
    {
        longOperation.Clear();
        longParams.Clear();
        shortOperation.Clear();
        shortParams.Clear();
        groups.Clear();
        indexGroups = -1;
    }
    private bool CheckInGroup(int indexParams, int result)
    {
        Group group = groups.FindLast(_groups => _groups.indexGroup == indexParams);
        if (group == null)
        {
            return false;
        }
        //Handle group 
        //xử lý group lấy các giá trị tham số bên trong add vào longparams, sau đó các giá trị sẽ được tính tổng lại vào add vào shortparams
        List<int> paramsInGroup = group.GetAnswers(result);
        foreach (var param in paramsInGroup)
        {
            longParams.Add(param);
        }
        shortParams.Add(CaculatingTotalParameter(paramsInGroup, group.operationsInGroup));
        return true;
    }
    private List<int> FindAllParamsInListByRandom(List<int> listOperation, int lowestLimit, int highestLimit)
    {
        int firstParameter = 0, secondParameter = 0;
        List<int> listParameter = new List<int>();
        for (int i = 0; i < listOperation.Count; i++)
        {
            if (i == 0)
            {
                switch (listOperation[i])
                {
                    // *
                    case 3:
                        //Phép nhân random tự do số thứ nhất + 1,số thứ hai random kiểm tra điều kiện 2 số nhân nhau phải nhỏ hơn highestLimit
                        firstParameter = RandomNumber(lowestLimit + 1, highestLimit / 4);
                        listParameter.Add(firstParameter);
                        secondParameter = FindSecondParameter(firstParameter, listOperation[i], lowestLimit, highestLimit);
                        listParameter.Add(secondParameter);
                        firstParameter = Calculate(firstParameter, secondParameter, listOperation[i]);
                        break;
                    // :
                    case 4:
                        do
                        {
                            firstParameter = RandomNumber(lowestLimit, highestLimit);
                        } while (firstParameter % lowestLimit != 0);
                        listParameter.Add(firstParameter);
                        secondParameter = FindSecondParameter(firstParameter, listOperation[i], lowestLimit, highestLimit);
                        listParameter.Add(secondParameter);
                        firstParameter = Calculate(firstParameter, secondParameter, listOperation[i]);
                        break;
                    // + or -
                    default:
                        if (listOperation[i] == 1)
                        {
                            firstParameter = RandomNumber(lowestLimit, highestLimit - 1);
                        }
                        else
                        {
                            firstParameter = RandomNumber(lowestLimit + 1, highestLimit);
                        }
                        listParameter.Add(firstParameter);
                        secondParameter = FindSecondParameter(firstParameter, listOperation[i], lowestLimit, highestLimit);
                        listParameter.Add(secondParameter);
                        firstParameter = Calculate(firstParameter, secondParameter, listOperation[i]);
                        break;
                }
            }
            else
            {
                switch (listOperation[i])
                {
                    case 1:
                        secondParameter = FindSecondParameter(firstParameter, listOperation[i], lowestLimit, highestLimit);
                        listParameter.Add(secondParameter);
                        firstParameter = Calculate(firstParameter, secondParameter, listOperation[i]);
                        break;
                    case 2:
                        secondParameter = FindSecondParameter(firstParameter, listOperation[i], lowestLimit, highestLimit);
                        listParameter.Add(secondParameter);
                        firstParameter = Calculate(firstParameter, secondParameter, listOperation[i]);
                        break;
                    case 3:
                        secondParameter = FindSecondParameter(firstParameter, listOperation[i], lowestLimit, highestLimit);
                        listParameter.Add(secondParameter);
                        firstParameter = Calculate(firstParameter, secondParameter, listOperation[i]);
                        break;
                    case 4:
                        secondParameter = FindSecondParameter(firstParameter, listOperation[i], lowestLimit, highestLimit);
                        listParameter.Add(secondParameter);
                        firstParameter = Calculate(firstParameter, secondParameter, listOperation[i]);
                        break;
                }
            }
        }
        return listParameter;
    }
    private int FindSecondParameter(int first, int operation, int lowestLimit, int highestLimit)
    {
        int second = 0;
        switch (operation)
        {
            case 1:
                //trường hợp số đầu bằng số lớn nhất thì mặc định số thứ hai là 0
                if (first == highestLimit)
                {
                    second = 0;
                    break;
                }
                second = RandomNumber(lowestLimit, highestLimit - first);
                break;
            case 2:
                //trường hợp số đầu bằng số nhỏ nhất thì mặc định số thứ hai là 0
                if (first == 0)
                {
                    second = 0;
                    break;
                }
                second = RandomNumber(lowestLimit, first - lowestLimit);
                break;
            case 3:
                //trường hợp số đầu lớn bằng số lớn nhất / 2 thì mặc định số thứ hai là 1 bởi vì số đó không thể nhân 2 trở lên dược
                if (first >= highestLimit / 2)
                {
                    Debug.Log(first);

                    second = 1;
                    break;
                }
                do
                {
                    second = RandomNumber(lowestLimit + 1, highestLimit);
                } while (first * second > highestLimit);
                break;
            case 4:
                //trường hợp first = 0 thì ngẫu nhiên cơ bản bởi vì chia số nào cũng về 0
                if (first == 0)
                {
                    second = RandomNumber(lowestLimit, highestLimit);
                    break;
                }
                //check phải số nguyên tố?
                for (int i = 0; i < first; i++)
                {
                    if (first % (i + 1) == 0 && (i + 1) != 1 && (i + 1) != first)
                    {
                        //k là số nguyên tố
                        do
                        {
                            second = RandomNumber(lowestLimit, highestLimit);
                            //đã là số nguyên tố thì không lọc ra k chia cho 1 và chính nó
                        } while (first % second != 0 || second == 1 || second == first);
                        break;
                    }
                }
                if (second == 0)
                {
                    //k thấy sự thay đổi từ second => first là số nguyên tố
                    second = first;
                }
                break;
        }
        return second;
    }
    private int CaculatingTotalParameter(List<int> parameters, List<int> operations)
    {
        int total = 0;
        if (parameters.Count != operations.Count + 1)
        {
            Debug.LogError("Tham số và phép tính k trùng khớp.Fix bug");
            return 0;
        }
        for (int i = 0; i < parameters.Count; i++)
        {
            if (i == 0)
            {
                total = parameters[i];
            }
            else
            {
                total = Calculate(total, parameters[i], operations[i - 1]);
            }
        }
        return total;
    }
    private int Calculate(int first, int second, int ope)
    {
        int kq = 0;
        switch (ope)
        {
            // +
            case 1:
                kq = first + second;
                break;
            // -
            case 2:
                kq = first - second;
                break;
            // *
            case 3:
                kq = first * second;
                break;
            // /
            case 4:
                kq = first / second;
                break;
        }
        return kq;
    }
    private void ShowQuestionTextGamePlay(int param, int operation)
    {
        string txtOperation = "";
        switch (operation)
        {
            case 1:
                txtOperation = "+";
                break;
            case 2:
                txtOperation = "-";
                break;
            case 3:
                txtOperation = "x";
                break;
            case 4:
                txtOperation = ":";
                break;
            default:
                txtOperation = "";
                break;
        }
        gamePlayUI.ShowTextQuestion(param.ToString() + txtOperation);
    }
    #endregion
    public float GetSpeedMove()
    {
        return curSpeed;
    }
    public void SetScoreGamePlay()
    {
        curScore += scoreAchieve;
        gamePlayUI.SetTextScore(curScore);
    }

    /// <summary>
    /// Lấy câu hỏi cho item
    /// </summary>
    /// <returns></returns>
    public Answer[] TakeAnswers()
    {
        return answers;
    }
    /// <summary>
    /// Thay đổi điểm nhận được từ item (biển clear để check nó hiện image thời gian
    /// </summary>
    /// <param name="number"></param>
    public void SetScoreFromItem(int number)
    {
        //nếu tham số truyền vào là 1 thì trả score về giá trị ban đầu
        scoreAchieve = firstScoreAchieve;
        scoreAchieve *= number;
        if (number != 1)
            gamePlayUI.TimeX2();
    }

    public void SetSlowInGameFromItem(int slowValue)
    {
        curSpeed += valueSlowInGame;
        valueSlowInGame = curSpeed - curSpeed / slowValue;
        curSpeed /= slowValue;
        gamePlayUI.TimeSlow();
    }
    public void ShowFlyScore(Transform pos)
    {
        ScoreFly scoreFly = instanceSM.scoreFlyPool.SpawnObjInPool(pos).GetComponent<ScoreFly>();
        scoreFly.Init(scoreAchieve.ToString());
    }
    public void FromSettingToPlay()
    {
        isPause = false;
        //Màn hình TabToPlay sẽ invisible
        gameWaitUI.gameObject.SetActive(false);
        btnGameSetting.gameObject.SetActive(true);
        gameSettingUI.gameObject.SetActive(false);
    }

    #region StateGame
    /// <summary>
    /// Trạng thái game
    /// </summary>
    /// <param name="state"></param>
    public void SetState(StateGame state)
    {
        switch (state)
        {
            case StateGame.GamePlay:
                isPause = false;
                Time.timeScale = 1f;
                //Màn hình TabToPlay sẽ invisible
                gameWaitUI.gameObject.SetActive(false);
                btnGameSetting.gameObject.SetActive(true);
                NextTurn();
                break;
            case StateGame.GameWait:
                isPause = false;
                Time.timeScale = 0f;
                gameWaitUI.gameObject.SetActive(true);
                btnGameSetting.gameObject.SetActive(false);
                //Màn hình TabToPlay sẽ visible
                break;
            case StateGame.GameOver:
                isPause = true;
                //StartCoroutine(InitGameOver());
                gameOverUI.gameObject.SetActive(true);
                gameSettingUI.gameObject.SetActive(false);
                //audio
                instanceAM.GameOverFx();
                int indexCarrotPoint = gamePlayUI.GetIndexCarrotPoint() - 1;
                gameOverUI.GetScore(curScore, curTurnInGame > levelScptObj.turns[curIndexTurnLvScptObj].turnEnd ? true : false);
                gameOverUI.GetCarrotPoint(indexCarrotPoint);
                //save db
                MapData map = instanceDM.GetDataMap(typeMap);
                if (map.GetScore(level) < curScore)
                {
                    map.SetScore(level, curScore);
                }
                if (map.GetCarrotStar(level) < indexCarrotPoint + 1)
                {
                    map.SetCarrotStar(level, indexCarrotPoint + 1);
                }
                instanceDM.SaveDataMap(typeMap, map);
                UnlockNextMap(map.GetCarrotStar(level));
                break;
            case StateGame.GameSetting:
                isPause = true;
                Time.timeScale = 1f;
                gameSettingUI.gameObject.SetActive(true);
                break;
        }
    }
    #endregion
    private void UnlockNextMap(int carrotStar)
    {
        if (typeMap == 5)
            return;
        if (carrotStar >= 2)
        {
            MapData nextMap = instanceDM.GetDataMap(typeMap + 1);
            if (nextMap == null)
            {
                instanceDM.SaveDataMap(typeMap + 1, new MapData(typeMap + 1));
            }
        }
    }
    class Group
    {
        public List<int> operationsInGroup = new List<int>();
        public int indexGroup;
        private int lowestLimit;
        private GameManager gameManager;

        public Group(GameManager gameManager)
        {
            this.gameManager = gameManager;
            lowestLimit = this.gameManager.lowestLimit;
        }

        public List<int> GetAnswers(int highestGroup)
        {
            return gameManager.FindAllParamsInListByRandom(operationsInGroup, lowestLimit, highestGroup);
        }
    }
}
