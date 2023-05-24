using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;    //シーンマネージャー呼び出し

public class TitleManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    //スタートボタンを押すとセレクト画面に切り替わる
    public void StartBt()
    {
        
        SceneManager.LoadScene("SelectScene");
    }
}
