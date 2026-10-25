using UnityEngine;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

public class AchievementsManager : MonoBehaviour
{
    private bool _isStatsRecieved;
    private bool _isAcievementStatusUpdated;
    private bool _isStatsStored;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        RequestStats();
    }
    private void RequestStats()
    {
#if !DISABLESTEAMWORKS
        _isStatsRecieved = SteamUserStats.RequestCurrentStats();
        Debug.Log("is Stats Recieved: " + _isStatsRecieved);
#endif
    }
    public void SetAchievement(string achName)
    {
#if !DISABLESTEAMWORKS
        _isAcievementStatusUpdated = SteamUserStats.SetAchievement(achName);
        Debug.Log("is Stats Recieved " + achName + " status updated" + _isAcievementStatusUpdated);
        StoreStats();
#endif
    }
#if !DISABLESTEAMWORKS
    private void StoreStats()
    {
        _isStatsStored = SteamUserStats.StoreStats();
        Debug.Log("is Stats Stored " + _isStatsStored);
    }
#endif
}
