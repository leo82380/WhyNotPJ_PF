using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Discord;
using UnityEditor.SceneManagement;
using System;
using System.Threading.Tasks;

[InitializeOnLoad]
public class DiscordController
{
    private static Discord.Discord discord;
    private static string ProjectName { get { return Application.productName; } }
    private static string Version { get { return Application.unityVersion; } }
    private static RuntimePlatform platform { get { return Application.platform; } }
    private static string ActiveSceneName { get { return EditorSceneManager.GetActiveScene().name; } }
    private static long lastTimeStamp;

    private const string ApplicationID = "1113841137577369682";

    static DiscordController()
    {
        DelayInit();
    }
    private static async void DelayInit(int delay = 1000)
    {
        await Task.Delay(delay);
        SetupDiscord();
    }

    private static void SetupDiscord()
    {
        discord = new Discord.Discord(long.Parse(ApplicationID), (ulong)CreateFlags.Default);
        lastTimeStamp = GetTimeStamp();
        UpdateActivity();

        EditorApplication.update += EditorUpdate;
        EditorSceneManager.sceneOpened += SceneOpend;
    }
    private static void EditorUpdate()
    {
        discord.RunCallbacks();
    }
    private static void SceneOpend(UnityEngine.SceneManagement.Scene scene, OpenSceneMode sceneMode)
    {
        UpdateActivity();
    }
    private static void UpdateActivity()
    {
        ActivityManager activityManager = discord.GetActivityManager();
        Activity activity = new Activity
        {
            Details = "Editing " + ProjectName,
            State = ActiveSceneName + " | " + platform,
            Timestamps =
            {
                Start = lastTimeStamp
            },
            Assets =
            {
                LargeImage = "unity2",
                LargeText = Version,
                SmallImage = "unity2",
                SmallText = Version
            }
        };
        activityManager.UpdateActivity(activity, Result =>
        {
            Debug.Log("Discord Result: " + Result);
        });
    }
    private static long GetTimeStamp()
    {
        long unixTimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        return unixTimeStamp;
    }
}
