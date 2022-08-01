using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public static UIManager sharedInstance; //singleton
    public TMP_Text score, highscore, lives;


    void Start()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        score.text = "SCORE:" + BoardManager.sharedInstance.score.ToString("00000");
        highscore.text = "HIGHSCORE:" + BoardManager.sharedInstance.highScore.ToString("00000");
        lives.text = "LIVES:" + BoardManager.sharedInstance.lives.ToString();

    }


}
