using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using System.IO.Compression;
using System.Reflection;

///   SharpCompress: https://sharpcompress.codeplex.com/
using SharpCompress;
using SharpCompress.Reader;
using SharpCompress.Common;

namespace Movinarc
{
    public class PUManager : EditorWindow
    {
        Vector2 scrollPos = Vector2.zero;
        Texture2D putitle = null;
        Texture2D puicon = null;
        Texture2D openicon = null;
        string customPackage = string.Empty;
        GUIStyle horAlignStyle = null;
        GUIStyle fontStyle = null;
        struct Package
        {
            public string fullPath;
            public string fileName;
        }
        static List<Package> packages = null;
        [MenuItem("Assets/Uninstall Package...")]
        static void CreateWindow()
        {
            packages = new List<Package>();
            var win = EditorWindow.GetWindow(typeof(PUManager));
            win.minSize = new Vector2(410, 450);
            win.title = "Uninstall Package";
#if UNITY_EDITOR_WIN
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = Path.Combine(path, "Unity");
            var files = Directory.GetFiles(path, "*.unitypackage", SearchOption.AllDirectories);
            packages = new List<Package>();
            foreach (var file in files)
            {
                packages.Add(new Package() { fileName = Path.GetFileNameWithoutExtension(file), fullPath = file });
            }
#elif UNITY_EDITOR_OSX
		var path = System.Environment.GetFolderPath (Environment.SpecialFolder.Personal) + "/Library/Unity";
		var info = new DirectoryInfo (path);
		foreach (var file in info.GetFiles("*.unitypackage",SearchOption.AllDirectories)) {
			packages.Add (new Package () { fileName = Path.GetFileNameWithoutExtension(file.FullName), fullPath = file.FullName });
		}
#endif
            packages = packages.OrderBy(i => i.fileName).ToList();
        }
        public void Start()
        {
            CreateWindow(); 
        }

        public void Awake()
        {
            putitle = Resources.Load("PU_title") as Texture2D;
            puicon = Resources.Load("PU_icon") as Texture2D;
            openicon = Resources.Load("Open_icon") as Texture2D;
            horAlignStyle = new GUIStyle();
            horAlignStyle.padding = new RectOffset(0, 0, 15, 0);
            fontStyle = new GUIStyle();
            fontStyle.fontSize = 8;
        }

        public void OnGUI()
        {

            GUILayout.Box(putitle);
            GUILayout.Space(5.0f);
            GUILayout.BeginHorizontal();
            GUILayout.Box("Select the unity package you want to uninstall ");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("?", GUILayout.ExpandWidth(false)))
            {
                Application.OpenURL("http://movinarc.com/unity-package-uninstaller");
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(5.0f);
            GUILayout.BeginHorizontal();


            if (GUILayout.Button(new GUIContent(openicon, "Open"), GUILayout.ExpandWidth(false))) // Custom Open Button
            {
                var f = EditorUtility.OpenFilePanel("Select the .unitypackage file you are going to remove from project", "", "unitypackage");
                if (System.IO.File.Exists(f))
                {
                    this.customPackage = f;
                }
            }
            if (string.IsNullOrEmpty(customPackage))
                GUI.enabled = false;
            else
                GUI.enabled = true;
            if (GUILayout.Button(new GUIContent(puicon, "Uninstall"), GUILayout.ExpandWidth(false))) // Custom Uninstall Button
            {
                CheckUninstall(this.customPackage);
            }
            GUI.enabled = true;
            if (string.IsNullOrEmpty(customPackage))
                GUILayout.Label("---");
            else
            {
                GUILayout.BeginVertical();

                GUILayout.Label(Path.GetFileNameWithoutExtension(customPackage));

                GUILayout.Label(customPackage, fontStyle);
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10.0f);
            GUILayout.Label("OR", EditorStyles.boldLabel);
            GUILayout.Space(10.0f);
            GUILayout.Box("Choose from list of packages already downloaded from Asset Store ");
            GUILayout.Space(5.0f);
            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(410), GUILayout.MinHeight(300));
            GUILayout.BeginVertical();

            if (packages != null && packages.Count > 0)
            {
                foreach (var item in packages)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(new GUIContent(puicon, "Uninstall"), GUILayout.ExpandWidth(false)))
                    {
                        CheckUninstall(item.fullPath);
                    }
                    GUILayout.Label(item.fileName, this.horAlignStyle, GUILayout.MinWidth(230));
                    GUILayout.EndHorizontal();
                    GUILayout.Space(5.0f);
                }
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        public int ImportToTemp(string file2import, string destination)
        {
            // test absolute
            if (!File.Exists(file2import))
            {
                // test relative
                file2import = Path.Combine(Environment.CurrentDirectory, file2import);
                if (!File.Exists(file2import))
                    throw new FileNotFoundException(file2import);
            }

            if (!file2import.ToLower().EndsWith(".unitypackage"))
                throw new Exception("You must select *.unitypackage files.");

            if (!Directory.Exists(destination))
                Directory.CreateDirectory(destination);

            EditorUtility.DisplayProgressBar("Uninstalling Package", "Initializing...", .25f);
            HolyGZip(file2import, destination);

            EditorUtility.DisplayProgressBar("Uninstalling Package", "Fetching Package Structure...", .5f);
            List<string> fileList = GenerateAssetList(destination);

            int delCnt = RemoveFiles(fileList);

            EditorUtility.DisplayProgressBar("Uninstalling Package", "Finalizing...", 1f);
            if (Directory.Exists(destination))
                RemoveMess(destination);

            EditorUtility.ClearProgressBar();
            return delCnt;
        }

