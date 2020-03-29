// UI Pack : Toony PRO
// Version: 1.2.0
// Compatilble: Unity 5.5.1 or higher, see more info in Readme.txt file.
//
// Developer:			Gold Experience Team (https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:4162)
// Unity Asset Store:	https://www.assetstore.unity3d.com/en/#!/content/44103
//
// Please direct any bugs/comments/suggestions to geteamdev@gmail.com

#region Namespaces

using UnityEngine;
using System.Collections;

#endregion // Namespaces

// ######################################################################
// UIPT_PRO_SoundController class
//
// Simple Play/stop BG music and sounds.
// ######################################################################

public class SoundManager : MonoBehaviour
{
	// ########################################
	// Variables
	// ########################################

	#region Variables

	// Private reference which can be accessed by this class only
	private static SoundManager instance;

	// 싱글톤
	// Public static reference that can be accesd from anywhere
	public static SoundManager Instance
	{
		get
		{
			// Check if instance has not been set yet and set it it is not set already
			// This takes place only on the first time usage of this reference
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<SoundManager>();
				DontDestroyOnLoad(instance.gameObject);
			}

			return instance;
		}
	}

	// Max number of AudioSource components
	public int m_MaxAudioSource = 8;

	//각각 효과음, 음악들을 변수로 생성한다.
	
	// AudioClip component for music
	public AudioClip m_Music = null;
	public AudioClip DetectionMusic = null;
	public AudioClip SynthesisMusic = null;
	public AudioClip AlternativeMusic = null;
	public AudioClip EliminationMusic = null;

	// AudioClip component for buttons
	public AudioClip m_ButtonBack = null;
	public AudioClip m_ButtonClick = null;
	public AudioClip m_ButtonDisable = null;
	public AudioClip m_ButtonNo = null;
	public AudioClip m_ButtonPause = null;
	public AudioClip m_ButtonPlay = null;
	public AudioClip m_ButtonTab = null;
	public AudioClip m_ButtonYes = null;

	public AudioClip m_ButtonExitGame = null;

	// 게임 선택 시 호버했을 떄나오는 소리 
	public AudioClip m_ButtonGameHover = null;

	// 문어가 움직일 때 나는 소리
	public AudioClip m_OctopusMove = null;

	// 말풍선 pop 할때 나는 소리
	public AudioClip m_SpeechBubblePop = null;

	// DetectionStage Barrel 나타나는 소리
	public AudioClip m_BarrelCreated = null;

	// 올바른 보기를 클릭했을 때
	public AudioClip m_ClickedCorrectAnswer = null;

	// 틀린 보기를 선택했을 떄
	public AudioClip m_ClickedWrongAnswer = null;

	// 결과창에서 별이 박힐 때
	public AudioClip m_StarShowedUp = null;

	// 별을 하나도 못 받았을 때 
	public AudioClip m_NoStarShowedUp = null;

	// 해마가 물 쏠 때
	public AudioClip m_SeahorseWaves = null;

	// 대치과제에서 단어가 제시될 때
	public AudioClip m_AlterWordShowedUp = null;

	// 대치과제에서 버블이 나올 때
	public AudioClip m_AlterBubbleShowedUp = null;

	// 탈락과제에서 상어가 튀어나올 때
	public AudioClip m_SharkShowedUp = null;

	// 탈락과제에서 상어가 물고기를 먹을 때
	public AudioClip m_SharkEatingFish = null;

	// 합성과제에서 해파리가 뽀글뽀글 올라올 떄
	public AudioClip m_JellyFishShowedUp = null;

	// 해파리가 틀렸을 때 전기충격
	public AudioClip m_JellyFishShocked = null;

	public AudioClip m_CrabHurray;
	
	// EliminationTutorialSampleSound
	public AudioClip m_EliminationTutorialSampleSound = null;

	// EliminationTutorial에서 생선을 클릭했을 때 맞췄습니다 ~ 소리
	public AudioClip m_EliminationTutorialFishClickedSound = null;

	// 각 게임에서 시간이 5초 
	public AudioClip m_HurryOutTime = null;
	
	// 타임오버 시 효과음
	public AudioClip m_TimeOver = null;


	// Sound volume
	public float m_SoundVolume = 0.5f;

	// Music volume
	public float m_MusicVolume = 0.5f;

	#endregion Variables

	// ########################################
	// MonoBehaviour Functions
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.html
	// ########################################

	#region MonoBehaviour

	// Awake is called when the script instance is being loaded.
	void Awake()
	{
		Debug.Log("AWAKE");
		if (instance == null)
		{
			// Make the current instance as the singleton
			instance = this;

			// Make it persistent  
			DontDestroyOnLoad(this);
		}
		else
		{
			Debug.Log("IN FIRST ELSE");
			// If more than one singleton exists in the scene find the existing reference from the scene and destroy it
			if (this != instance)
			{
				Debug.Log("IN SECONDE ELSE");
				InitAudioListener();
				Destroy(this.gameObject);
			}
		}
	}

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
	void Start()
	{
		Debug.Log("START");
		// Initial AudioListener
		InitAudioListener();

		// 필요없는 코드
		// Automatically play music if it is not playing
//		if (IsMusicPlaying() == false)
//		{
//			// Play music
//			Play_Music(m_Music);
//		}
	}

	// Update is called every frame, if the MonoBehaviour is enabled.
	// http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
	void Update()
	{

	}

	#endregion // MonoBehaviour

	// ########################################
	// Utilitie functions
	// ########################################

	#region Functions

	// Initial AudioListener
	// This function remove all AudioListener in other objects then it adds new one this object.
	void InitAudioListener()
	{
		Debug.Log("INIT AUDIO LISTENER");
		// Destroy other's AudioListener components
		AudioListener[] pAudioListenerToDestroy = GameObject.FindObjectsOfType<AudioListener>();
		foreach (AudioListener child in pAudioListenerToDestroy)
		{
			if (child.gameObject.GetComponent<SoundManager>() == null)
			{
				Destroy(child);
			}
		}

		// Adds new AudioListener to this object
		AudioListener pAudioListener = gameObject.GetComponent<AudioListener>();
		if (pAudioListener == null)
		{
			pAudioListener = gameObject.AddComponent<AudioListener>();
		}
	}

	// Play music
	void PlayMusic(AudioClip pAudioClip)
	{
		// Return if the given AudioClip is null
		if (pAudioClip == null)
			return;

		AudioListener pAudioListener = GameObject.FindObjectOfType<AudioListener>();
		if (pAudioListener != null)
		{
			// Look for an AudioListener component that is not playing background music or sounds.
			bool IsPlaySuccess = false;
			AudioSource[] pAudioSourceList = pAudioListener.gameObject.GetComponents<AudioSource>();
			if (pAudioSourceList.Length > 0)
			{
				for (int i = 0; i < pAudioSourceList.Length; i++)
				{
					// Play music
					if (pAudioSourceList[i].isPlaying == false)
					{
						pAudioSourceList[i].loop = true;
						pAudioSourceList[i].clip = pAudioClip;
						pAudioSourceList[i].ignoreListenerVolume = true;
						pAudioSourceList[i].playOnAwake = false;
						pAudioSourceList[i].Play();
						break;
					}
				}
			}

			// If there is not enough AudioListener to play AudioClip then add new one and play it
			if (IsPlaySuccess == false && pAudioSourceList.Length < 16)
			{
				AudioSource pAudioSource = pAudioListener.gameObject.AddComponent<AudioSource>();
				pAudioSource.rolloffMode = AudioRolloffMode.Linear;
				pAudioSource.loop = true;
				pAudioSource.clip = pAudioClip;
				pAudioSource.ignoreListenerVolume = true;
				pAudioSource.playOnAwake = false;
				pAudioSource.Play();
			}
		}
	}



	// Play sound one shot
	void PlaySoundOneShot(AudioClip pAudioClip)
	{

		// Return if the given AudioClip is null
		if (pAudioClip == null)
			return;

//		// We wait for a while after scene loaded
//		if (Time.timeSinceLevelLoad < 1.5f)
//			return;
//	
		// Look for an AudioListener component that is not playing background music or sounds.
		AudioListener pAudioListener = GameObject.FindObjectOfType<AudioListener>();
		if (pAudioListener != null)
		{
			bool IsPlaySuccess = false;
			AudioSource[] pAudioSourceList = pAudioListener.gameObject.GetComponents<AudioSource>();
			if (pAudioSourceList.Length > 0)
			{
				for (int i = 0; i < pAudioSourceList.Length; i++)
				{
					if (pAudioSourceList[i].isPlaying == false)
					{
						// Play sound
						pAudioSourceList[i].PlayOneShot(pAudioClip);
						break;
					}
				}
			}

			// If there is not enough AudioListener to play AudioClip then add new one and play it
			if (IsPlaySuccess == false && pAudioSourceList.Length < 16)
			{
				// Play sound
				AudioSource pAudioSource = pAudioListener.gameObject.AddComponent<AudioSource>();
				pAudioSource.rolloffMode = AudioRolloffMode.Linear;
				pAudioSource.playOnAwake = false;
				pAudioSource.PlayOneShot(pAudioClip);
			}
		}
	}

	// Set music volume between 0.0 to 1.0
	public void SetMusicVolume(float volume)
	{
		m_MusicVolume = volume;

		AudioListener pAudioListener = GameObject.FindObjectOfType<AudioListener>();
		if (pAudioListener != null)
		{
			AudioSource[] pAudioSourceList = pAudioListener.gameObject.GetComponents<AudioSource>();
			if (pAudioSourceList.Length > 0)
			{
				for (int i = 0; i < pAudioSourceList.Length; i++)
				{
					if (pAudioSourceList[i].ignoreListenerVolume)
					{
						pAudioSourceList[i].volume = volume;
					}
				}
			}
		}
	}

	// If music is playing, return true.
	public bool IsMusicPlaying()
	{
		//Debug.Log("a");
		AudioListener pAudioListener = GameObject.FindObjectOfType<AudioListener>();
		if (pAudioListener != null)
		{
		//	Debug.Log("b");
//			AudioSource[] pAudioSourceList = pAudioListener.gameObject.GetComponents<AudioSource>();
			// 내가 짠 거
			AudioSource[] pAudioSourceList = GetComponents<AudioSource>();
			Debug.Log(pAudioSourceList.Length);
			if (pAudioSourceList.Length > 0)
			{
		//		Debug.Log("c");
				for (int i = 0; i < pAudioSourceList.Length; i++)
				{
		//			Debug.Log("d");
					if (pAudioSourceList[i].ignoreListenerVolume == true)
					{
		//				Debug.Log("e");
						if (pAudioSourceList[i].isPlaying == true)
						{
		//					Debug.Log("f");
							return true;
						}
					}
				}
			}
		}

		return false;
	}

	public void StopMusic()
	{

		AudioListener pAudioListener = GameObject.FindObjectOfType<AudioListener>();
//		Debug.Log("1");
		if (pAudioListener != null)
		{
//			Debug.Log("2");
			// Look for an AudioListener component that is not playing background music or sounds.
			bool IsPlaySuccess = false;
//			AudioSource[] pAudioSourceList = pAudioListener.gameObject.GetComponents<AudioSource>();
			AudioSource[] pAudioSourceList = GetComponents<AudioSource>();
			if (pAudioSourceList.Length > 0)
			{
//				Debug.Log("3");
				for (int i = 0; i < pAudioSourceList.Length; i++)
				{
//					Debug.Log("4");
					// Stop music

					pAudioSourceList[i].Stop();
//							break;

				}
			}


		}

	}

	// Set sound volume between 0.0 to 1.0
	public void SetSoundVolume(float volume)
	{
		m_SoundVolume = volume;
		AudioListener.volume = volume;
	}

	// Play music
	public void Play_Music(AudioClip clip)
	{
		PlayMusic(clip);
	}

	public void Play_MenuMusic()
	{
		Debug.Log("PLAY_MENUMUSIC");
		PlayMusic(m_Music);
	}

	public void Play_DetectionMusic()
	{
		PlayMusic(DetectionMusic);
	}

	public void Play_AlternativeMusic()
	{
		Play_Music(AlternativeMusic);
	}

	public void Play_EliminationMusic()
	{
		Play_Music(EliminationMusic);
	}

	public void Play_SynthesisMusic()
	{
		Play_Music(SynthesisMusic);
	}

	// Play Back button sound
	public void Play_SoundBack()
	{
		PlaySoundOneShot(m_ButtonBack);
	}

	// Play Click sound
	public void Play_SoundClick()
	{
		PlaySoundOneShot(m_ButtonClick);
	}

	// Play Disabled button sound
	public void Play_SoundDisable()
	{
		PlaySoundOneShot(m_ButtonDisable);
	}

	// Play No button sound
	public void Play_SoundNo()
	{
		PlaySoundOneShot(m_ButtonNo);
	}

	// Play Pause button sound
	public void Play_SoundPause()
	{
		PlaySoundOneShot(m_ButtonPause);
	}

	// Play Play button sound
	public void Play_SoundPlay()
	{
		PlaySoundOneShot(m_ButtonPlay);
	}

	// Play Tap sound
	public void Play_SoundTap()
	{
		PlaySoundOneShot(m_ButtonTab);
	}

	// Play Yes sound
	public void Play_SoundYes()
	{
		PlaySoundOneShot(m_ButtonYes);
	}

	// When exit game
	public void Play_SoundExitGame()
	{
		PlaySoundOneShot(m_ButtonExitGame);
	}

	public void Play_SoundGameHover()
	{
		PlaySoundOneShot(m_ButtonGameHover);
	}

	public void Play_SoundOctopusMove()
	{
		PlayMusic(m_OctopusMove);
	}

	public void Play_SpeechBubblePop()
	{
		PlaySoundOneShot(m_SpeechBubblePop);
	}

	public void Play_BarrelCreated()
	{
		PlaySoundOneShot(m_BarrelCreated);
	}

	public void Play_ClickedCorrectAnswer()
	{
		PlaySoundOneShot(m_ClickedCorrectAnswer);
	}

	public void Play_ClickedWrongAnswer()
	{
		PlaySoundOneShot(m_ClickedWrongAnswer);
	}

	public void Play_StarShowedUp()
	{
		PlaySoundOneShot(m_StarShowedUp);
	}

	public void Play_NoStarShowedUp()
	{
		PlaySoundOneShot(m_NoStarShowedUp);
	}

	public void Play_SeahorseWaves()
	{
		PlaySoundOneShot(m_SeahorseWaves);
	}

	public void Play_AlterWordShowedUp()
	{
		PlaySoundOneShot(m_AlterWordShowedUp);
	}

	public void Play_AlterBubbleShowedUp()
	{
		PlaySoundOneShot(m_AlterBubbleShowedUp);
	}

	public void Play_SharkShowedUp()
	{
		PlaySoundOneShot(m_SharkShowedUp);
	}

	public void Play_SharkEatingFish()
	{
		PlaySoundOneShot(m_SharkEatingFish);
	}

	public void Play_JellyFishShowedUp()
	{
		PlayMusic(m_JellyFishShowedUp);
	}

	public void Play_JellyFishShocked()
	{
		PlaySoundOneShot(m_JellyFishShocked);
	}

	public void Play_CrabHurray()
	{
		PlaySoundOneShot(m_CrabHurray);
	}
	
	public void Play_EliminationTutorialSampleSound()
	{
		PlaySoundOneShot(m_EliminationTutorialSampleSound);
	}

	public void Play_EliminationTutorialFishClickedSound()
	{
		PlaySoundOneShot(m_EliminationTutorialFishClickedSound);
	}

	public void Play_HurryOutTimeSound()
	{
		PlaySoundOneShot(m_HurryOutTime);
	}

	public void Play_TimeOverSound()
	{
		PlaySoundOneShot(m_TimeOver);
	}

	#endregion // Functions
}
