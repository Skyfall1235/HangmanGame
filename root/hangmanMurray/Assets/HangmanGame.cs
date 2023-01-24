using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class HangmanGame : MonoBehaviour
{
    private string wordFromFile;
    [SerializeField] private TextMeshProUGUI textDisplay;
    [SerializeField] private TextMeshProUGUI playerGuesses;
    [SerializeField] private TextMeshProUGUI usedLetters;
    private string guessedLetters;
    private string[] blankArray;
    private List <string> failedLetters = new List<string>();
    [SerializeField] private GameObject[] gamePanels = new GameObject[2];
    [SerializeField] private Button[] ConditionalButtons;
    [SerializeField] private TextMeshProUGUI[] Counters = new TextMeshProUGUI[2];
    private int chosenWordLength = 1;
    private char[] stringCharArray;
    private int playerGuessesLeft = 6;
    private char LastSelectedCharacter;

    private bool gameIsRunning = false;
    //declare the size after we set the game up
    #region start n update
    // Start is called before the first frame update
    void Start()
    {
        SettheScene();
        GuessStringSetup();
    }

    // Update is called once per frame
    void Update()
    {
        Counters[0].text = playerGuessesLeft.ToString() + " guesses left";
        if (gameIsRunning)
        {
            LastSelectedKey();
        }
    }
    #endregion
    #region game setup
    //run at the begggining to manage the game
    void ChooseRandomWordFromFile()
    {
        //give a random line in the text
        int chosenLine = UnityEngine.Random.Range(0, 101);
        //read from the text
        string path = Application.dataPath + "/words.txt";
        using (StreamReader reader = new StreamReader(path))
        {
            List<string> lines = new List<string>();
            //iterate through every line to see if there is text. if there is, add them the the line list
            for (int i = 0; i < 101; i++)
            {
                string line = reader.ReadLine();
                //we know the text file only has 1 word per line, so we only need to check if something messed up.
                if(line != null)
                {
                    lines.Add(line);
                }
            }
            //sets the wordfrom file to equal the random line from the text
            wordFromFile = lines[chosenLine];
        }
    }
    void SettheScene()
    {
        ChooseRandomWordFromFile();
        chosenWordLength = wordFromFile.Length;
        //initialisess the  array based no the size ofd the string
        //gives us an array of the chars
        stringCharArray = wordFromFile.ToCharArray();
        Debug.Log(wordFromFile);
    }
    void GuessStringSetup()
    {
        //display the blank string witt hte correct amount of letters in the word
        blankArray = new string[chosenWordLength];
        for (int i = 0; i < chosenWordLength; i++)
        {
            blankArray[i] = "_";
        }
        textDisplay.text = string.Join(" ", blankArray);

    }
    #endregion

    //manages the state of the sprite display
    void SpriteStateManager()
    {
        //manages the sprints based ona state machine
        switch (playerGuessesLeft)
        {
            //case 0:
            //set the sprite to be the first sprite
        }
    }

    //bool to check if the guess was a letter in the string
    bool CheckIfLetterIsInString(char guessChar)
    {
        for (int i = 0; i < chosenWordLength ; i++)
        {
            if (stringCharArray[i] == guessChar)
            {
                
                return true;
            }
        }
        // if it cant find it, the method returns false.
        return false;
    }
    void LastSelectedKey()
    {
        if (Input.anyKeyDown)
        {
            char key = (char)Input.inputString[0];
            if (char.IsLetter(key) && char.IsLower(key))
            {
                LastSelectedCharacter = key;
                
            }
            PlayerResponse();
        }
    }



    void PlayerResponse()
    {
        //if the player slected a key, test if it isnt already in the fail list. if it isnt, see if it is in the string. if no,
        if (CheckIfLetterIsInString(LastSelectedCharacter) && !failedLetters.Contains(LastSelectedCharacter.ToString()))
        {
            for (int i = 0; i < chosenWordLength; i++)
            {
                if (stringCharArray[i] == LastSelectedCharacter)
                {
                    blankArray[i] = LastSelectedCharacter.ToString();
                }
            }
        }
        else if (failedLetters.Contains(LastSelectedCharacter.ToString()))
        {
            playerGuessesLeft--;
        }
        else
        {
            failedLetters.Add(LastSelectedCharacter.ToString());
            playerGuessesLeft--;
        }
        //usedLetters.text = string.Join(", ", failedLetters);
    }
# region buttons
    public void Startbutton()
    {
        //turn panel 1 off
        gamePanels[0].SetActive(false);
        gameIsRunning = true;
    }

    public void Restartbutton()
    {

    }

    public void Quitbutton()
    {
        Application.Quit();
    }
    #endregion

}
