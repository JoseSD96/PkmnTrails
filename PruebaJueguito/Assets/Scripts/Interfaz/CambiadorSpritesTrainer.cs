using System.Collections;
using UnityEngine;

public class CambiadorSpritesTrainer : MonoBehaviour
{
    public string trainerName;
    public int walkRepetitions = 5;
    public GameObject trainerGameObject;
    private SpriteRenderer spriteRenderer;
    private Sprite[] walkSprites = new Sprite[4];
    private Sprite idle;
    private Sprite search;

    private Coroutine walkCoroutine;

    private bool isWalking = false;

    void Start()
    {
        spriteRenderer = trainerGameObject.GetComponent<SpriteRenderer>();
        LoadSprites();
        SetIdle();
    }

    void LoadSprites()
    {
        string path = "Sprites/Trainer/";
        string fileName = trainerName;

        string fullPath = path + fileName;

        Sprite[] loadedSprites = Resources.LoadAll<Sprite>(fullPath);

        if (loadedSprites.Length == 0)
        {
            return;
        }

        int posicion = 0;
        for (int i = 4; i <= 7; i++)
        {
            walkSprites[posicion] = loadedSprites[i];
            posicion++;
        }

        idle = loadedSprites[12];
        search = loadedSprites[0];

        SetIdle();
    }

    void SetIdle()
    {
        if (walkCoroutine != null)
        {
            StopCoroutine(walkCoroutine);
            walkCoroutine = null;
        }

        spriteRenderer.sprite = idle;
        isWalking = false;
    }

    public void StartWalking(System.Action onFinish = null)
    {
        if (isWalking) return;

        isWalking = true;
        walkCoroutine = StartCoroutine(WalkAnimation(onFinish));
    }

    IEnumerator WalkAnimation(System.Action onFinish)
    {
        int currentFrame = 0;
        int walkCounter = 0;

        while (walkCounter < walkRepetitions)
        {
            spriteRenderer.sprite = walkSprites[currentFrame];
            currentFrame = (currentFrame + 1) % walkSprites.Length;

            if (currentFrame == 0)
            {
                walkCounter++;
            }

            yield return new WaitForSeconds(0.1f);
        }
        SetIdle();
        onFinish?.Invoke();
    }

    IEnumerator SetSearch()
    {
        spriteRenderer.sprite = search;
        yield return new WaitForSeconds(2f);
        SetIdle();
    }

    public void Search()
    {
        if (!isWalking) StartCoroutine(SetSearch());
    }
}