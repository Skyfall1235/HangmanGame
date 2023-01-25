//////////////////////////////////////////////
//Assignment/Lab/Project: Hangman Game
//Name: Wyatt Murray
//Section: 2023SP.SGD.213.2172
//Instructor: BryanSowers
//Date: 1/24/2023
/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;
using UnityEngine.UI;


public class HangmanGame : MonoBehaviour
{
    #region variables
    [SerializeField] private TextMeshProUGUI textDisplay;
    [SerializeField] private TextMeshProUGUI playerGuesses;
    [SerializeField] private TextMeshProUGUI usedLetters;
    [SerializeField] private TextMeshProUGUI endOfGameResponse;
    
    
    private List <string> failedLetters = new List<string>();
    [SerializeField] private GameObject[] gamePanels = new GameObject[3];
    [SerializeField] private Button[] ConditionalButtons;
    [SerializeField] private TextMeshProUGUI playerGuessCounter;
    
    private string wordFromFile;
    private string[] blankArray;
    private char[] stringCharArray;
    private char LastSelectedCharacter;
    private int chosenWordLength = 1;
    private int playerGuessesLeft = 6;
    private bool inputIsAllowed = false;
    private bool gameIsRunning = false;
    #endregion
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
        //updates the guess amount and the failed letters array to the screen
        playerGuessCounter.text = playerGuessesLeft.ToString() + " guesses left";
        usedLetters.text = string.Join(", ", failedLetters);
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
        playerGuessesLeft = 12;
        inputIsAllowed = true;
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
    #region Game State Management
    //bool to check if the guess was a letter in the string
    bool CheckIfLetterIsInString(char guessChar)
    {
        //Debug.Log(string.Join(" ", stringCharArray));
        for (int i = 0; i < chosenWordLength ; i++)
        {
            if (stringCharArray[i] == guessChar)
            {
                //Debug.Log("returns true");
                return true;
            }
        }
        // if it cant find it, the method returns false.
        return false;
    }
    void LastSelectedKey()
    {
        if (Input.anyKeyDown && inputIsAllowed)
        {
            char key = (char)Input.inputString[0];
            if (char.IsLetter(key) && char.IsLower(key))
            {
                LastSelectedCharacter = key;
                //Debug.Log(LastSelectedCharacter);
            }
            PlayerResponse();
        }
    }
    void PlayerResponse()
    {
        //if the player slected a key, test if it isnt already in the fail list. if it isnt, see if it is in the string. if no,
        if (CheckIfLetterIsInString(LastSelectedCharacter) && !failedLetters.Contains(LastSelectedCharacter.ToString()))
        {
            //checking every element in the original array to see if it contains the letter
            for (int i = 0; i < chosenWordLength; i++)
            {
                if (stringCharArray[i] == LastSelectedCharacter)
                {
                    //check if the element contains a string version of the last selected character
                    blankArray[i] = LastSelectedCharacter.ToString();
                    //join the array elements as text together
                    textDisplay.text = string.Join(" ", blankArray);
                    //see what i selected
                    //Debug.Log(blankArray[i]);
                }
            }
        }
        else if (failedLetters.Contains(LastSelectedCharacter.ToString()))
        {
            //if the failed string contains a variable alreadt and a person choses it again, punish them cause i say so
            playerGuessesLeft--;
        }
        else
        {
            //add the letter to the used letters list and deduct a player guess
            failedLetters.Add(LastSelectedCharacter.ToString());
            playerGuessesLeft--;
        }
        //usedLetters.text = string.Join(", ", failedLetters);
        WinOrLose();
    }
#endregion
    void WinOrLose()
    {
        //for testing to try and figure out why i couldnt get a true statement for comparing the arrays
        //Debug.Log(String.Join("", blankArray) == String.Join("", stringCharArray));
        //Debug.Log(blankArray.ToString());
        //Debug.Log(stringCharArray.ToString());




        //compare the 2 strings, and if they are correct perform the win condition
        if ( playerGuessesLeft == 0)
        {
            //display the lose panel and turn off input 
            EndOfGamePanel(false);
            inputIsAllowed = false;
        }
        if (String.Join("", blankArray) == String.Join("", stringCharArray))
        {
            //turn on the win screen and turn off input.
            EndOfGamePanel(true);
            inputIsAllowed = false;
        }
    }

    void EndOfGamePanel(bool playerWins)
    {
        if(playerWins)
        {
            gamePanels[2].SetActive(true);
            Image img = gamePanels[2].GetComponent<Image>();
            img.color = UnityEngine.Color.green;
            endOfGameResponse.text = $"You Won! \n with {playerGuessesLeft} \n guesses left";

        }
        else
        {
            gamePanels[2].SetActive(true);
            Image img = gamePanels[2].GetComponent<Image>();
            img.color = UnityEngine.Color.red;
            endOfGameResponse.text = $"You Lost! \n please retry";
        }
    }
    # region buttons
    public void Startbutton()
    {
        //turn panel 1 off and start the game
        gamePanels[0].SetActive(false);
        gameIsRunning = true;
    }

    public void Restartbutton()
    {
        //run the setup commands for the game
        SettheScene();
        GuessStringSetup();
        gamePanels[2].SetActive(false);
    }

    public void Quitbutton()
    {
        Application.Quit();
    }
    #endregion

}
