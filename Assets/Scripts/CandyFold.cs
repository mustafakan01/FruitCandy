using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyFold : MonoBehaviour
{
    public float x,y;

    public string Colour;
    GameObject secim;

    public bool locationChance=false;
    bool fall=true;

    public static CandyFold firstCandy;
    public static CandyFold secondCandy;

    public Vector3 targetLocation;

    public List<CandyFold> candy_x;
    public List<CandyFold> candy_y;
    
    void Start()
    {
        secim= GameObject.FindGameObjectWithTag("Secim");
    }

   
    void Update()
    {
        
        if (fall==true)
        {
            if (transform.position.y-y<0.01f)
        {
            fall=false;
            transform.position= new Vector3(x,y,0);
        }
            transform.position=Vector3.Lerp( transform.position, new Vector3(x,y,0),Time.deltaTime*2f);
        }
        if (locationChance)
        {
            LocChance();
        }
    }

    public void NewLoc(float _x, float _y)
    {
        x=_x;
        y=_y;
    }

    private void OnMouseDown() 
    {
        secim.transform.position= transform.position;
        CandyControl();
        
    }

    void CandyControl()
    
    {
        if (firstCandy==null)
        {
            firstCandy=this;
        }
        else
        {
            secondCandy=this;
            if (firstCandy!=secondCandy)
            {
                float exraction_x=Mathf.Abs(firstCandy.x-secondCandy.x);
                float exraction_y=Mathf.Abs(firstCandy.y-secondCandy.y);

                if (exraction_x+exraction_y==1)
                {
                    //yer değiştirme
                    Debug.Log("Değiş");

                    firstCandy.targetLocation=secondCandy.transform.position;
                    secondCandy.targetLocation=firstCandy.transform.position;
                    firstCandy.locationChance=true;
                    secondCandy.locationChance=true;

                    

                    

                    Chance();
                    firstCandy.ControlX();
                    firstCandy.ControlY();
                    

                    firstCandy=null;
                    
                }
                else
                {
                    firstCandy=secondCandy;
                 
                }
            }
           
            
                secondCandy=null;
            
        }

        
    }
   
   void Chance()
   {
        creator.candies[(int)firstCandy.x,(int)firstCandy.y]=secondCandy;
        creator.candies[(int)secondCandy.x,(int)secondCandy.y]=firstCandy;

        float firstCandy_x=firstCandy.x;
        float firstCandy_y=firstCandy.y;

        firstCandy.x=secondCandy.x;
        firstCandy.y=secondCandy.y;

        secondCandy.x=firstCandy.x;
        secondCandy.y=firstCandy.y;
   }
    void LocChance()
        {
            transform.position=Vector3.Lerp(transform.position,targetLocation,0.2f);
        }

    void ControlX()
    {
        for (int i = (int)x+1; i < creator.candies.GetLength(0); i++)
        {
            CandyFold rightCandy=creator.candies[i,(int)y];
            if (Colour==rightCandy.Colour)
            {
                candy_x.Add(rightCandy);
            }
            else
            {
                break;
            }
        }
        for (int i = (int)x-1; i > 0; i--)
        {
            CandyFold rightCandy=creator.candies[i,(int)y];
            if (Colour==rightCandy.Colour)
            {
                candy_x.Add(rightCandy);
            }
            else
            {
                break;
            }
        }
    }
     void ControlY()
    {
        for (int i = (int)y+1; i < creator.candies.GetLength(0); i++)
        {
            CandyFold backCandy=creator.candies[(int)x,i];
            if (Colour==backCandy.Colour)
            {
                candy_y.Add(backCandy);
            }
            else
            {
                break;
            }
        }
        for (int i = (int)y-1; i > 0; i--)
        {
            CandyFold backCandy=creator.candies[(int)x,i];
            if (Colour==backCandy.Colour)
            {
                candy_y.Add(backCandy);
            }
            else
            {
                break;
            }
        }
    }
}

