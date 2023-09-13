using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creator : MonoBehaviour
{
    public GameObject[] sekerler;
    public int width;
    public int height;
    public CandyFold[ , ] candies;
    void Start()
    {
        candies = new CandyFold[width,height];
        for (int a = 0; a < width; a++)
        {
            for (int b = 0; b < height; b++)
            {
                CreatorCandy(a,b);
            }
        }
    }

    void Update()
    {
        
    }

    public void CreatorCandy(int x, int y)
    {
        GameObject newCandy = GameObject.Instantiate(RandomCandy(),new Vector2(x , y + 10), Quaternion.identity);
        CandyFold candyFold= newCandy.GetComponent<CandyFold>();
        candyFold.NewLoc(x,y);
        candies [x,y]=candyFold;
    }

    public GameObject RandomCandy()
    {
        int random= Random.Range(0, sekerler.Length);
        return sekerler[random];

    }
}
