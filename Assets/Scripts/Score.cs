using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public GameObject textScore;
    public Text Skor;

    void Start()
    {
        Skor=textScore.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Skor.text="seks";
    }
}
