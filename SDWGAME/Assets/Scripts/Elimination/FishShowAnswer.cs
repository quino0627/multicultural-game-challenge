using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FishShowAnswer : MonoBehaviour
{
    
    //excel data
    public Entity_EliminationTest data;
    
    // 초급/중급/고급
    public int level;
    
    // 각 난이도 안의 stage index
    public static int stageIndex;
    
    //보기 이 게임에선 물고기
    public GameObject[] choices;
    public TextMeshPro[] choiceTexts;
    public List<int> ansPosIndex;
    
    //Shark
    public GameObject shark;
    private AudioSource sharkAudio;

    public GameObject stimulation;
    private TextMeshPro stimulText;

    public GameObject eliminStimul;
    public TextMeshPro eliminText;
    
    // ui level과 stage표시 하기 위한 변수
    public TextMeshPro quizLevel;
    public TextMeshPro quizStage;
    
    // Start is called before the first frame update
    void Start()
    {
        sharkAudio = shark.GetComponent<AudioSource>();
        stimulText = stimulation.GetComponent<TextMeshPro>();
        QuizInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuizInit()
    {
        StartCoroutine(EnableCoroutine());
    }

    IEnumerator EnableCoroutine()
    {
        //quizLevel.text = "" + (level + 1);
        //quizStage.text = "" + (stageIndex + 1);
        //Debug.Log("level "+level);
        //Debug.Log("stageIndex "+stageIndex);
        //yield return new WaitForSecondsRealtime(GetComponent<AudioSource>().clip.length);
        yield return new WaitForSeconds(0.0f);
        //CheckToggleGroup.SetAllTogglesOff();

        for (int i = 0; i < 5; i++)
        {
            int tmp; 
            do {
                tmp = Random.Range(0, 5);
            } while (ansPosIndex.Contains(tmp));
            //Debug.Log("AnsPosIndex[" + i + "] = " + tmp );
            ansPosIndex.Add(tmp);
        }
        //Debug.Log("correctAnsPosIndex: " + ansPosIndex[0]);
        
        //정답 물고기에 정답음성 넣기
        string wordFileLink = 
            $"Sounds/Detection/{data.sheets[level].list[stageIndex].정답음성}";
        AudioSource corrFish = 
            choices[ansPosIndex[0]].GetComponentInChildren<AudioSource>();
        corrFish.loop = false;
        corrFish.clip = Resources.Load(wordFileLink) as AudioClip;
        
        //테스트로 글자 넣기
        choiceTexts[ansPosIndex[0]].text = 
            data.sheets[level].list[stageIndex].정답;
        
        //오답 물고기에 오답음성 넣기
        int j = 1;
        string link;
        AudioSource fishSpeaker;
        
        for (int i = 1; i < 5; i++)
        {
            fishSpeaker = choices[ansPosIndex[i]].GetComponentInChildren<AudioSource>();

            if (i == 1)
            {
                link = $"Sounds/Detection/{data.sheets[level].list[stageIndex].오답음성1}";
                fishSpeaker.loop = false;
                fishSpeaker.clip = Resources.Load(link) as AudioClip;
                
                choiceTexts[ansPosIndex[i]].text =
                    data.sheets[level].list[stageIndex].오답1;
            }

            else if (i == 2)
            {
                link = $"Sounds/Detection/{data.sheets[level].list[stageIndex].오답음성2}";
                fishSpeaker.loop = false;
                fishSpeaker.clip = Resources.Load(link) as AudioClip;
                
                choiceTexts[ansPosIndex[i]].text =
                    data.sheets[level].list[stageIndex].오답2;
            }

            else if (i == 3)
            {
                link = $"Sounds/Detection/{data.sheets[level].list[stageIndex].오답음성3}";
                fishSpeaker.loop = false;
                fishSpeaker.clip = Resources.Load(link) as AudioClip;
                
                choiceTexts[ansPosIndex[i]].text =
                    data.sheets[level].list[stageIndex].오답3;
            }
            else if (i == 4)
            {
                link = $"Sounds/Detection/{data.sheets[level].list[stageIndex].오답음성4}";
                fishSpeaker.loop = false;
                fishSpeaker.clip = Resources.Load(link) as AudioClip;
                
                choiceTexts[ansPosIndex[i]].text =
                    data.sheets[level].list[stageIndex].오답4;
            }
        }
        
        StartCoroutine(ShowSharkNeed());

    }

    IEnumerator ShowSharkNeed()
    {
        // 자극 제시 ex) 만들
        stimulText.text = data.sheets[level].list[stageIndex].자극;
        stimulation.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        
        // 탈락 자극 제시 ex) ㄴ
        stimulation.SetActive(false);
        eliminText.text = data.sheets[level].list[stageIndex].탈락자극;
        eliminStimul.SetActive(true);

        StartCoroutine(ShowAnswer());
    }
    IEnumerator ShowAnswer()
    {
        int i = 0;
        while (i < 5)
        {
            yield return new WaitForSeconds(0.2f);
            choices[i].SetActive(true);
            i++;
        }
        
        
        
    }
    
    
    IEnumerator HideAnswers()
    {
        int i = 0;
        while (i < 5)
        {
            yield return new WaitForSeconds(0.1f);
            choices[i].SetActive(false);
            i++;
        }
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void GoNextStage()
    {
        stageIndex++;
        StartCoroutine(HideAnswers());
    }
}
