namespace EnigMouseSendMaster
{
    /// <summary>
    /// ファイルの参照を管理
    /// </summary>
    public class FilePath
    {
        public static string assetsRelativePath = @"assets";

        public static string assetsPath = null;

        public static string modelFilePath = null;

        //絶対パスを取得する
        static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }
        static FilePath()
        {
            assetsPath = GetAbsolutePath(assetsRelativePath);
        }
    }
}