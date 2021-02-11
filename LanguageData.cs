using ConsoleEngine;

/// <summary>
/// Class that is used to return needed string in specific Language.
/// Now it supports only RUS and ENG locales.
/// THIS IS TEMPLATE: if you want to create your own template data, you can just copy this file.
/// or you can inherit from EngineLanguageData.
/// </summary>
class LanguageData : EngineLanguageData
{
    /// <summary> Represents Language (RUS or ENG) </summary>

    /// <summary>
    /// </summary>
    /// <param name="lang"> set Language variable to a chosen language </param>
    public LanguageData(Language lang) : base(lang) { }

    // ALL THE NEXT METHODS JUST RETURN STRINGS IN THE CHOSEN LANGUAGE
    // =========================| Titles |================================

    #region Titles
    public string MainMenuTitle() => Language == Language.Rus ? "Главное меню" : "Main menu";
    public string ManagerTitle() => Language == Language.Rus ? "Менеджер" : "Manager";

    #endregion

    // ==========================| Items |================================

    #region Items
    public string[] MainMenuItems()
    {
        switch (Language)
        {
            case Language.Rus:
            default:
                return new[]
                {
                        "Начать",
                        "Инструкция",
                        "Выход"
                    };
            case Language.Eng:
                return new[]
                {
                        "Start",
                        "Instruction",
                        "Exit"
                    };
        }
    }

    #endregion

    // ================================| Input information |==================================

    #region Input_information

    #endregion

    // ===========================| Basic information data |====================================

    #region Basic_information_data


    #endregion

    // =========================| Generators |===============================

    #region Generators

    #endregion


    // ==========================| Errors |===============================================

    #region Errors

    #endregion
}
