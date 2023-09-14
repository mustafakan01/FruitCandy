using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyFold : MonoBehaviour
{
    public float x, y;
    public string Colour;

    private GameObject secim;
    private Animator animator;

    public bool locationChance = false;
    private bool fall = true;

    public static CandyFold firstCandy;
    public static CandyFold secondCandy;

    public Vector3 targetLocation;

    private List<CandyFold> candy_x = new List<CandyFold>();
    private List<CandyFold> candy_y = new List<CandyFold>();

    private void Start()
    {
        secim = GameObject.FindGameObjectWithTag("Secim");
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (fall)
        {
            locationChance=false;
            if (Mathf.Abs(transform.position.y - y) < 0.01f)
            {
                fall = false;
                transform.position = new Vector3(x, y, 0);
            }
            transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, 0), Time.deltaTime * 2f);
        }

        if (locationChance)
        {
            LocChance();
        }
    }

 public void NewLoc(float _x, float _y)
{
    // Eski konumu temizle
    creator.candies[(int)x, (int)y] = null;

    // Yeni konumu ayarla
    x = _x;
    y = _y;

    // Yeni konumu güncelle
    creator.candies[(int)x, (int)y] = this;

    // Şekerin düşme durumunu sıfırla
    fall = true;
}


    private void OnMouseDown()
    {
        secim.transform.position = transform.position;
        CandyControl();
    }

    private void CandyControl()
    {
        if (firstCandy == null)
        {
            firstCandy = this;
        }
        else
        {
            secondCandy = this;
            if (firstCandy != secondCandy)
            {
                float exraction_x = Mathf.Abs(firstCandy.x - secondCandy.x);
                float exraction_y = Mathf.Abs(firstCandy.y - secondCandy.y);

                if (exraction_x + exraction_y == 1)
                {
                    Debug.Log("Değiş");

                    firstCandy.targetLocation = secondCandy.transform.position;
                    secondCandy.targetLocation = firstCandy.transform.position;
                    firstCandy.locationChance = true;
                    secondCandy.locationChance = true;

                    Chance();

                    firstCandy.ControlX();
                    firstCandy.ControlY();
                    secondCandy.ControlX();
                    secondCandy.ControlY();

                    StartCoroutine(firstCandy.Destroyer());
                    StartCoroutine(secondCandy.Destroyer());

                    firstCandy = null;
                }
                else
                {
                    firstCandy = secondCandy;
                }
            }

            secondCandy = null;
        }
    }

    private void Chance()
    {
        creator.candies[(int)firstCandy.x, (int)firstCandy.y] = secondCandy;
        creator.candies[(int)secondCandy.x, (int)secondCandy.y] = firstCandy;

        float tempX = firstCandy.x;
        float tempY = firstCandy.y;

        firstCandy.x = secondCandy.x;
        firstCandy.y = secondCandy.y;

        secondCandy.x = tempX;
        secondCandy.y = tempY;
    }

    private void LocChance()
    {
        transform.position = Vector3.Lerp(transform.position, targetLocation, 0.2f);
    }

    private void ControlX()
{
    candy_x.Clear();

    // Sağa doğru kontrol
    for (int i = (int)x + 1; i < creator.candies.GetLength(0); i++)
    {
        CandyFold rightCandy = creator.candies[i, (int)y];
        if (rightCandy != null && Colour == rightCandy.Colour)
        {
            candy_x.Add(rightCandy);
        }
        else
        {
            break; // Renk değiştiğinde döngüyü sonlandır
        }
    }

    // Sola doğru kontrol
    for (int i = (int)x - 1; i >= 0; i--)
    {
        CandyFold leftCandy = creator.candies[i, (int)y];
        if (leftCandy != null && Colour == leftCandy.Colour)
        {
            candy_x.Add(leftCandy);
        }
        else
        {
            break; // Renk değiştiğinde döngüyü sonlandır
        }
    }
}


   private void ControlY()
{
    candy_y.Clear();

    // Yukarı doğru kontrol
    for (int i = (int)y + 1; i < creator.candies.GetLength(1); i++)
    {
        CandyFold backCandy = creator.candies[(int)x, i];
        if (backCandy != null && Colour == backCandy.Colour)
        {
            candy_y.Add(backCandy);
        }
        else
        {
            break; // Renk değiştiğinde döngüyü sonlandır
        }
    }

    // Aşağı doğru kontrol
    for (int i = (int)y - 1; i >= 0; i--)
    {
        CandyFold frontCandy = creator.candies[(int)x, i];
        if (frontCandy != null && Colour == frontCandy.Colour)
        {
            candy_y.Add(frontCandy);
        }
        else
        {
            break; // Renk değiştiğinde döngüyü sonlandır
        }
    }
}


    private IEnumerator Destroyer()
    {
        yield return new WaitForSeconds(0.3f);

        if (candy_x.Count >= 2 || candy_y.Count >= 2)
        {
            animator.SetBool("yok_ol", true);

            if (candy_x.Count >= 2)
            {
                foreach (var item in candy_x)
                {
                    item.animator.SetBool("yok_ol", true);
                }
            }
            else
            {
                foreach (var item in candy_y)
                {
                    item.animator.SetBool("yok_ol", true);
                }
            }
        }
    }
     public void DisappearAndReplace()
    {
        // Patlayan şekerleri kaldır
        foreach (var candy in candy_x)
        {
            candy.Disappear();
        }
        foreach (var candy in candy_y)
        {
            candy.Disappear();
        }

        // Patlayan şekerlerin yerine yeni şekerler oluştur
        creator.PopulateCandies(); // Yeni şekerlerin oluşturulduğu bir yöntem ekleyin
    }

    public void Disappear()
    {
        Destroy(gameObject);
        int controlx=(int)x;
        int index=0;

        if (candy_y.Count > 0 || candy_x.Count > 0)
        {
            for (int i = (int)y + 1; i < creator.candies.GetLength(1); i++)
            {
                CandyFold control = candy_y.Find(e => e.x == controlx && e.y == i);
                
                if (control==null)
                {
                    control = creator.candies[(int)controlx, i];  
                    if (control != null)
                    {
                    control.NewLoc(controlx, i - (candy_y.Count + 1));
                    }
                }
            if (i+1==creator.candies.GetLength(1))
            {
                if (index<candy_x.Count)
                {
                    
                    i=(int)y;
                    controlx=(int)candy_x[index].x;
                    index++;
                    
                }
                
            }
                
            }
        }

        Debug.Log("Yok oldu");
    }
}
