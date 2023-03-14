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
        /// Defines what property should be set by the <see cref="ThemeForeColor"/>
        /// </summary>
        public static Action<ThemeableTextBox, Color> ThemeForeColorPropertyToEdit { get { return (ctrl, value) => ctrl.ForeColor = value; } }
        private Func<Color> themeForeColor = () => Color.White;
        /// <summary>
        /// The theme color that should be used for the ForeColor property of the <see cref="ThemeableTextBox"/>
        /// </summary>
        public Func<Color> ThemeForeColor
        {
            get { return themeForeColor; }
            set
            {
                themeForeColor = value;
                base.ForeColor = themeForeColor();
            }

        }

        /// <summary>
        /// Defines what property should be set by the <see cref="ThemeBackColor"/>
        /// </summary>
        public static Action<ThemeableTextBox, Color> ThemeBackColorPropertyToEdit { get { return (ctrl, value) => ctrl.BackColor = value; } }
        private Func<Color> themeBackColor = () => Color.White;
        /// <summary>
        /// The theme color that should be used for the BackColor property of the <see cref="ThemeableTextBox"/>
        /// </summary>
        public Func<Color> ThemeBackColor
        {
            get { return themeBackColor; }
            set
            {
                themeBackColor = value;
                base.BackColor = themeBackColor();
            }
        }
    }
}
