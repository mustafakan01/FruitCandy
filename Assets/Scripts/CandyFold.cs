using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CandyFold : MonoBehaviour
{
    public float x, y; // Şekerin konum koordinatları
    public string Colour; // Şekerin rengi

    private GameObject secim; // Seçim nesnesi
    private Animator animator; // Animator bileşeni

    public bool locationChance = false; // Şekerin hareket etme izni
    private bool fall = true; // Şekerin düşme durumu

    public static CandyFold firstCandy; // İlk seçilen şeker
    public static CandyFold secondCandy; // İkinci seçilen şeker

    public Vector3 targetLocation; // Şekerin gitmeye çalıştığı hedef konum

    private List<CandyFold> candy_x = new List<CandyFold>(); // Şekerlerin yatay sıralaması
    private List<CandyFold> candy_y = new List<CandyFold>(); // Şekerlerin dikey sıralaması

    private void Start()
    {
        secim = GameObject.FindGameObjectWithTag("Secim"); // "Secim" etiketini taşıyan nesneyi al
        animator = GetComponent<Animator>(); // Animator bileşenini al
    }

    private void Update()
    {
        if (fall)
        {
            locationChance = false; // Şeker düşerken hareket etme izni kapat

            // Şekerin hedef konumuna yavaşça ilerlemesi
            if (Mathf.Abs(transform.position.y - y) < 0.01f)
            {
                fall = false;
                transform.position = new Vector3(x, y, 0);
            }
            transform.position = Vector3.Lerp(transform.position, new Vector3(x, y, 0), Time.deltaTime * 2f);
        }

        if (locationChance)
        {
            LocChance(); // Şekerin hedef konumuna gitmesi için işlevi çağır
        }
    }

    // Şekerin yeni konumunu ayarlar
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

    // Fare tıklaması algılandığında çalışır
    private void OnMouseDown()
    {
        secim.transform.position = transform.position; // Seçim nesnesinin pozisyonunu güncelle
        CandyControl(); // Şeker kontrolünü yap
    }

    // Şeker kontrolünü yapar
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

                    Chance(); // Şekerlerin yerlerini değiştir

                    firstCandy.ControlX(); // X ekseni kontrolünü yap
                    firstCandy.ControlY(); // Y ekseni kontrolünü yap
                    secondCandy.ControlX(); // X ekseni kontrolünü yap
                    secondCandy.ControlY(); // Y ekseni kontrolünü yap

                    StartCoroutine(firstCandy.Destroyer()); // Şekerleri yok etmeyi başlat
                    StartCoroutine(secondCandy.Destroyer()); // Şekerleri yok etmeyi başlat

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

    // Şekerlerin yerlerini değiştirir
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

    // Şekerin hedef konumuna gitmesini sağlar
    private void LocChance()
    {
        transform.position = Vector3.Lerp(transform.position, targetLocation, 0.2f);
    }

    // X ekseni kontrolünü yapar
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

    // Y ekseni kontrolünü yapar
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

    // Şekerleri yok etmeyi başlatır
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

    // Şekerleri yok eder
    public void Disappear()
    {
        Destroy(gameObject); // Şekeri yok et
        int controlx = (int)x;
        int index = 0;

        if (candy_y.Count > 0 || candy_x.Count > 0)
        {
            for (int i = (int)y + 1; i < creator.candies.GetLength(1); i++)
            {
                CandyFold control = candy_y.Find(e => e.x == controlx && e.y == i);

                if (control == null)
                {
                    control = creator.candies[(int)controlx, i];
                    if (control != null)
                    {
                        control.NewLoc(controlx, i - (candy_y.Count + 1));
                    }
                }
                if (i + 1 == creator.candies.GetLength(1))
                {
                    if (index < candy_x.Count)
                    {
                        i = (int)y;
                        controlx = (int)candy_x[index].x;
                        index++;
                    }
                }
            }
        }

        // Debug.Log("Yok oldu");
    }
}
