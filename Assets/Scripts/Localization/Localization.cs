using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Localization : MonoBehaviour
{
    public const string THAI = "THAI";
    public const string ENGLISH = "ENGLISH";

    public const string ERROR = "ERROR";

    public const string CONVERT_PROGRESSION_START = "CONVERT_PROGRESSION_START";
    public const string CONVERT_PROGRESSION_FETCHING_ITEM = "CONVERT_PROGRESSION_FETCHING_ITEM";
    public const string CONVERT_PROGRESSION_FETCHING_RESOURCE_NAME = "CONVERT_PROGRESSION_FETCHING_RESOURCE_NAME";
    public const string CONVERT_PROGRESSION_FETCHING_SKILL = "CONVERT_PROGRESSION_FETCHING_SKILL";
    public const string CONVERT_PROGRESSION_FETCHING_CLASS_NUMBER = "CONVERT_PROGRESSION_FETCHING_CLASS_NUMBER";
    public const string CONVERT_PROGRESSION_FETCHING_CLASS_MONSTER = "CONVERT_PROGRESSION_FETCHING_CLASS_MONSTER";
    public const string CONVERT_PROGRESSION_FETCHING_ITEM_COMBO = "CONVERT_PROGRESSION_FETCHING_ITEM_COMBO";
    public const string CONVERT_PROGRESSION_FETCHING_RESOURCE_NAME_WITH_TYPE = "CONVERT_PROGRESSION_FETCHING_RESOURCE_NAME_WITH_TYPE";
    public const string CONVERT_PROGRESSION_FETCHING_PLEASE_WAIT = "CONVERT_PROGRESSION_FETCHING_PLEASE_WAIT";

    public const string AUTO_BONUS_3 = "AUTO_BONUS_3";

    [Serializable]
    public class JsonData
    {
        public List<Data> datas = new List<Data>();

        [Serializable]
        public class Data
        {
            public string language;

            public List<KeyData> keyDatas = new List<KeyData>();

            [Serializable]
            public class KeyData
            {
                public string key;
                public string texts;
            }
        }

    }

    List<LocalizationDatabase> _localizationDatabases = new List<LocalizationDatabase>();

    string _currentLanguage;
    LocalizationDatabase _currentLocalizationDatabase = new LocalizationDatabase();

    void Start()
    {
        ParseJson();

        SetupLanguage(THAI);
    }

    void ParseJson()
    {
        var path = Application.dataPath + "/Assets/localization.json";

        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);

            if (!string.IsNullOrEmpty(json))
            {
                JsonData jsonData = null;

                try
                {
                    jsonData = JsonUtility.FromJson<JsonData>(json);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                if (jsonData != null)
                {
                    _localizationDatabases = new List<LocalizationDatabase>();

                    for (int i = 0; i < jsonData.datas.Count; i++)
                    {
                        LocalizationDatabase localizationDatabase = new LocalizationDatabase();

                        localizationDatabase.language = jsonData.datas[i].language;

                        localizationDatabase.datas = new Dictionary<string, string>();

                        for (int j = 0; j < jsonData.datas[i].keyDatas.Count; j++)
                            localizationDatabase.datas.Add(jsonData.datas[i].keyDatas[j].key, jsonData.datas[i].keyDatas[j].texts);

                        _localizationDatabases.Add(localizationDatabase);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Get translated texts
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetTexts(string key)
    {
        if (_currentLocalizationDatabase.datas.ContainsKey(key))
            return _currentLocalizationDatabase.datas[key];
        else
            return string.Empty;
    }

    /// <summary>
    /// Setup language
    /// </summary>
    /// <param name="language"></param>
    public void SetupLanguage(string language)
    {
        _currentLanguage = language;

        _currentLocalizationDatabase = new LocalizationDatabase();

        for (int i = 0; i < _localizationDatabases.Count; i++)
        {
            if (_localizationDatabases[i].language == _currentLanguage)
            {
                _currentLocalizationDatabase = _localizationDatabases[i];

                break;
            }
        }
    }
}