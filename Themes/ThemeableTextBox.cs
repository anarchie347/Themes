using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Anarchie.Themes
{
    /// <summary>
    /// An implementation of <see cref="IThemeableControl"/> for <see cref="TextBox"/> controls
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class ThemeableTextBox : TextBox, IThemeableControl
    {
        /// <summary>
        /// Raised when the theme for the parent <see cref="ThemeableForm{ThemeType}"/> changes
        /// </summary>
        public event EventHandler ThemeChanged = delegate { };


        /// <summary>
        /// Called by the parent <see cref="ThemeableForm{ThemeType}"/> when the theme changes
        /// </summary>
        /// <typeparam name="ThemeType">The implementation of <see cref="Theme"/> used by the form</typeparam>
        /// <param name="oldTheme">The old theme</param>
        /// <param name="newTheme">The new theme</param>
        public void OnThemeChange<ThemeType>(ThemeType oldTheme, ThemeType newTheme) where ThemeType : Theme
        {
            ThemeChangedEventArgs<ThemeType> tcev = new(oldTheme, newTheme);
            ThemeChanged?.Invoke(this, tcev);
        }

        /// <summary>
        /// Defines what property should be set by the <see cref="ThemeForeColor"/>
        /// </summary>
        public static Action<ThemeableTextBox, Color>? ThemeForeColorPropertyToEdit { get { return (ctrl, value) => ctrl.ForeColor = value; } }
        private Func<Color>? themeForeColor = null;
        /// <summary>
        /// The theme color that should be used for the ForeColor property of the <see cref="ThemeableTextBox"/>
        /// </summary>
        public Func<Color>? ThemeForeColor
        {
            get { return themeForeColor; }
            set
            {
                themeForeColor = value;
                if (themeForeColor != null)
                    base.ForeColor = themeForeColor();
            }

        }

        /// <summary>
        /// Defines what property should be set by the <see cref="ThemeBackColor"/>
        /// </summary>
        public static Action<ThemeableTextBox, Color>? ThemeBackColorPropertyToEdit { get { return (ctrl, value) => ctrl.BackColor = value; } }
        private Func<Color>? themeBackColor = null;
        /// <summary>
        /// The theme color that should be used for the BackColor property of the <see cref="ThemeableTextBox"/>
        /// </summary>
        public Func<Color>? ThemeBackColor
        {
            get { return themeBackColor; }
            set
            {
                themeBackColor = value;
                if (themeBackColor != null)
                    base.BackColor = themeBackColor();
            }
        }
    }
}
