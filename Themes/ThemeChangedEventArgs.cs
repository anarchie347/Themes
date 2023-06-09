﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Anarchie.Themes
{
    /// <summary>
    /// Handles the ThemeChanged event from a <see cref="ThemeableForm{ThemeType}"/>
    /// </summary>
    /// <param name="sender">The control invoking the event</param>
    /// <param name="e">The <see cref="ThemeChangedEventArgs"/> containing the new and old themes</param>
    public delegate void ThemeChangedEventHandler(object sender, ThemeChangedEventArgs e);

    /// <summary>
    /// Event arguements for a theme change
    /// </summary>
    public class ThemeChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The <see cref="Theme"/> before the theme changed
        /// </summary>
        public Theme OldTheme { get; init; }

        /// <summary>
        /// The <see cref="Theme"/> after the theme changed
        /// </summary>
        public Theme NewTheme { get; init; }

        /// <summary>
        /// Creates a new instance of <see cref="ThemeChangedEventArgs"/>
        /// </summary>
        /// <param name="oldTheme">The <see cref="Theme"/> before the theme changed</param>
        /// <param name="newTheme">The <see cref="Theme"/> after the theme changed</param>
        public ThemeChangedEventArgs(Theme oldTheme, Theme newTheme)
        {
            OldTheme = oldTheme;
            NewTheme = newTheme;
        }
    }
}
