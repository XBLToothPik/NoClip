using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using GTA;
using GTA.Math;
using GTA.Native;
namespace NoClipNoVirus
{
    public static class Utils
    {

        public static Vector2 ScreenRes
        {
            get
            {
                unsafe
                {
                    int x = 0, y = 0;
                    Function.Call(Hash.GET_SCREEN_RESOLUTION, &x, &y);
                    return new Vector2(x, y);
                }
            }
        }
        public static Stream GetResourceAsStream(string resName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("NoClipNoVirus.Resources.{0}", resName));
        }
        public static Func<double, float> DegreesToRadians = angleR => (float)(angleR * Math.PI / 180f);
    }
}
