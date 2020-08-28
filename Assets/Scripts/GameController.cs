using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

[Serializable]
public class GameController : Singleton<GameController>
{
    public int CurrentScore = 0;
    public int Lifes = 5;
    public float MoleSpawnDelay;
    public float MoleLifeTime;
    public Board Board;

    Coroutine _moleSpawnRoutine;
    string _savePath; 
    PlayerData _player;
    [SerializeField] public List<PlayerData> _players;
    bool _isGameRunning = false;
    bool _isReadyToBegin = false;
    bool _isGameOver = false;

    public bool IsGameRunning { get => _isGameRunning; set => _isGameRunning = value; }

    private void Start()
    {
        _savePath = Application.dataPath + "/PlayersData.json";
        _players = new List<PlayerData>();

        Load(_savePath);

        StartCoroutine(ExecuteGameLoop());
    }

    IEnumerator ExecuteGameLoop()
    {
        yield return StartCoroutine(StartGameRoutine());
        yield return StartCoroutine(PlayGameRoutine());
        yield return StartCoroutine(EndGameRoutine());
    }


    public void BeginGame()
    {
        _player = new PlayerData
        {
            Name = UIController.Instance.PlayerNameInputField.text
        };
        _isGameOver = false;
        _isReadyToBegin = true;
    }

    IEnumerator StartGameRoutine()
    {
        while (!_isReadyToBegin)
        {
            yield return null;
        }
        UIController.Instance.ScreenFader.FadeOff();
        UIController.Instance.StartMessageWindow.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        IsGameRunning = true;
        yield return new WaitForSeconds(1f);
        StartCoroutine(DifficultyRoutine());
        Board.Spawning();
    }

    IEnumerator PlayGameRoutine()
    {
        while(!_isGameOver)
        {
            _isGameOver = Lifes <= 0;
            yield return null;
        }
        _player.Score = CurrentScore;

        _players.Add(_player);
    }

    IEnumerator EndGameRoutine()
    {
        IsGameRunning = false;

        Save(_savePath);

        yield return new WaitForSeconds(1f);
        UIController.Instance.ScreenFader.FadeOn();
        yield return new WaitForSeconds(0.5f);

        _players.Sort(new PlayerComparer());

        UIController.Instance.ScorePanel.SetActive(true);
        
        UIController.Instance.UpdateScoreTable(_players);
    }

    IEnumerator DifficultyRoutine()
    {
        while(IsGameRunning)
        {
            yield return new WaitForSeconds(5f);
            MoleSpawnDelay -= 0.1f;
            MoleLifeTime-=0.1f;

            MoleSpawnDelay = Mathf.Clamp(MoleSpawnDelay, 0.8f, 2f);
            MoleLifeTime = Mathf.Clamp(MoleLifeTime, 0.6f, 2f);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Save(string path)
    {
        string[] data = new string[_players.Count];
        for (int i = 0; i < _players.Count; i++)
        {
            data[i] = JsonUtility.ToJson(_players[i]);
        }

        File.WriteAllLines(path, data);
    }

    void Load(string path)
    {
        string[] data = File.ReadAllLines(path);
        for (int i = 0; i < data.Length; i++)
        {
            _players.Add(JsonUtility.FromJson<PlayerData>(data[i]));
        }
    }
}
