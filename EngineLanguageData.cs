namespace ConsoleEngine
{
    /*
     * EngineLanguageData is used by Engine internally and should not be used outside Engine
     */

    // Current language.
    public enum Language
    {
        Rus,
        Eng
    }

    /*
     * Class is just a hard coded database of keywords for engine.
     * TODO: make it less stupid :D .
     */
    public class EngineLanguageData
    {

        protected Language Language;

        public EngineLanguageData(Language lang)
        {
            Language = lang;
        }

        public string Instruction() => Language is Language.Rus ? "Инструкция" : "Instruction";
        public string LogStarter() => Language is Language.Rus ? "Начало лога" : "Log start";
        public string Choose() => Language is Language.Rus ? "Выберите" : "Choose";
        public string Continue() => Language is Language.Rus
            ? "Нажмите любую клавишу, чтобы продолжить"
            : "Press any key to continue";
        public string Menu() => Language is Language.Rus ? "Меню" : "Menu";
        public string Size() => Language is Language.Rus ? "Размер" : "Size";
        public string Error() => Language is Language.Rus ? "Ошибка" : "Error";
        public string Matrix() => Language is Language.Rus ? "Матрица" : "Matrix";
        public string KeyPress() => Language is Language.Rus ? "Нажмите нужную клавишу" : "Press needed key";


        public string[] Pages(int cur, int max)
        {
            switch (Language)
            {
                case Language.Rus:
                default:
                    return new[]
                    {
                        $"{cur} из {max} страниц",
                        "[<] [>] для листания",
                        "[Esc] для возврата"
                    };
                case Language.Eng:
                    return new[]
                    {
                        $"{cur} / {max} page",
                        "[<] [>] to change the page",
                        "[Esc] to exit"
                    };
            }
        }
        public string[] RerunMessage()
        {
            switch (Language)
            {
                case Language.Rus:
                default:
                    return new[]{
                        "Введённые вами данные неверны.",
                        "Прочитайте инструкцию, чтобы знать какой формат вводимых данных должен быть",
                    "Нажмите [Esc], если вы хотите выйти.",
                    "Нажмите любую другую клавишу, если вы хотите ввести данные заново."
                    };
                case Language.Eng:
                    return new[]
                    {
                        "The data you entered is incorrect.",
                        "Read the instruction to know what format the input data should be",
                        "Press [Esc] if you want to exit.",
                        "Press any other key if you want to enter data again."
                    };
            }
        }

        public virtual string[] IntroDefault()
        {
            return new[]
            {
                "Программа Program"
            };
        }
        public string[] IntroInfo()
        {
            return new[]
            {
                "Выберите язык / Choose language",
                "[1] - Русский [2] - English",
                "Нажмите любую другую клавишу для выхода"
            };
        }

        public string[] DrawInstruction()
        {
            switch (Language)
            {
                case Language.Rus:
                default:
                    return new[]
                    {
                        "Программа \"Менеджер склада\" позволяет симулировать работу склада.",
                        "Программа работает как на MacOS, так и на Windows",
                        "Для начала работы выберите режим работы склада - ручное или автоматическое",
                        "В ручном управлении вам нужно будет настроить параметры склада:",
                        "            размер склада и стоимость хранения одного контейнера",
                        "Далее вы сможете добавлять или убирать контейнера, а также совершать поиск по названию",
                        "В левом меню пользователь контролирует заполненность склада",
                        "Подробнее о возможностях склада:",
                        "Добавление контейнера: пользователь может добавить новый контейнер.",
                        "При создании контейнера автоматически генерируется максимальная масса контейнера",
                        "Далее пользователь указывает имя контейнера(в программе предусмотрена система, генерирующая",
                        "название для контейнера автоматически",
                        "Далее пользователь указывает количество ящиков, которые нужно поместить в контейнер",
                        "После этого пользователь заполняет контейнер ящиками,",
                        "после каждого ящика пользователь имеет возможность",
                        "закончить ввод ящиков.",
                        "Далее программа расчитывает выгоду от контейнера, если она меньше стоимости хранения,",
                        "то контейнер не будет добавлен на склад.",
                        "Удаление контейнера: пользователь может удалить любой из контейнеров.",
                        "Пользователь может удалить контейнер по названию или по номеру(номер начинается с 1)",
                        "Просмотр контейнеров: пользователь может просмотреть все контейнеры.",
                        "Поиск по названию: пользователь может получить подробную информацию про определённый контейнер",
                        "по его названию.",
                        "Выписка: пользователь может узнать стоимость склада на текущий момент.",
                        "Выписка в txt: пользователь может сохранить полную информацию о складе и граф склада в файл.",
                        "Работа в автоматическом режиме:",
                        "Для работы в автоматическом режиме пользователю нужно загрузить в программу 3 файла:",
                        "файл с базовой настройкой склада, файл с инструкциями, файл с данными для инструкций",
                        "и указать файл в который будет выводиться вся информация(логгирование).",
                        "Формат файлов: все файлы поддерживают единый формат: новые данные с новой строки, т.е.",
                        "каждая строка хранит 1 число. Так же в файлах поддерживаются комментирование и пустые строки",
                        "строка с комментарием начинается с ';'.",
                        "В файле с базовой информацией о складе нужно указать 2 числа - размер склада и стоимость места",
                        "В файле с командами идёт последовательность команд",
                        "Примеры файлов в папке с проектом",
                        "Add - добавить контейнер; Remove удалить контейнер",
                        "В файле с данными идут последовательно числа - входные данные.",
                        "Сначала кол-во ящиков, затем информация про каждый ящик: стоимость и масса - каждое число",
                        "в новой строке. ПРИМЕРЫ ФАЙЛОВ ЕСТЬ В ПАПКЕ В ПРОЕКТЕ.",
                        "После ввода всех данных, программа проведёт симуляцию",
                        "и выведет всю нужную информацию в указанный файл.",
                        "Программа полностью локализированна на русском и английском языках."
                    };
                case Language.Eng:
                    return new[]
                    {
                        "The program \"Warehouse manager\" allows you to simulate the work of a warehouse.",
                        "The program works on both MacOS and Windows",
                        "To get started, select the warehouse operation mode - manual or automatic",
                        "In manual control, you will need to adjust the warehouse parameters:",
                        "the size of the warehouse and the cost of storing one container",
                        "Next, you can add or remove containers, as well as search by name",
                        "In the left menu, the user controls the capacity of the warehouse",
                        "More about the capabilities of the warehouse:",
                        "Add container: user can add a new container.",
                        "When creating a container, the maximum weight of the container is automatically generated",
                        "Next, the user specifies the name of the container ",
                        "(the program provides for a system that generates",
                        "name for the container automatically",
                        "Next, the user specifies the number of boxes to be placed in the container",
                        "The user then fills the container with boxes",
                        "after each box, the user has the opportunity to finish entering boxes.",
                        "Next, the program calculates the benefit from the container",
                        " if it is less than the cost of storage,",
                        "then the container will not be added to the warehouse.",
                        "Delete container: user can delete any of the containers.",
                        "The user can delete the container by name or by id (id starts with 1)",
                        "View containers: user can view all containers.",
                        "Search by name: the user can get detailed information about a specific container",
                        "by its name.",
                        "Statement: the user can find out the cost of the warehouse at the current moment.",
                        "Statement in txt: the user can save complete information about the warehouse",
                        " and the warehouse graph to a file.",
                        "Work in automatic mode:",
                        "To work in automatic mode, the user needs to load 3 files into the program:",
                        "a file with a basic warehouse setup, a file with instructions, a file with data for instructions",
                        "and specify the file to which all information will be output (logging).",
                        "File format: all files support the same format: new data on a new line, i.e.",
                        "each line stores 1 number. Comments and empty lines are also supported in files",
                        "comment line starts with ';'",
                        "In the file with basic information about the warehouse, you need to specify 2 numbers",
                        "the size of the warehouse and the cost of space",
                        "The command file contains a sequence of commands",
                        "Add - add container; Remove remove container",
                        "The data file contains numbers in sequence - the input data.",
                        "Examples of files can be found in project folder",
                        "First, the number of boxes, then information about each box: cost and weight - each number",
                        "in a new line. THE SAMPLE FILES ARE IN A FOLDER IN THE PROJECT.",
                        "After entering all the data, the program will simulate",
                        "and will output all the necessary information to the specified file.",
                        "The program is fully localized in Russian and English."
                    };
            }
        }

        public string[] ExitMessage()
        {
            switch (Language)
            {
                case Language.Rus:
                default:
                    return new[]
                    {
                        "До свидания!"
                    };
                case Language.Eng:
                    return new[]
                    {
                        "Good bye!"
                    };
            }
        }
    }
}
