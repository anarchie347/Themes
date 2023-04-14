﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Anarchie.Themes
{
    /// <summary>
    /// Interface for all themeable controls. See documentation for how to implement a new themeable using this interface
    /// </summary>
    public interface IThemeableControl
    {
        /// <summary>
        /// Defines what property should be set by the <see cref="ThemeForeColor"/>
        /// </summary>
        static Action<IThemeableControl, Color>? ThemeForeColorPropertyToEdit { get; }
        /// <summary>
        /// The theme color that should be used for the ForeColor property of a <see cref="Control"/>
        /// </summary>
        Func<Color>? ThemeForeColor { get; set; }

        /// <summary>
        /// Defines what property should be set by the <see cref="ThemeBackColor"/>
        /// </summary>
        static Action<IThemeableControl, Color>? ThemeBackColorPropertyToEdit { get; }
        /// <summary>
        /// The theme color that should be used for the BackColor property of a <see cref="Control"/>
        /// </summary>
        Func<Color>? ThemeBackColor { get; set; }

    }
}
