using System;
using System.IO;

namespace FileManager3._1
{
    class Program
    {

        static void Main(string[] args)
        {
            double weghtBefore = 0;
            double weghtAfter = 0;
            EnterAdress();
            weghtBefore = FileWeight(Datas.Adress, ref weghtBefore);
            FileManager.GetFiles();
            weghtAfter = FileWeight(Datas.Adress, ref weghtAfter);
            Console.WriteLine("Размер директории до очистки {0} MB.", weghtBefore);
            Console.WriteLine("Размер директории после очистки {0} MB", weghtAfter);
            Console.WriteLine("Очищено {0} MB", weghtBefore - weghtAfter);
        }
        static void EnterAdress()
        {
            Console.WriteLine("Введите директорию размер которой вы хотите знать");
            Datas.Adress = Console.ReadLine();
            if (AdressCheck(Datas.Adress))
            {
                Console.WriteLine("Адресс указан корректно, ваша папка: " + Datas.Adress);
            }
            else EnterAdress();
        }

        static bool AdressCheck(string adress)
        {
            if (Directory.Exists(adress))
                return true;
            else return false;
        }

        static double FileWeight(string adress, ref double weight)
        {
            try
            {
                DirectoryInfo dInfo = new DirectoryInfo(adress);
                DirectoryInfo[] dIinfoAdress = dInfo.GetDirectories();
                FileInfo[] fInfo = dInfo.GetFiles();

                foreach (FileInfo f in fInfo)
                    weight += f.Length;

                foreach (DirectoryInfo df in dIinfoAdress)
                    FileWeight(df.FullName, ref weight);

                return Math.Round((double)(weight / 1024 / 1024), 1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }
    }

    public class Datas
    {
        private static string adress;

        public static string Adress
        {
            get { return adress; }
            set { adress = value; }
        }
    }

    class FileManager
    {
        public static int howMuchDeletedFiles = 0;
        public static void GetFiles()
        {
            DirectoryInfo dirinfo = new DirectoryInfo(Datas.Adress);
            try
            {
                if (dirinfo.GetDirectories().Length != 0)
                {
                    string[] dirs = Directory.GetDirectories(Datas.Adress);
                    for (int i = 0; i < dirs.Length; i++)
                    {
                        var updateTime = File.GetLastWriteTime(dirs[i]);
                        var currentTime = DateTime.Now;
                        if (currentTime - updateTime > TimeSpan.FromMinutes(30))
                        {
                            Console.WriteLine("Файл с именем: {0} удален", dirs[i]);
                            Directory.Delete(dirs[i]);
                            howMuchDeletedFiles++;
                        }
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            try
            {
                if (dirinfo.GetFiles().Length != 0)
                {
                    string[] files = Directory.GetFiles(Datas.Adress);
                    for (int i = 0; i < files.Length; i++)
                    {
                        var updateTime = File.GetLastWriteTime(files[i]);
                        var currentTime = DateTime.Now;
                        if (currentTime - updateTime > TimeSpan.FromMinutes(30))
                        {
                            Console.WriteLine("Файл с именем: {0} удален", files[i]);
                            File.Delete(files[i]);
                            howMuchDeletedFiles++;
                        }
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

            Console.WriteLine("Процедура очистки завершена, всего удалено: {0} файлов", howMuchDeletedFiles);
        }
    }
}
