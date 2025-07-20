using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public List<Level> levels = new List<Level>();
    Level currentLevel;
    public Player player;
    int level = 1;

    private void Start()
    {
        LoadLevel();
        UI.Instance.OpenPlayGameUI();
    }

    public void LoadLevel()
    {
        LoadLevel(level);
        OnInit();
    }
    public void LoadLevel(int llevel)
    {
        if(currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }

        currentLevel = Instantiate(levels[llevel - 1]);
    }

    public void NextLevel()
    {
        level++;
        LoadLevel();
    }
    public void OnInit()
    {
        player.transform.position = currentLevel.startPoint.position;
        player.OnInit();
    }

    public void OnStart()
    {
        GameplayManager.Instance.ChangeState(GameState.Play);
    }

    public void OnFinish()
    {
        UI.Instance.OpenFinishUI();
        GameplayManager.Instance.ChangeState(GameState.Finish);
    }
}
    
