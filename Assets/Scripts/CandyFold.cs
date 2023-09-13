using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyFold : MonoBehaviour
{
    public float x,y;
    GameObject secim;

    public bool locationChance=false;
    bool fall=true;

    public static CandyFold firstCandy;
    public static CandyFold secondCandy;

    public Vector3 targetLocation;
    
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
    void LocChance()
        {
            transform.position=Vector3.Lerp(transform.position,targetLocation,0.2f);
        }
}

