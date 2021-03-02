using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class PlayManager : MonoBehaviour
{
    // Instance
    public static PlayManager Instance = null;
    
    // Public
    public Text DebugText;
    public int playerScore;
    public string leaderboardID = "";
    public string achievementID = "";

    private static PlayGamesPlatform _platform;

    // Initialize the singleton instance.
    private void Awake()
    {
        // If there is not already an instance of GameManager, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Set PlayManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("Platform: Android");
        }
        else
        {
            Debug.Log("Platform: Windows");
        }
        
        if (_platform == null)
        {
            Debug.Log("Platform: NULL");
            try
            {
                // config
                PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
                // Instance
                PlayGamesPlatform.InitializeInstance(config);
                // recommended for debugging:
                PlayGamesPlatform.DebugLogEnabled = true;
                // Activate the Google Play Games platform
                _platform = PlayGamesPlatform.Activate();     
            }
            catch (Exception exception)
            {
                DebugText.text = "Platform Exception: " + exception.Message;
                Debug.Log(exception.Message);
            }
        }

        // Social Auth
        Social.localUser.Authenticate((bool success) => {
            if (success)
            {
                DebugText.text = "Auth Success";
            }
            else
            {
                DebugText.text = "Auth Faild";
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        //    
    }

    public void AuthLeaderboard()
    {
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.Authenticate((success) => {
                if (success)
                {
                    DebugText.text = "Signin succeess";
                }
                else
                {
                    DebugText.text = "Signin failed";
                }
            }, false);
        }
        else
        {
            PlayGamesPlatform.Instance.SignOut();
            DebugText.text = "Signin out";
        }
    }

    public void AddScoreToLeaderboard()
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportScore(100, leaderboardID, success => {
                if (success)
                {
                    DebugText.text = "Score Success";
                }
                else
                {
                    DebugText.text = "Score Faild";
                }
            });
        }
        else
        {
            DebugText.text = "Score - Auth Faild";
        }
    }

    public void ShowLeaderboard()
    {
        if (Social.localUser.authenticated)
        {
            DebugText.text = "Leaderboard Before";
            Social.ShowLeaderboardUI();
            DebugText.text = "Leaderboard After";
        }
        else
        {
            DebugText.text = "Leaderboard - Auth Faild";
        }
    }

    public void ShowAchievements()
    {
        if (Social.localUser.authenticated)
        {
            DebugText.text = "Achievement Before";
            Social.ShowAchievementsUI();
            DebugText.text = "Achievement After";
        }
        else
        {
            DebugText.text = "Achievement - Auth Faild";
        }
    }

    public void UnlockAchievement()
    {
        if (Social.localUser.authenticated)
        {
            Social.ReportProgress(achievementID, 100f, success => {
                if (success)
                {
                    DebugText.text = "Unlock Success";
                }
                else
                {
                    DebugText.text = "Unlock Faild";
                }
            });
        }
        else
        {
            DebugText.text = "Unlock - Auth Faild";
        }
    }
}
