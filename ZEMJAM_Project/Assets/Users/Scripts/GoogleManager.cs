using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoogleManager : MonoBehaviour
{
    public static GoogleManager Inst { get; set; }

    void Awake()
    {
        var obj = FindObjectsOfType<GoogleManager>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Inst = this;
    }

    public Text LogText;

    private void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        LogIn();
    }
     
    public void LogIn()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                LogText.text = Social.localUser.id + " \n " + Social.localUser.userName;
                StartCoroutine(MoveScene());
            }
            else LogText.text = "���۷α��ν���";
        });
    }

    ISavedGameClient SavedGame()
    {
        return PlayGamesPlatform.Instance.SavedGame;
    }

    public void LoadCloud()
    {
        SavedGame().OpenWithAutomaticConflictResolution("savegame", DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLastKnownGood, LoadGame);
    }

    void LoadGame(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            SavedGame().ReadBinaryData(game, LoadData);
        }
    }

    void LoadData(SavedGameRequestStatus status, byte[] LoadedData)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            string data = System.Text.Encoding.UTF8.GetString(LoadedData);
            LogText.text = data;

        }
        else LogText.text = "�ҷ����� ����";

    }

    public void SavedCloud()
    {
        SavedGame().OpenWithAutomaticConflictResolution("savegame", DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLastKnownGood, SaveGame);
    }

    public void SaveGame(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            var update = new SavedGameMetadataUpdate.Builder().Build();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes("1000��");
            SavedGame().CommitUpdate(game, update, bytes, SaveData);
        }

    }

    void SaveData(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            LogText.text = "���强��";
        }
        else LogText.text = "���� ����";
    }

    public void DeleteCloud()
    {
        SavedGame().OpenWithAutomaticConflictResolution("savegame", DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, DeleteGame);
    }

    void DeleteGame(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            SavedGame().Delete(game);
            LogText.text = "���� ����";
        }
        else LogText.text = "���� ����";
    }

    IEnumerator MoveScene()
    {
        yield return YieldInstructionCache.WaitForSeconds(1.8f);
        Fade.Inst.Fadein(0.2f);
        yield return YieldInstructionCache.WaitForSeconds(0.2f);
        SceneManager.LoadScene("Main");
    }
}