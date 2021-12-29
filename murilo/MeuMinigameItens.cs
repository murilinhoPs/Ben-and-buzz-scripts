using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeuMinigameItens : MonoBehaviour
{
    private Color myRed = new Color(233, 69, 69);
    private Color myGreen = new Color(72, 233, 70);

    private string myTag = "engrenagemPuzzle";

    public MeuMinigame minigame1;

    public float velocity = 10f;

    private bool canMove = true;

    public Transform pivot;

    private SpriteRenderer sprite;

    public Sprite connected;


    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (canMove)
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                pivot.Rotate(Vector3.forward * velocity * Time.deltaTime);


            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                pivot.Rotate(Vector3.back * velocity * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "minigameMu")
        {
            print("Trigger");

            minigame1.step++;

            if (other.name != "FimPuzzle")
                other.GetComponentInChildren<MeuMinigameItens>().enabled = true;

            if (other.name == "FimPuzzle" && minigame1.step >= minigame1.completeSteps)
                other.GetComponent<SpriteRenderer>().color = Color.green;

            sprite.sprite = connected;

            this.enabled = false;

            other.GetComponent<CapsuleCollider>().enabled = false;
        }

    }
}
