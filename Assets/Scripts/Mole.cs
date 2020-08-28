using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    public int xIndex;
    public int yIndex;

    private void Start()
    {
        StartCoroutine(MoleLifeTimeRoutine());
    }

    public void Init(int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }

    private void OnMouseDown()
    {
        GameController.Instance.CurrentScore++;
        UIController.Instance.UpdateScoreText();
        Destroy(gameObject);
    }

    IEnumerator MoleLifeTimeRoutine()
    {
        yield return new WaitForSeconds(GameController.Instance.MoleLifeTime);
        GameController.Instance.Lifes--;
        GameController.Instance.Lifes = Mathf.Clamp(GameController.Instance.Lifes, 0, 5);
        UIController.Instance.UpdateLifes();
        Destroy(gameObject);
    }
}
