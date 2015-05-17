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
        private static bool VSBuild = false; //true on debug only!!!
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
                string settingsFileName = "scripts/noclip_settings.txt";
                string settingsData = "IyBEZWZhdWx0IENvbnRyb2wgU2V0dXANCiMga2V5IGRvY3M6IGh0dHBzOi8vbXNkbi5taWNyb3NvZnQuY29tL2VuLXVzL2xpYnJhcnkvc3lzdGVtLndpbmRvd3MuZm9ybXMua2V5cyUyOHY9dnMuMTEwJTI5LmFzcHgNCiMgWEJMVG9vdGhQaWsNCiMgTm9DbGlwTm9WaXJ1cw0KDQouS2V5cw0KQWN0aXZhdGUgPSBOdW1QYWQwDQpVcFNwZWVkID0gTnVtUGFkMg0KRG93blNwZWVkID0gTnVtUGFkMQ0KTW92ZVVwID0gTFNoaWZ0S2V5DQpNb3ZlRG93biA9IExDb250cm9sS2V5";
                byte[] settingsDataBytes = Convert.FromBase64String(settingsData);
                if (File.Exists(settingsFileName))
                    setupIn = File.Open(settingsFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                else
                {
                    //Writing default settings file, base64 encoded
                    setupIn = File.Create(settingsFileName);
                    System.IO.BinaryWriter Writer = new System.IO.BinaryWriter(setupIn);
                    Writer.BaseStream.Position = 0;

                    Writer.Write(settingsDataBytes, 0, settingsDataBytes.Length);
                    
                    Writer.Flush();
                    Writer.BaseStream.Seek(0, SeekOrigin.Begin);
   
                }

            };
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
