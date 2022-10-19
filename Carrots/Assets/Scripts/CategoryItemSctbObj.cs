using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CategoryItem", menuName = "ScriptableObject/Create CategoryItem")]
public class CategoryItemSctbObj : ScriptableObject
{
    
    public enum Category
    {
        HalfAnswer, Shield, TransAnswer, DoublePoint,SlowSpeed,Heart
    }
    public Category curCategory;
    public Sprite sprite;
    public Sprite sptVFX;
    
    
}
