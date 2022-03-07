using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExMain : MonoBehaviour
{
    private float sortTime = 0;

    public float SortTime
    {
        get { return sortTime; }
    }

    public Ball ballPrf;
    public int ballsCount = 250;
    public int yellowBallsCount = 10;

    public Target target;

    private List<Ball> ballList = new List<Ball>();

    private Ball[] ballArray;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject balls = GameObject.FindGameObjectWithTag("Balls");
        
        for (int i = 0; i < ballsCount; i++)
        {
            Ball ball = Instantiate(ballPrf, balls.transform, true);

            ballList.Add(ball);
        }

        Instantiate(target);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Ball ball in ballList)
        {
            ball.CalcDistance(target.transform.position);
        }
        
        
    }
}
