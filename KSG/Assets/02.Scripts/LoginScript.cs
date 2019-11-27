using UnityEngine;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour
{
    private PostScript ps = new PostScript();
    private string val_str;
    public Text ID_Text;
    public InputField Pw_Text;
    private int isLogin;
    string[] temp_ID_arr = new string[]
    {
    "Korea0001","Korea0002","Korea0003","Korea0004","Korea0005","Korea0006","Korea0007","Korea0008","Korea0009","Korea0010",
    "Korea0011","Korea0012","Korea0013","Korea0014","Korea0015","Korea0016","Korea0017","Korea0018","Korea0019","Korea0020",
    "Korea0021","Korea0022","Korea0023","Korea0024","Korea0025","Korea0026","Korea0027","Korea0028","Korea0029","Korea0030",
    };

    //void Start()
    //{
    //    PlayerPrefs.SetInt("isLogin", 0);
    //}

    //void OnEnable()
    //{
    //    ps = GameObject.FindObjectOfType<PostScript>();
    //    if (PlayerPrefs.GetInt("isLogin") == 1)//로그인 상태이면
    //    {
    //        gameObject.SetActive(false);
    //        gameObject.transform.parent.Find("Home").Find("id_text").GetComponent<Text>().text = PlayerPrefs.GetString("user_id");
    //    }
    //    //else//로그인을 하려는 상태이면
    //    //{
            

    //    //}
    //}

    public void Login()
    {
        if (Pw_Text.text == "0000")
            for (int i = 0; i < temp_ID_arr.Length; i++)
                if (temp_ID_arr[i] == ID_Text.text)
                {
                    PlayerPrefs.SetString("user_id", temp_ID_arr[i]);
                    gameObject.SetActive(false);
                    gameObject.transform.parent.Find("Home").Find("id_text").GetComponent<Text>().text = PlayerPrefs.GetString("user_id");

                    PlayerPrefs.SetInt("isLogin", 1);//로그인 되어있는 상태인지 확인하기 위해서 저장
                    val_str = "day1,1";
                    ps.UserupPostInit(val_str);
                    //val_str = NextBtnClickedScript.sentence_count.ToString() + ",TestResultTable,q3_result" + ",1";
                    //ps.PostInit(val_str);

                    //db.UserIdTableInsert(top_id_text, 1);
                    //여기다가는 로그인할때 해당 아이디의 week랑 Day를 업데이트 하는 부분
                    //if(db에서 user_id가 있으면)
                    //{}
                    //else(db에서 user_id가 없으면)
                    //{insert user_id, day1, day2~ day10, week}
                }
    }
}