using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Target : MonoBehaviour
{
    public float speed = 2;

    private bool flying = true;

    public bool Flying
    {
        get { return flying; }
        set { SetFlying(value); }
    }

    private RandomNumberGenerator random;
    private Vector2 screenSize = Vector2.zero;
    
    private Vector3 velocity = Vector3.zero;
    private Vector2 remVelocity = Vector2.zero;
    
    private int bounceCounter = 0;
    private const float offset = 0.3f;
    private int bounceActive = 0;

    public void ResetPosition()
    {
        this.transform.position = new Vector3(0, 0);
        remVelocity = Get45Direction();
        velocity = Vector3.zero;
        flying = false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        random = RandomNumberGenerator.Create();
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        
        screenSize = new Vector2(width, height);

        Vector2 rndPos = GetRandomPosition();
        Debug.Log(rndPos);
        this.transform.position = rndPos;

        velocity = GetRandomDirection();
    }

    // Update is called once per frame
    void Update()
    {
        bool bounceFlag = InBounceArea();
        if (bounceFlag)
        {
            bounceCounter++;

            DoBouncing(ref bounceFlag);

            //velocity = AddNoise(velocity);
        }

        transform.position += velocity * Time.deltaTime * speed;
    }

    private void DoBouncing(ref bool bFlag)
    {
        GetBounds(out Vector2 p1, out Vector2 p2);

        if (transform.position.y > p1.y)
        {
            velocity.y = -velocity.y;
            transform.position = new Vector2(transform.position.x, p1.y);
            bFlag = false;
        }
        if (transform.position.x > p2.x)
        {
            velocity.x = -velocity.x;
            transform.position = new Vector2(p2.x, transform.position.y);
            bFlag = false;
        }
        if (transform.position.y < p2.y)
        {
            velocity.y = -velocity.y;
            transform.position = new Vector2(transform.position.x, p2.y);
            bFlag = false;
        }
        if (transform.position.x < p1.x)
        {
            velocity.x = -velocity.x;
            transform.position = new Vector2(p1.x, transform.position.y);
            bFlag = false;
        }
    }
    
    private void SetFlying(bool value)
    {
        flying = value;
        if (value == true)
        {
            velocity = remVelocity;
        } else
        {
            remVelocity = velocity;
            velocity = Vector2.zero;
        }
    }
    
    private bool InBounceArea()
    {
        bool bFlag = false;
        
        GetBounds(out Vector2 p1, out Vector2 p2);
        
        if (transform.position.y > p1.y)
            bFlag = true;
        if (transform.position.x > p2.x)
            bFlag = true;
        if (transform.position.y < p2.y)
            bFlag = true;
        if (transform.position.x < p1.x)
            bFlag = true;

        return bFlag;
    }

    private Vector2 AddNoise(Vector2 v)
    {
        float angle = Vector2.Angle(Vector2.right, v);

        float rnd = Random.Range(-Mathf.PI / 128, Mathf.PI / 128);
        angle += rnd;
        
        Vector2 result = Vector2.zero;
        result.x = Mathf.Cos(angle);
        result.y = Mathf.Sin(angle);

        return result.normalized;
    }
    
    private Vector2 GetRandomPosition()
    {
        GetBounds(out Vector2 p1, out Vector2 p2, offset);

        Vector2 rndPos = Vector2.zero;
        rndPos.x = Random.Range(p1.x, p2.x);
        rndPos.y = Random.Range(p1.y, p2.y);

        return rndPos;
    }

    private Vector2 GetRandomDirection()
    {
        int rnd = Random.Range(0, 3);

        float angle = (rnd * Mathf.PI / 2) + Mathf.PI / 4;
        
        Vector2 delta = Vector2.zero;
        delta.x = Mathf.Cos(angle);
        delta.y = Mathf.Sin(angle);

        return delta.normalized;
    }
    
    private Vector2 Get45Direction()
    {
        float angle = Mathf.PI + (Mathf.PI / 4);
        
        Vector2 delta = Vector2.zero;
        delta.x = Mathf.Cos(angle);
        delta.y = Mathf.Sin(angle);

        return delta.normalized;
    }

    private void GetBounds(out Vector2 p1, out Vector2 p2, float off = offset)
    {
        p1 = new Vector2(-screenSize.x / 2 + off, screenSize.y / 2 - off);
        p2 = new Vector2(screenSize.x / 2 - off, -screenSize.y / 2 + off);
    }
}
