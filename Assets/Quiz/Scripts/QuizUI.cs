using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
//using GoogleMobileAds.Api;
using System;



public class QuizUI : MonoBehaviour
{
    [SerializeField] private QuizManager1 quizManager1;
    [SerializeField] private CategoryBtnScript categoryBtnPrefab;
    [SerializeField] private GameObject scrollHolder;
    [SerializeField]private Text questionText, scoreText, timerText;
    [SerializeField] private List<Image> lifeImageList;
    [SerializeField] private GameObject gameOverPanal, mainMenuPanel, gameMenuPanel, gameComplete, gameTimerPanel;
    [SerializeField]private Image questionImage;
    [SerializeField]private UnityEngine.Video.VideoPlayer questionVideo;
    [SerializeField]private AudioSource questionAudio;
    [SerializeField] private List<Button> options;
    [SerializeField] private Color correctCol, wrongCol, normalCol;
    [SerializeField] private AudioClip wrong;
    [SerializeField] private AudioClip correct;
    private AudioSource sonido;


    private Question question;
    private bool answered;
    private float audioLength;

    public Text ScoreText { get { return scoreText; } }
    public Text TimerText { get { return timerText; } }


    public GameObject  GameOverPanel { get { return gameOverPanal; } }

    public GameObject GameTimerPanel { get { return gameTimerPanel; } }
    public GameObject  GameComplete { get { return gameComplete; } }

    

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < options.Count; i++)
        {
            Button localBtn = options[i];
            localBtn.onClick.AddListener(() => OnClick(localBtn));
        }

        CreateCategoryButtons();
        sonido = GetComponent<AudioSource>();

        
    } 


  

    public void SetQuestion(Question question)
    {
        this.question = question;

        switch(question.questionType){

            case QuestionType.TEXT:

                questionImage.transform.parent.gameObject.SetActive(false);

                break;

            case QuestionType.IMAGE:
                ImageHolder();
                questionImage.transform.gameObject.SetActive(true);

                questionImage.sprite = question.questionImg;
                break;
            case QuestionType.VIDEO:
                ImageHolder();
                questionVideo.transform.gameObject.SetActive(true);

                // CONFIGURACIÓN PARA WEBGL:
                questionVideo.source = UnityEngine.Video.VideoSource.Url; // Cambiamos la fuente a URL

                // Construimos la ruta: StreamingAssets + nombre del archivo
                string path = Path.Combine(Application.streamingAssetsPath, question.videoFileName);

                questionVideo.url = path;
                questionVideo.Prepare(); // Prepara el video (necesario en Web)

                // Suscribirse para que empiece a sonar/verse solo cuando esté listo
                questionVideo.prepareCompleted += (vp) => {
                    vp.Play();
                };
                break;
            case QuestionType.AUDIO:
                ImageHolder();
                questionAudio.transform.gameObject.SetActive(true);

                audioLength = question.questionClip.length;

                StartCoroutine(PlayAudio());

                break;
        }

        questionText.text = question.questionInfo;

        List<string> answerList = ShuffleList1.ShuffleListItems<string>(question.options);

        for(int i = 0; i < options.Count; i++)
        {
            options[i].GetComponentInChildren<Text>().text = answerList[i];

            options[i].name = answerList[i];
            options[i].image.color = normalCol;
        }

        answered = false;
    }

    IEnumerator PlayAudio()
    {
        if(question.questionType == QuestionType.AUDIO)
        {
            questionAudio.PlayOneShot(question.questionClip);

            yield return new WaitForSeconds(audioLength + 0.5f);

            StartCoroutine(PlayAudio());
        }
        else
        {
            StopCoroutine(PlayAudio());
            yield return null;
        }
    }

    void ImageHolder()
    {
        questionImage.transform.parent.gameObject.SetActive(true);
        questionImage.transform.gameObject.SetActive(false);
        questionAudio.transform.gameObject.SetActive(false);
        questionVideo.transform.gameObject.SetActive(false);

    }


    private void OnClick(Button btn)
    {
        if(quizManager1.GameStatus == GameStatus1.Playing)
        {
            if (!answered)
            {
                answered = true;
                bool val = quizManager1.Answer(btn.name);

                if (val)
                {
                    btn.image.color = correctCol;
                    if (SoundManager.instance.IsSoundFXMuted() == false)
                        sonido.PlayOneShot(correct);

                }
                else
                {
                    if (SoundManager.instance.IsSoundFXMuted() == false)
                        sonido.PlayOneShot(wrong);
                    StartCoroutine(BlinkImg(btn.image));


                }
            }
        }      
      
      
    }

    void CreateCategoryButtons()
    {
        for(int i = 0; i < quizManager1.QuizData.Count; i++)
        {
            CategoryBtnScript categoryBtn = Instantiate(categoryBtnPrefab, scrollHolder.transform);
            categoryBtn.SetButton(quizManager1.QuizData[i].categoryName, quizManager1.QuizData[i].questions.Count);
            int index = i;
            categoryBtn.Btn.onClick.AddListener(() => CategoryBtn(index, quizManager1.QuizData[index].categoryName));
        }
    }

    private void CategoryBtn(int index, string category)
    {
        quizManager1.StartGame(index, category);
        mainMenuPanel.SetActive(false);
        gameMenuPanel.SetActive(true);
    }

    IEnumerator BlinkImg(Image img)
    {
        for (int i = 0; i < 2; i++)
        {
            img.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            img.color = wrongCol;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void RetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReduceLife(int index)
    {
        lifeImageList[index].color = wrongCol;
    }
}
