using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Xml.Serialization;
using NMSSaveManager.ObjectBase;
using NMSSaveManager.ObjectBase.Collections;

namespace NMSSaveManager.Framework
{
    public partial class MainForm : Form
    {
        private GameDataInfoCollection _gameSaves;
        private Process _gameProcess;
        private FileSystemWatcher _fileWatcher;

        public MainForm()
        {
            _gameSaves = new GameDataInfoCollection();
            InitializeComponent();
            LoadGameSaves();
        }

        protected override void OnShown(EventArgs e)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(Program.GameDataPath);
                DirectoryInfo[] saves = dir.GetDirectories("st_*");
                if (saves.Length > 0)
                {
                    FileInfo[] dataIdFiles = saves[0].GetFiles();
                    foreach (FileInfo fI in dataIdFiles)
                    {
                        if (fI.Name == "nmssm_id.xml")
                        {
                            base.OnShown(e);
                            return;
                        }
                    }
                    // Existing save data found, prompt user to either save as new game data, or erase
                    // Erasing will simply delete data - !!Warn user of this!!
                    // Saving will create new game entity, with user provided name, and copy into new subdirectory
                    // as well as serialize the game data info object into this save directory, for identification purposes
                    if (MessageBox.Show("Existing save detected, create new game from this save?\n\nWarning: Creating a new game from scratch will wipe existing save data!!", "Existing Save Detected", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        CreateNewGameDialog newGameDialog = new CreateNewGameDialog(saves[0].Name);
                        if (newGameDialog.ShowDialog() == DialogResult.OK)
                        {
                            GameDataInfo newGame = new GameDataInfo(newGameDialog.NewGameName, newGameDialog.ExistingDataDirectory, newGameDialog.SavePath);
                            DateTime lastSaveTime = new DateTime();
                            if (File.Exists(Program.GameDataPath + "\\" + newGameDialog.ExistingDataDirectory + "\\storage.hg"))
                            {
                                FileInfo storageFile = new FileInfo(Program.GameDataPath + "\\" + newGameDialog.ExistingDataDirectory + "\\storage.hg");
                                lastSaveTime = storageFile.LastWriteTime;
                            }
                            newGame.LastSaveTime = lastSaveTime;
                            _gameSaves.Add(newGame);
                            gameSavesBindingSource.ResetBindings(false);
                            SaveSaveIdFile(saves[0].FullName + "\\nmssm_id.xml", newGame);
                            CopyDirectory(saves[0].FullName, Program.GameDataPath + "\\" + newGame.SavePath, true, true);
                        }
                    }
                }
            }
            catch (IOException iEx) { }
            base.OnShown(e);
        }

        private delegate void RefreshGamesListCallback();

        private void RefreshGamesList()
        {
            if(availableGamesList.InvokeRequired)
            {
                Delegate d = new RefreshGamesListCallback(RefreshGamesList);
                Invoke(d);
            }
            else
            {
                gameSavesBindingSource.ResetBindings(false);
            }
        }

