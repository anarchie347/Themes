using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Anarchie.Themes
{
    /// <summary>
    /// Event arguements for a theme change
    /// </summary>
    /// <typeparam name="ThemeType">The type of theme, inherits from <see cref="Theme"/></typeparam>
    public class ThemeChangedEventArgs<ThemeType> : EventArgs where ThemeType : Theme
    {
        /// <summary>
        /// The <see cref="Theme"/> before the theme changed
        /// </summary>
        public ThemeType OldTheme { get; init; }

        /// <summary>
        /// The <see cref="Theme"/> after the theme changed
        /// </summary>
        public ThemeType NewTheme { get; init; }
    }
}
