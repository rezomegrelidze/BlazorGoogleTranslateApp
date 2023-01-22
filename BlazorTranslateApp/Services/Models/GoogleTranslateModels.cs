namespace BlazorTranslateApp.Services.Models;

public class LanguagesData
{
    public List<Language> languages { get; set; }
}

public class Language
{
    public string language { get; set; }
}

public class LanguagesResponse
{
    public LanguagesData data { get; set; }
}


public class TranslateData
{
    public List<Translation> translations { get; set; }
}

public class TranslateResponse
{
    public TranslateData data { get; set; }
}

public class Translation
{
    public string translatedText { get; set; }
}