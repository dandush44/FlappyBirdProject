using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool gameStarted;
    private int score;
    private int best;

    private GameObject player;

    public Canvas guideCanvas;
    public Canvas scoreCanvas;
    public Text scoreText;
    public GameObject flash;

    [Space(20)]

    public SpriteRenderer background;
    public Sprite[] backgrounds;

    [Space(20)]

    public GameObject pipes;
    public float pipeSpawnDelay;
    public float pipeMinHeight;
    public float pipeMaxHeight;
    public float pipeSpeed;

    [Space(20)]

    public AudioSource pointSound;
    public AudioSource hitSound;
    public AudioSource dieSound;

    [Space(20)]

    public GameObject gameOverCanvas;
    public Text goScore;
    public Text goBest;
    public Image medal;
    public Sprite[] medals;
    public Image newLabel;

    // Start is called before the first frame update
    void Start()
    {
        //setting framerate to 60 fps
        Application.targetFrameRate = 60;

        //choosing random background
        background.sprite = backgrounds[Random.Range(0, backgrounds.Length)];

        //initializing stuff
        player = GameObject.FindGameObjectWithTag("Player");
        gameStarted = false;
        score = 0;
        best = PlayerPrefs.GetInt("Best", 0);
        guideCanvas.enabled = true;
        scoreCanvas.enabled = false;
        newLabel.enabled = false;
        gameOverCanvas.SetActive(false);
        UpdateScore(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartGame();
        }
    }

    IEnumerator SpawnPipes()
    {
        //spawning pipe at random height and giving it speed
        GameObject newPipes = Instantiate(pipes);
        var pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        newPipes.transform.position = new Vector3(pos.x + 2, Random.Range(pipeMinHeight, pipeMaxHeight), 0);
        newPipes.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-pipeSpeed, 0);


        yield return new WaitForSeconds(pipeSpawnDelay);

        StartCoroutine("SpawnPipes");
    }

    public void UpdateScore(int x)
    {
        if (x > 0)
        {
            pointSound.Play();
            score += x;
        }

        //setting new highscore
        if (score > best)
        {
            best = score;
            PlayerPrefs.SetInt("Best", best);
            newLabel.enabled = true;
        }

        //updating score texts
        scoreText.text = score.ToString();
        goScore.text = score.ToString();
        goBest.text = best.ToString();
    }

    public void StartGame()
    {
        //starting game
        if (!gameStarted)
        {
            guideCanvas.enabled = false;
            scoreCanvas.enabled = true;
            StartCoroutine("SpawnPipes");
            player.GetComponent<PlayerController>().StartGame();
            gameStarted = true;
        }
    }

    public void GameOver()
    {
        //playing hit and die sound
        hitSound.Play();
        dieSound.PlayDelayed(0.7f);

        //white flash effect
        Destroy(Instantiate(flash), 1);

        //stopping the ground
        GameObject ground = GameObject.FindGameObjectWithTag("Ground");
        ground.GetComponent<Animator>().speed = 0;

        //stopping the pipes
        StopAllCoroutines();
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipes");
        for (int i = 0; i < pipes.Length; i++)
        {
            pipes[i].GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }

        //choosing medal
        if (score >= 0.75 * best)
        {
            medal.sprite = medals[3];
        }
        else if (score >= 0.5 * best)
        {
            medal.sprite = medals[2];
        }
        else if (score >= 0.25 * best)
        {
            medal.sprite = medals[1];
        }
        else if (score > 0)
        {
            medal.sprite = medals[0];
        }
        else
        {
            medal.enabled = false;
        }

        //bringing up game over screen
        gameOverCanvas.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
