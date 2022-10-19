using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameWait : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtReady;

    private GameManager instanceGM;
    private void OnEnable()
    {
        txtReady.text = "Ready...";
    }
    // Start is called before the first frame update
    void Start()
    {
        instanceGM = GameManager.instance;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            instanceGM.SetState(GameManager.StateGame.GamePlay);
        }
    }
}
