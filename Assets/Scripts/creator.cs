using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creator : MonoBehaviour
{
    public GameObject[] sekerler;
    public int width;
    public int height;
    public static CandyFold[ , ] candies;
     public GameObject candyPrefab; // Şeker nesnesi için bir önceden tanımlanmış şeker prefabı
    public int numRows = 8; // Satır sayısı
    public int numColumns = 8; // Sütun sayısı
    void Start()
    {
        PopulateCandies();
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
      public  void PopulateCandies()
    {
        // Eşleştirme oyunu için rastgele veya belirli bir kurallara göre yeni şekerler oluşturun
        // Bu örnekte rastgele renklerle yeni şekerler oluşturuyoruz
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numColumns; col++)
            {
                Vector3 spawnPosition = new Vector3(col, row, 0); // Yeni şekerin konumu

                // Yeni şeker nesnesini oluşturun ve sahneye ekleyin
                GameObject newCandy = Instantiate(candyPrefab, spawnPosition, Quaternion.identity);
                
                // Yeni şekerin rengini rastgele seçin veya belirli bir kurala göre ayarlayın
                string randomColor = GetRandomColor();
                
                // Yeni şekerin rengini CandyFold bileşenine atayın
                CandyFold candyFold = newCandy.GetComponent<CandyFold>();
                candyFold.Colour = randomColor;
                candyFold.x = col;
                candyFold.y = row;

                // Yeni şekerleri oyun sahnesine yerleştirdikten sonra, bunları CandyFold sınıfında da saklayabilirsiniz.
                // creator.candies[(int)candyFold.x, (int)candyFold.y] = candyFold;
            }
        }
    }

    // Rastgele bir renk döndüren basit bir örnek
    private string GetRandomColor()
    {
        string[] colors = { "Red", "Green", "Blue", "Yellow", "Purple" };
        int randomIndex = Random.Range(0, colors.Length);
        return colors[randomIndex];
    }

    public void CreatorCandy(int x, int y)
    {
        GameObject RandomCandyObject= RandomCandy();
        GameObject newCandy = GameObject.Instantiate(RandomCandyObject,new Vector2(x , y + 10), Quaternion.identity);
        CandyFold candyFold= newCandy.GetComponent<CandyFold>();
        candyFold.Colour=RandomCandyObject.name;
        candyFold.NewLoc(x,y);
        candies [x,y]=candyFold;
    }

    public GameObject RandomCandy()
    {
        int random= Random.Range(0, sekerler.Length);
        return sekerler[random];

    }
}
