using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class InteractionManager : Singleton<InteractionManager>
{
    [Header("Display Info")]
    [SerializeField] [Range(0.0f, 5.0f)] float m_fadeTime = 1.0f;
    [SerializeField] [Range(0.0f, 1.0f)] float m_tintOpacity = 0.4f;
    [SerializeField] GameObject m_interactionCanvas = null;
    [SerializeField] GameObject m_character = null;
    [SerializeField] GameObject m_carret = null;
    [SerializeField] Transform m_characterContainer = null;
    [SerializeField] KeyCode m_continueKey = KeyCode.Space;
    [SerializeField] TextMeshProUGUI m_dialogText = null;
    [SerializeField] TextMeshProUGUI m_entity1NameText = null;
    [SerializeField] TextMeshProUGUI m_entity2NameText = null;
    [SerializeField] TextMeshProUGUI m_continueText = null;
    [SerializeField] Image m_tint = null;
    [Header("Dialog Info")]
    [SerializeField] Image m_firstEntity = null;
    [SerializeField] Image m_secondEntity = null;
    [SerializeField] Transform m_leftPosition = null;
    [SerializeField] Transform m_leftPositionEnd = null;
    [SerializeField] Transform m_leftPositionStart = null;
    [SerializeField] Transform m_rightPosition = null;
    [SerializeField] Transform m_rightPositionEnd = null;
    [SerializeField] Transform m_rightPositionStart = null;
    [Header("General")]

    AudioManager m_audioManager;
    DialogData m_dialogData = null;
    Interactable m_npc = null;
    Interactee[] m_currentInteractees = null;
    CameraController m_camera = null;
    int m_dialogIndex = 0;
    bool m_playAction = false;
    bool m_textFinished = false;
    bool m_contFadeIn = false;

    private void Awake()
    {
        m_audioManager = AudioManager.Instance;
        m_camera = CameraController.Instance;
        m_currentInteractees = new Interactee[2];
    }

    private void Start()
    {
        m_continueText.text = "Press " + m_continueKey.ToString() + " to Continue";
        Color c = m_continueText.color;
        c.a = 0.0f;
        m_continueText.color = c;
    }

    private void Update()
    {
        if (m_textFinished)
        {
            FadeBlink();
        }

        if (Input.GetKeyDown(m_continueKey) && m_textFinished)
        {
            m_continueText.enabled = false;
            ResetDialog();
            if (m_dialogData == null)
            {
                StartCoroutine(ResetSprites());
            }
        }

        if (m_dialogData != null)
        {
            if (m_playAction)
            {
                DialogAction action;
                Image entity;
                UpdateCurrent(out action, out entity);

                if (action.shakeDuration > 0.0f)
                {
                    Shake(action);
                }
                if (action.blinkDuration > 0.0f)
                {
                    StartCoroutine(BlinkSprite(entity, action.blinkSize, action.blinkDuration));
                }
                if (action.entryPoint != DialogAction.EntryPoint.NONE)
                {
                    StartCoroutine(MoveSprite(entity, action.entryPoint, 2.0f));
                }

                float animSpeed = action.textAnimationSpeed == 0.0f ? 1.0f : action.textAnimationSpeed;
                StartCoroutine(DrawText(action.text, action.textSpeed, animSpeed, action.interactee.pitch * action.pitchModifier, action.interactee.color));

                m_playAction = false;
                m_textFinished = false;
                m_dialogIndex++;

                if (m_dialogIndex >= m_dialogData.actions.Count)
                {
                    m_dialogData = null;
                    m_dialogIndex = 0;
                    m_currentInteractees[0] = null;
                    m_currentInteractees[1] = null;
                }
            }
        }
    }

    private void FadeBlink()
    {
        Color c = m_continueText.color;
        float speed = Time.deltaTime * 0.5f;

        if (m_contFadeIn)
        {
            c.a += speed;
            if (c.a >= 1.0f)
            {
                m_contFadeIn = false;
                c.a = 0.99f;
            }
        }
        else
        {
            c.a -= speed;
            if (c.a < 0.1f)
            {
                m_contFadeIn = true;
                c.a = 0.1f;
            }
        }

        m_continueText.color = c;
    }

    public void Interact(InteractionData data, Interactable npc)
    {
        m_interactionCanvas.SetActive(true);
        StartCoroutine(FadeTint(m_tint, 1.0f, true));
        DialogData dialogData;
        ParseFileIntoDialog(data.dialogFileName, data, out dialogData);
        m_dialogData = dialogData;
        ResetDialog();
        m_dialogIndex = 0;
        m_entity1NameText.text = "";
        m_entity2NameText.text = "";
        m_npc = npc;
        m_npc.Busy = true;
    }

    private void UpdateCurrent(out DialogAction action, out Image entity)
    {
        var prevCurrent = m_dialogData.current;
        action = m_dialogData.actions[m_dialogIndex];
        if (m_currentInteractees[0] == null)
            m_currentInteractees[0] = action.interactee;
        else if (m_currentInteractees[1] == null && m_currentInteractees[0] != action.interactee)
            m_currentInteractees[1] = action.interactee;

        if (m_currentInteractees[0] == action.interactee)
            m_dialogData.current = DialogData.CurrentInteractee.FIRST;
        else if (m_currentInteractees[1] == action.interactee)
            m_dialogData.current = DialogData.CurrentInteractee.SECOND;

        entity = null;
        if (prevCurrent != m_dialogData.current)
        {
            if (m_dialogData.current == DialogData.CurrentInteractee.FIRST)
            {
                StartCoroutine(FadeEntity(m_firstEntity, 2.0f, true, 1.0f, true));
                StartCoroutine(FadeEntity(m_secondEntity, 2.0f, false, 0.5f, true));
                StartCoroutine(FadeText(m_entity1NameText, 2.0f, true, 1.0f, true));
                StartCoroutine(FadeText(m_entity2NameText, 2.0f, false, 0.5f, true));
                m_entity1NameText.color = action.interactee.color;
                m_entity1NameText.text = action.interactee.entityName;
                m_firstEntity.sprite = action.interactee.sprite;
                entity = m_firstEntity;
            }
            else if (m_dialogData.current == DialogData.CurrentInteractee.SECOND)
            {
                StartCoroutine(FadeEntity(m_firstEntity, 2.0f, false, 0.5f, true));
                StartCoroutine(FadeEntity(m_secondEntity, 2.0f, true, 1.0f, true));
                StartCoroutine(FadeText(m_entity1NameText, 2.0f, false, 0.5f, true));
                StartCoroutine(FadeText(m_entity2NameText, 2.0f, true, 1.0f, true));
                m_entity2NameText.color = action.interactee.color;
                m_entity2NameText.text = action.interactee.entityName;
                m_secondEntity.sprite = action.interactee.sprite;
                entity = m_secondEntity;
            }
        }
    }

    private void Shake(DialogAction action)
    {
        float UIMultiplier = 100.0f;
        m_camera.Shake(action.shakeDuration, action.shakeAmplitude, action.shakeRate);
        Shaker.Instance.ShakeObject(m_firstEntity.gameObject, action.shakeDuration, action.shakeAmplitude * UIMultiplier, action.shakeRate);
        Shaker.Instance.ShakeObject(m_secondEntity.gameObject, action.shakeDuration, action.shakeAmplitude * UIMultiplier, action.shakeRate);
        Shaker.Instance.ShakeObject(m_entity1NameText.gameObject, action.shakeDuration, action.shakeAmplitude * UIMultiplier, action.shakeRate);
        Shaker.Instance.ShakeObject(m_entity2NameText.gameObject, action.shakeDuration, action.shakeAmplitude * UIMultiplier, action.shakeRate);
    }

    private IEnumerator ResetSprites()
    {
        StartCoroutine(FadeTint(m_tint, 1.0f, false));
        StartCoroutine(FadeEntity(m_firstEntity, 1.0f, false, 0.0f, true));
        StartCoroutine(FadeEntity(m_secondEntity, 1.0f, false, 0.0f, true));
        StartCoroutine(FadeText(m_entity1NameText, 2.0f, false, 0.0f, true));
        StartCoroutine(FadeText(m_entity2NameText, 2.0f, false, 0.0f, true));
        StartCoroutine(MoveSprite(m_firstEntity, DialogAction.EntryPoint.LEFT_START, 2.0f));
        StartCoroutine(MoveSprite(m_secondEntity, DialogAction.EntryPoint.RIGHT_START, 2.0f));

        yield return new WaitForSeconds(1.0f);
        m_npc.Busy = false;
        m_npc = null;
        m_firstEntity.sprite = null;
        m_secondEntity.sprite = null;
        m_firstEntity.transform.position = m_leftPositionStart.position;
        m_secondEntity.transform.position = m_rightPositionStart.position;
        m_interactionCanvas.SetActive(false);
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

    private IEnumerator FadeText(TextMeshProUGUI text, float speed, bool fadeIn, float limit = 0.0f, bool keepStartingOpacity = false)
    {
        if (fadeIn)
        {
            limit = limit == 0.0f ? 1.0f : limit;
            float start = text.color.a;
            if (!keepStartingOpacity)
            {
                start = 0.0f;
                Color color = text.color;
                color.a = 0.0f;
                text.color = color;
            }

            for (float i = start; i <= limit; i += Time.deltaTime * speed)
            {
                Color c = text.color;
                c.a = i;
                text.color = c;
                yield return null;
            }
            Color co = text.color;
            co.a = limit;
            text.color = co;
        }
        else
        {
            float start = text.color.a;
            if (!keepStartingOpacity)
            {
                start = 1.0f;
                Color color = text.color;
                color.a = 1.0f;
                text.color = color;
            }
            for (float i = start; i >= limit; i -= Time.deltaTime * speed)
            {
                Color c = text.color;
                c.a = i;
                text.color = c;
                yield return null;
            }
            Color co = text.color;
            co.a = limit;
            text.color = co;
        }
    }

    private IEnumerator FadeEntity(Image entity, float speed, bool fadeIn, float limit = 0.0f, bool keepStartingOpacity = false)
    {
        if (fadeIn)
        {
            limit = limit == 0.0f ? 1.0f : limit;
            float start = entity.color.a;
            if (!keepStartingOpacity)
            {
                start = 0.0f;
                Color color = entity.color;
                color.a = 0.0f;
                entity.color = color;
            }

            for (float i = start; i <= limit; i += Time.deltaTime * speed)
            {
                Color c = entity.color;
                c.a = i;
                entity.color = c;
                yield return null;
            }
            Color co = entity.color;
            co.a = limit;
            entity.color = co;
        }
        else
        {
            float start = entity.color.a;
            if (!keepStartingOpacity)
            {
                start = 1.0f;
                Color color = entity.color;
                color.a = 1.0f;
                entity.color = color;
            }
            for (float i = start; i >= limit; i -= Time.deltaTime * speed)
            {
                Color c = entity.color;
                c.a = i;
                entity.color = c;
                yield return null;
            }
            Color co = entity.color;
            co.a = limit;
            entity.color = co;
        }
    }

    private IEnumerator FadeTint(Image tint, float speed, bool fadeIn, bool keepStartingOpacity = false)
    {
        if (fadeIn)
        {
            if (!keepStartingOpacity)
            {
                Color color = tint.color;
                color.a = 0.0f;
                tint.color = color;
            }

            for (float i = 0.0f; i <= m_tintOpacity; i += Time.deltaTime * m_fadeTime * speed)
            {
                Color c = tint.color;
                c.a = i;
                tint.color = c;
                yield return null;
            }
            Color co = tint.color;
            co.a = m_tintOpacity;
            tint.color = co;
        }
        else
        {
            if (!keepStartingOpacity)
            {
                Color color = tint.color;
                color.a = m_tintOpacity;
                tint.color = color;
            }
            for (float i = tint.color.a; i >= 0.0f; i -= Time.deltaTime * m_fadeTime * speed)
            {
                Color c = tint.color;
                c.a = i;
                tint.color = c;
                yield return null;
            }
            Color co = tint.color;
            co.a = 0.0f;
            tint.color = co;
        }
    }

    private IEnumerator DrawText(string text, float speed, float animSpeed, float pitch, Color color)
    {
        RectTransform rect = m_dialogText.transform.parent.GetComponent<RectTransform>();
        RectTransform carSize = m_carret.GetComponent<RectTransform>();
        float xStart = -rect.sizeDelta.x * 0.5f + carSize.sizeDelta.x;
        float yStart = rect.sizeDelta.x * 0.5f - carSize.sizeDelta.x;
        float x = xStart;
        float y = yStart;
        float xGrowth = 20.0f;
        float yGrowth = 40.0f;
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

            var t = CreateCharacter(color);
            t.text = text[i].ToString();
            t.transform.position = new Vector3(m_carret.transform.position.x, 0.0f);
            StartCoroutine(AnimateText(t, animSpeed, m_carret.transform.position));
            float multiplier = Input.GetKey(m_continueKey) ? 0.2f : 1.0f;

            m_audioManager.PlayClip("Dialog", Vector3.zero, false, transform, pitch);
            yield return new WaitForSeconds(speed * multiplier);
        }

        yield return new WaitForSeconds(0.5f);
        m_continueText.enabled = true;
        StartCoroutine(FadeText(m_continueText, 2.0f, true, 1.0f, false));
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

    private TextMeshProUGUI CreateCharacter(Color color)
    {
        GameObject obj = Instantiate(m_character, Vector3.zero, Quaternion.identity, m_characterContainer);
        obj.transform.localPosition = Vector3.zero;
        TextMeshProUGUI text = obj.GetComponent<TextMeshProUGUI>();
        text.color = color;

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

    private IEnumerator BlinkSprite(Image sprite, float size, float duration)
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
            sprite.transform.localScale = Vector3.Lerp(startScale, maxScale, i);
            yield return null;
        }

        sprite.transform.localScale = startScale;
    }

    private IEnumerator MoveSprite(Image sprite, DialogAction.EntryPoint entry, float speed = 1.0f)
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
            case DialogAction.EntryPoint.LEFT_START:
                endPoint = m_leftPositionEnd.position;
                break;
            case DialogAction.EntryPoint.RIGHT_START:
                endPoint = m_rightPositionEnd.position;
                break;
        }

        for (float i = 0.0f; i <= 1.0f; i += Time.deltaTime * speed)
        {
            sprite.transform.position = Vector3.Lerp(startPoint, endPoint, i);
            yield return null;
        }
    }

    public Interactee CreateInteractee(Sprite sprite, string name)
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
        string path = Application.dataPath + "/StreamingAssets/Interactions/" + fileName + ".txt";
        if (!Directory.Exists(Application.dataPath + "/StreamingAssets/Interactions/"))
        {
            Directory.CreateDirectory(Application.dataPath + "/StreamingAssets/Interactions/");
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
                if (dialog[i].ToLower() == "[" + interactionData.firstInteractor.entityName.ToLower() + "]")
                {
                    action.interactee = interactionData.firstInteractor;
                    continue;
                }
                else if (dialog[i].ToLower() == "[" + interactionData.secondInteractor.entityName.ToLower() + "]")
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
                    if (action != null)
                    {
                        SetActionDefaults(ref action);
                        ParseAction(action, dialog[i]);
                    }
                }
            }
        }
    }

    private void SetActionDefaults(ref DialogAction action)
    {
        action.pitchModifier = 1.0f;
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
            case "PITCH":
                float pitch;
                float.TryParse(value, out pitch);
                action.pitchModifier = pitch;
                break;
        }
    }
}
