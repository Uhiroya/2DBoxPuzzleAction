using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    //セレクトボタンを押すと対応したステージに切り替わる
    public void SelectBt(int Stage)
    {
        SceneManager.LoadScene("Stage" + Stage);
    }
}