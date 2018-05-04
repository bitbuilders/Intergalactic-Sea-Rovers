using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : Singleton<InteractionManager>
{
    [SerializeField] [Range(0.0f, 5.0f)] float m_fadeTime = 1.0f;
    [SerializeField] [Range(0.0f, 50.0f)] float m_textSpeed = 10.0f;
    [SerializeField] Transform m_leftPosition = null;
    [SerializeField] Transform m_rightPosition = null;
    [SerializeField] KeyCode m_continueKey = KeyCode.Space;
    [SerializeField] List<InteractionData> m_interactionData = null;

    AudioSource m_audioSource;
    float m_turboTextSpeed;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        m_turboTextSpeed = m_textSpeed * 2.0f;
    }

    public void Interact(InteractionData data)
    {
        if (!m_interactionData.Contains(data))
        {
            m_interactionData.Add(data);
        }

        string[] leftDialog;
        string[] rightDialog;
        ParseFileIntoDialog("test", out leftDialog, out rightDialog, "bob", "billy");

        if (leftDialog != null)
        {
            for (int i = 0; i < leftDialog.Length; ++i)
            {
                if (leftDialog[i] != null)
                {
                    print(leftDialog[i]);
                }
                if (rightDialog[i] != null)
                {
                    print(rightDialog[i]);
                }
            }
        }
    }

    public void Interact(string leftInteractor, string rightInteractor)
    {
        InteractionData data = FindInteractionData(leftInteractor, rightInteractor);
        Interact(data);
    }

    private InteractionData FindInteractionData(string left, string right)
    {
        InteractionData data = null;

        foreach (InteractionData intData in m_interactionData)
        {
            if (intData.leftInteractor.entityName == left && intData.rightInteractor.entityName == right)
            {
                data = intData;
            }
        }

        return data;
    }

    private void ParseFileIntoEvent()
    {

    }

    private void ParseFileIntoDialog(string fileName, out string[] leftDialog, out string[] rightDialog, string leftName, string rightName)
    {
        // TODO: Make files more complex with camera shake info and other stuff like text speed

        string path = Application.dataPath + "/Dialog/Interactions/" + fileName + ".txt";
        if (!Directory.Exists(Application.dataPath + "/Dialog/Interactions/"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Dialog/Interactions/");
        }
        if (!File.Exists(path))
        {
            rightDialog = null;
            leftDialog = null;
        }
        else
        {
            string[] dialog = File.ReadAllLines(path);

            int count = 0;
            for (int i = 0; i < dialog.Length; ++i)
            {
                if (dialog[i] == "[" + leftName + "]" || dialog[i] == "[" + rightName + "]")
                {
                    count++;
                }
            }

            leftDialog = new string[count];
            rightDialog = new string[count];

            bool leftTalking = false;
            bool rightTalking = false;
            int leftCount = 0;
            int rightCount = 0;
            for (int i = 0; i < dialog.Length; ++i)
            {
                if (dialog[i] == "[" + leftName + "]")
                {
                    leftTalking = true;
                    rightTalking = false;
                }
                else if (dialog[i] != "[" + rightName + "]")
                {
                    if (leftTalking)
                    {
                        leftDialog[leftCount++] = dialog[i];
                        rightCount++;
                    }
                }
                if (dialog[i] == "[" + rightName + "]")
                {
                    rightTalking = true;
                    leftTalking = false;
                }
                else if (dialog[i] != "[" + leftName + "]")
                {
                    if (rightTalking)
                    {
                        rightDialog[rightCount++] = dialog[i];
                        leftCount++;
                    }
                }
            }
        }
    }
}
