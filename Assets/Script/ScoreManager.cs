using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private int _deathcount;
    private float _elaptime;
    //[SerializeField] private Text highscoreText;
    void Start()
    {
        _deathcount = PlayerPrefs.GetInt(this.name+"DeathCount", 0);
        _elaptime = PlayerPrefs.GetFloat(this.name+"Time", 0);
        GetComponent<Text>().text = "MinDeath: " + _deathcount.ToString()+"\nTime"
            + ((int)_elaptime / 60).ToString("00") + ":" + ((_elaptime % 60).ToString("00.00"));
    }


}