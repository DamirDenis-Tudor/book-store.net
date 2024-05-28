using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class HelloWorld
    {
        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
        static void Main()
        {
            var newImage = Image.FromFile(@"favicon.png");

            foreach(var b in ImageToByteArray(newImage))
                Console.Write(b +", ");
        }
    }
}
