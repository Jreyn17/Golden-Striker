using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;

    public static event Action OnGameStarted;

    public int points;

    #region Buttons
    public void OnPlayClicked()
    {
        startPanel.SetActive(false);
        OnGameStarted?.Invoke();
    }

    public void OnLeaderboardClicked()
    {

    }
    #endregion

    void Update()
    {
        if (points % 10 == 0)
        {

        }
    }
}
