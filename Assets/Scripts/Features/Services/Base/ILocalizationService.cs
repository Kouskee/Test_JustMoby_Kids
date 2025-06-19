using System;
using UnityEngine;

public interface ILocalizationService
{
    IObservable<string> GetLocalizedText(string key);
    void SetLanguage(SystemLanguage language);
}
