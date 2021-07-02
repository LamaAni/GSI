using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ThemesBlockBaseThemeAttribute : Attribute
    {
        // This is a positional argument
        public ThemesBlockBaseThemeAttribute()
        {
        }
    }
}
