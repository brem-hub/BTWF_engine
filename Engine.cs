using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
#if WINDOWS
using System.Runtime.InteropServices;
#endif

namespace ConsoleEngine
{
    /* Engine - is the class that handles most of the basic work of the app.
       Engine class can Read from console, output data using Drawer class.
       Engine is used to simplify some basic routines like creating a menu or reading numbers.
       For now it is implemented with multiple redeclaration, but in future it will be updated.
     */
    public static class Engine
    {
        // Instance of Drawer class, that is capable of drawing data.
        private static Drawer Drawer { get; } = Drawer.GetInstance();

        private static Language _lang  = Language.Eng;
        public static Language Lang
        {
            get => _lang;
            set
            {
                _lang = value;
                Drawer.SetLanguage(_lang);
            }
        }

        /// <summary>
        /// Method is used to set ELanguage of the whole system.
        /// It uses old style of choosing Items.
        /// </summary>
        /// <returns> "rus" or "eng" depending on the language </returns>
        public static bool SetLanguage()
        {
            Drawer.DrawIntro();

            // Choose the language.
            Language language;
            var data = Console.ReadKey();
            switch (data.Key)
            {
                case ConsoleKey.D1:
                    language = Language.Rus;
                    break;
                case ConsoleKey.D2:
                    language = Language.Eng;
                    break;
                // If any other key is pressed - exit.
                default:
                    Exit();
                    return false;
            }
            Drawer.SetLanguage(language);
            Lang = language;
            return true;
        }

#if WINDOWS

        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        private static extern uint GetLastError();
        
#endif

        static Engine()
        {

#if MACOS
            DrawWindow(Drawer.ELanguage.MacOSInfo(), Drawer.ELanguage.MacOSInfoTitle());
#endif

#if WINDOWS
            try
            {
                var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);
                if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
                {
                    HandleException(new SystemException("failed to get output console mode"));
                    //Console.WriteLine("failed to get output console mode");
                    //Console.ReadKey();
                    //return;
                }

                outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;
                if (!SetConsoleMode(iStdOut, outConsoleMode))
                {
                    HandleException(new SystemException($"failed to set output console mode, error code: {GetLastError()}"));
                    //Console.WriteLine($"failed to set output console mode, error code: {GetLastError()}");
                    //Console.ReadKey();
                }

            }
            catch(Exception e)
            {
                HandleException(e);
            }
#endif
        }
        // =============================| Drawer Interface |===================================
        // These methods make the process of drawing menus, pages, listings much easier.

#region Drawer Methods



        /// <summary>
        /// Method draws text as pages and user can move around using [<] and [>]
        /// </summary>
        /// <param name="data"> text to output </param>
        /// <param name="title"> title of the window </param>
        /// <param name="wait"> wait for key press </param>
        public static void DrawWindowWithPages(string[] data, string title = null, bool wait = true)
        {
            title = title ?? Drawer.ELanguage.Menu();

            var currentActive = 0;
            var pagedData = new List<string>(data);
            var maxPages = (int)(pagedData.Count / Drawer.PageSize);

            while (true)
            {
                var sizes = CalcRange(pagedData, (int)Drawer.PageSize, currentActive);
                Drawer.DrawInfoView(
                    title: title,
                    lines: pagedData.GetRange(sizes.Item1, sizes.Item2).ToArray(),
                    addInfo: Drawer.ELanguage.Pages(currentActive + 1, maxPages + 1),
                    wait: false
                    );

                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        LeftPage(ref currentActive, maxPages);
                        break;
                    case ConsoleKey.RightArrow:
                        RightPage(ref currentActive, maxPages);
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }

        }

        /// <summary>
        /// Method calculates current offset and number of strings on current page
        /// </summary>
        /// <param name="pagedData"> text to output </param>
        /// <param name="pageSize"> total number of pages </param>
        /// <param name="currentActive"> current active page </param>
        /// <returns> index of the first string and number of string after it to output </returns>
        private static Tuple<int, int> CalcRange(List<string> pagedData, int pageSize, int currentActive)
        {
            int index = (int)(currentActive * pageSize);
            int size = 0;
            if ((currentActive + 1) * pageSize > pagedData.Count)
                size = pagedData.Count - index;
            else
                size = pageSize;

            return new Tuple<int, int>(index, size);
        }

