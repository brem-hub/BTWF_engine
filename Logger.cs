using System;
using System.IO;

namespace ConsoleEngine
{
    class Logger
    {

        private const string Separator = "============================================================\n";

        private static Logger _instance;
        private string _errorPrefix = "[!]";
        private string _warningPrefix = "[.]";
        private string _infoPrefix = "[-]";
        private string _logPath;
        private StreamWriter _sw;
        private Logger()
        { }

        /// <summary>
        /// Method starts logging into file and file should be specified before
        ///     by property LogPath
        /// </summary>
        /// <exception cref="WrongPathException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="IOException"></exception>
        public void Start()
        {
            if (!IsValidPath(_logPath))
                throw new WrongPathException(_logPath);

            _sw = new StreamWriter(_logPath) { AutoFlush = true };

            _sw.Write($"[!][!][!] Logging started at {DateTime.Now} [!][!][!]\n");
            _sw.Write(Separator);
        }

        /// <summary>
        /// Method finishes logging into file and cleans the logger
        /// </summary>
        public void Close()
        {
            _sw?.Write(Separator);
            _sw?.Write($"[!][!][!] Logging is closed at {DateTime.Now} [!][!][!]\n");

            _sw?.Close();
            _instance = null;
            _logPath = null;
            _sw = null;
            _errorPrefix = "[!]";
            _warningPrefix = "[.]";
            _infoPrefix = "[-]";
        }

        /// <summary>
        /// Method writes separator to logfile
        /// </summary>
        /// <param name="data"> extra data to write down before separator </param>
        public void LogSep(string data = null)
        {
            if (data != null)
                _sw?.Write(data + "\n");
            _sw?.Write(Separator);
        }
        
        /// <summary>
        /// Method logs data without any prefixes
        /// </summary>
        /// <param name="data"></param>
        public void LogNoPrefix(string data)
        {
            _sw?.Write(data + "\n");
        }
        
        /// <summary>
        /// Method logs data with warning prefix
        /// </summary>
        /// <param name="data"></param>
        public void LogWarning(string data)
        {
            _sw?.Write($"{_warningPrefix} {data}\n");
        }
        
        /// <summary>
        /// Method logs data with error prefix
        /// </summary>
        /// <param name="data"></param>
        public void LogError(string data)
        {
            _sw?.Write($"{_errorPrefix} {data}\n");
        }
        
        /// <summary>
        /// Method logs data with basic prefix
        /// </summary>
        /// <param name="data"></param>
        public void Log(string data)
        {
            _sw?.Write($"{_infoPrefix} {data}\n");
        }
        
        public string LogPath
        {
            get => _logPath;
            set => _logPath = value;
        }


        public string ErrorPrefix
        {
            set => _errorPrefix = value;
        }

        public string WarningPrefix
        {
            set => _warningPrefix = value;
        }

        public string InfoPrefix
        {
            set => _infoPrefix = value;
        }

        /// <summary>
        /// Singleton implementation
        /// </summary>
        /// <returns></returns>
        public static Logger GetInstance() => _instance = _instance ?? new Logger();

        /// <summary>
        /// Method checks if path is valid
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsValidPath(string path)
        {
            string result;
            return TryGetFullPath(path, out result);
        }

        /// <summary>
        /// Method checks if path is valid
        /// </summary>
        /// <param name="path"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private static bool TryGetFullPath(string path, out string result)
        {
            result = string.Empty;
            if (string.IsNullOrWhiteSpace(path)) { return false; }
            var status = false;
            try
            {
                result = Path.GetFullPath(path);
                status = true;
            }
            catch (Exception e)
            {
                // ignored
            }

            return status;
        }

        // ===================| Exceptions |====================
        public class WrongPathException : Exception
        {
            public WrongPathException()
            {
            }

            public WrongPathException(string message)
                : base(message)
            {
            }

            public WrongPathException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }


    }
}
