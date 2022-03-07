using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private float distance;

    public float Distance
    {
        get { return distance;  }
    }

    public float offset = 0.3f;

    public float minDist = 1;
    public float maxDist = 3;
    
    public float minSpeed = 1;
    public float maxSpeed = 5;

    private float speed;

    private Vector2 screenSize = Vector2.zero;
    private Vector2 destPoint = Vector2.zero;

    public Sprite startSprite;
    public List<Sprite> possibleSprites;

    public void CalcDistance(Vector2 pos)
    {
        distance = Vector2.Distance(transform.position, pos);
    }

    // Start is called before the first frame update
    void Start()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        
        screenSize = new Vector2(width, height);

        transform.position = GetRandomPosition();
        destPoint = transform.position;

        int index = Random.Range(0, possibleSprites.Count - 1);

        Sprite sprite = this.GetComponent<SpriteRenderer>().sprite = possibleSprites[index];
        startSprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, destPoint) < 0.1f) {
            speed = Random.Range(minSpeed, maxSpeed);
            destPoint = GetNewPosition();
        }

        transform.position = Vector3.MoveTowards(transform.position,destPoint, Time.deltaTime * speed);
    }
    
    private Vector2 GetRandomPosition()
    {
        Vector2 p1 = new Vector2(-screenSize.x / 2 + offset, screenSize.y / 2 - offset);
        Vector2 p2 = new Vector2(screenSize.x / 2 - offset, -screenSize.y / 2 + offset);

        Vector2 rndPos = Vector2.zero;
        rndPos.x = Random.Range(p1.x, p2.x);
        rndPos.y = Random.Range(p1.y, p2.y);

        return rndPos;
    }
    
    private Vector2 GetNewPosition()
    {
        Vector2 p1 = new Vector2(-screenSize.x / 2 + offset, screenSize.y / 2 - offset);
        Vector2 p2 = new Vector2(screenSize.x / 2 - offset, -screenSize.y / 2 + offset);

        float distance = Random.Range(minDist, maxDist);
        
        float angle = Random.Range(-Mathf.PI, Mathf.PI);
        
        Vector3 delta = Vector3.zero;
        delta.x = distance * Mathf.Cos(angle);
        delta.y = distance * Mathf.Sin(angle);
        
        Vector3 newPos = transform.position + delta;

        if (newPos.x < p1.x)
            newPos.x = p1.x + (p1.x - newPos.x);
        if (newPos.x > p2.x)
            newPos.x = p2.x - (newPos.x - p2.x);
        if (newPos.y > p1.y)
            newPos.y = p1.y + (p1.y - newPos.y);
        if (newPos.y < p2.y)
            newPos.y = p2.y - (newPos.y - p2.y);
        
        return newPos;
    }
}
