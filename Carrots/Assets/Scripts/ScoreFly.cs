using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreFly : MonoBehaviour
{
    private SpawnManager instanceSM;
    private TextMeshProUGUI txtScore;
    private void Awake()
    {
        txtScore = GetComponent<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        instanceSM = SpawnManager.instance;
    }
    public void RemoveThis()
    {
        instanceSM.scoreFlyPool.RemoveObjInPool(gameObject);
    }
    public void Init(string txt)
    {
        txtScore.text = "+" + txt; 
    }
}
