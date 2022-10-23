using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialoguePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public bool playingDialogue = false;
    public List<string> lines = new List<string>();
    public GameObject textPanel;
    public TextMeshProUGUI textObject;
    private bool finishedStrand = false;
    private int charIndexEnd = 1;
    private int lineIndex = 0;
    private string lineToRead = "";
    //private int frameTimer = 0;

    void Start()
    {
        textPanel = GameObject.Find("GUICanvas/DialoguePanel");
        textObject = textPanel.GetComponentInChildren<TextMeshProUGUI>();
        textObject.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (playingDialogue)
        {
            lineToRead = lines[lineIndex];
            textObject.text = lineToRead.Substring(0, charIndexEnd);
            Debug.Log(lineToRead);
            if (charIndexEnd < lineToRead.Length)
            {
                charIndexEnd++;
            }
            
            // check if at end of text
            if (textObject.text.Length == lineToRead.Length)
            {
                finishedStrand = true;
            }

            // check for z key press and release
            if (Input.GetKeyUp(KeyCode.Z))
            {
                // either skip to end or go to next line
                if (!finishedStrand)
                {
                    // finish the line
                    textObject.text = lineToRead;
                    finishedStrand = true;
                }
                else
                {
                    // line is already finished, go to next line
                    // reset char counter
                    charIndexEnd = 1;
                    // check if at end of dialogue
                    if (lineIndex == lines.Count - 1)
                    {
                        // dialogue is over, close panel
                        playingDialogue = false;
                        textPanel.SetActive(false);
                        lineIndex = 0;
                        Debug.Log("Line index reset");
                        charIndexEnd = 1;
                        lineToRead = "";
                        textObject.text = "";
                        // set game state back to standard
                        FindObjectOfType<GameManager>().gameState = GameState.overworld;
                    }
                    else
                    {
                        // more lines to read, increment line index
                        lineIndex++;
                        Debug.Log("Line index: " + lineIndex);
                        finishedStrand = false;
                    }
                }
            }
        }       
    }

    public void PlayDialogue()
    {
        playingDialogue = true;
        textPanel.SetActive(true);
        FindObjectOfType<GameManager>().gameState = GameState.dialogue;
    }
}