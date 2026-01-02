using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> names;
    [SerializeField] private List<TextMeshProUGUI> scores;

    private string publicLeaderboardKey = "e552eb83d9a62c7d762c7851083cf79e19cd0efa9788934441845e075e6ea21d"; //Version 1.0.0
    private string publicLeaderboardKey2 = "a801d46e6de97ee0162b123c8c662589060fbdcd65a6d2bc0e2dc0bab4659e9e"; //Version 1.0.1

    void Start()
    {
        GetLeaderboard(publicLeaderboardKey);
    }

    void OnEnable()
    {
        GameManager.OnSubmitScore += SetLeaderboardEntry;
    }

    void OnDisable()
    {
        GameManager.OnSubmitScore -= SetLeaderboardEntry;
    }

    #region Buttons
    public void Leaderboard1Button()
    {
        GetLeaderboard(publicLeaderboardKey);
    }

    public void Leaderboard2Button()
    {
        GetLeaderboard(publicLeaderboardKey2);
    }
    #endregion

    public void GetLeaderboard(string key)
    {
        for (int i = 0; i < names.Count; i++)
        {
            names[i].text = null;
            scores[i].text = null;
        }
        LeaderboardCreator.GetLeaderboard(key, ((msg) =>
        {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;

            for (int i = 0; i < loopLength; i++)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey2, username, score, ((msg) =>
        {
            GetLeaderboard(publicLeaderboardKey2);
        }));
    }
}
