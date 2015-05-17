using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
namespace NoClipNoVirus
{
    public static class Globals
    {
        private static bool VSBuild = true; //true on debug only!!!
        public static Structs.SetupFile MainSettings { get; set; }
        public static Keys ActivateKey { get; set; }
        public static Keys SpeedUpKey { get; set; }
        public static Keys SpeedDownKey { get; set; }
        public static Keys MoveUpKey { get; set; }
        public static Keys MoveDownKey { get; set; }

        public static void InitGlobals()
        {
            LoadSettings();
        }
        public static void LoadSettings()
        {
            Stream setupIn = null;
            if (VSBuild)
            {
                string settingsFileName = "scripts/noclip_settings.txt";
                if (File.Exists(settingsFileName))
                    setupIn = File.Open(settingsFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                else
                {
                    setupIn = File.Create(settingsFileName);
                    Stream defaultStream = Utils.GetResourceAsStream("DefaultControlSetup.txt");
                    defaultStream.Seek(0, SeekOrigin.Begin);
                    defaultStream.CopyTo(setupIn);

                    setupIn.Flush();
                    setupIn.Seek(0, SeekOrigin.Begin);
                }

            }
            else
            {
                //ON RELEASE BUILD ONLY!!
                //CAN'T DO THIS YET. 
                string settingsFileName = "../../noclip_settings.txt";
                if (File.Exists(settingsFileName))
                    setupIn = File.Open(settingsFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                else
                {
                    setupIn = File.Create(settingsFileName);
                    Stream defaultStream = File.Open("../Resources/DefaultControlSetup.txt", FileMode.Open);
                    defaultStream.CopyTo(setupIn);
                    setupIn.Flush();
                    setupIn.Seek(0, SeekOrigin.Begin);
                }
            }
            MainSettings = new Structs.SetupFile(setupIn);
            setupIn.Close();

            ActivateKey = MainSettings.GetEntryByName("Keys", "Activate").KeyValue;
            SpeedUpKey = MainSettings.GetEntryByName("Keys", "UpSpeed").KeyValue;
            SpeedDownKey = MainSettings.GetEntryByName("Keys", "DownSpeed").KeyValue;
            MoveUpKey = MainSettings.GetEntryByName("Keys", "MoveUp").KeyValue;
            MoveDownKey = MainSettings.GetEntryByName("Keys", "MoveDown").KeyValue;
        }
    }
}
