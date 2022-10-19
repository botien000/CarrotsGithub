using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonItemManager : MonoBehaviour
{
    [SerializeField] private List<ButtonItem> inactiveBtnItems;

    private List<CategoryItemSctbObj> categories = new List<CategoryItemSctbObj>();

    /// <summary>
    /// Singleton 
    /// </summary>
    public static ButtonItemManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //Khi vào game tất cả button invisible
        foreach (var item in inactiveBtnItems)
        {
            item.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SpawnButtonItem(CategoryItemSctbObj category)
    {
        //bỏ item đầu thêm item mới ở cuối
        if (categories.Count == inactiveBtnItems.Count)
        {
            categories.RemoveAt(0);
        }
        categories.Add(category);
        SortOfButtonItem();
    }
    public void RemoveObjInPool(ButtonItem buttonItem)
    {
        int index = inactiveBtnItems.IndexOf(buttonItem);
        categories.RemoveAt(index);
        SortOfButtonItem();
    }
    public void ClearObjInPool()
    {
        inactiveBtnItems.Clear();
        categories.Clear();
    }
    private void SortOfButtonItem()
    {
        ButtonItem buttonItem;
        for (int i = 0; i < inactiveBtnItems.Count; i++)
        {
            if (i < categories.Count)
            {
                inactiveBtnItems[i].gameObject.SetActive(true);
                buttonItem = inactiveBtnItems[i];
                buttonItem.Init(categories[i]);
            }
            else
            {
                inactiveBtnItems[i].gameObject.SetActive(false);
            }
        }

    }
}
