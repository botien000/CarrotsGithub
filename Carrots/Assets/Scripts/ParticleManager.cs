using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    private ParticleSystem particle;
    private readonly float duration = 0.9f;
    private float curTime;
    private SpawnManager instanceSM;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        instanceSM = SpawnManager.instance;
        curTime = duration;
    }
    // Update is called once per frame
    void Update()    
    {
        curTime -= Time.deltaTime;
        if (curTime <= 0)
        {
            curTime = duration;
            instanceSM.vfxPool.RemoveObjInPool(gameObject);
        }
    }
    public void Init(Sprite sprite)
    {
        particle.textureSheetAnimation.SetSprite(0, sprite);
    }
}
