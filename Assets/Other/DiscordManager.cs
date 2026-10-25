using System;
using UnityEngine;

public class DiscordManager : MonoBehaviour
{
#if !PLAYMARKET
    private Discord.Discord discord;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        try
        {
            discord = new Discord.Discord(1314302479256518687, (ulong)Discord.CreateFlags.NoRequireDiscord);
            ChangeActivity();
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }
    private void OnDisable()
    {
        if(discord != null) discord.Dispose();
    }
    public void ChangeActivity()
    {
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            State = "Hunts for the souls of criminals",
            Assets = {
                LargeImage = "app_icon",
                LargeText = "Cradle of Nightmares"
            }

        };
        activityManager.UpdateActivity(activity, (res) =>
        {
            Debug.Log("Activity Updated!");
        });
    }
    void Update()
    {
        if (discord != null) discord.RunCallbacks();
    }
#else
    private void Start()
    {
        Destroy(gameObject);
    }
#endif
}
