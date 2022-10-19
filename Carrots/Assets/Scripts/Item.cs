using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    private ButtonItemManager instanceBtnItem;
    private CategoryItemSctbObj curCategory;
    private Image image;
    private GameManager instanceGM;
    private SpawnManager instanceSM;
    private Road road;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    private void OnEnable()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        instanceGM = GameManager.instance;
        instanceSM = SpawnManager.instance;
        instanceBtnItem = ButtonItemManager.instance;
        road = FindObjectOfType<Road>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += road.DistanceRoad2;
        if (transform.position.y <= -6)
        {
            Die();
        }

    }
    public void Init(CategoryItemSctbObj category)
    {
        curCategory = category;
        image.sprite = curCategory.sprite;
    }
    public void DieByPlayer()
    {
        instanceGM.SetCarrot();
        instanceGM.NextTurn();
        instanceBtnItem.SpawnButtonItem(curCategory);
        //buttonItem.Init(curCategory);
        instanceSM.itemPool.RemoveObjInPool(gameObject);
        ParticleManager particle = instanceSM.vfxPool.SpawnObjInPool(transform).GetComponent<ParticleManager>();
        particle.Init(curCategory.sptVFX);
    }
    public void Die()
    {
        instanceGM.NextTurn();
        instanceSM.itemPool.RemoveObjInPool(gameObject);
    }
   
}
