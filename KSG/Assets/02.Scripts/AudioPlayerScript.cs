using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class AudioPlayerScript : MonoBehaviour
{
    private AudioClip controlAudioClip;
    private string objName;
    private int sound_day;
    private int sound_sentence;
    private int sound_word;
    private bool study3_soundbtnclicked = false;
    private int cutSoundIndex;
    public static int cutSoundLastIndex;
    public GameObject CantTouchPage;

    PostScript ps;

    void AudioControlAndPlay()
    {
        GetComponent<AudioSource>().loop = false;
        GetComponent<AudioSource>().clip = controlAudioClip;
        GetComponent<AudioSource>().Play();

        if (gameObject.name.Contains("Sound"))
        {
            CantTouchPage.SetActive(true);
            StartCoroutine(ChangeSpeakerImage());
        }
    }

    IEnumerator ChangeSpeakerImage()
    {
        yield return new WaitForSecondsRealtime(controlAudioClip.length);
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Speaker/speaker_off");
        CantTouchPage.SetActive(false);
    }

    void OnEnable()
    {
        if (ps == null)
            ps = GameObject.FindObjectOfType<PostScript>();

        if (SceneManager.GetActiveScene().name == "01_2.Quiz")
        {
            sound_day = QuizManager.day;
            sound_sentence = QuizManager.sentence_count;
            sound_word = QuizManager.word_count;
            cutSoundIndex = 1;
        }
        else if (SceneManager.GetActiveScene().name == "01_1.Study")
        {
            sound_day = NextBtnClickedScript.day;
            sound_sentence = NextBtnClickedScript.sentence_count;
            sound_word = NextBtnClickedScript.word_count;
            cutSoundIndex = 1;
        }

        Debug.Log("gameObject.name.Remove(gameObject.name.Length - 1, 1) = " + gameObject.name.Remove(gameObject.name.Length - 1, 1));

        if (gameObject.name.Remove(gameObject.name.Length - 1, 1) == "" || gameObject.name.Remove(gameObject.name.Length - 1, 1) == "Study4_")
        {
            Debug.Log("=========================Coroutine=======================");
            StartCoroutine(AudioStart());
        }
    }

    public void SoundBtnClicked()
    {
        if (GetComponent<AudioSource>().isPlaying == false)
        {
            StopAllCoroutines();
            StartCoroutine(AudioStart());
        }
    }

    public void Study3SoundBtnClicked()
    {
        StopAllCoroutines();
        StartCoroutine(Study3SoundStart());
    }

    public void CutSoundBtnClicked()
    {
        StopAllCoroutines();
        string p = "Sound/" + sound_sentence.ToString() + "_cut" + (cutSoundIndex % cutSoundLastIndex).ToString();
        controlAudioClip = Resources.Load(p) as AudioClip;
        AudioControlAndPlay();
        gameObject.transform.Find("StudyText").gameObject.GetComponent<Text>().text = null;

        for (int i = 0; i < cutSoundLastIndex; i++)
        {
            if (i == (cutSoundIndex % cutSoundLastIndex) && (cutSoundIndex % cutSoundLastIndex) != 0)
            {
                gameObject.transform.Find("StudyText").gameObject.GetComponent<Text>().text += "<color=#C8C8C8FF>";
            }

            if (i == cutSoundLastIndex - 1)
            {
                string temp = NextBtnClickedScript.StudyText1_Spilt[i];
                if (NextBtnClickedScript.StudyText1_Spilt[i][0] == '<')
                {
                    if ((cutSoundIndex % cutSoundLastIndex) < i + 1 && (cutSoundIndex % cutSoundLastIndex) != 0) temp = NextBtnClickedScript.StudyText1_Spilt[i].Replace("2D88C1FF", "C8C8C8FF");
                }

                gameObject.transform.Find("StudyText").gameObject.GetComponent<Text>().text += temp;
                if ((cutSoundIndex % cutSoundLastIndex) != 0) gameObject.transform.Find("StudyText").gameObject.GetComponent<Text>().text += "</color>";
            }
            else
            {
                string temp = NextBtnClickedScript.StudyText1_Spilt[i]; ;
                if (NextBtnClickedScript.StudyText1_Spilt[i][0] == '<')
                {
                    if ((cutSoundIndex % cutSoundLastIndex) < i + 1 && (cutSoundIndex % cutSoundLastIndex) != 0) temp = NextBtnClickedScript.StudyText1_Spilt[i].Replace("2D88C1FF", "C8C8C8FF");
                }
                gameObject.transform.Find("StudyText").gameObject.GetComponent<Text>().text += (temp + " ");
            }
        }
        cutSoundIndex++;
    }

    public void Quiz3SoundBtnClicked()
    {
        string p = "Sound/" + sound_sentence.ToString() + "_Study1_1_1";
        PlaySound((sound_sentence).ToString() + "_Study1_1_1");
        AudioControlAndPlay();
    }

    public void Quiz2_1SoundBtnClicked()
    {
        //버튼 눌렀을 때 스피커 이미지 바꾸고 크게 함
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/Speaker/speaker_on");
        Debug.Log("ps.quizJson[data][0][pageNum][quiz_id].Value = " + ps.quizJson["data"][0][QuizManager.pageNum]["quiz_id"].Value);
        sound_sentence = Int32.Parse(ps.parse_WordID[QuizManager.pageNum]);
        Debug.Log("sound_sentence = " + Int32.Parse(ps.parse_WordID[QuizManager.pageNum]));

        int sound_type = 0;
        string [] tempStr = ps.quizJson["data"][0][QuizManager.pageNum]["quiz_id"].ToString().Split(';');
        if (tempStr[1] == "5\"")
            sound_type = 3;

        if (RandomQuizScript.quiz2_answer_choice == Int32.Parse(gameObject.name.Remove(0, gameObject.name.Length - 1)))
        {
            Debug.Log("Answer");

            if (RandomQuizScript.quiz2_answer_choice <= 3)
            {
                string p = "Sound/" + sound_sentence.ToString() + "_" + (1 + sound_type).ToString();
                PlaySound((sound_sentence).ToString() + "_" + (1 + sound_type).ToString());
            }
            else
            {
                string p = "Sound/" + sound_sentence.ToString() + "_" + 4.ToString();
                PlaySound((sound_sentence).ToString() + "_" + 4.ToString());
            }

            AudioControlAndPlay();
        }
        else
        {
            if (RandomQuizScript.quiz2_answer_choice == 3)
            {
                Debug.Log("----------------1 || " + gameObject.name.Remove(0, gameObject.name.Length - 1));
                if (Int32.Parse(gameObject.name.Remove(0, gameObject.name.Length - 1)) == 1)
                {
                    Debug.Log("----------------1-1 || " + (sound_sentence).ToString() + "_" + (2+ sound_type).ToString());
                    string p = "Sound/" + sound_sentence.ToString() + "_" + (2 + sound_type).ToString();
                    PlaySound((sound_sentence).ToString() + "_" + (2 + sound_type).ToString());
                    AudioControlAndPlay();
                }
                else if (Int32.Parse(gameObject.name.Remove(0, gameObject.name.Length - 1)) == 2)
                {
                    Debug.Log("----------------1-2 || " + (sound_sentence).ToString() + "_" + (3 + sound_type).ToString());
                    string p = "Sound/" + sound_sentence.ToString() + "_" + (3 + sound_type).ToString();
                    PlaySound((sound_sentence).ToString() + "_" + (3 + sound_type).ToString());
                    AudioControlAndPlay();
                }
            }
            else if (RandomQuizScript.quiz2_answer_choice == 2)
            {
                Debug.Log("----------------2 || " + gameObject.name.Remove(0, gameObject.name.Length - 1));
                if (Int32.Parse(gameObject.name.Remove(0, gameObject.name.Length - 1)) == 3)
                {
                    Debug.Log("----------------2-1 || " + (sound_sentence).ToString() + "_" + (2 + sound_type).ToString());
                    string p = "Sound/" + sound_sentence.ToString() + "_" + (2 + sound_type).ToString();
                    PlaySound((sound_sentence).ToString() + "_" + (2 + sound_type).ToString());
                    AudioControlAndPlay();
                }
                else if (Int32.Parse(gameObject.name.Remove(0, gameObject.name.Length - 1)) == 1)
                {
                    Debug.Log("----------------2-2 || " + (sound_sentence).ToString() + "_" + (3 + sound_type).ToString());
                    string p = "Sound/" + sound_sentence.ToString() + "_" + (3 + sound_type).ToString();
                    PlaySound((sound_sentence).ToString() + "_" + (3 + sound_type).ToString());
                    AudioControlAndPlay();
                }
            }
            else if (RandomQuizScript.quiz2_answer_choice == 1)
            {
                Debug.Log("----------------3 || " + gameObject.name.Remove(0, gameObject.name.Length - 1));
                if (Int32.Parse(gameObject.name.Remove(0, gameObject.name.Length - 1)) == 2)
                {
                    string p = "Sound/" + sound_sentence.ToString() + "_" + (2 + sound_type).ToString();
                    PlaySound((sound_sentence).ToString() + "_" + (2 + sound_type).ToString());
                    AudioControlAndPlay();
                }
                else if (Int32.Parse(gameObject.name.Remove(0, gameObject.name.Length - 1)) == 3)
                {
                    string p = "Sound/" + sound_sentence.ToString() + "_" + (3 + sound_type).ToString();
                    PlaySound((sound_sentence).ToString() + "_" + (3 + sound_type).ToString());
                    AudioControlAndPlay();
                }
            }
            else if (RandomQuizScript.quiz2_answer_choice == 4)
            {
                Debug.Log("----------------4");
                if (Int32.Parse(gameObject.name.Remove(0, gameObject.name.Length - 1)) == 5)
                {
                    string p = "Sound/" + sound_sentence.ToString() + "_" + 5.ToString();
                    PlaySound((sound_sentence).ToString() + "_" + 5.ToString());
                    AudioControlAndPlay();
                }
                else if (Int32.Parse(gameObject.name.Remove(0, gameObject.name.Length - 1)) == 6)
                {
                    string p = "Sound/" + sound_sentence.ToString() + "_" + 6.ToString();
                    PlaySound((sound_sentence).ToString() + "_" + 6.ToString());
                    AudioControlAndPlay();
                }
            }
            else if (RandomQuizScript.quiz2_answer_choice == 5)
            {
                if (Int32.Parse(gameObject.name.Remove(0, gameObject.name.Length - 1)) == 4)
                {
                    string p = "Sound/" + sound_sentence.ToString() + "_" + 5.ToString();
                    PlaySound((sound_sentence).ToString() + "_" + 5.ToString());
                    AudioControlAndPlay();
                }
                else if (Int32.Parse(gameObject.name.Remove(0, gameObject.name.Length - 1)) == 6)
                {
                    string p = "Sound/" + sound_sentence.ToString() + "_" + 6.ToString();
                    PlaySound((sound_sentence).ToString() + "_" + 6.ToString());
                    AudioControlAndPlay();
                }
            }
            else if (RandomQuizScript.quiz2_answer_choice == 6)
            {
                if (Int32.Parse(gameObject.name.Remove(0, gameObject.name.Length - 1)) == 5)
                {
                    string p = "Sound/" + sound_sentence.ToString() + "_" + 5.ToString();
                    PlaySound((sound_sentence).ToString() + "_" + 5.ToString());
                    AudioControlAndPlay();
                }
                else if (Int32.Parse(gameObject.name.Remove(0, gameObject.name.Length - 1)) == 4)
                {
                    string p = "Sound/" + sound_sentence.ToString() + "_" + 6.ToString();
                    PlaySound((sound_sentence).ToString() + "_" + 6.ToString());
                    AudioControlAndPlay();
                }
            }
        }
    }

    public void Quiz2_2SoundBtnClicked()
    {
        string p = "Sound/" + sound_sentence.ToString() + "_" + 1.ToString();
        PlaySound((sound_sentence).ToString() + "_" + 1.ToString());
        AudioControlAndPlay();
    }

    IEnumerator AudioStart()
    {
        Debug.Log("=========================AudioStart=======================");
        objName = gameObject.name;

        if (objName == "Study2")//UIpage보다 바깥에 위치시키기
        {
            gameObject.transform.SetParent(gameObject.transform.parent.parent);
            gameObject.transform.SetSiblingIndex(2);
        }

        else if (objName == "Study3")
        {
            yield return new WaitForSecondsRealtime(2.0f);
        }

        else if (objName == "Study5")
        {
            objName = "Study1";//1이랑 5랑 같은 사운드를 재생 시켜야 하므로
            sound_word = 1;//sound_word2는 소리가 없음
        }

        int i = 1;
        while (true)
        {
            string p = "Sound/" + sound_sentence.ToString() + "_" + objName + "_" + sound_word.ToString() + "_" + i.ToString();
            string split = sound_sentence.ToString() + "_" + objName + "_" + sound_word.ToString() + "_" + i.ToString();

            for (int j = 0; j < ps.tempAudioClipList.Count; j++)
            {
                if (ps.tempAudioClipList[j].name == split)
                    controlAudioClip = ps.tempAudioClipList[j];
            }

            if (controlAudioClip == null || controlAudioClip.name != split) break;
            AudioControlAndPlay();

            if (objName == "Study2")//study 2일때는 소리나오고 2초뒤에 사라지게 만들기
            {
                yield return new WaitForSecondsRealtime(2.0f);
                gameObject.transform.SetParent(gameObject.transform.parent.GetChild(0));
                gameObject.transform.SetSiblingIndex(2);
                gameObject.transform.parent.Find("Study2").gameObject.SetActive(false);
            }
            else if (objName == "Study1")
            {
                yield return new WaitForSecondsRealtime(controlAudioClip.length);
            }
            else yield return new WaitForSecondsRealtime(controlAudioClip.length);
            i++;
        }
    }

    IEnumerator Study3SoundStart()
    {
        // int i = 1;
        // while (true)
        // {
        //     Debug.Log("sound_sentence = " + sound_sentence);
        //     Debug.Log("sound_word = " + sound_word);
        //     //string p = "Sound/" + sound_sentence.ToString() + "_Study3_" + sound_word.ToString() + "_" + i.ToString();
        //     //controlAudioClip = Resources.Load(p) as AudioClip;
        controlAudioClip = GetComponent<AudioSource>().clip;
        //     if (controlAudioClip == null) break;
        AudioControlAndPlay();
        //     yield return new WaitForSecondsRealtime(controlAudioClip.length);
        //     i++;
        //}
        yield return null;
    }

    void PlaySound(string m_FileName)
    {
        Debug.Log("m_fileName = " + m_FileName);
        for (int j = 0; j < ps.tempAudioClipList.Count; j++)
            if (ps.tempAudioClipList[j].name == m_FileName)
                controlAudioClip = ps.tempAudioClipList[j];
    }

    public void SetWordSoundButton()
    {
        GetComponent<AudioSource>().clip = GameObject.Find("BackgroundPage").transform.Find("Study2").GetComponent<AudioSource>().clip;

        GetComponent<AudioSource>().Play();
    }
}