        /// <summary>
        /// Method turns the page back
        /// </summary>
        /// <param name="currentActive"> current page </param>
        /// <param name="maxPages"> total number of pages </param>
        private static void LeftPage(ref int currentActive, int maxPages)
        {
            if (currentActive - 1 < 0)
                currentActive = maxPages;
            else
                currentActive--;
        }

        /// <summary>
        /// Method turns the page front
        /// </summary>
        /// <param name="currentActive"> current page </param>
        /// <param name="maxPages">total number of pages </param>
        private static void RightPage(ref int currentActive, int maxPages)
        {
            if (currentActive + 1 > maxPages)
                currentActive = 0;
            else
                currentActive++;
        }


        public static void DrawIntro(string[] lines)
        {
            Drawer.DrawIntro(lines);
        }
        /// <summary>
        /// Method draws basic window
        /// </summary>
        /// <param name="data"> text to output </param>
        /// <param name="title"> title of the window</param>
        /// <param name="wait"> wait for key press </param>
        public static void DrawWindow(string[] data, string title = null, bool wait = true)
        {
            if (title == null)
                title = Drawer.ELanguage.Menu();
            Drawer.DrawInfoView(title, data, wait);
        }

        [Obsolete("This method is not used anymore")]
        public static void DrawListing(string[] listing, string title = null, bool wait = true)
        {
            if (title != null)
                Drawer.DrawListingView(title, listing, wait);
        }

        /// <summary>
        /// Method draws info window with text and error title
        /// </summary>
        /// <param name="data"> text to output </param>
        public static void DrawErrorData(string[] data)
        {
            Drawer.DrawInfoView(Drawer.ELanguage.Error(), data);
        }

        public enum WindowStatus
        {
            Yes,
            No,
            Abort
        }
        /// <summary>
        /// Method prints Yes or No question without third option
        /// </summary>
        /// <param name="data"> List of 2 values(strings) </param>
        /// <param name="additionalData"> left menu data </param>
        /// <returns> if 1 value is chosen - true </returns>
        public static WindowStatus YesNoWindow(string[] data, string[] additionalData = null)
        {
            string[] ids = { "yes", "no" };
            var menu = GenerateIDsForMenuItems(ids, data);

            string[] additionalInfo = null;
            if (additionalData != null)
                additionalInfo = additionalData;

            while (true)
            {
                switch (Engine.Menu(menu, Drawer.ELanguage.Choose(), additionalInfo))
                {
                    case "yes":
                        return WindowStatus.Yes;
                    case "no":
                        return WindowStatus.No;
                    case null:
                        break;
                }
            }
        }

        /// <summary>
        /// Method that calls exit info
        /// </summary>
        public static void Exit()
        {
            Drawer.Exit();
        }
#endregion

        //============================| Methods that are needed for output and input control |=============================
#region Menu

