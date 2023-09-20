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
            else LogText.text = "구글로그인실패";
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
        else LogText.text = "불러오기 실패";

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
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes("1000점");
            SavedGame().CommitUpdate(game, update, bytes, SaveData);
        }

    }

    void SaveData(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            LogText.text = "저장성공";
        }
        else LogText.text = "저장 실패";
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
            LogText.text = "삭제 성공";
        }
        else LogText.text = "삭제 실패";
    }

    IEnumerator MoveScene()
    {
        yield return YieldInstructionCache.WaitForSeconds(1.8f);
        Fade.Inst.Fadein(0.2f);
        yield return YieldInstructionCache.WaitForSeconds(0.2f);
        SceneManager.LoadScene("Main");
    }
}