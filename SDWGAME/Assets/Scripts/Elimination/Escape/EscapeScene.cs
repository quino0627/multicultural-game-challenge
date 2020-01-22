using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeScene : MonoBehaviour
{
    private GameObject shark;
    public GameObject sharkBubble;
    private GameObject fish;

    public float fastSpeed;
    public float rotSpeed;
    public float slowSpeed;
    private bool isSharkArrived;
    private bool isFishExited;
    private bool isDongExited;
    
    private Transform sharkMeetFishPos;
    private Transform fishExitPos;
    private Transform dongExitPos;

    public GameObject fishDong;
    private Transform fishDongTransform;
    private TextMeshPro fishDongText;
    public GameObject DongOrbitTarget;
    public float orbitDistance;
    public float orbitDegreesPerSec = 180.0f;
    public Vector3 relativeDistance = Vector3.zero;
    
    //public GameObject QuizManager;
    private FishShowAnswer script;
    
    // Start is called before the first frame update
    void Start()
    {
        dongExitPos = GameObject.Find("DongExitPos").transform;
        shark = GameObject.Find("Shark");
        fish = GameObject.Find("Fish");
        sharkMeetFishPos = GameObject.Find("SharkFishMeetPos").transform;
        fishExitPos = GameObject.Find("FishExitPos").transform;
        //fishDong = GameObject.Find("FishDong");
        //QuizManager = GameObject.Find("QuizManager");
        //script = QuizManager.GetComponent<FishShowAnswer>();
        fishDongTransform = fishDong.transform;
        relativeDistance = fishDongTransform.position - DongOrbitTarget.transform.position;
        StartCoroutine(SharkTry());    
        
        
    }
    
    
    IEnumerator SharkTry()
    {
        yield return new WaitForSeconds(3.0f);
        Debug.Log("SharkTry");
        //먼저 상어가 들어온다
        while (!isSharkArrived)
        {
            yield return new WaitForEndOfFrame();
            float distance = Vector2.Distance(
                shark.transform.position,
                sharkMeetFishPos.position);
            if (distance > 0.01f)
            {
                MoveObject(fastSpeed, shark, sharkMeetFishPos.position);
                //Debug.Log("Shark moved");
            }
            else
            {
                isSharkArrived = true;
                //Debug.Log("Shark arrived");
            }
        }
        
        
        //물고기와 만났으니 
        shark.GetComponent<Animator>().SetTrigger("Eat");
        yield return new WaitForSeconds(0.5f);
        //물고기 바로 도망가쥬
        shark.GetComponent<Animator>().SetTrigger("Idle");
        StartCoroutine(FishEscape());

    }
    void MoveObject(float speed, GameObject obj, Vector2 destination)
    {
        float step = speed * Time.deltaTime;
        obj.transform.position = Vector2.MoveTowards(
            obj.transform.position, destination, step);
    }

    IEnumerator FishEscape()
    {
        Debug.Log("FishEscape");
        //똥이 보여짐
        fishDong.SetActive(true);
        /*fishDongText.text = 
            script.data.sheets[script.level].list[script.refStageIndex].탈락자극;*/
        //fishDongText.text = "ㄱ";
        
        //똥이 데굴데굴...
        StartCoroutine(DongTargetMove());
        StartCoroutine(DongDaeGulDaeGul());
        
        //도망치는 물고기
        while (!isFishExited)
        {
            yield return new WaitForEndOfFrame();
            float distance = Vector2.Distance(
                fish.transform.position,
                fishExitPos.position);
            if (distance > 0.01f)
            {
                MoveObject(fastSpeed, fish, fishExitPos.position);
                //Debug.Log("Shark moved");
            }
            else
            {
                isFishExited = true;
                //Debug.Log("Shark arrived");
            }
        }
        
        //황당해하는 상어 소환
        StartCoroutine(GoCatchFish());
    }

    IEnumerator DongTargetMove(){
        while(!isDongExited){
            yield return new WaitForEndOfFrame();
             float distance = 
             Vector2.Distance(
                            DongOrbitTarget.transform.position,
                            dongExitPos.position);
                        if (distance > 0.01f)
                        {
                            MoveObject(slowSpeed, DongOrbitTarget, dongExitPos.position);
                            Debug.Log("DongTarget moved");
                        }
                        else
                        {
                            isDongExited = true;
                            Debug.Log("DongTarget arrived");
                        }
        }
    }

    IEnumerator DongDaeGulDaeGul()
    {
        while (!isDongExited)
        {
            yield return new WaitForEndOfFrame();
            fishDongTransform.position = DongOrbitTarget.transform.position + relativeDistance;
            //fishDongTransform.position = DongOrbitTarget.transform.position + orbitDistance;
            fishDongTransform.RotateAround(
                DongOrbitTarget.transform.position, Vector3.forward, 
                orbitDegreesPerSec * Time.deltaTime);
            relativeDistance = fishDongTransform.position - DongOrbitTarget.transform.position;
        }
    }

    IEnumerator GoCatchFish()
    {
        sharkBubble.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        
        //상어 뿔남 애니메이션 예정
        
        
        Debug.Log("LoadScene");
        SceneManager.LoadScene("Elimination");

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
