using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;
using UnityEngine.UI;

public class HangmanGame : MonoBehaviour
{
    string wordFromFile;
    TextMeshProUGUI[] textDisplay;
    [SerializeField] Button[] ConditionalButtons;
    int chosenWordLength = 1;
    char[] stringCharArray;
    int playerGuessesLeft;
    char LastSelectedCharacter;
    //declare the size after we set the game up
    #region start n update
    // Start is called before the first frame update
    void Start()
    {
        SettheScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    void ChooseRandomWordFromFile()
    {
        //give a random line in the text
        int chosenLine = UnityEngine.Random.Range(0, 101);
        //read from the text
        string path = Application.dataPath + "words.txt";
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
                //sets the wordfrom file to equal
                wordFromFile = lines[chosenLine];
            }
        }
    }

    //run at the begggining to manage the game
    void SettheScene()
    {
        ChooseRandomWordFromFile();
        chosenWordLength = wordFromFile.Length;
        //initialisess the  array based no the size ofd the string
        textDisplay = new TextMeshProUGUI[chosenWordLength];
        //gives us an array of the chars
        stringCharArray = wordFromFile.ToCharArray();
        //sets the text of each display to be 1 letter, in order, of the string
        for (int i = 0; i < chosenWordLength; i++)
        {
            textDisplay[i].text = stringCharArray[i].ToString();
        }


    }

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
        }
    }
# region buttons
    void Startbutton()
    {

    }

    void Restartbutton()
    {

    }

    void Quitbutton()
    {
        Application.Quit();
    }
    #endregion

}
