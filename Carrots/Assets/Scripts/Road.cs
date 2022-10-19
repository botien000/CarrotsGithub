using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [SerializeField] private Transform rangeTop, rangeBot;
    [SerializeField] private Transform road_1, road_2;

    private Vector3 originPos;
    private GameManager instanceGM;
    private Vector3 toPos;
    private Vector3 distanceRoad2;
    public Vector3 ToPos { get => toPos; }
    public Vector3 DistanceRoad2 => distanceRoad2;

    // Start is called before the first frame update
    void Start()
    {
        originPos = road_1.position;
        road_2.position = rangeTop.position;
        instanceGM = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        //thay đổi loại chạy đứng trên 1 màn hình : lấy 1 làm chuẩn sau đó trả về vị trí cũ
        Move(road_1);

    }
    public void Move(Transform roadMove)
    {
        //toPos = Vector3.down * instanceGM.GetSpeedMove() * Time.deltaTime;
        //roadMove.position += toPos;
        //if(roadMove.position.y <= rangeBot.position.y)
        //{   
        //    roadMove.position = rangeTop.position;
        //}
        toPos = Vector2.MoveTowards(roadMove.position, rangeBot.position, instanceGM.GetSpeedMove() * Time.deltaTime);
        distanceRoad2 = (Vector2) toPos - (Vector2)roadMove.position;
        roadMove.position = (Vector2) toPos;
        road_2.position += DistanceRoad2;
        if (roadMove.position == rangeBot.position)
        {
            roadMove.position = originPos;
            road_2.position = rangeTop.position;
        }
    }
}
