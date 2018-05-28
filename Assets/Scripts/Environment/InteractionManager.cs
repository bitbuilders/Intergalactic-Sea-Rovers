using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class InteractionManager : Singleton<InteractionManager>
{
    [SerializeField] [Range(0.0f, 5.0f)] float m_fadeTime = 1.0f;
    [SerializeField] Transform m_leftPosition = null;
    [SerializeField] Transform m_rightPosition = null;
    [SerializeField] GameObject m_character = null;
    [SerializeField] GameObject m_carret = null;
    [SerializeField] Transform m_characterContainer = null;
    [SerializeField] KeyCode m_continueKey = KeyCode.Space;
    [SerializeField] SpriteRenderer m_first = null;
    [SerializeField] SpriteRenderer m_second = null;
    [SerializeField] TextMeshProUGUI m_dialogText = null;

    AudioSource m_audioSource;
    DialogData m_dialogData = null;
    CameraController m_camera = null;
    float m_textSpeed = 0.0f;
    int m_dialogIndex = 0;
    bool m_playAction = false;
    bool m_textFinished = false;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_camera = CameraController.Instance;
    }

    private void Start()
    {
        Interactee first = CreateInteractee(m_first, "bob");
        Interactee second = CreateInteractee(m_second, "billy");
        InteractionData data = CreateInteraction(first, InteractionData.InteractionType.CONVERSATION, "test", second);
        Interact(data);
    }

    private void Update()
    {
        if (Input.GetKeyDown(m_continueKey) && m_textFinished)
        {
            ResetDialog();
        }

        if (m_dialogData != null)
        {
            if (m_playAction)
            {
                DialogAction action = m_dialogData.actions[m_dialogIndex];

                if (action.shakeDuration > 0.0f)
                {
                    m_camera.Shake(action.shakeDuration, action.shakeAmplitude, action.shakeRate);
                }
                if (action.blinkDuration > 0.0f)
                {
                    StartCoroutine(BlinkSprite(action.interactee.sprite, action.blinkSize, action.blinkDuration));
                }
                if (action.entryPoint != DialogAction.EntryPoint.NONE)
                {
                    StartCoroutine(MoveSprite(action.interactee.sprite, action.entryPoint));
                }

                float animSpeed = action.textAnimationSpeed == 0.0f ? 1.0f : action.textAnimationSpeed;
                StartCoroutine(DrawText(action.text, action.textSpeed, animSpeed));

                m_playAction = false;
                m_textFinished = false;
                m_dialogIndex++;

                if (m_dialogIndex >= m_dialogData.actions.Count)
                {
                    m_dialogData = null;
                }
            }
        }
    }

    public void Interact(InteractionData data)
    {
        DialogData dialogData;
        ParseFileIntoDialog(data.dialogFileName, data, out dialogData);
        m_dialogData = dialogData;
        ResetDialog();
        m_dialogIndex = 0;
    }

    private void ResetDialog()
    {
        m_playAction = true;
        m_textFinished = false;
        m_dialogText.text = "";
        Transform[] children = m_characterContainer.GetComponentsInChildren<Transform>();
        for (int i = children.Length - 1; i >= 0; i--)
        {
            if (children[i] != m_characterContainer.transform)
            {
                Destroy(children[i].gameObject);
            }
        }
    }

    private IEnumerator DrawText(string text, float speed, float animSpeed)
    {
        m_textSpeed = speed;

        RectTransform rect = m_dialogText.transform.parent.GetComponent<RectTransform>();
        RectTransform carSize = m_carret.GetComponent<RectTransform>();
        float xStart = -rect.sizeDelta.x * 0.5f + carSize.sizeDelta.x;
        float yStart = rect.sizeDelta.x * 0.5f - carSize.sizeDelta.x;
        float x = xStart;
        float y = yStart;
        float xGrowth = 20.0f;
        float yGrowth = 35.0f;
        float xGrown = 0.0f;
        for (int i = 0; i < text.Length; ++i)
        {
            //m_dialogText.text += text[i];
            int amountLeft = (int)(rect.sizeDelta.x - xGrown) / (int)xGrowth;
            if (WillWrap(text, i, amountLeft))
            {
                xGrown = 0.0f;
                x = xStart;
                y -= yGrowth;
            }
            m_carret.transform.localPosition = new Vector3(x, y);
            x += xGrowth;
            xGrown += xGrowth;
            if (xGrown >= rect.sizeDelta.x - 25.0f)
            {
                xGrown = 0.0f;
                x = xStart;
                y -= yGrowth;
            }

            var t = CreateCharacter();
            t.text = text[i].ToString();
            t.transform.position = new Vector3(m_carret.transform.position.x, 0.0f);
            StartCoroutine(AnimateText(t, animSpeed, m_carret.transform.position));

            float multiplier = Input.GetKey(m_continueKey) ? 0.5f : 1.0f;
            yield return new WaitForSeconds(speed * multiplier);
        }

        m_textFinished = true;
    }

    private bool WillWrap(string text, int charIndex, int maxAmount)
    {
        bool willWrap = true;

        if (text[charIndex].ToString() == "")
            return false;

        for (int i = charIndex; i < charIndex + maxAmount; ++i)
        {
            if (i >= text.Length)
            {
                if (i <= charIndex + maxAmount)
                    willWrap = false;

                break;
            }
            if (text[i].ToString() == " ")
            {
                willWrap = false;
                break;
            }
        }

        return willWrap;
    }

    private TextMeshProUGUI CreateCharacter()
    {
        GameObject obj = Instantiate(m_character, Vector3.zero, Quaternion.identity, m_characterContainer);
        obj.transform.localPosition = Vector3.zero;
        TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();

        return text;
    }

    private IEnumerator AnimateText(TextMeshProUGUI text, float speed, Vector3 target)
    {
        target.x = 0.0f;
        for (float i = 0.0f; i <= 1.0f; i += Time.deltaTime * speed)
        {
            Vector3 pos = Interpolation.BackOut(i) * target;
            text.transform.position = new Vector3(text.transform.position.x, pos.y);
            yield return null;
        }

        text.transform.position = new Vector3(text.transform.position.x, target.y);
    }

    private IEnumerator BlinkSprite(SpriteRenderer sprite, float size, float duration)
    {
        Vector3 maxScale = sprite.transform.localScale * size;
        Vector3 startScale = sprite.transform.localScale;

        float time = duration / 2.0f;
        for (float i = 0.0f; i <= 1.0f; i += Time.deltaTime / time)
        {
            sprite.transform.localScale = Vector3.Lerp(startScale, maxScale, i);
            yield return null;
        }

        for (float i = 1.0f; i >= 0.0f; i -= Time.deltaTime / time)
        {
            sprite.transform.localScale = Vector3.Lerp(maxScale, startScale, i);
            yield return null;
        }

        sprite.transform.localScale = startScale;
    }

    private IEnumerator MoveSprite(SpriteRenderer sprite, DialogAction.EntryPoint entry)
    {
        Vector3 startPoint = sprite.transform.position;
        Vector3 endPoint = Vector3.zero;
        switch (entry)
        {
            case DialogAction.EntryPoint.LEFT:
                endPoint = m_leftPosition.position;
                break;
            case DialogAction.EntryPoint.RIGHT:
                endPoint = m_rightPosition.position;
                break;
        }

        for (float i = 0.0f; i <= 1.0f; i += Time.deltaTime * 2.0f)
        {
            sprite.transform.position = Vector3.Lerp(startPoint, endPoint, i);
            yield return null;
        }
    }

    public Interactee CreateInteractee(SpriteRenderer sprite, string name)
    {
        Interactee interactee = ScriptableObject.CreateInstance<Interactee>();
        interactee.sprite = sprite;
        interactee.entityName = name;

        return interactee;
    }

    public InteractionData CreateInteraction(Interactee firstInteractor, InteractionData.InteractionType type, string fileName, Interactee secondInteractor = null)
    {
        InteractionData data = ScriptableObject.CreateInstance<InteractionData>();
        data.firstInteractor = firstInteractor;
        data.secondInteractor = secondInteractor;
        data.interactionType = type;
        data.dialogFileName = fileName;

        return data;
    }

    private void ParseFileIntoDialog(string fileName, InteractionData interactionData, out DialogData dialogData)
    {
        string path = Application.dataPath + "/Dialog/Interactions/" + fileName + ".txt";
        if (!Directory.Exists(Application.dataPath + "/Dialog/Interactions/"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Dialog/Interactions/");
        }
        if (!File.Exists(path))
        {
            dialogData = null;
        }
        else
        {
            dialogData = ScriptableObject.CreateInstance<DialogData>();
            dialogData.actions = new List<DialogAction>();

            string[] dialog = File.ReadAllLines(path);
            
            DialogAction action = ScriptableObject.CreateInstance<DialogAction>();
            for (int i = 0; i < dialog.Length; ++i)
            {
                if (dialog[i] == "[" + interactionData.firstInteractor.entityName.ToLower() + "]")
                {
                    action.interactee = interactionData.firstInteractor;
                    continue;
                }
                else if (dialog[i] == "[" + interactionData.secondInteractor.entityName.ToLower() + "]")
                {
                    action.interactee = interactionData.secondInteractor;
                    continue;
                }

                if (dialog[i].Length > 0 && dialog[i][0] == '*')
                {
                    action.text = dialog[i].Replace("*", "");
                    if (action != null) dialogData.actions.Add(action);
                    Interactee interactee = action.interactee;
                    action = ScriptableObject.CreateInstance<DialogAction>();
                    if (i + 2 < dialog.Length && dialog[i + 2].Length > 0 && dialog[i + 2][0] == '*')
                        action.interactee = interactee;
                }
                else
                {
                    if (action != null) ParseAction(action, dialog[i]);
                }
            }
        }
    }

    private void ParseAction(DialogAction dialogAction, string action)
    {
        string[] actionParameters = action.Split('|');

        for (int i = 0; i < actionParameters.Length; i += 1)
        {
            string[] param = actionParameters[i].Split('=');
            if (param[0].Length > 0 && param[1].Length > 0)
            {
                ParseActionParameter(dialogAction, param[0], param[1]);
            }
        }
    }

    private void ParseActionParameter(DialogAction action, string parameter, string value)
    {
        switch (parameter)
        {
            case "SHAKE_AMPLITUDE":
                float amp;
                float.TryParse(value, out amp);
                action.shakeAmplitude = amp;
                break;
            case "SHAKE_RATE":
                float rate;
                float.TryParse(value, out rate);
                action.shakeRate = rate;
                break;
            case "SHAKE_DURATION":
                float dur;
                float.TryParse(value, out dur);
                action.shakeDuration = dur;
                break;
            case "BLINK_SIZE":
                float size;
                float.TryParse(value, out size);
                action.blinkSize = size;
                break;
            case "BLINK_DURATION":
                float blinkDur;
                float.TryParse(value, out blinkDur);
                action.blinkDuration = blinkDur;
                break;
            case "TEXT_SPEED":
                float speed;
                float.TryParse(value, out speed);
                action.textSpeed = speed;
                break;
            case "TEXT_ANIM_SPEED":
                float anim;
                float.TryParse(value, out anim);
                action.textAnimationSpeed = anim;
                break;
            case "ENTRY_POINT":
                action.entryPoint = (DialogAction.EntryPoint)Enum.Parse(typeof(DialogAction.EntryPoint), value);
                break;
        }
    }
}
