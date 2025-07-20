using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : Singleton<UI>
{
    public GameObject PlayGameUI;
    public GameObject FinishUI;


    public void OpenPlayGameUI()
    {
        PlayGameUI.SetActive(true);
        FinishUI.SetActive(false);
    }

    public void OpenFinishUI()
    {
        PlayGameUI.SetActive(false);
        FinishUI.SetActive(true);
    }

    public void PlayGameBtn()
    {
        PlayGameUI.SetActive(false);
        LevelManager.Instance.OnStart();
        GameplayManager.Instance.ChangeState(GameState.Play);
    }

    public void RetryBtn()
    {
        LevelManager.Instance.LoadLevel();
        GameplayManager.Instance.ChangeState(GameState.MainMenu);
        OpenPlayGameUI();
    }

    public void NextLVBtn()
    {
        LevelManager.Instance.NextLevel();
        GameplayManager.Instance.ChangeState(GameState.MainMenu);
        OpenPlayGameUI();
    }

    public void ContinueBtn()
    {

    }
}