        private void SaveGameSaves()
        {
            try
            {
                using (FileStream stream = File.Create("gameSaves.xml"))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(GameDataInfoCollection));
                    xml.Serialize(stream, _gameSaves);
                    stream.Close();
                }
            }
            catch(IOException iOx) { }
        }

        private void LoadGameSaves()
        {
            try
            {
                DirectoryInfo gameDirectory = new DirectoryInfo(Program.GameDataPath);
                DirectoryInfo[] saves = gameDirectory.GetDirectories();
                foreach(DirectoryInfo dI in saves)
                {
                    if (dI.Name.StartsWith("st_")) continue;
                    
                    DirectoryInfo[] dataDirs = dI.GetDirectories("st_*");
                    if(dataDirs.Length > 0)
                    {
                        if(File.Exists(dataDirs[0].FullName + "\\nmssm_id.xml"))
                        {
                            DateTime lastSaveTime = new DateTime();
                            if (File.Exists(dataDirs[0].FullName + "\\storage.hg"))
                            {
                                FileInfo fI = new FileInfo(dataDirs[0].FullName + "\\storage.hg");
                                lastSaveTime = fI.LastWriteTime;
                            }
                            else
                            {
                                lastSaveTime = dataDirs[0].LastWriteTime;
                            }
                            GameDataInfo loadId = OpenSaveIdFile(dataDirs[0].FullName + "\\nmssm_id.xml");
                            loadId.LastSaveTime = lastSaveTime;
                            SaveSaveIdFile(loadId.SavePath + "\\" + loadId.DirectoryName + "\\nmssm_id.xml", loadId);                           
                            _gameSaves.Add(loadId);
                        }
                    }
                    else
                    {
                        FileInfo[] idFiles = dI.GetFiles("nmssm_id.xml");
                        if(idFiles.Length > 0)
                        {
                            GameDataInfo loadId = OpenSaveIdFile(idFiles[0].FullName);
                            _gameSaves.Add(loadId);
                        }
                    }
                }
                foreach(DirectoryInfo dI in saves)
                {
                    if (dI.Name.StartsWith("st_"))
                    {
                        GameDataInfo gI = OpenSaveIdFile(dI.FullName + "\\nmssm_id.xml");
                        if (gI != null)
                        {
                            if (_gameSaves.Contains(gI.Name)) continue;
                            Directory.CreateDirectory(Program.GameDataPath + "\\" + gI.SavePath);
                            CopyDirectory(dI.FullName, Program.GameDataPath + "\\" + gI.SavePath, true, true);
                            if (!_gameSaves.Contains(gI.Name))
                            {
                                FileInfo fI = new FileInfo(dI.FullName + "\\storage.hg");
                                gI.LastSaveTime = fI.LastWriteTime;
                                SaveSaveIdFile(gI.SavePath + "\\" + gI.DirectoryName + "\\nmssm_id.xml", gI);
                                _gameSaves.Add(gI);
                            }
                        }
                    }
                }
                gameSavesBindingSource.DataSource = _gameSaves;
                gameSavesBindingSource.ResetBindings(false);
            }
            catch(IOException iOx) { }
        }

        private void SaveSaveIdFile(String file, GameDataInfo id)
        {
            try
            {
                using (FileStream stream = File.Create(file))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(GameDataInfo));
                    xml.Serialize(stream, id);
                    stream.Close();
                }
            }
            catch(IOException iOx) { }
        }

        private GameDataInfo OpenSaveIdFile(String file)
        {
            try
            {
                using (FileStream stream = File.Open(file, FileMode.Open))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(GameDataInfo));
                    GameDataInfo id = xml.Deserialize(stream) as GameDataInfo;
                    stream.Close();
                    return id;
                }
            }
            catch(IOException iOx) { return null; }
        }

        private bool SaveValid(String path)
        {
            DirectoryInfo importDir = new DirectoryInfo(path);
            DirectoryInfo[] importDataDirs = importDir.GetDirectories("st_*");
            if (importDataDirs.Length > 0)
            {
                if (!File.Exists(importDataDirs[0].FullName + "\\mf_storage.hg")) return false;
                //if (!File.Exists(importDataDirs[0].FullName + "\\mf_storage2.hg")) return false;
                //if (!File.Exists(importDataDirs[0].FullName + "\\mf_storage3.hg")) return false;
                if (!File.Exists(importDataDirs[0].FullName + "\\storage.hg")) return false;
                //if (!File.Exists(importDataDirs[0].FullName + "\\storage2.hg")) return false;
                //if (!File.Exists(importDataDirs[0].FullName + "\\storage3.hg")) return false;
                //if (!File.Exists(importDataDirs[0].FullName + "\\cache\\mf_storage.hg")) return false;
                //if (!File.Exists(importDataDirs[0].FullName + "\\cache\\mf_storage2.hg")) return false;
                //if (!File.Exists(importDataDirs[0].FullName + "\\cache\\mf_storage3.hg")) return false;
                //if (!File.Exists(importDataDirs[0].FullName + "\\cache\\storage.hg")) return false;
                //if (!File.Exists(importDataDirs[0].FullName + "\\cache\\storage2.hg")) return false;
                //if (!File.Exists(importDataDirs[0].FullName + "\\cache\\storage3.hg")) return false;
                return true;
            }
            else return false;
        }

        private void CopyFile(String source, String destination)
        {
            try
            {
                File.Copy(source, destination);
            }
            catch(IOException iOx)
            {
                System.Threading.Thread.Sleep(10);
                CopyFile(source, destination);
            }
        }

        private void CopyDirectory(String directory, String copyPath, bool includeSubdirectories, bool overwrite)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            if (Directory.Exists(copyPath + "\\" + dirInfo.Name) && overwrite) Directory.Delete(copyPath + "\\" + dirInfo.Name, true);
            else if (Directory.Exists(copyPath + "\\" + dirInfo.Name)) return;
            if(!Directory.Exists(copyPath)) Directory.CreateDirectory(copyPath);
            Directory.CreateDirectory(copyPath + "\\" + dirInfo.Name);
            foreach (FileInfo file in dirInfo.GetFiles()) CopyFile(file.FullName, copyPath + "\\" + dirInfo.Name + "\\" + file.Name);
            foreach (DirectoryInfo dir in dirInfo.GetDirectories()) CopyDirectory(dir.FullName, copyPath + "\\" + dirInfo.Name, true, true);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loadSelectedButton_Click(object sender, EventArgs e)
        {
            if (_fileWatcher != null) _fileWatcher.EnableRaisingEvents = false;
            GameDataInfo loadGame = gameSavesBindingSource.Current as GameDataInfo;
            DirectoryInfo dirInfo = new DirectoryInfo(Program.GameDataPath);
            if(dirInfo.GetDirectories("st_*").Length > 0)
            {
                DirectoryInfo existingDir = dirInfo.GetDirectories("st_*")[0];
                FileInfo[] idFiles = existingDir.GetFiles("nmssm_id.xml");
                if (idFiles.Length > 0)
                {
                    GameDataInfo existingGI = OpenSaveIdFile(idFiles[0].FullName);
                    DateTime existingSaveTime = new DateTime();
                    if (File.Exists(existingDir.FullName + "\\storage.hg")) existingSaveTime = (new FileInfo(existingDir.FullName + "\\storage.hg")).LastWriteTime;
                    else existingSaveTime = existingDir.LastWriteTime;
                    DateTime saveLastSaveTime = new DateTime();
                    if (File.Exists(Program.GameDataPath + "\\" + existingGI.SavePath + "\\" + existingGI.DirectoryName + "\\storage.hg")) saveLastSaveTime = (new FileInfo(Program.GameDataPath + "\\" + existingGI.SavePath + "\\" + existingGI.DirectoryName + "\\storage.hg")).LastWriteTime;
                    else saveLastSaveTime = (new DirectoryInfo(Program.GameDataPath + "\\" + existingGI.SavePath + "\\" + existingGI.DirectoryName)).LastWriteTime;
                    if (existingSaveTime > saveLastSaveTime)
                    {
                        // prompt user to use newest save data instead
                        switch (MessageBox.Show("Existing data is a newer version of selected game, do you wish to use\nthe newest data instead?\n\nWarning:\nSaying no will permanently erase newer data!!\nSaying Yes will permanently delete older data!!", "Newer Save Detected", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning))
                        {
                            case DialogResult.Yes:
                                CopyDirectory(existingDir.FullName, Program.GameDataPath + "\\" + loadGame.SavePath, true, true);
                                break;
                            case DialogResult.No:
                                Directory.Delete(existingDir.FullName, true);
                                CopyDirectory(Program.GameDataPath + "\\" + loadGame.SavePath + "\\" + loadGame.DirectoryName, Program.GameDataPath, true, true);
                                break;
                            case DialogResult.Cancel:
                                return;
                        }
                    }
                    else
                    {
                        Directory.Delete(existingDir.FullName, true);
                        CopyDirectory(Program.GameDataPath + "\\" + loadGame.SavePath + "\\" + loadGame.DirectoryName, Program.GameDataPath, true, true);
                    }
                }
                else
                {
                    switch(MessageBox.Show("Existing save detected, create new game from this save?\n\nWarning: Creating a new game from scratch will wipe existing save data!!", "Existing Save Detected", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning))
                    {
                        case DialogResult.Yes:
                            CreateNewGameDialog newGameDialog = new CreateNewGameDialog(existingDir.Name);
                            if (newGameDialog.ShowDialog() == DialogResult.OK)
                            {
                                GameDataInfo newGame = new GameDataInfo(newGameDialog.NewGameName, newGameDialog.ExistingDataDirectory, newGameDialog.SavePath);
                                _gameSaves.Add(newGame);
                                gameSavesBindingSource.ResetBindings(false);
                                SaveSaveIdFile(existingDir.FullName + "\\nmssm_id.xml", newGame);
                                CopyDirectory(existingDir.FullName, Program.GameDataPath + "\\" + newGame.SavePath, true, true);
                                Directory.Delete(existingDir.FullName, true);
                                CopyDirectory(Program.GameDataPath + "\\" + loadGame.SavePath + "\\" + loadGame.DirectoryName, Program.GameDataPath, true, true);
                            }
                            else return;
                            break;
                        case DialogResult.No:
                            Directory.Delete(existingDir.FullName, true);
                            CopyDirectory(Program.GameDataPath + "\\" + loadGame.SavePath + "\\" + loadGame.DirectoryName, Program.GameDataPath, true, true);
                            break;
                        case DialogResult.Cancel:
                            return;
                    }
                }
            }
            else if (Directory.Exists(Program.GameDataPath + "\\" + loadGame.SavePath + "\\" + loadGame.DirectoryName) && loadGame.DirectoryName.StartsWith("st_")) CopyDirectory(Program.GameDataPath + "\\" + loadGame.SavePath, Program.GameDataPath, true, true);
            _gameProcess = new Process();
            _gameProcess.StartInfo.FileName = @"steam://rungameid/275850";
            _gameProcess.EnableRaisingEvents = true;
            if (_fileWatcher == null)
            {
                _fileWatcher = new FileSystemWatcher(Program.GameDataPath);
                _fileWatcher.IncludeSubdirectories = true;
                _fileWatcher.Created += FileWatcher_Created;
                _fileWatcher.Changed += FileWatcher_Changed;
            }
            _fileWatcher.EnableRaisingEvents = true;
            _gameProcess.Start();
        }

        private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                GameDataInfo activeGameInfo = gameSavesBindingSource.Current as GameDataInfo;
                String storageFile = Program.GameDataPath + "\\" + activeGameInfo.DirectoryName + "\\storage.hg";
                if (e.Name.Contains("\\storage"))
                {
                    _fileWatcher.EnableRaisingEvents = false;
                    FileInfo fI = new FileInfo(e.FullPath);
                    activeGameInfo.LastSaveTime = fI.LastWriteTime;
                    SaveSaveIdFile(Program.GameDataPath + "\\" + activeGameInfo.DirectoryName + "\\nmssm_id.xml", activeGameInfo);
                    CopyDirectory(Program.GameDataPath + "\\" + activeGameInfo.DirectoryName, Program.GameDataPath + "\\" + activeGameInfo.SavePath, true, true);
                    RefreshGamesList();
                    _fileWatcher.EnableRaisingEvents = true;
                }
            }
        }

        private void FileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if(e.ChangeType == WatcherChangeTypes.Created)
            {
                GameDataInfo activeGameInfo = gameSavesBindingSource.Current as GameDataInfo;
                if(e.Name.StartsWith("st_") && !e.Name.Contains("\\"))
                {
                    _fileWatcher.EnableRaisingEvents = false;
                    activeGameInfo.DirectoryName = e.Name;
                    DirectoryInfo dI = new DirectoryInfo(e.FullPath);
                    activeGameInfo.LastSaveTime = dI.LastWriteTime;
                    SaveSaveIdFile(e.FullPath + "\\nmssm_id.xml", activeGameInfo);
                    if (File.Exists(Program.GameDataPath + "\\" + activeGameInfo.SavePath + "\\nmssm_id.xml")) File.Delete(Program.GameDataPath + "\\" + activeGameInfo.SavePath + "\\nmssm_id.xml");
                    CopyDirectory(e.FullPath, Program.GameDataPath + "\\" + activeGameInfo.SavePath, true, true);
                    RefreshGamesList();
                    _fileWatcher.EnableRaisingEvents = true;
                }
                else if(e.Name.Contains("\\storage"))
                {
                    _fileWatcher.EnableRaisingEvents = false;
                    FileInfo fI = new FileInfo(e.FullPath);
                    activeGameInfo.LastSaveTime = fI.LastWriteTime;
                    SaveSaveIdFile(Program.GameDataPath + "\\" + activeGameInfo.DirectoryName + "\\nmssm_id.xml", activeGameInfo);
                    CopyDirectory(Program.GameDataPath + "\\" + activeGameInfo.DirectoryName, Program.GameDataPath + "\\" + activeGameInfo.SavePath, true, true);
                    RefreshGamesList();
                    _fileWatcher.EnableRaisingEvents = true;
                }
                else
                {
                    _fileWatcher.EnableRaisingEvents = false;
                    CopyDirectory(Program.GameDataPath + "\\" + activeGameInfo.DirectoryName, Program.GameDataPath + "\\" + activeGameInfo.SavePath, true, true);
                    _fileWatcher.EnableRaisingEvents = true;
                }
            }
        }

        private void importSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.RootFolder = Environment.SpecialFolder.MyComputer;
            if(folder.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo importDir = new DirectoryInfo(folder.SelectedPath);
                if(SaveValid(importDir.FullName))
                {
                    if (_fileWatcher != null) _fileWatcher.EnableRaisingEvents = false;
                    DirectoryInfo dataDir = importDir.GetDirectories("st_*")[0];
                    GameDataInfo importGI = new GameDataInfo(importDir.Name, dataDir.Name, importDir.Name);
                    FileInfo storageFile = new FileInfo(dataDir.FullName + "\\storage.hg");
                    importGI.LastSaveTime = storageFile.LastWriteTime;
                    _gameSaves.Add(importGI);
                    gameSavesBindingSource.ResetBindings(false);
                    SaveSaveIdFile(importDir.FullName + "\\" + importGI.DirectoryName + "\\nmssm_id.xml", importGI);
                    CopyDirectory(importDir.FullName, Program.GameDataPath, true, true);
                    if (_fileWatcher != null) _fileWatcher.EnableRaisingEvents = true;
                }
                else
                {
                    MessageBox.Show("Selected path is not a valid save, save data not found.", "Invalid Save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void availableGamesList_SelectionChanged(object sender, EventArgs e)
        {
            deleteButton.Enabled = loadSelectedButton.Enabled = availableGamesList.SelectedRows.Count > 0;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            GameDataInfo gI = gameSavesBindingSource.Current as GameDataInfo;
            switch(MessageBox.Show("Do you wish to remove the save data as well?","Delete Save Data",MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    Directory.Delete(Program.GameDataPath + "\\" + gI.SavePath, true);
                    if (File.Exists(Program.GameDataPath + "\\" + gI.DirectoryName + "\\nmssm_id.xml"))
                    {
                        GameDataInfo existingGI = OpenSaveIdFile(Program.GameDataPath + "\\" + gI.DirectoryName + "\\nmssm_id.xml");
                        if (existingGI.Name == gI.Name) File.Delete(Program.GameDataPath + "\\" + gI.DirectoryName + "\\nmssm_id.xml");
                    }
                    _gameSaves.Remove(gI);
                    gameSavesBindingSource.ResetBindings(false);
                    break;
                case DialogResult.No:
                    if (File.Exists(Program.GameDataPath + "\\" + gI.SavePath + "\\nmssm_id.xml")) File.Delete(Program.GameDataPath + "\\" + gI.SavePath + "\\nmssm_id.xml");
                    File.Delete(Program.GameDataPath + "\\" + gI.SavePath + "\\" + gI.DirectoryName + "\\nmssm_id.xml");
                    if (File.Exists(Program.GameDataPath + "\\" + gI.DirectoryName + "\\nmssm_id.xml"))
                    {
                        GameDataInfo existingGI = OpenSaveIdFile(Program.GameDataPath + "\\" + gI.DirectoryName + "\\nmssm_id.xml");
                        if (existingGI.Name == gI.Name) File.Delete(Program.GameDataPath + "\\" + gI.DirectoryName + "\\nmssm_id.xml");
                    }
                    _gameSaves.Remove(gI);
                    gameSavesBindingSource.ResetBindings(false);
                    break;
                case DialogResult.Cancel:
                    return;
            }
            if (Directory.Exists(Program.GameDataPath + "\\" + gI.SavePath) && Directory.GetFiles(Program.GameDataPath + "\\" + gI.SavePath).Length < 1 && Directory.GetDirectories(Program.GameDataPath + "\\" + gI.SavePath).Length < 1) Directory.Delete(Program.GameDataPath + "\\" + gI.SavePath);
        }

        private void createNewGameButton_Click(object sender, EventArgs e)
        {
            bool createFromCurrent = false;
            String existingPath = String.Empty;
            String existingDirName = String.Empty;
            DirectoryInfo gameDirectory = new DirectoryInfo(Program.GameDataPath);
            DirectoryInfo[] saveDirs = gameDirectory.GetDirectories("st_*");
            if (saveDirs.Length > 0)
            {
                DirectoryInfo existingDir = saveDirs[0];
                GameDataInfo gI = OpenSaveIdFile(existingDir.FullName + "\\nmssm_id.xml");
                if (gI == null)
                {
                    if (MessageBox.Show("Existing save detected, create new game from this save?\n\nWarning: Creating a new game from scratch will wipe existing save data!!", "Existing Save Detected", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    {
                        createFromCurrent = true;
                        existingPath = existingDir.FullName;
                        existingDirName = existingDir.Name;
                    }
                }
            }

            CreateNewGameDialog newGame;
            if (createFromCurrent)
            {
                newGame = new CreateNewGameDialog(existingDirName);
                if (newGame.ShowDialog() == DialogResult.OK)
                {
                    GameDataInfo newGameInfo = new GameDataInfo(newGame.NewGameName, newGame.ExistingDataDirectory, newGame.SavePath);
                    DateTime lastSaveTime = new DateTime();
                    if (File.Exists(existingPath + "\\storage.hg"))
                    {
                        FileInfo storageFile = new FileInfo(existingPath + "\\storage.hg");
                        lastSaveTime = storageFile.LastWriteTime;
                    }
                    newGameInfo.LastSaveTime = lastSaveTime;
                    _gameSaves.Add(newGameInfo);
                    gameSavesBindingSource.ResetBindings(false);
                    Directory.CreateDirectory(Program.GameDataPath + "\\" + newGameInfo.SavePath);
                    CopyDirectory(existingPath, Program.GameDataPath + "\\" + newGameInfo.SavePath, true, true);
                    SaveSaveIdFile(existingPath + "\\nmssm_id.xml", newGameInfo);
                    SaveSaveIdFile(Program.GameDataPath + "\\" + newGameInfo.SavePath + "\\" + newGameInfo.DirectoryName + "\\nmssm_id.xml", newGameInfo);
                }
            }
            else
            {
                newGame = new CreateNewGameDialog();
                if (newGame.ShowDialog() == DialogResult.OK)
                {
                    if (_fileWatcher != null) _fileWatcher.EnableRaisingEvents = false;
                    if (saveDirs.Length > 0)
                    {
                        DirectoryInfo existingDir = saveDirs[0];
                        GameDataInfo gI = OpenSaveIdFile(existingDir.FullName + "\\nmssm_id.xml");
                        if (gI != null)
                        {
                            if (!Directory.Exists(gameDirectory.FullName + "\\" + gI.SavePath)) Directory.CreateDirectory(gameDirectory.FullName + "\\" + gI.SavePath);
                            CopyDirectory(existingDir.FullName, gameDirectory.FullName + "\\" + gI.SavePath, true, true);
                            Directory.Delete(existingDir.FullName, true);
                        }
                    }
                    GameDataInfo newGameInfo = new GameDataInfo(newGame.NewGameName, String.Empty, newGame.SavePath);
                    if (newGame.OverwriteExisting)
                    {
                        Directory.CreateDirectory(gameDirectory.FullName + "\\" + newGame.SavePath);
                        SaveSaveIdFile(gameDirectory.FullName + "\\" + newGame.SavePath + "\\nmssm_id.xml", newGameInfo);
                    }
                    else
                    {
                        if (!Directory.Exists(gameDirectory.FullName + "\\" + newGame.SavePath))
                        {
                            Directory.CreateDirectory(gameDirectory.FullName + "\\" + newGame.SavePath);
                            SaveSaveIdFile(gameDirectory.FullName + "\\" + newGame.SavePath + "\\nmssm_id.xml", newGameInfo);
                        }
                        else
                        {
                            DirectoryInfo saveDir = new DirectoryInfo(Program.GameDataPath + "\\" + newGame.SavePath);
                            DirectoryInfo[] dataDirs = saveDir.GetDirectories("st_*");
                            if (dataDirs.Length > 0)
                            {
                                FileInfo[] storageFiles = dataDirs[0].GetFiles("storage.hg");
                                if (storageFiles.Length > 0) newGameInfo.LastSaveTime = storageFiles[0].LastWriteTime;
                                SaveSaveIdFile(gameDirectory.FullName + "\\" + newGame.SavePath + "\\" + dataDirs[0].Name + "\\nmssm_id.xml", newGameInfo);
                            }
                            else
                            {
                                SaveSaveIdFile(gameDirectory.FullName + "\\" + newGame.SavePath + "\\nmssm_id.xml", newGameInfo);
                            }
                        }

                    }
                    _gameSaves.Add(newGameInfo);
                    gameSavesBindingSource.ResetBindings(false);
                    if (_fileWatcher != null) _fileWatcher.EnableRaisingEvents = true;
                }
            }
        }
    }
}
