using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creator : MonoBehaviour
{
    public GameObject[] sekerler; // Bir dizi şekeri temsil eden oyun nesneleri
    public int width; // Oluşturulan şekerlerin genişliği
    public int height; // Oluşturulan şekerlerin yüksekliği
    public static CandyFold[, ] candies; // Şekerleri saklamak için kullanılan bir dizi

    void Start()
    {
        candies = new CandyFold[width, height]; // candies dizisini boyutlarına uygun olarak oluştur

        for (int a = 0; a < width; a++)
        {
            for (int b = 0; b < height; b++)
            {
                CreatorCandy(a, b); // Her hücreye bir şeker oluştur
            }
        }
    }

    void Update()
    {
        // Güncelleme işlevi, bu örnekte kullanılmıyor
    }

    public void CreatorCandy(int x, int y)
    {
        GameObject RandomCandyObject = RandomCandy(); // Rastgele bir şeker oyun nesnesi seç
        GameObject newCandy = GameObject.Instantiate(RandomCandyObject, new Vector2(x, y + 10), Quaternion.identity); // Seçilen şeker nesnesini belirtilen konumda oluştur
        CandyFold candyFold = newCandy.GetComponent<CandyFold>(); // Oluşturulan nesnenin CandyFold bileşenini al
        candyFold.Colour = RandomCandyObject.name; // Şekerin rengini belirle (nesnenin adıyla)
        candyFold.NewLoc(x, y); // Şekerin yeni konumunu ayarla
        candies[x, y] = candyFold; // Şekeri candies dizisinde sakla
    }

    public GameObject RandomCandy()
    {
        int random = Random.Range(0, sekerler.Length); // rastgele bir indeks seç
        return sekerler[random]; // seçilen indeksteki şeker oyun nesnesini döndür
    }
}
