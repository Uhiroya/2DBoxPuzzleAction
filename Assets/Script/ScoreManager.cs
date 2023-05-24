using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private int Deathcount;
    private float elaptime;
    //[SerializeField] private Text highscoreText;
    void Start()
    {
        Deathcount = PlayerPrefs.GetInt(this.name+"DeathCount", 0);
        elaptime = PlayerPrefs.GetFloat(this.name+"Time", 0);
        GetComponent<Text>().text = "MinDeath: " + Deathcount.ToString()+"\nTime"
            + ((int)elaptime / 60).ToString("00") + ":" + ((elaptime % 60).ToString("00.00"));
    }


}