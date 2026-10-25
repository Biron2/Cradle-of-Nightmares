using UnityEngine;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

public class LeaderboardRecorder : MonoBehaviour
{
#if !DISABLESTEAMWORKS
    private SteamLeaderboard_t leaderboard; // Идентификатор лидерборда
    private CallResult<LeaderboardFindResult_t> findLeaderboardResult;
    private CallResult<LeaderboardScoreUploaded_t> scoreUploadedResult;
#endif

    public string leaderboardName = "con_leaderboard"; // Название лидерборда

    void Start()
    {
#if !DISABLESTEAMWORKS
        findLeaderboardResult = CallResult<LeaderboardFindResult_t>.Create(OnLeaderboardFindResult);
        scoreUploadedResult = CallResult<LeaderboardScoreUploaded_t>.Create(OnScoreUploaded);

        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steamworks не инициализирован!");
            return;
        }

        // Найти или создать лидерборд
        SteamAPICall_t handle = SteamUserStats.FindOrCreateLeaderboard(
            leaderboardName,
            ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending,
            ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric
        );
        findLeaderboardResult.Set(handle);
#endif
    }

#if !DISABLESTEAMWORKS
    private void OnLeaderboardFindResult(LeaderboardFindResult_t result, bool bIOFailure)
    {
        if (bIOFailure || result.m_bLeaderboardFound == 0)
        {
            Debug.LogError("Лидерборд не найден!");
            return;
        }

        leaderboard = result.m_hSteamLeaderboard;
        Debug.Log($"Лидерборд найден: {leaderboardName}");
    }
#endif

    public void SubmitScore(int score)
    {
#if !DISABLESTEAMWORKS
        if (leaderboard.m_SteamLeaderboard == 0)
        {
            Debug.LogWarning("Лидерборд еще не найден!");
            return;
        }

        SteamAPICall_t handle = SteamUserStats.UploadLeaderboardScore(
            leaderboard,
            ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest,
            score,      // Новый результат
            null,       // Дополнительные данные (например, статистика матча)
            0           // Длина массива данных
        );
        scoreUploadedResult.Set(handle);
#endif
    }

#if !DISABLESTEAMWORKS
    private void OnScoreUploaded(LeaderboardScoreUploaded_t result, bool bIOFailure)
    {
        if (bIOFailure)
        {
            Debug.LogError("Ошибка при загрузке рекорда!");
            return;
        }

        if (result.m_bSuccess != 0)
        {
            Debug.Log($"Рекорд успешно загружен! Новый рекорд: {result.m_nScore}");
        }
        else
        {
            Debug.LogWarning("Рекорд не был загружен.");
        }
    }
#endif
}
