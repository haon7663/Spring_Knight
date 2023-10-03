using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
public class GoogleLogin : MonoBehaviour
{
    [SerializeField] Text LogText;
    void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();//�����÷��� �÷��� Ȱ��ȭ
        //���� �Լ��� �����ϸ� Social.Active= PlayGamesPlatform.Instance�� �ȴ�
    }
    public void Login()//�����÷��� �α��� ��ư�� ����
    {
        Social.localUser.Authenticate
        (
            (bool success) =>
            {
                if (success)//�����ÿ�
                {
                    LogText.text = "�α��� ����";
                    StartCoroutine(LoadMain());
                }
                else
                {
                    LogText.text = "�α��� ����";
                }
            }
        );
    }
    IEnumerator LoadMain()//�����÷��� �α��� �����ϰ� 4�� �̵��� Mainȭ�� �ҷ�����
    {
        yield return new WaitForSecondsRealtime(4.0f);
        SceneManager.LoadScene("Lobby");
    }
}