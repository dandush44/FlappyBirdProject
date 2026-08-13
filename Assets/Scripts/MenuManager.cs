using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public SpriteRenderer background;
    public Sprite[] backgrounds;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        //choosing random background
        background.sprite = backgrounds[Random.Range(0, backgrounds.Length)];

        //choosing random bird
        player = GameObject.FindGameObjectWithTag("Player");
        Animator anim = player.GetComponent<Animator>();
        anim.SetInteger("birdType", Random.Range(0, 3));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