        /// <summary>
        /// Method creates menu with Items and allows user to move around menu using ^ v and choose Item.
        /// </summary>
        /// <param name="data"> Dictionary of ids and names of the Items.
        ///                                     { "o", "Open"}
        ///                     If user chooses "Open" method will return "o"
        /// </param>
        /// <param name="title"> Title of the menu </param>
        /// <param name="additionalInfo"> Additional info, that will be placed on the left of the screen </param>
        /// <returns> id of the chosen Item. or null if [Esc] if pressed </returns>
        public static string Menu(Dictionary<string, string> data, string title = "", string[] additionalInfo = null)
        {
            var menuList = GenerateMenuListFromInput(data);
            var currentActive = 0;

            while (true)
            {
                Drawer.DrawMenuView(title, menuList, additionalInfo);
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        DecreaseIterator(ref menuList, ref currentActive);
                        break;
                    case ConsoleKey.DownArrow:
                        IncreaseIterator(ref menuList, ref currentActive);
                        break;
                    case ConsoleKey.Enter:
                        return data[menuList[currentActive].Content];
                    case ConsoleKey.Escape:
                        return null;
                }
            }
        }

        /// <summary>
        /// Menu that can be scrolled.
        /// </summary>
        /// <param name="data"> buttons to output </param>
        /// <param name="numberOfItems"> number of buttons per page </param>
        /// <param name="title">title of the menu </param>
        /// <param name="additionalInfo"> left menu data if needed </param>
        /// <returns> chosen button or null </returns>
        public static string ScrollableMenu(Dictionary<string, string> data, uint numberOfItems, string title = "",
            string[] additionalInfo = null)
        {

            var menuList = GenerateMenuListFromInput(data);
            var currentActive = 0;

            while (true)
            {
                var list = GenerateListWithOffset(menuList, currentActive, (int)numberOfItems);
                Drawer.DrawMenuView(title, list, additionalInfo);
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        DecreaseIterator(ref menuList, ref currentActive);
                        break;
                    case ConsoleKey.DownArrow:
                        IncreaseIterator(ref menuList, ref currentActive);
                        break;
                    case ConsoleKey.Enter:
                        return data[menuList[currentActive].Content];
                    case ConsoleKey.Escape:
                        return null;
                }
            }
        }


        /// <summary>
        /// Menu that has scrollable left sub menu.
        /// </summary>
        /// <param name="data">buttons to output </param>
        /// <param name="title"> title of the menu </param>
        /// <param name="pagedData">data for left menu </param>
        /// <param name="headerInfo"> data for header of the sun menu </param>
        /// <returns></returns>
        public static string MenuWithScrollableSideMenu(Dictionary<string, string> data, string title,
            List<string> pagedData = null, List<string> headerInfo = null)
        {
            var menuList = GenerateMenuListFromInput(data);
            var currentMenuItem = 0;
            var currentSubPage = 0;
            var maxPages = (int)(pagedData.Count / Drawer.MenuListingSize);

            if (pagedData.Count % Drawer.MenuListingSize == 0)
                maxPages--;

            while (true)
            {
                var (item1, item2) = CalcRange(pagedData, (int)Drawer.MenuListingSize, currentSubPage);
                var additionalData = pagedData.GetRange(item1, item2);
                if (additionalData.Count == 0)
                    additionalData.Add(" ");
                Drawer.DrawMenuView(
                    title,
                    menuList,
                    additionalData,
                    headerInfo
                    );

                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        DecreaseIterator(ref menuList, ref currentMenuItem);
                        break;
                    case ConsoleKey.DownArrow:
                        IncreaseIterator(ref menuList, ref currentMenuItem);
                        break;
                    case ConsoleKey.LeftArrow:
                        LeftPage(ref currentSubPage, maxPages);
                        break;
                    case ConsoleKey.RightArrow:
                        RightPage(ref currentSubPage, maxPages);
                        break;
                    case ConsoleKey.Enter:
                        return data[menuList[currentMenuItem].Content];
                    case ConsoleKey.Escape:
                        return null;

                }
            }
        }

        /// <summary>
        /// Method generates menu List from data and sets the first Item to selected.
        /// </summary>
        /// <param name="data"> data to transform to List </param>
        /// <returns>List of Items </returns>
        public static Dictionary<string, string> GenerateIDsForMenuItems(string[] ids, string[] menuItems)
        {
            Dictionary<string, string> retDict = new Dictionary<string, string>();
            if (menuItems.Length != ids.Length)
                throw new ArgumentException($"Number of ids {ids.Length} does not match number of menuItems {menuItems.Length}");

            for (int i = 0; i < ids.Length; i++)
                retDict.Add(menuItems[i], ids[i]);
            return retDict;
        }

        /// <summary>
        /// Method generates Items from input array
        /// </summary>
        /// <param name="data"> dict of {id, item.Content} </param>
        /// <returns> Array of items </returns>
        private static List<Item> GenerateMenuListFromInput(Dictionary<string, string> data)
        {
            var ret = data.Select(item => new Item(item.Key)).ToList();
            if (ret.Count > 0)
                ret[0].SetActive();
            return ret;
        }

        /// <summary>
        /// Method that allows scroll for menu. Changes current "page" that is calculated with modulus.
        /// Page of size 5: 0 1 2 3 4, if currentActive is at 5 - new page will be loaded.
        /// </summary>
        /// <param name="menuList"> menu buttons </param>
        /// <param name="currentActive"> current selected button </param>
        /// <param name="numberOfItems"> number of buttons per page </param>
        /// <returns></returns>
        private static List<Item> GenerateListWithOffset(List<Item> menuList, int currentActive, int numberOfItems)
        {
            // If current item is within the page( page of size 5 can be represented as 0 1 2 3 4) then we don't switch.
            // If current item is out of current page (0 1 2 3 4 {here}) then we load next page.
            var closest = currentActive % numberOfItems == 0 ? currentActive : currentActive - (currentActive % numberOfItems);

            // If the last page is not divisible without remainder, we leave extra page for reminder.
            if (closest + numberOfItems > menuList.Count)
                return menuList.GetRange(closest, menuList.Count - closest);

            return menuList.GetRange(closest, numberOfItems);
        }


        /// <summary>
        ///     Method increases currentItem iterator.
        ///     Used to move v
        ///     Also provides out of range check.
        /// </summary>
        /// <param name="items"> dirs and files </param>
        /// <param name="currentActive"> iterator over items </param>
        private static void IncreaseIterator(ref List<Item> items, ref int currentActive)
        {
            items.Find(item => item.Active)?.SetInactive();
            if (currentActive >= items.Count - 1)
                currentActive = 0;
            else
                currentActive++;
            items[currentActive].SetActive();
        }

        /// <summary>
        ///     Method decreases currentItem iterator.
        ///     Used to move ^
        ///     Also provides out of range check.
        /// </summary>
        /// <param name="items"> dirs and files </param>
        /// <param name="currentActive"> iterator over items </param>
        private static void DecreaseIterator(ref List<Item> items, ref int currentActive)
        {
            items.Find(item => item.Active)?.SetInactive();
            if (currentActive - 1 < 0)
                currentActive = items.Count - 1;
            else
                currentActive--;

            items[currentActive].SetActive();
        }
