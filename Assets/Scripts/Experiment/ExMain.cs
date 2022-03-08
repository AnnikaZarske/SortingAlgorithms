using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Sorting;
using UnityEngine;

public class ExMain : MonoBehaviour
{
    private float sortTime = 0;

    public float SortTime
    {
        get { return sortTime; }
    }
    
    [Header("Settings")]
    public int ballsCount = 250;
    public int closestBallsCount = 10;
    public Algorithms algoDropdown;
    
    [Header("Attached")]
    public Ball ballPrf;
    public Sprite whiteBallSprite;
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

        //Instantiate(target);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Ball ball in ballList)
        {
            ball.CalcDistance(target.transform.position);
        }

        ballArray = ballList.ToArray();
        
        SortHelper sh = new SortHelper();
        sh.SetAlgorithm(algoDropdown);
        sh.sort(ballArray);
        

        for (int i = closestBallsCount - 1; i < ballArray.Length; i++)
        {
            ballArray[i].GetComponent<SpriteRenderer>().sprite = ballArray[i].startSprite;
        }
        
        for (int i = 0; i < closestBallsCount; i++)
        {
            ballArray[i].GetComponent<SpriteRenderer>().sprite = whiteBallSprite;
        }
    }
}
