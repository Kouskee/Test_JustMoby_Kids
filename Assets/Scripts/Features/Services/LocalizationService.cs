using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class LocalizationService : ILocalizationService
{
    private readonly ReactiveProperty<SystemLanguage> _currentLanguage = new ReactiveProperty<SystemLanguage>(SystemLanguage.English);
    private readonly Dictionary<string, Dictionary<SystemLanguage, string>> _localizedTexts;

    public LocalizationService()
    {
        _localizedTexts = new Dictionary<string, Dictionary<SystemLanguage, string>>
        {
            ["cube_added_to_tower"] = new Dictionary<SystemLanguage, string>
            {
                [SystemLanguage.English] = "Cube added to tower!",
                [SystemLanguage.Russian] = "Кубик добавлен в башню!"
            },
            ["cube_thrown_to_hole"] = new Dictionary<SystemLanguage, string>
            {
                [SystemLanguage.English] = "Cube thrown away!",
                [SystemLanguage.Russian] = "Кубик выброшен!"
            },
            ["cube_missed"] = new Dictionary<SystemLanguage, string>
            {
                [SystemLanguage.English] = "Cube missed!",
                [SystemLanguage.Russian] = "Кубик пропал!"
            },
            ["height_limit_reached"] = new Dictionary<SystemLanguage, string>
            {
                [SystemLanguage.English] = "Tower height limit reached!",
                [SystemLanguage.Russian] = "Достигнут лимит высоты башни!"
            }
        };

        SetLanguage(Application.systemLanguage);
    }

    public IObservable<string> GetLocalizedText(string key)
    {
        return _currentLanguage.Select(lang => GetText(key, lang));
    }

    public void SetLanguage(SystemLanguage language)
    {
        _currentLanguage.Value = language;
    }

    private string GetText(string key, SystemLanguage language)
    {
        if (_localizedTexts.TryGetValue(key, out var translations))
        {
            if (translations.TryGetValue(language, out var text))
                return text;

            if (translations.TryGetValue(SystemLanguage.English, out var fallback))
                return fallback;
        }

        return key;
    }
}
