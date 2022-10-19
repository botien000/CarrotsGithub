using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelsData", menuName = "ScriptableObject/Create Levels")]
public class LevelScptObj : ScriptableObject
{
    public enum Operation
    {
        Plus = 1,
        SubTract = 2,
        Multiply = 3,
        Divide = 4
    }
    public List<TurnLevel> turns;
    [System.Serializable]
    public class TurnLevel
    {
        public int turnEnd;
        public float speedUp;
        public int[] turnItem;
        public int numberOfOpeSptObj;
        public Operation[] operationsEnumSptObj;
        [HideInInspector]
        private int[] operationsSptObj;
        public int[] GetOperations()
        {
            operationsSptObj = new int[operationsEnumSptObj.Length];
            for (int i = 0; i < operationsEnumSptObj.Length; i++)
            {
                operationsSptObj[i] = (int)operationsEnumSptObj[i];
            }
            return operationsSptObj;
        }
    }
}
