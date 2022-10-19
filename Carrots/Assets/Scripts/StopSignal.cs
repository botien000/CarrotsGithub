using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopSignal : MonoBehaviour
{
    private Road road;
    // Start is called before the first frame update
    void Start()
    {
        road = FindObjectOfType<Road>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += road.DistanceRoad2;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.SetState(GameManager.StateGame.GameOver);
        }
    }
}
