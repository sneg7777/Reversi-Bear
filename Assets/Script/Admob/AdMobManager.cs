using System;
using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdMobManager : MonoBehaviour
{
	private static AdMobManager instance;
	public static AdMobManager Instance { get { return instance; } }

	private string adUnitId;
	private RewardedAd rewardedAd;

	public System.Action<Reward> onHandleUserEarnedReward;
	public System.Action<AdFailedToLoadEventArgs> onHandleRewardedAdFailedToLoad;

	public System.Action onHandleRewardedAdFailedToShow;
	public System.Action onHandleRewardedAdClosed;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(gameObject);
		}
	}


	public void Init()
	{
		//adUnitId ����
#if UNITY_EDITOR
		string adUnitId = "unused";
#elif UNITY_ANDROID
        adUnitId = "ca-app-pub-3380965308864260~9781993420";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3380965308864260~9781993420";
#else
        string adUnitId = "unexpected_platform";
#endif

        // ����� ���� SDK�� �ʱ�ȭ��.
        MobileAds.Initialize(initStatus => { });

		//���� �ε� : RewardedAd ��ü�� loadAd�޼��忡 AdRequest �ν��Ͻ��� ����
		AdRequest request = new AdRequest.Builder().Build();
		this.rewardedAd = new RewardedAd(adUnitId);
		this.rewardedAd.LoadAd(request);


		this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded; // ���� �ε尡 �Ϸ�Ǹ� ȣ��
		this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad; // ���� �ε尡 �������� �� ȣ��
		this.rewardedAd.OnAdOpening += HandleRewardedAdOpening; // ���� ǥ�õ� �� ȣ��(��� ȭ���� ����)
		this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow; // ���� ǥ�ð� �������� �� ȣ��
		this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;// ���� ��û�� �� ������ �޾ƾ��� �� ȣ��
		this.rewardedAd.OnAdClosed += HandleRewardedAdClosed; // �ݱ� ��ư�� �����ų� �ڷΰ��� ��ư�� ���� ������ ���� ���� �� ȣ��
	}
	public void HandleRewardedAdLoaded(object sender, EventArgs args)
	{
		Debug.Log("HandleRewardedAdLoaded");
	}

	public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		Debug.Log("HandleRewardedAdFailedToLoad");
		this.onHandleRewardedAdFailedToLoad(args);

	}

	public void HandleRewardedAdOpening(object sender, EventArgs args)
	{
		Debug.Log("HandleRewardedAdOpening");
	}

	public void HandleRewardedAdFailedToShow(object sender, EventArgs args)
	{
		Debug.Log("HandleRewardedAdFailedToShow");
		this.onHandleRewardedAdFailedToShow();
	}

	public void HandleRewardedAdClosed(object sender, EventArgs args)
	{
		Debug.Log("HandleRewardedAdClosed");
		SceneManager.LoadScene(0);
		//SceneManager.LoadScene("Main", LoadSceneMode.Single);
		//this.onHandleRewardedAdClosed();
	}

	public void HandleUserEarnedReward(object sender, Reward args)
	{
		Debug.Log("HandleUserEarnedReward");
		//this.onHandleUserEarnedReward(args);

	}

	public bool IsLoaded()
	{
		return this.rewardedAd.IsLoaded();
	}

	public void ShowAds()
	{
		StartCoroutine(this.ShowAdsRoutine());
	}

	private IEnumerator ShowAdsRoutine()
	{
		while (true)
		{
			bool check = IsLoaded();
			if (check == true)
			{
				this.rewardedAd.Show();
				break;
			}
			else
			{
				Debug.Log("reward ad not loaded.");
			}

			yield return null;
		}
	}
}