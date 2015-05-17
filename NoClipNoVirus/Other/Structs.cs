using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace NoClipNoVirus
{
    public static class Structs
    {
        public class SetupFile
        {
            Dictionary<string, List<SetupFileEntry>> Entries { get; set; }

            public string GetDataByName(string section, string name)
            {
                foreach (SetupFileEntry _entry in Entries[section])
                {
                    if (_entry.Name == name)
                        return _entry.Data;
                }
                return "NO_ENTRY";
            }
            public SetupFileEntry GetEntryByName(string section, string name)
            {
                foreach (SetupFileEntry _entry in Entries[section])
                {
                    if (_entry.Name == name)
                        return _entry;
                }
                return null;
            }
            public SetupFile()
            {
                Entries = new Dictionary<string, List<SetupFileEntry>>();
            }
            public SetupFile(Stream xIn)
            {
                Entries = new Dictionary<string, List<SetupFileEntry>>();
                ReadFromStream(xIn);
            }
            public void ReadFromStream(Stream xIn)
            {
                StreamReader reader = new StreamReader(xIn);

                string currentSection = string.Empty;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line == null || line == "" || string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                        continue;

                    if (line.StartsWith("."))
                    {
                        Entries.Add(line.Substring(1), new List<SetupFileEntry>());
                        currentSection = line.Substring(1);
                    }
                    else
                    {
                        int assignerIndex = line.IndexOf('=');
                        string entryName = line.Substring(0, assignerIndex - 1);
                        string entryData = line.Substring(assignerIndex + 2);
                        Entries[currentSection].Add(new SetupFileEntry() { Name = entryName, Data = entryData });

                    }
                }
            }
            public class SetupFileEntry
            {
                public string Name;
                public string Data;

                public int Intvalue
                {
                    get
                    {
                        return int.Parse(Data);
                    }
                }
                public float FloatValue
                {
                    get
                    {
                        return float.Parse(Data);
                    }
                }
                public bool BoolValue
                {
                    get
                    {
                        return bool.Parse(Data);
                    }
                }
                public  System.Windows.Forms.Keys KeyValue
                {
                    get
                    {
                        return (System.Windows.Forms.Keys)Enum.Parse(typeof(System.Windows.Forms.Keys), Data);
                    }
                }
            }
        }
    }
}
