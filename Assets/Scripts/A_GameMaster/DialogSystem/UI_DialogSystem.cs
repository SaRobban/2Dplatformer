using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_DialogSystem : MonoBehaviour
{
    [Header("IO")]
    private string dialogFilePath = "/Files/Dialogs/Dialogs.tsv";
  
    static class ProcessRawData
    {
        public static DialogContent[] ProcessData(string rawData)
        {
            string[] enterSeparated = SeparateByEnter(rawData);
            List<DialogContent> dialogList = new List<DialogContent>();

            for (int i = 0; i < enterSeparated.Length; i++)
            {
                if (string.IsNullOrEmpty(enterSeparated[i]))
                    continue;

                string[] lineContent = SeperateByTab(enterSeparated[i]);
                string place = lineContent[0];
                string text = lineContent[1];
                string expression = lineContent[2];
                dialogList.Add(new DialogContent(place, text, expression));
            }
            return dialogList.ToArray();
        }

        static string[] SeparateByEnter(string data)
        {
            string[] result = data.Split(new[] { '\r', '\n' });
            return result;
        }

        static string[] SeperateByTab(string data)
        {

            string[] tabSeperated = data.Split(new[] { '\t' });

            string[] result = new string[3];

            int x = 0;
            for (int y = 0; y < tabSeperated.Length; y++)
            {
                if (!string.IsNullOrEmpty(tabSeperated[y]))
                {
                    result[x] = tabSeperated[y];
                    x++;
                }
            }
            return result;
        }
    }
    [System.Serializable]
    private class DialogAnimations
    {
        [Header("Recources")]
        [SerializeField] GameObject rootObject;
        [SerializeField] Image protrateImage;
        [SerializeField] TMP_Text dialogText;
        [SerializeField] Image buttonAnim1;
        [SerializeField] Image buttonAnim2;

        [SerializeField] float scrollSpeed = 0.025f;
        [SerializeField] float buttonAnimationSpeed = 0.5f;
        float buttonAnimationTime = 0;


        [System.Serializable]
        public struct SpriteExpression { public Sprite sprite; public string mood; }
        public SpriteExpression[] m_Expressions;
        int i = 0;
        int c = 0;

        public void HideAll()
        {
            buttonAnim1.gameObject.SetActive(false);
            buttonAnim2.gameObject.SetActive(false);
            rootObject.SetActive(false);
        }

        public void ExitDialog()
        {
            CanvasFocus.Unfocus();
            CanvasManager.dialogSystem.OnClose();
          //  PlayerMain.mainCharacter.ChangeStateTo<CC_Walk>();
        }
        public IEnumerator StartDialogArray(DialogContent[] contents)
        {
            i = 0;
            c = 0;
            buttonAnimationTime = 0;
            rootObject.SetActive(true);

            CanvasFocus.SetFocus(null);

            while (i < contents.Length)
            {
                ShowContent(contents[i]);
                if (c < contents[i].text.Length)
                {
                    c++;
                    yield return new WaitForSeconds(scrollSpeed);
                }
                else
                {
                    WaitForInputToToggleDialog();
                    yield return null;
                }

            }
            HideAll();
            ExitDialog();
        }

        void ShowContent(DialogContent content)
        {
            rootObject.SetActive(true);
            SetPortrateTo(content.expression);
            ScrollText(content.text);
        }

        void SetPortrateTo(string mood)
        {
            foreach (SpriteExpression expression in m_Expressions)
            {
                if (mood == expression.mood)
                {
                    protrateImage.sprite = expression.sprite;
                    return;
                }
            }
        }

        void ScrollText(string text)
        {
            if (c < text.Length + 1)
            {
                dialogText.SetText(text.Substring(0, c));
                return;
            }

        }

        void WaitForInputToToggleDialog()
        {
            AnimatePressButton();
            if (Input.anyKeyDown)
            {
                buttonAnim1.gameObject.SetActive(false);
                buttonAnim2.gameObject.SetActive(false);
                c = 0;
                i++;
                return;
            }
        }

        void AnimatePressButton()
        {
            buttonAnimationTime += Time.deltaTime;
            if (buttonAnimationTime > buttonAnimationSpeed * 0.5f)
            {
                if (buttonAnimationTime > buttonAnimationSpeed)
                    buttonAnimationTime = 0;

                buttonAnim1.gameObject.SetActive(true);
                buttonAnim2.gameObject.SetActive(false);
                return;
            }

            buttonAnim1.gameObject.SetActive(false);
            buttonAnim2.gameObject.SetActive(true);
        }
    }

    [SerializeField] DialogAnimations dialogAnimations;

    private Coroutine dialogRunning;

    public System.Action a_OnDialogFinished;

    private struct DialogContent
    {
        public string place;
        public string text;
        public string expression;
        public DialogContent(string place, string text, string expression)
        {
            this.place = place;
            this.text = text;
            this.expression = expression;
        }
    }

    private DialogContent[] allReadDialogs;

    // Start is called before the first frame update
    void Start()
    {
        string rawdata = IO_FileManager.LoadFromHDD(Application.dataPath + dialogFilePath);
        allReadDialogs = ProcessRawData.ProcessData(rawdata);
        dialogAnimations.HideAll();
    }

    public void EnterDialogFor(string place)
    {
        Debug.Log("Search for : " + place);
       // PlayerMain.mainCharacter.ChangeStateTo<CC_FreezePlayer>();

        DialogContent[] contents = FindDialog(place);

        if (contents != null && contents.Length > 0)
        {
            HaltDialog();
            dialogRunning = StartCoroutine(dialogAnimations.StartDialogArray(contents));
            return;
        }

        Debug.LogError("Dialog not found : " + place);
        HaltDialog();
        dialogAnimations.ExitDialog();
    }

    public void HaltDialog()
    {
        if (dialogRunning != null)
        {
            StopCoroutine(dialogRunning);
            dialogAnimations.HideAll();
        }
    }

    void OnClose()
    {
        a_OnDialogFinished?.Invoke();
    }

    private DialogContent[] FindDialog(string place)
    {
        List<DialogContent> matchingDialogs = new List<DialogContent>();

        for (int i = 0; i < allReadDialogs.Length; i++)
        {
            if (place == allReadDialogs[i].place)
            {
                matchingDialogs.Add(allReadDialogs[i]);
            }
        }
        return matchingDialogs.ToArray();
    }
}