        void CheckUninstall(string customPackage)
        {
            var fileName = System.IO.Path.GetFileNameWithoutExtension(customPackage);
            if (EditorUtility.DisplayDialog("Delete Imported Unitypackage", string.Format("You're going to uninstall the '{0}'. Are you sure you want to delete all the files related to this package?", fileName),
                    "Yes", "No"))
            {
                try
                {
                    if (EditorUtility.DisplayDialog("Delete Imported Unitypackage", string.Format("The operation can not be undone! Are you sure?"), "Yes. Do It!", "No"))
                    {
                        var cnt = UninstallPackage(customPackage);
                        string msg = string.Format("{0} files/folders related to '{1}' deleted from project.", cnt, fileName);
                        if (EditorUtility.DisplayDialog("Package Uninstaller", msg, "Ok"))
                        {
                            Debug.Log(msg);
                            EditorWindow.GetWindow(typeof(PUManager)).Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    EditorUtility.ClearProgressBar();
                    Debug.LogError(ex.Message);
                }
            }
        }
        private int RemoveFiles(List<string> filelist)
        {
            float step = .5f / filelist.Count;
            float progress = .5f + step;
            List<string> dirs = new List<string>();
            int deleted = 0;
            string appPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf(@"Assets"));


            foreach (var f in filelist)
            {

                EditorUtility.DisplayProgressBar("Uninstalling Package", string.Format("Removing {0}", f), progress);
                progress += step;

                string fullPath = Path.Combine(appPath, f);
                GetDirectoryNames(f, dirs);

                try
                {
                    //deleted += (AssetDatabase.DeleteAsset(f) ? 1 : 0);
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                        deleted++;
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);

                }
                finally
                {
                    try
                    {
                        //deleting meta file
                        var meta = fullPath + ".meta";
                        if (File.Exists(meta))
                            File.Delete(meta);
                    }
                    catch { }
                }
            }
            dirs = dirs.OrderByDescending(i => i.Count(x => x == '/')).ToList();
            foreach (var item in dirs)
            {
                var fullpath = Path.Combine(appPath, item);
                try
                {
                    //deleted += (AssetDatabase.DeleteAsset(item) ? 1 : 0);
                    if (Directory.Exists(fullpath))
                    {
                        if (Directory.GetFiles(fullpath).Length <= 0)
                        {
                            Directory.Delete(fullpath);
                            deleted++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                }
                finally
                {
                    try
                    {
                        var meta = fullpath + ".meta";
                        if (File.Exists(meta))
                            File.Delete(meta);
                    }
                    catch { }
                }
            }
            AssetDatabase.Refresh();
            return deleted;
        }

        public int UninstallPackage(string path)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), "PU" + DateTime.Now.Ticks.ToString());

            var delCnt = ImportToTemp(path, tempPath);
            return delCnt;
        }
        public void HolyGZip(string gzipFileName, string targetDir)
        {
            using (Stream stream = File.OpenRead(gzipFileName))
            {
                var reader = ReaderFactory.Open(stream);
                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                    {
                        reader.WriteEntryToDirectory(targetDir, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                    }
                }
            }
        }
        private List<string> GenerateAssetList(string contentPath)
        {
            string appPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf(@"Assets"));
            List<string> lst = new List<string>();
            var directoryInfo = new DirectoryInfo(contentPath).GetDirectories();
            foreach (var item in directoryInfo)
            {
                string pathnameFromFile = File.ReadAllLines(Path.Combine(item.FullName, "pathname")).ToList().First();
                //ensure path correction
                pathnameFromFile = pathnameFromFile.Replace(@"\\", "/");
                pathnameFromFile = pathnameFromFile.Replace(@"\", "/");

                if (Directory.Exists(Path.Combine(appPath, pathnameFromFile)))
                {
                    if (!pathnameFromFile.EndsWith("/"))
                    {
                        pathnameFromFile += "/";
                    }
                }
                lst.Add(pathnameFromFile);
            }
            var ordered = lst.OrderByDescending(i => i.Count(x => x == '/'));

            return ordered.ToList();
        }
        private void RemoveMess(string tempPath)
        {
            try
            {
                var tempPathInfo = new DirectoryInfo(tempPath);

                foreach (FileInfo file in tempPathInfo.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in tempPathInfo.GetDirectories())
                {
                    dir.Delete(true);
                }
                if (Directory.GetFiles(tempPath).Length <= 0)
                    Directory.Delete(tempPath);
            }
            catch { }
        }

        private void GetDirectoryNames(string path, List<string> list)
        {
            var parent = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(parent) && !list.Contains(parent.ToLower()) && string.Compare(parent, "assets", true) != 0)
            {
                list.Add(parent.ToLower());
                if (path.Contains("/"))
                {
                    GetDirectoryNames(parent, list);
                }
            }
        }
    }
}