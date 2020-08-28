using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] int _width = 3;
    [SerializeField] int _height = 3;
    [SerializeField] int _borderSize = 2;
    [Space(20)]
    [SerializeField] GameObject _tilePrefab;
    [SerializeField] GameObject _molePrefab;

    Tile[,] _allTiles;
    Mole[,] _allMoles;

    private void Start()
    {
        _allTiles = new Tile[_width, _height];
        _allMoles = new Mole[_width, _height];
        SetupCamera();
        SetupTiles();
    }

    public void Spawning()
    {
        StartCoroutine(SpawnMoleRoutine());
    }

    void SetupCamera()
    {
        Camera.main.transform.position = new Vector3(_width / 2f, _height / 2f, -10f);

        float aspectRatio = (float)Screen.width / Screen.height;

        float verticalSize = _height / 2f + _borderSize;
        float horizontalSize = (_width / 2f + _borderSize) / aspectRatio;

        Camera.main.orthographicSize = (verticalSize > horizontalSize) ? verticalSize : horizontalSize;
    }
    void MakeTile(GameObject tilePrefab, int x, int y)
    {
        if(tilePrefab!=null)
        {
            GameObject tile = Instantiate(tilePrefab, new Vector3(x * 1.5f, y * 1.5f, 1), Quaternion.identity);
            tile.name = "Tile: " + x + " " + y;
            _allTiles[x, y] = tile.GetComponent<Tile>();
            tile.transform.parent = transform;
            _allTiles[x, y].Init(x, y);
        }
    }
    

    void SetupTiles()
    {

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (_allTiles[i, j] == null)
                {
                    MakeTile(_tilePrefab, i, j);
                }
            }
        }
    }
    void SpawnMole(int x, int y)
    {
        GameObject mole = Instantiate(_molePrefab, new Vector3(x * 1.5f, y * 1.5f, -2), Quaternion.identity);
        _allMoles[x, y] = mole.GetComponent<Mole>();
        _allMoles[x, y].Init(x, y);
    }
    void RandomSpawnMole()
    {
        int xIndex = Random.Range(0, _width);
        int yIndex = Random.Range(0, _height);
        if (_allMoles[xIndex, yIndex] == null)
            SpawnMole(xIndex, yIndex);
        else
            RandomSpawnMole();
    }
    void SpawnManyMoles(int value=1)
    {
        for (int i = 0; i < value; i++)
        {
            RandomSpawnMole();
        }
    }
    IEnumerator SpawnMoleRoutine()
    {
        while (GameController.Instance.IsGameRunning)
        {
            SpawnManyMoles(Random.Range(2, 4));
            yield return new WaitForSeconds(GameController.Instance.MoleSpawnDelay);
        }
    }

}
