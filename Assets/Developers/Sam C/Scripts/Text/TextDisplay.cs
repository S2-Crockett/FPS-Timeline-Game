using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDisplay : MonoBehaviour
{

    public float speed = 0.1f;
    public Level_Manager levels;
    private string levelText;
    private string difficultyText;
    private string descriptionText;

    private string currentText = "";

    public TextMesh TextLevel;
    public TextMesh TextDifficulty;
    public TextMesh TextDescription;

    public int level = 1;
    public int Section = 1;

    public bool show = true;
    public bool finished = true;
    public bool selectable = false;

    public Select input;

    void Start()
    {
        levelText = levels.levels[level - 1].levelNumber;
        difficultyText = levels.levels[level - 1].levelDifficulty;
        descriptionText = levels.levels[level - 1].levelDescription;
        input = new Select();

        input.Move.MoveLeft.performed += e => CheckLeftInput();
        input.Move.MoveRight.performed += e => CheckRightInput();
        input.Move.Select.performed += e => SetLevel();

        input.Enable();
    }

    private void Update()
    {
        SetText();
    }

    private void SetLevel()
    {
        if(finished)
        {
            levels.levelSelected = level;
        }
    }

    private void CheckLeftInput()
    {
        if(finished && level != 1 && selectable)
        {
            level -= 1;
            show = true;
        }
    }
    private void CheckRightInput()
    {
        if (finished && level < levels.levels.Length && selectable)
        {
            level += 1;
            show = true;
        }
    }

    private void SetText()
    {
        levelText = levels.levels[level - 1].levelNumber;
        difficultyText = levels.levels[level - 1].levelDifficulty;
        descriptionText = levels.levels[level - 1].levelDescription;
        if (show)
        {
            ClearText();
            StartCoroutine(ShowText());
            show = false;
        }
    }

    IEnumerator ShowText()
    {
        finished = false;
        for (int i = 0; i < levelText.Length + 1; i++)
        {
            currentText = levelText.Substring(0, i);
            TextLevel.text = currentText;
            yield return new WaitForSeconds(speed);
        }
        for (int i = 0; i < difficultyText.Length + 1; i++)
        {
            currentText = difficultyText.Substring(0, i);
            TextDifficulty.text = currentText;
            yield return new WaitForSeconds(speed);
        }
        for (int i = 0; i < descriptionText.Length + 1; i++)
        {
            currentText = descriptionText.Substring(0, i);
            TextDescription.text = currentText;
            yield return new WaitForSeconds(speed);
        }
        finished = true;
    }

    public void ClearText()
    {
        TextLevel.text = "";
        TextDifficulty.text = "";
        TextDescription.text = "";
    }
}