#endregion

        [Obsolete("Can be revived")]
        public static bool KeyPressed(string context, ConsoleKey keyToCheck)
        {
            DrawWindow(new[] { context }, Drawer.ELanguage.KeyPress());
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == keyToCheck)
                return true;
            return false;
        }

        //====================| General methods that are needed for calculations |========================

        /// <summary>
        /// Method reads string from console with given context.
        /// </summary>
        /// <param name="context"> string to output for user </param>
        /// <param name="interrupted"> if user pressed [Esc] becomes true </param>
        /// <returns> string from user or null of interrupted </returns>
        public static string GetStringWithContext(string context, ref bool interrupted, bool checkForEmpty = false)
        {
            // TODO: make method return bool and set string as ref or out, so it would use less space in program
            Drawer.DrawInfoView("", new[] { context }, false);
            string retStr = Console.ReadLine();

            if (checkForEmpty)
            {
                while (retStr == "\n" || string.IsNullOrEmpty(retStr))
                {
                    Drawer.DrawInfoView(Drawer.ELanguage.Error(), Drawer.ELanguage.RerunMessage(), false);
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                    {
                        interrupted = true;
                        return null;
                    }
                    Console.Clear();
                    Drawer.DrawInfoView("", new[] { context }, false);
                    retStr = Console.ReadLine();
                }

                return retStr;
            }

            //if (checkForEmpty && (retStr == "\n" || string.IsNullOrEmpty(retStr)))
            //{
            //    Drawer.DrawInfoView(Drawer.ELanguage.Error(), Drawer.ELanguage.RerunMessage(), false);
            //    var key = Console.ReadKey(true);
            //    if (key.Key == ConsoleKey.Escape)
            //    {
            //        interrupted = true;
            //        return null;
            //    }
            //    retStr = Console.ReadLine();
            //}

            return retStr;
        }

        /// <summary>
        /// Method reads number from console with given context.
        /// </summary>
        /// <typeparam name="T"> any number type </typeparam>
        /// <param name="context"> string to output for user </param>
        /// <param name="interrupted"> if [Esc] pressed - return sets true </param>
        /// <param name="numberPredicate"> basic predicate to check </param>
        /// <param name="y"> if needed extra check - should be set </param>
        /// <param name="extraPredicate"> extra predicate that checks <= >= for given y </param>
        /// <returns></returns>
        public static T GetNumberWithContext<T>(string context, ref bool interrupted,
            Predicate<T> numberPredicate = null, T y = default(T), Func<T, T, bool> extraPredicate = null)
        {
            T returnNumber;

            Drawer.DrawInfoView("", new[] { context }, false);

            string numStr = Console.ReadLine();

            while (!TryParse(numStr, out returnNumber) || !PredicateCheck<T>(numberPredicate, returnNumber, y, extraPredicate))
            {
                Drawer.DrawInfoView(Drawer.ELanguage.Error(), Drawer.ELanguage.RerunMessage(), false);
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    interrupted = true;
                    return default(T);
                }
                Drawer.DrawInfoView("", new[] { context }, false);
                numStr = Console.ReadLine();

            }
            return returnNumber;
        }

        /// <summary>
        /// Special method that is used for GetNumberWithContext only
        /// to check if given data can be parsed by C#.
        /// </summary>
        /// <typeparam name="T"> any type, but Engine uses Int and Double only </typeparam>
        /// <param name="data"> data given by user </param>
        /// <param name="el"> if parse if successful - sets it to parsed value
        ///                   if not successful - sets it to default value </param>
        /// <returns></returns>
        private static bool TryParse<T>(string data, out T el)
        {
            el = default;
            try
            {
                el = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(data);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Method checks if given x is valid for chosen predicates
        /// </summary>
        /// <typeparam name="T"> type of the x and y </typeparam>
        /// <param name="predicate"> basic predicate - for example checks if x is greater then zero</param>
        /// <param name="x"> given value </param>
        /// <param name="y"> additional value </param>
        /// <param name="extraPredicate"> checks x for y </param>
        /// <returns> true if check is successful </returns>
        private static bool PredicateCheck<T>(Predicate<T> predicate, T x, T y = default(T), Func<T, T, bool> extraPredicate = null)
        {
            bool check = true;
            if (predicate != null)
                check = predicate(x);
            if (extraPredicate != null)
                check = check && extraPredicate(x, y);

            return check;
        }

        //=========================| Predicate definitions |================================
        // Predicates are small functions that help Engine to check if given x is valid.
        // If I want to check if given X is greater than zero - I'll use PositiveNonZero.

#region Predicates



        public static bool PositiveNonZero(int x) => x > 0;
        public static bool Positive(int x) => x >= 0;
        public static bool NegativeNonZero(int x) => x < 0;
        public static bool Negative(int x) => x <= 0;

        public static bool NotBiggerThan(int x, int y) => x <= y;
        public static bool NotSmallerThan(int x, int y) => x >= y;

        public static bool ModulusIn(int x, int y) => x <= y && x >= -y;
        public static bool ModulusOut(int x, int y) => x >= y || x <= -y;
        public static bool DModulusIn(double x, double y) => x <= y && x >= -y;
        public static bool DModulusOut(double x, double y) => x >= y || x <= -y;

#endregion



        // ========================| Exception Workflow |==================
        // Custom exceptions.
        public static void HandleException(Exception e)
        {
            Drawer.DrawError(e);
        }
    }
}