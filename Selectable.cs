using System;

namespace ConsoleEngine
{
    /// <summary>
    /// This interface is needed for any selectable GUI element.
    /// It can be Buttons, Items of list, Menus.
    /// </summary>
    /// TODO: Add function that allows only one element to be Active.
    interface ISelectable
    {
        void SetActive();
        void SetInactive();

        string ColorContext();
    }

    /// <summary>
    /// Class used for listing dirs and files and allows to create pointer * that shows currently chosen Item.
    /// </summary>
    public class Item : ISelectable
    {
        public bool Active { get; private set; }

        public bool IsFile { get; set; }

        //public bool Active { get; set; }
        public string Content { get; }


        public Item(string content, bool isFile = false, bool active = false)
        {
            Content = content;
            //Content = content;
            Active = active;
            IsFile = isFile;
        }

        public void SetActive()
        {
            Active = true;
        }

        public void SetInactive()
        {
            Active = false;
        }

        public string ColorContext()
        {
#if WINDOWS
            return $"\u001b[7m{Content}\u001b[0m";
#else
            return Content;
#endif
        }
    }
}