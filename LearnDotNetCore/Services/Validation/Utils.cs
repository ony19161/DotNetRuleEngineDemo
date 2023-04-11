using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnDotNetCore.Services.Validation
{
    public static class Utils
    {
        public static bool CheckNullOrEmpty(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}
