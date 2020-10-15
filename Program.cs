using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace _1lab {
    class Program {

        static string CurrentDirectory = $"{Directory.GetCurrentDirectory ()}";

        static void AddFolder (string path) {
            if (Directory.Exists ($"{CurrentDirectory}/{path}")) {
                throw new Exception ("Directory has already been created");
            } else
                Directory.CreateDirectory ($"{CurrentDirectory}/{path}");
        }

        static void RemoveFolder (string path) {
            if (!Directory.Exists ($"{CurrentDirectory}/{path}")) {
                throw new Exception ("Directory doesn't exist");
            } else
                Directory.Delete ($"{CurrentDirectory}/{path}");
        }

        static List<string> GetAllFiles () {
            return Directory.GetDirectories (CurrentDirectory).ToList ().Concat (Directory.GetFiles (CurrentDirectory, "*", SearchOption.TopDirectoryOnly).ToList ()).Select (path => path.Split ('/') [path.Split ('/').Length - 1]).ToList ();
        }

        static void RenameFile (string oldFile, string newFile) {
            if (File.Exists ($"{CurrentDirectory}/{oldFile}")) {

                File.Move ($"{CurrentDirectory}/{oldFile}", $"{CurrentDirectory}/{newFile}");
            } else {
                throw new Exception ("File doesn't exist");
            }
        }

        static void MoveFile (string fileName, string newDest) {
            if (File.Exists ($"{CurrentDirectory}/{fileName}")) {
                if (Directory.Exists ($"{CurrentDirectory}/{newDest}")) {
                    File.Move ($"{CurrentDirectory}/{fileName}", $"{CurrentDirectory}/{newDest}/{fileName}");
                } else throw new Exception ("Directory doesnt exist");
            } else throw new Exception ("File doesnt exist");
        }

        static void RenameFolder (string oldName, string newName) {
            if (Directory.Exists ($"{CurrentDirectory}/{oldName}")) {
                Directory.Move ($"{CurrentDirectory}/{oldName}", $"{CurrentDirectory}/{newName}");
            } else throw new Exception ("Folder doesn't exist");
        }

        static void WriteToFile (string path, List<string> _text) {
            if (File.Exists ($"{CurrentDirectory}/{path}")) {
                string Text = String.Join (" ", _text.ToArray ());
                using (StreamWriter sw = new StreamWriter ($"{CurrentDirectory}/{path}", false, System.Text.Encoding.Default)) {
                    sw.WriteLine (Text);
                }
            } else throw new Exception ("File doesnt exist");
        }

        static void ReadFromFile (string path, ref List<string> _list) {
            if (File.Exists ($"{CurrentDirectory}/{path}")) {
                string text = File.ReadAllText ($"{CurrentDirectory}/{path}");
                _list = text.Split (" ").ToList ();
            } else throw new Exception ("File doesnt exist");
        }

        static void PrintList<T> (List<T> _list) {
            foreach (T t in _list) {
                Console.WriteLine (t);
            }
        }

        static List<string> FillList () {
            string text = Console.ReadLine ();
            return text.Split (" ").ToList ();
        }

        static void ChangeDirectory (string newDest) {
            if (Directory.Exists ($"{CurrentDirectory}/{newDest}"))
                CurrentDirectory += $"/{newDest}";
            else throw new Exception ("Directory doesnt exist");
        }

        static void CopyFile (string fileName, string destination) {
            if (File.Exists ($"{CurrentDirectory}/{fileName}")) {
                if (Directory.Exists ($"{CurrentDirectory}/{destination}")) {
                    FileInfo fileInfo = new FileInfo ($"{CurrentDirectory}/{fileName}");
                    fileInfo.CopyTo ($"{CurrentDirectory}/{destination}/{fileName}");
                } else throw new Exception ("Directory doesnt exist");
            } else throw new Exception ("File doesnt exist");
        }

        static string GetCurrentDirectory () {
            return CurrentDirectory;
        }

        static void SuccessMessage (string text) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine ($"Custom@terminal~{PutCurrentDirectoryCut()}: {text}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static void ErrorMessage (string text) {
            Console.ForegroundColor = ConsoleColor.DarkRed;

            Console.WriteLine ($"Custom@terminal~{PutCurrentDirectoryCut()}: {text}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static string PutCurrentDirectoryCut () {
            return String.Join ("/", CurrentDirectory.Split ("/").Reverse ().Take (2).Reverse ());
        }

        static void BWrite (string fileName, List<string> _list) {
            string path = @$"{CurrentDirectory}/{fileName}";
            if (File.Exists (path)) {
                using (BinaryWriter binWriter =
                    new BinaryWriter (File.Open (path, FileMode.Create))) {
                    binWriter.Write (String.Join (" ", _list));
                }
            } else throw new Exception ("File doesnt exist");
        }

        static void BRead (string fileName, ref List<string> _list) {
            string path = $"{CurrentDirectory}/{fileName}";
            if (File.Exists (path)) {
                using (BinaryReader reader = new BinaryReader (File.Open (path, FileMode.Open))) {
                    while (reader.PeekChar () > -1) {
                        _list.Add (reader.ReadString ());
                    }
                }
            } else throw new Exception ("File doesnt exist");
        }

        static void CreateFile (string fileName) {
            string path = $"{CurrentDirectory}/{fileName}";
            if (!File.Exists (path)) {
                File.Create (path);
            } else throw new Exception ("File is already created");
        }

        static void DeleteFile (string fileName) {
            string path = $"{CurrentDirectory}/{fileName}";
            if (File.Exists (path)) {
                File.Delete (path);
            } else throw new Exception ("File doesnt exist");
        }

        public static void Compress (string sourceFile) {

            string path = @$"{CurrentDirectory}/{sourceFile}";
            string dest = @$"{CurrentDirectory}/{sourceFile.Split('.')[0]}.gz";
            if (File.Exists (path)) {
                using (FileStream sourceStream = new FileStream (path, FileMode.OpenOrCreate)) {
                    using (FileStream targetStream = File.Create (dest)) {
                        using (GZipStream compressionStream = new GZipStream (targetStream, CompressionMode.Compress)) {
                            sourceStream.CopyTo (compressionStream);
                        }
                    }
                }
            } else throw new Exception ("File doesnt exitst");
        }

        public static void Decompress (string compressedFile, string targetFile) {
            string path = $"{CurrentDirectory}/{compressedFile}";
            string dest = $"{CurrentDirectory}/{targetFile}";
            if (File.Exists (path)) {
                using (FileStream sourceStream = new FileStream (path, FileMode.OpenOrCreate)) {
                    // поток для записи восстановленного файла
                    using (FileStream targetStream = File.Create (dest)) {
                        // поток разархивации
                        using (GZipStream decompressionStream = new GZipStream (sourceStream, CompressionMode.Decompress)) {
                            decompressionStream.CopyTo (targetStream);
                        }
                    }
                }
            } else throw new Exception ("File doesnt exist");
        }

        static void Main (string[] args) {

            List<string> _list = new List<string> ();
            // Commands: pwd, cd, rm, mkdir, read, write, ls, cp, showList, >, gzip, ungzip
            Console.Write ($"Custom@terminal~{PutCurrentDirectoryCut()}: ");
            string command = Console.ReadLine ();

            while (command != "quit") {
                if (command.Split (" ") [0] == "cp") {
                    try {
                        CopyFile (command.Split (" ") [1], command.Split (" ") [2]);
                    } catch (Exception e) {
                        ErrorMessage (e.Message);
                        throw;
                    }
                }
                if (command.Split (" ") [0] == "cd") {
                    try {

                        ChangeDirectory (command.Split (" ") [1]);
                        SuccessMessage ($"Succesfully, current directory is {GetCurrentDirectory ()} ");
                    } catch (Exception e) {
                        ErrorMessage (e.Message);
                    }
                }

                if (command.Split (" ") [0] == "pwd") {
                    SuccessMessage ($"Current directory is {GetCurrentDirectory ()}");
                }

                if (command.Split (" ") [0] == "mkdir") {
                    try {
                        AddFolder (command.Split (" ") [1]);
                        SuccessMessage ($"Directory /{command.Split (" ") [1]} has been created succesfully");
                    } catch (Exception e) {
                        ErrorMessage (e.Message);
                    }
                }

                if (command.Split (" ") [0] == "rm-rf") {
                    try {
                        RemoveFolder (command.Split (" ") [1]);
                        SuccessMessage ($"Folder {command.Split (' ') [1]} has been removed");
                    } catch (Exception e) {

                        ErrorMessage (e.Message);
                    }
                }

                if (command.Split (" ") [0] == "ls") {
                    GetAllFiles ().ForEach (dir => {
                        Console.WriteLine (dir);
                    });
                }

                if (command.Split (" ") [0] == ">") {
                    try {
                        CreateFile (command.Split (" ") [1]);
                        SuccessMessage ($"File {command.Split(' ')[1]} has been created succesfully");
                    } catch (Exception e) {
                        ErrorMessage (e.Message);
                    }
                }

                if (command.Split (" ") [0] == "rm") {
                    try {
                        DeleteFile (command.Split (" ") [1]);
                        SuccessMessage ($"File {command.Split(' ')[1]} has been deleted");
                    } catch (Exception e) {
                        ErrorMessage (e.Message);
                    }
                }

                if (command.Split (" ") [0] == "binRead") {
                    try {
                        BRead (command.Split (" ") [1], ref _list);
                        SuccessMessage ("bin Info has been read from the file");
                    } catch (Exception e) {
                        ErrorMessage (e.Message);
                    }
                }

                if (command.Split (" ") [0] == "binWrite") {
                    try {
                        BWrite (command.Split (" ") [1], _list);
                        SuccessMessage ("bin Info has been written into the file");
                    } catch (Exception e) {

                        ErrorMessage (e.Message);
                    }
                }

                if (command.Split (" ") [0] == "showList") {
                    PrintList (_list);
                }

                if (command.Split (" ") [0] == "read") {
                    try {
                        ReadFromFile (command.Split (" ") [1], ref _list);
                        SuccessMessage ("Info has been read from the file");

                    } catch (Exception e) {

                        ErrorMessage (e.Message);
                    }
                }

                if (command.Split (" ") [0] == "write") {
                    try {
                        WriteToFile (command.Split (" ") [1], _list);
                        SuccessMessage ("Info has been written into the file");

                    } catch (Exception e) {

                        ErrorMessage (e.Message);
                    }
                }

                if (command.Split (" ") [0] == "gzip") {
                    try {
                        Compress (command.Split (" ") [1]);
                        SuccessMessage ($"File {command.Split(" ")[1]} has been successfully zipped");
                    } catch (Exception e) {
                        ErrorMessage (e.Message);
                    }
                }
                try {
                    if (command.Split (" ") [0] == "ungzip") {
                        try {
                            if (command.Split (" ").Length < 3) {
                                throw new Exception ("Not enough params, need 3");
                            } else {
                                Decompress (command.Split (" ") [1], command.Split (" ") [2]);

                                SuccessMessage ($"File {command.Split(" ")[1]} has been successfully restored");
                            }

                        } catch (Exception e) {
                            ErrorMessage (e.Message);
                        }
                    }
                } catch (Exception e) {
                    ErrorMessage (e.Message);
                }
                try {
                    if (command.Split (" ") [0] == "rename") {
                        try {
                            if (command.Split (" ").Length < 3) {
                                throw new Exception ("Not enough params, need 3");
                            } else {
                                RenameFile (command.Split (" ") [1], command.Split (" ") [2]);

                                SuccessMessage ($"File {command.Split(" ")[1]} has been renamedx");
                            }

                        } catch (Exception e) {
                            ErrorMessage (e.Message);
                        }
                    }
                } catch (Exception e) {
                    ErrorMessage (e.Message);
                }
                Console.Write ($"Custom@terminal~{PutCurrentDirectoryCut()}: ");
                command = Console.ReadLine ();
            }
        }
    }
}