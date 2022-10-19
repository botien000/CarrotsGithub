using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetRoad : MonoBehaviour
{
    [SerializeField] private Material roadMaterial;

    private Vector2 offset;
    private int mainTextId;
    private float value;
    private GameManager instanceGM;
    // Start is called before the first frame update
    void Start()
    {
        instanceGM = GameManager.instance;
        mainTextId = Shader.PropertyToID("_MainTex");
        roadMaterial.SetTextureOffset(mainTextId, Vector2.zero);
    }

    // Update is called once per frame
    void Update()
    {
        if (offset.y == 1000000)
            offset.y = 0;
            
        offset = roadMaterial.GetTextureOffset(mainTextId);
        value = instanceGM.GetSpeedMove() * 0.1f * Time.deltaTime;
        offset -= new Vector2(0, value);
        roadMaterial.SetTextureOffset(mainTextId, offset);
        
    }
    public float GetValue()
    {
        return value;
    }
}
