using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeScene : MonoBehaviour
{
    //KeepTrackController ConclusionData
    public GameObject totalStorageObject;
    private TotalDataManager _totalStorageScript;

    private GameObject StageStorage;
    private StageDataManager StageStorageScript;

    private GameObject shark;
    public GameObject sharkBubble;
    private GameObject fish;
    private TextMeshPro fishText;

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

    public int level;
    public int stage;
    public static int questionNumber;
    public int questionMaxNumber;
    public ELM_DataList data;

    public int questionId;
    private static List<int> randomNoDuplicates;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        totalStorageObject = GameObject.Find("TotalStorage");
        _totalStorageScript = totalStorageObject.GetComponent<TotalDataManager>();
        StageStorage = GameObject.Find("StageStorage");
        StageStorageScript = StageStorage.GetComponent<StageDataManager>();

        questionMaxNumber = 10;
        level = _totalStorageScript.chosenLevel;
        stage = _totalStorageScript.chosenStage;
        if (questionNumber == questionMaxNumber)
        {
            questionNumber = 0;
        }

        if (questionNumber == 0)
        {
            randomNoDuplicates = new List<int>();
            for (int i = 0; i < questionMaxNumber; ++i)
            {
                int tmp = Random.Range(0, questionMaxNumber);
                while (randomNoDuplicates.Contains(tmp))
                {
                    tmp = Random.Range(0, questionMaxNumber);
                }

                randomNoDuplicates.Add(tmp);
            }

            StageStorageScript.EliminationRandomNoDuplicates = randomNoDuplicates;
        }

        questionId = stage * 10 + randomNoDuplicates[questionNumber];


        dongExitPos = GameObject.Find("DongExitPos").transform;
        shark = GameObject.Find("Shark");
        fish = GameObject.Find("Fish");
        fishText = fish.transform.GetChild(0).GetComponent<TextMeshPro>();

        sharkMeetFishPos = GameObject.Find("SharkFishMeetPos").transform;
        fishExitPos = GameObject.Find("FishExitPos").transform;

        fishDongText = fishDong.GetComponentInChildren<TextMeshPro>();
        fishDongTransform = fishDong.transform;
        relativeDistance = fishDongTransform.position - DongOrbitTarget.transform.position;
        StartCoroutine(SharkTry());

        //물고기에 정답음성 넣기
        string wordFileLink =
            $"Sounds/Detection/{data.sheets[level].list[questionId].정답음성}";
        AudioSource fishAudioSource =
            fish.GetComponentInChildren<AudioSource>();
        fishAudioSource.loop = false;
        fishAudioSource.clip = Resources.Load(wordFileLink) as AudioClip;

        //테스트로 글자 넣기
        fishText.text =
            data.sheets[level].list[questionId].원자극;

        fishDongText.text =
            data.sheets[level].list[questionId].탈락음소;
    }


    IEnumerator SharkTry()
    {
        yield return new WaitForSeconds(3.0f);
        //Debug.Log("SharkTry");
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
        //Debug.Log("FishEscape");
        //똥이 보여짐
        fishDong.SetActive(true);


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

    IEnumerator DongTargetMove()
    {
        while (!isDongExited)
        {
            yield return new WaitForEndOfFrame();
            float distance =
                Vector2.Distance(
                    DongOrbitTarget.transform.position,
                    dongExitPos.position);
            if (distance > 0.01f)
            {
                MoveObject(slowSpeed, DongOrbitTarget, dongExitPos.position);
                //Debug.Log("DongTarget moved");
            }
            else
            {
                isDongExited = true;
                //Debug.Log("DongTarget arrived");
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


        //Debug.Log("LoadScene");
        questionNumber++;
        SceneManager.LoadScene("Elimination");
    }

    // Update is called once per frame
    void Update()
    {
    }
}