using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class PostScript : MonoBehaviour
{
    public string base_url = "https://kukp.forelink-cloud.co.kr/moodle3/kukp/insert_app_data.php";
    public string renum_url = "https://kukp.forelink-cloud.co.kr/moodle3/kukp/insert_app_data_daytime.php";
    public string reset_url = "https://kukp.forelink-cloud.co.kr/moodle3/kukp/init_db_data.php";
    public string userup_url = "https://kukp.forelink-cloud.co.kr/moodle3/kukp/insert_user_class.php";

    public string tempDirectory = "";
    public string key_string = "";
    public string value_string = "";

    public List<string> key_List = new List<string>() { "korea_swink", "word_id", "update_table_name", "update_type", "update_value", "user_id" };
    public List<string> value_List = new List<string>() { "korea_swink" };

    public List<string> renum_key_List = new List<string>() { "korea_swink", "word_id", "update_table_name", "update_type", "update_value", "ReNumber", "user_id" };
    public List<string> renum_value_List = new List<string>() { "korea_swink" };

    public List<string> reset_key_List = new List<string>() { "korea_swink", "user_id" };
    public List<string> reset_value_List = new List<string>() { "korea_swink" };

    public List<string> userup_key_List = new List<string>() { "korea_swink", "day", "update_value", "user_id" };
    public List<string> userup_value_List = new List<string>() { "korea_swink" };

    public List<string> parse_WordID = new List<string>();
    public List<string> parse_Type = new List<string>();
    public List<string> parse_Attribute = new List<string>();
    public List<string> pars_QuizAttribute = new List<string>();
    public List<string> userinfo_Attribute = new List<string>();
    public List<string> day9or10_Attribute = new List<string>();

    public bool isParseDone = false;
    public bool isQuizDone = false;
    public bool isDownloadDone = false;

    public string SplitString = "";

    public List<Sprite> tempSpriteList = new List<Sprite>();
    public List<AudioClip> tempAudioClipList = new List<AudioClip>();
    private static PostScript instance;
    public static PostScript Getinstance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("Single is null");
            }

            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this as PostScript;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        System.GC.GetTotalMemory(true);
    }

    void OnLevelWasLoaded()
    {
        if (SceneManager.GetActiveScene().name == "00.Home")
            Destroy(GameObject.Find("Post"));
        //if (Application.loadedLevelName == "00.Home")
    }

    /// <summary>
    /// 외부에서 호출할때 ,로 구분지어 Attribut와 Value를 입력
    /// </summary>
    /// <param name="m_Keystring"> ,로 구분지어 Attribute를 입력</param>
    /// <param name="m_ValueString">,로 구분지어 Value를 입력</param>
    public void PostInit(string m_ValueString)
    {
        if (m_ValueString.Contains(","))
        {
            m_ValueString.Trim();
            string[] value_string_split_arr = m_ValueString.Split(',');

            for (int i = 0; i < value_string_split_arr.Length; i++)
                value_List.Add(value_string_split_arr[i]);
        }

        value_List.Add(PlayerPrefs.GetString("user_id"));
        Post(key_List, value_List, base_url);
    }

    public void RenumPostInit(string r_ValueString)
    {
        if (r_ValueString.Contains(","))
        {
            r_ValueString.Trim();
            string[] value_string_split_arr = r_ValueString.Split(',');

            for (int i = 0; i < value_string_split_arr.Length; i++)
                renum_value_List.Add(value_string_split_arr[i]);
        }

        renum_value_List.Add(PlayerPrefs.GetString("user_id"));
        RenumPost(renum_key_List, renum_value_List, renum_url);
    }

    public void ResetPostInit()
    {
        reset_value_List.Add(PlayerPrefs.GetString("user_id"));
        Post(reset_key_List, reset_value_List, reset_url);
    }

    public void UserupPostInit(string up_ValueString)
    {
        if (up_ValueString.Contains(","))
        {
            up_ValueString.Trim();
            string[] value_string_split_arr = up_ValueString.Split(',');

            for (int i = 0; i < value_string_split_arr.Length; i++)
                userup_value_List.Add(value_string_split_arr[i]);
        }

        userup_value_List.Add(PlayerPrefs.GetString("user_id"));
        UserupPost(userup_key_List, userup_value_List, userup_url);
    }

    public void Post(List<string> key, List<string> value, string url)
    {
        if ((key.Count == value.Count) && key.Count >= 2)
        {
            WWWForm form = new WWWForm();

            for (int i = 0; i < key.Count; i++)
            {
                form.AddField(key[i], value[i]);
                //Debug.Log("key[i] = " + key[i]);
                //Debug.Log("value[i] = " + value[i]);
            }

            Debug.Log("url = " + url);
            WWW www = new WWW(url, form);
            StartCoroutine(Posting(www));
        }
        else
        {
            Debug.Log("key, value count is not match!!");
        }
    }

    public void RenumPost(List<string> key, List<string> value, string url)
    {
        if ((key.Count == value.Count) && key.Count >= 2)
        {
            WWWForm form = new WWWForm();

            for (int i = 0; i < key.Count; i++)
            {
                form.AddField(key[i], value[i]);
            }

            //for(int i = 0; i < key.Count; i++)
            //{
            //    Debug.Log(key[i].ToString());
            //    Debug.Log(value[i].ToString());
            //}


            WWW www = new WWW(url, form);
            StartCoroutine(RenumPosting(www));
        }
        else
        {
            Debug.Log("key, value count is not match!!");
        }
    }

    public void UserupPost(List<string> key, List<string> value, string url)
    {
        if ((key.Count == value.Count) && key.Count >= 2)
        {
            WWWForm form = new WWWForm();

            for (int i = 0; i < key.Count; i++)
            {
                form.AddField(key[i], value[i]);
            }

            //Debug.Log("url = " + url);
            WWW www = new WWW(url, form);
            StartCoroutine(UserupPosting(www));
        }
        else
        {
            Debug.Log("key, value count is not match!!");
        }
    }

    public void ParseInit(string m_classId, string m_day, string m_wordId, string m_parameterId)
    {
        Debug.Log("ParseInit Start");
        isParseDone = false;
        string parsing_url = "https://kukp.forelink-cloud.co.kr/moodle3/kukp/get_sentencebyID.php";

        WWWForm form = new WWWForm();

        form.AddField("korea_swink", "korea_swink");
        form.AddField("class_id", m_classId);
        form.AddField("type", "day");
        //TODO : user_id, day = Application.AbsoulteURL
        form.AddField("day", m_day);
        form.AddField("user_id", m_parameterId);
        form.AddField("ver", "1.1");

        Debug.Log("ParseInit Started From Sentence = " + m_wordId);

        WWW parse_www = new WWW(parsing_url, form);

        StartCoroutine(Parsing(parse_www, m_wordId, m_parameterId));

        if (tempSpriteList.Count == 0 && tempAudioClipList.Count == 0)
        {
            Debug.Log("ParseInit Async Start DownloadImage From Sentence = " + m_wordId);
            StartCoroutine(DownloadImage(parse_www, m_wordId));
            Debug.Log("ParseInit Async Start DownloadAudioClip From Sentence = " + m_wordId);
            StartCoroutine(DownloadAudioClip(parse_www, m_wordId));
        }
    }

    public void QuizInit(string m_classId, string m_day, string m_wordid, string m_parameter_id)
    {
        Debug.Log("QuizInit Started from sentence " + m_wordid);
        isQuizDone = false;

        string parsing_url = "https://kukp.forelink-cloud.co.kr/moodle3/kukp/get_quizbyID.php";

        WWWForm form = new WWWForm();

        form.AddField("korea_swink", "yes");
        form.AddField("class_id", m_classId);
        form.AddField("type", "day");
        form.AddField("day", m_day);
        form.AddField("user_id", m_parameter_id);
        form.AddField("ver", "1.1");

        WWW quiz_www = new WWW(parsing_url, form);
        Debug.Log("QuizInit Async Start QuizParsing(class = " + m_classId + ", sentence = " + m_wordid + ")");
        StartCoroutine(QuizParsing(quiz_www, m_classId, m_wordid));

        if (tempAudioClipList.Count == 0 && tempSpriteList.Count == 0)
        {
            StartCoroutine(DownloadAudioClip(quiz_www, m_wordid));
        }

        Debug.Log("QuizInit End");
    }

    public void WordIDParse(string m_classId, int m_Day, string m_ID)
    {
        string parsing_url;
        if (SceneManager.GetActiveScene().name == "01_2.Quiz")
        {
            parsing_url = "https://kukp.forelink-cloud.co.kr/moodle3/kukp/get_quizbyID.php";
        }
        else
        {
            parsing_url = "https://kukp.forelink-cloud.co.kr/moodle3/kukp/get_sentencebyID.php";
        }

        WWWForm form = new WWWForm();

        form.AddField("korea_swink", "korea_swink");
        form.AddField("class_id", m_classId);
        form.AddField("type", "day");
        form.AddField("day", m_Day);
        form.AddField("user_id", m_ID);
        form.AddField("ver", "1.1");

        WWW parse_www = new WWW(parsing_url, form);
        StartCoroutine(WordIDParsing(parse_www, m_Day));
    }

    public void QuizInfoInit(List<float> m_time, List<int> m_answer, string m_classId, int m_wordId, int m_day)
    {
        string parsing_url = "https://kukp.forelink-cloud.co.kr/moodle3/kukp/insert_quiz_info.php";
        WWWForm form = new WWWForm();
        form.AddField("korea_swink", "korea_swink");
        form.AddField("word_id", m_wordId);

        Debug.Log("QuizInfoInit Send Quiz Result for Sentence " + m_wordId);
        for (int i = 1; i < (m_answer.Count + 1); i++)
        {
            Debug.Log("m_answer["+(i - 1)+"] "+ " = " + m_answer[(i - 1)] + " || " + "q_" + i);
            form.AddField("q" + i, m_answer[i - 1]);
        }

        for (int i = 1; i < (m_time.Count + 1); i++)
        {
            form.AddField("q" + i + "_t", m_time[i - 1].ToString());
        }

        form.AddField("user_id", PlayerPrefs.GetString("user_id"));

        form.AddField("class_id", m_classId);

        if (m_day == 15 || m_day == 16)
        {
            Debug.Log("m_day == " + m_day);
            form.AddField("type", m_day);
        }

        form.AddField("score", WordSentenceQuizScript.resultSc.ToString());
        WordSentenceQuizScript.resultSc = 0;

        WWW QuizInfoWWW = new WWW(parsing_url, form);
        StartCoroutine(QuizInfoPosting(QuizInfoWWW, m_time, m_answer));
    }

    public void StudyInfoInit(List<float> m_time, string m_classId, int m_wordId)
    {
        string parsing_url = "https://kukp.forelink-cloud.co.kr/moodle3/kukp/insert_study_time.php";
        WWWForm form = new WWWForm();
        form.AddField("korea_swink", "korea_swink");
        form.AddField("word_id", m_wordId);
        form.AddField("user_id", PlayerPrefs.GetString("user_id"));
        form.AddField("class_id", m_classId);

        float tempFloat = 0;

        for (int i = 0; i < m_time.Count; i++)
            tempFloat += m_time[i];

        form.AddField("time", tempFloat.ToString());

        WWW SentenceTimeWWW = new WWW(parsing_url, form);
        StartCoroutine(StudyInfoPostring(SentenceTimeWWW, m_time));
    }

    IEnumerator Parsing(WWW www, string m_wordid, string m_attribute)
    {
        parse_Attribute.Clear();
        yield return www;
        isParseDone = false;

        if (www.error == null)
        {
            // Debug.Log("################### " + www.text);
            JSONNode json = JSON.Parse(www.text);
            for (int i = 0; i < 15; i++) // forelink, change max sentence. 5 -> 15
            {
                Debug.Log("Parsing Start");
                string tempString = json["data"][0][i]["word_id"].Value;
                if (m_wordid.Equals(tempString))
                {
                    for (int j = 0; j < json["data"][0][i].Count; j++)
                    {
                        parse_Attribute.Add(json["data"][0][i]["word_id"]);
                        parse_Attribute.Add(json["data"][0][i]["word_number"]);
                        parse_Attribute.Add(json["data"][0][i]["word1"]);
                        parse_Attribute.Add(json["data"][0][i]["word2"]);
                        parse_Attribute.Add(json["data"][0][i]["word3"]);
                        parse_Attribute.Add(json["data"][0][i]["study_sent"]);
                        parse_Attribute.Add(json["data"][0][i]["word1_mean1"]);
                        parse_Attribute.Add(json["data"][0][i]["word1_ex"]);
                        parse_Attribute.Add(json["data"][0][i]["word2_mean1"]);
                        parse_Attribute.Add(json["data"][0][i]["word2_ex"]);
                        parse_Attribute.Add(json["data"][0][i]["word3_mean1"]);
                        parse_Attribute.Add(json["data"][0][i]["word3_ex"]);
                        Debug.Log("sentence: " + json["data"][0][i]["word_id"] + "[" + json["data"][0][i]["word_number"] + "], " + json["data"][0][i]["study_sent"]);
                        isParseDone = true;
                    }
                }
                Debug.Log("Parsing End");
            }
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            Debug.Log(www.error);
        }
    }

    IEnumerator UserInfoParsing(WWW www)
    {
        userinfo_Attribute.Clear();
        yield return www;
        if (www.error == null)
        {
            Debug.Log("UserInfoParsing Start");
            JSONNode json = JSON.Parse(www.text);
            userinfo_Attribute.Add(json["data"][0][0]["day1"]);
            userinfo_Attribute.Add(json["data"][0][0]["day2"]);
            userinfo_Attribute.Add(json["data"][0][0]["day3"]);
            userinfo_Attribute.Add(json["data"][0][0]["day4"]);
            userinfo_Attribute.Add(json["data"][0][0]["day5"]);
            userinfo_Attribute.Add(json["data"][0][0]["day6"]);
            userinfo_Attribute.Add(json["data"][0][0]["day7"]);
            userinfo_Attribute.Add(json["data"][0][0]["day8"]);
            userinfo_Attribute.Add(json["data"][0][0]["day9"]);
            userinfo_Attribute.Add(json["data"][0][0]["day10"]);
            Debug.Log("UserInfoParsing End");
            isParseDone = true;

            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            Debug.Log(www.error);
        }
    }


    public JSONNode quizJson;
    IEnumerator QuizParsing(WWW www, string m_classId, string m_wordid)
    {
        Debug.Log("QuizParsing Started by Async");
        pars_QuizAttribute.Clear();
        yield return www;

        if (www.error == null)
        {
            int subIndex = 0;

            JSONNode json = JSON.Parse(www.text);
            quizJson = json;

            for (int i = 0; i < json["data"][0].Count; i++)
            {
                WordSentenceQuizScript.resultSc = 0;
                string tempString = json["data"][0][i]["quiz_id"];

                pars_QuizAttribute.Add(json["data"][0][i]["quiz_id"]);
                pars_QuizAttribute.Add(json["data"][0][i]["quiz_type"]);
                pars_QuizAttribute.Add(json["data"][0][i]["quiz_data"]);

                string type = json["data"][0][i]["quiz_type"];
                string examples = json["data"][0][i]["quiz_examples"];

                string[] temp = null;

                if (('2').ToString().Equals(type))
                {
                    temp = examples.Split('#');
                    pars_QuizAttribute.Add(temp[1]);
                }
                else
                    pars_QuizAttribute.Add(json["data"][0][i]["quiz_examples"]);

                pars_QuizAttribute.Add(json["data"][0][i]["quiz_answer"]);
                pars_QuizAttribute.Add(json["data"][0][i]["quiz_randomize"]);


                if (('2').ToString().Equals(type))
                {
                    Debug.Log("DownloadAudioClip ID:" + tempString + "/" + type + ", WAV:" + examples + ", " + m_wordid + "_Study1_1_1");
                    string[] audioNum = temp[0].Split(';');
                    StartCoroutine(DownloadAudioClip(audioNum[0]));
                    StartCoroutine(DownloadAudioClip(audioNum[1]));
                    StartCoroutine(DownloadAudioClip(audioNum[2].Substring(0, audioNum[2].Length - 1)));
                    StartCoroutine(DownloadAudioClip(m_wordid + "_Study1_1_1")); // forelink, TODO
                }
                else {
                    Debug.Log("QuizParsing ID:" + tempString + "/" + type + ", " + examples);
                }
                // type 5,6 image load
                if (('5').ToString().Equals(type) || ('6').ToString().Equals(type))
                {
                    Debug.Log("DownloadImage ID:" + tempString + "/" + type + ", png:" + examples + ", " + m_wordid + "_Study1_1_1");
                    temp = examples.Split(';');
                    string mediaUrl = Global.mediaUrl; // added parameter by forelink
                    if (mediaUrl == null || mediaUrl == "")
                    {
                        // PC SA http is working but WebGL http is not // use http to remove encryption/decryption time
                        mediaUrl = "https://kukp.forelink-cloud.co.kr/moodle3/kukp/media_elem2";
                    }
                    for (int j = 0; j < temp.Length; j++)
                    {
                        using (UnityWebRequest wwwQuest = UnityWebRequest.Get(mediaUrl + "/" + temp[j] + ".png"))
                        {
                            Debug.Log("DownloadAudioClip() ImageFile = " + temp[j] + ".png");

                            yield return wwwQuest.Send();

                            if (wwwQuest.isNetworkError)
                                Debug.Log(wwwQuest.error);
                            else if (wwwQuest.isDone)
                            {
                                byte[] results = wwwQuest.downloadHandler.data;
                                Texture2D texture = new Texture2D(100, 100);
                                texture.LoadImage(results);
                                Vector2 tempVector2 = new Vector2(0.5f, 0.5f);
                                tempSpriteList.Add(Sprite.Create(texture, new Rect(0, 0, 216, 243), tempVector2));
                                tempSpriteList[tempSpriteList.Count - 1].name = temp[j];
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("QuizParsing ID:" + tempString + "/" + type + ", " + examples);
                }
                /* forelink removed, not required
                StartCoroutine(DownloadAudioClip(m_wordid + "_1"));
                StartCoroutine(DownloadAudioClip(m_wordid + "_2"));
                StartCoroutine(DownloadAudioClip(m_wordid + "_3"));
                StartCoroutine(DownloadAudioClip(m_wordid + "_4"));
                StartCoroutine(DownloadAudioClip(m_wordid + "_5"));
                StartCoroutine(DownloadAudioClip(m_wordid + "_6"));
                */
                isQuizDone = true; // set when first quiz audio file is downloaded
            }
            isQuizDone = true;
            yield return new WaitForSeconds(0.3f);
        }
        else
            Debug.Log(www.error);
    }

    IEnumerator Posting(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            JSONNode json = JSON.Parse(www.text);
        }
        else
            Debug.Log(www.error);

        //key_List.Clear();
        value_List.Clear();

        //key_List.Add("korea_swink");
        //key_List.Add("word_id");

        value_List.Add("korea_swink");
    }

    IEnumerator RenumPosting(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            JSONNode json = JSON.Parse(www.text);
        }
        else
            Debug.Log(www.error);

        //key_List.Clear();
        renum_value_List.Clear();

        //key_List.Add("korea_swink");
        //key_List.Add("word_id");
        renum_value_List.Add("korea_swink");
    }

    IEnumerator UserupPosting(WWW www)
    {
        yield return www;

        if (www.error == null)
        {
            JSONNode json = JSON.Parse(www.text);

            Debug.Log("www.text = " + www.text);
        }
        else
            Debug.Log(www.error);

        userup_value_List.Clear();
        userup_value_List.Add("korea_swink");
    }

    IEnumerator WordIDParsing(WWW www, int m_day)
    {
        Debug.Log("WordIDParsing Start");
        parse_WordID.Clear();
        yield return www;
        if (www.error == null)
        {
            JSONNode json = JSON.Parse(www.text);
            if (!(www.text.Contains("word_id")))
            {
                Debug.Log("quiz count = " + json["data"].Count);
                for (int i = 0; i < json["data"][0].Count; i++)
                {
                    string tempString = json["data"][0][i]["quiz_id"];
                    Debug.Log(tempString + " : " + tempString.Length);
                    if (tempString.Length == 3)
                    {
                        parse_WordID.Add((json["data"][0][i]["quiz_id"]).ToString().Substring(1, 1));
                    }
                    else if (tempString.Length == 4)
                    {
                        parse_WordID.Add((json["data"][0][i]["quiz_id"]).ToString().Substring(1, 2));
                    }
                    else
                    {
                        Debug.Log("else here - " + i);
                    }
                    Debug.Log("quiz id[" + i + "] = " + json["data"][0][i]["quiz_id"]);
                }
            }
            else if ((www.text.Contains("word_id")))
            {
                Debug.Log("study sentence count = " + json["data"][0].Count);
                for (int i = 0; i < json["data"][0].Count; i++)
                {
                    parse_WordID.Add((json["data"][0][i]["word_id"]));
                    Debug.Log("study sentence[" + i + "] = " + json["data"][0][i]["word_id"] + ", " + json["data"][0][i]["study_sent"]);
                }
            }
            yield return new WaitForSeconds(0.3f);
        }
        else {
            Debug.Log("www.error = " + www.error);
        }

        isParseDone = true;
        Debug.Log("WordIDParsing End");
    }

    IEnumerator DownloadImage(WWW www, string m_wordid)
    {
        List<string> tempParseList = new List<string>();
        yield return www;
        isDownloadDone = false; // <- isParseDone, forelink modified
        if (www.error == null)
        {
            JSONNode json = JSON.Parse(www.text);
            for (int i = 0; i < json["data"][0].Count; i++)
            {
                tempParseList.Add(json["data"][0][i]["word1_Image"]);
                tempParseList.Add(json["data"][0][i]["word2_Image"]);
                tempParseList.Add(json["data"][0][i]["word3_Image"]);
                tempParseList.Add(json["data"][0][i]["study_sent_Image"]);
            }
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            Debug.Log(www.error);
        }

        for (int i = 0; i < tempParseList.Count; i++)
        {
            if (tempParseList[i] != null)
            {
                string mediaUrl = Global.mediaUrl; // added parameter by forelink
                if (mediaUrl == null || mediaUrl == "")
                {
                    // PC SA http is working but WebGL http is not // use http to remove encryption/decryption time
                    mediaUrl = "https://kukp.forelink-cloud.co.kr/moodle3/kukp/media";
                }
                using (UnityWebRequest wwwQuest = UnityWebRequest.Get(mediaUrl + "/" + tempParseList[i] + ".png"))
                {
                    Debug.Log("DownloadAudioClip() ImageFile = " + tempParseList[i] + ".png");

                    yield return wwwQuest.Send();

                    if (wwwQuest.isNetworkError)
                        Debug.Log(wwwQuest.error);
                    else if (wwwQuest.isDone)
                    {
                        byte[] results = wwwQuest.downloadHandler.data;
                        Texture2D texture = new Texture2D(100, 100);
                        texture.LoadImage(results);
                        Vector2 tempVector2 = new Vector2(0.5f, 0.5f);
                        tempSpriteList.Add(Sprite.Create(texture, new Rect(0, 0, 640, 720), tempVector2));
                        tempSpriteList[tempSpriteList.Count - 1].name = tempParseList[i];
                    }
                }
            }
        }
        isDownloadDone = true;
    }

    IEnumerator DownloadAudioClip(WWW www, string m_wordid)
    {
        List<string> tempParseList = new List<string>();
        yield return www;
        isParseDone = false;
        if (www.error == null)
        {
            JSONNode json = JSON.Parse(www.text);
            for (int i = 0; i < json["data"][0].Count; i++)
            {
                tempParseList.Add(json["data"][0][i]["word1_sound"]);
                tempParseList.Add(json["data"][0][i]["word2_sound"]);
                tempParseList.Add(json["data"][0][i]["word3_sound"]);
                tempParseList.Add(json["data"][0][i]["study_sent_sound"]);
                tempParseList.Add(json["data"][0][i]["word1_mean1_sound"]);
                tempParseList.Add(json["data"][0][i]["word1_ex_sound_1"]);
                tempParseList.Add(json["data"][0][i]["word1_ex_sound_2"]);
                tempParseList.Add(json["data"][0][i]["word1_ex_sound_3"]);
                tempParseList.Add(json["data"][0][i]["word2_mean1_sound"]);
                tempParseList.Add(json["data"][0][i]["word2_ex_sound_1"]);
                tempParseList.Add(json["data"][0][i]["word2_ex_sound_2"]);
                tempParseList.Add(json["data"][0][i]["word2_ex_sound_3"]);
                tempParseList.Add(json["data"][0][i]["word3_mean1_sound"]);
                tempParseList.Add(json["data"][0][i]["word3_ex_sound_1"]);
                tempParseList.Add(json["data"][0][i]["word3_ex_sound_2"]);
                tempParseList.Add(json["data"][0][i]["word3_ex_sound_3"]);
            }
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            Debug.Log(www.error);
        }

        for (int i = 0; i < tempParseList.Count; i++)
        {
            if (tempParseList[i] != null)
            {
                string mediaUrl = Global.mediaUrl; // added parameter by forelink
                if (mediaUrl == null || mediaUrl == "")
                {
                    // PC SA http is working but WebGL http is not // use http to remove encryption/decryption time
                    mediaUrl = "https://kukp.forelink-cloud.co.kr/moodle3/kukp/media";
                }
                using (UnityWebRequest wwwQuest = UnityWebRequestMultimedia.GetAudioClip(mediaUrl + "/" + tempParseList[i] + ".wav", AudioType.WAV))
                {
                    Debug.Log("DownloadAudioClip() AudioFile = " + tempParseList[i] + ".wav");
                    yield return wwwQuest.Send();
                    if (wwwQuest.isNetworkError)
                    {
                        Debug.Log(wwwQuest.error);
                    }
                    else if (wwwQuest.isDone)
                    {
                        tempAudioClipList.Add(DownloadHandlerAudioClip.GetContent(wwwQuest));
                        tempAudioClipList[tempAudioClipList.Count - 1].name = tempParseList[i];
                    }
                }
                isParseDone = true; // set when first sentence audio file is downloaded
            }
        }
        isParseDone = true; // <- isParseDone, forelink modified
    }

    IEnumerator DownloadAudioClip(string m_FileName)
    {
        string mediaUrl = Global.mediaUrl; // added parameter by forelink
        if (mediaUrl == null || mediaUrl == "")
        {
            // PC SA http is working but WebGL http is not // use http to remove encryption/decryption time
            mediaUrl = "https://kukp.forelink-cloud.co.kr/moodle3/kukp/media";
        }
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(mediaUrl + "/" + m_FileName + ".wav", AudioType.WAV))
        {
            // Debug.Log("DownloadAudioClip() m_FileName = " + m_FileName);
            yield return www.Send();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else if (www.isDone)
            {

                tempAudioClipList.Add(DownloadHandlerAudioClip.GetContent(www));
                tempAudioClipList[tempAudioClipList.Count - 1].name = m_FileName;
            }
        }
    }

    IEnumerator QuizInfoPosting(WWW www, List<float> m_time, List<int> m_answer)
    {
        yield return www;

        if (www.error == null)
        {
            JSONNode json = JSON.Parse(www.text);
            Debug.Log("user_id = " + PlayerPrefs.GetString("user_id") + ", www.text = " + www.text);
        }
        else {
            Debug.Log("user_id = " + PlayerPrefs.GetString("user_id") + ", www.error = " + www.error);
        }
    }

    IEnumerator StudyInfoPostring(WWW www, List<float> m_time)
    {
        yield return www;

        if (www.error == null)
        {
            JSONNode json = JSON.Parse(www.text);
            Debug.Log("user_id = " + PlayerPrefs.GetString("user_id") + ", www.text = " + www.text);
        }
        else {
            Debug.Log("user_id = " + PlayerPrefs.GetString("user_id") + ", www.error = " + www.error);
        }
    }
}