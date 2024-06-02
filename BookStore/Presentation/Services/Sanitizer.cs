using Ganss.Xss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Services
{
    public class Sanitizer
    {
        private static HtmlSanitizer sanitizer = new HtmlSanitizer();
        public static string SanitizeString(string input)
        {
            return sanitizer.Sanitize(input);
        }

    }
}
