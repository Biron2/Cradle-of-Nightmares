using UnityEngine;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif

public class LeaderboardManager : MonoBehaviour
{
    public string leaderboardName = "con_leaderboard"; // Название вашего лидерборда
    public GameObject leaderboardEntryPrefab; // Префаб для отображения строки данных игрока
    public Transform scrollViewContent; // Контент ScrollView

#if !DISABLESTEAMWORKS
    private SteamLeaderboard_t leaderboard; // Хранит идентификатор лидерборда
    private CallResult<LeaderboardFindResult_t> findLeaderboardResult;
    private CallResult<LeaderboardScoresDownloaded_t> leaderboardScoresDownloaded;
#endif

    [SerializeField] private Settings _Settings;
    private void OnMouseEnter()
    {
        _Settings.OnBeginScroll();
    }
    private void OnMouseExit()
    {
        _Settings.OnEndScroll();
    }
    void Start()
    {
#if !DISABLESTEAMWORKS
        // Инициализация CallResult
        findLeaderboardResult = CallResult<LeaderboardFindResult_t>.Create(OnLeaderboardFindResult);
        leaderboardScoresDownloaded = CallResult<LeaderboardScoresDownloaded_t>.Create(OnLeaderboardEntriesDownloaded);

        // Инициализация Steamworks
        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steamworks не инициализирован!");
            return;
        }

        // Запросить лидерборд
        SteamAPICall_t handle = SteamUserStats.FindOrCreateLeaderboard(
            leaderboardName,
            ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending,
            ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric
        );
        findLeaderboardResult.Set(handle);
#else
        gameObject.SetActive(false);
#endif
    }

#if !DISABLESTEAMWORKS
private void OnLeaderboardFindResult(LeaderboardFindResult_t result, bool bIOFailure)
    {
        if (bIOFailure || result.m_bLeaderboardFound == 0)
        {
            Debug.LogError("Лидерборд не найден или ошибка ввода/вывода!");
            return;
        }

        leaderboard = result.m_hSteamLeaderboard;
        Debug.Log($"Лидерборд найден: {leaderboardName}");
        LoadTopEntries();
    }

    public void LoadTopEntries()
    {
        SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntries(
            leaderboard,
            ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal,
            0, // Начало (с 1 места)
            51  // Конец (50 место)
        );
        leaderboardScoresDownloaded.Set(handle);
    }

    private void OnLeaderboardEntriesDownloaded(LeaderboardScoresDownloaded_t result, bool bIOFailure)
    {
        if (bIOFailure || result.m_cEntryCount <= 0)
        {
            Debug.LogWarning("Не удалось загрузить данные из лидерборда!");
            return;
        }

        // Очистка старых записей в ScrollView
        foreach (Transform child in scrollViewContent)
        {
            Destroy(child.gameObject);
        }

        // Добавление записей в ScrollView
        for (int i = 0; i < result.m_cEntryCount; i++)
        {
            LeaderboardEntry_t entry;
            SteamUserStats.GetDownloadedLeaderboardEntry(result.m_hSteamLeaderboardEntries, i, out entry, null, 0);

            string playerName = SteamFriends.GetFriendPersonaName(entry.m_steamIDUser);
            int score = entry.m_nScore;

            // Создаем визуальный элемент для каждой записи
            GameObject entryGO = Instantiate(leaderboardEntryPrefab, scrollViewContent);
            var entryUI = entryGO.GetComponent<LeaderboardEntryUI>();
            if (entryUI != null)
            {
                entryUI.Setup(i + 1, playerName, score);
            }
        }
    }
#endif
}
