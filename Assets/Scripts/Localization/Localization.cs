using System.Collections.Generic;
using UnityEngine;

public class Localization : SingletonMonoBehaviour<Localization>
{
    Dictionary<string, string> _localizedText = new Dictionary<string, string>();

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);

        LoadLocalizedText();
    }

    private void LoadLocalizedText(string language = "ru")
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Localization/" + language);

        if (textAsset != null)
        {
            string[] textRows = textAsset.text.Split(new char[] { '\n' });

            for (int loop = 0; loop < textRows.Length; loop++)
            {
                string[] textCols = textRows[loop].Split(new string[] { " ~ " }, System.StringSplitOptions.None);

                if (textCols.Length > 1)
                    _localizedText.Add(textCols[0], textCols[1].Replace("\\n", "\n"));
            }

            Debug.Log("Data loaded, dictionary contains: " + _localizedText.Count + " entries");
        }
    }

    public string Get(string key)
    {
        if (!_localizedText.ContainsKey(key))
            return key;

        return _localizedText[key];
    }
}
