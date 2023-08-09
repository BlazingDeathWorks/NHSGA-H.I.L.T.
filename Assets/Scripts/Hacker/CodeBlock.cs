using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeBlock : MonoBehaviour
{
    [SerializeField] private int initialTabCount;
    [SerializeField] [TextArea(3, 5)] [Tooltip("Use an \'!\' to symbol code fragment placement; Use an \'*\' to symbol highlight wrap")] private string codeTemplate;
    [SerializeField] private string highlightColor = "#FF0094";
    private Text lineOfCode;

    private void Awake()
    {
        lineOfCode = GetComponent<Text>();

        //Format: Tabs and Create highlight wrap
        if (string.IsNullOrEmpty(codeTemplate)) return;
        string newCodeTemplate = "";
        bool finishedWrap = true;

        for (int i = 0; i < initialTabCount; i++)
        {
            newCodeTemplate += "\t";
        }

        int index = codeTemplate.IndexOf("*");
        while (index != -1)
        {
            newCodeTemplate += codeTemplate.Substring(0, index);
            if (finishedWrap)
            {
                finishedWrap = false;
                newCodeTemplate += $"<color={highlightColor}>";
            }
            else
            {
                finishedWrap = true;
                newCodeTemplate += "</color>";
            }
            codeTemplate = codeTemplate.Substring(index + 1);

            index = codeTemplate.IndexOf("*");
        }
        newCodeTemplate += codeTemplate.Substring(0);

        codeTemplate = newCodeTemplate;
    }

    private string GenerateCode(string newFragment)
    {
        int index = codeTemplate.IndexOf("!");
        return codeTemplate.Substring(0, index) + newFragment + codeTemplate.Substring(index + 1);
    }

    //Call on PNB
    public string GetCodeFragment()
    {
        int index = codeTemplate.IndexOf("!");
        return lineOfCode.text.Substring(index).Trim();
    }
    public void ReplaceCodeFragment(string newFragment)
    {
        if (lineOfCode == null) return;
        lineOfCode.text = GenerateCode(newFragment);
    }
}
