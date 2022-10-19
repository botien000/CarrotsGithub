using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image imgLoad;
    [SerializeField] private TextMeshProUGUI txtPercentLoading;
    private float progress;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadSceneHome()
    {
        StartCoroutine(IELoadingScreen(0));
    }
    public void LoadSceneGamePlay(int level)
    {
        StartCoroutine(IELoadingScreen(level));
    }
    IEnumerator IELoadingScreen(int buildIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(buildIndex);
        while (!asyncOperation.isDone)
        {
            progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            imgLoad.fillAmount = progress;
            txtPercentLoading.text = progress * 100 + "%";
            yield return null;
        }
    }
}
