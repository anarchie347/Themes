using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Anarchie.Themes
{
    /// <summary>
    /// An implementation of <see cref="IThemeableControl"/> for <see cref="Label"/> controls
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    internal class ThemeableLabel : Label, IThemeableControl
    {
        /// <summary>
        /// Defines what property should be set by the <see cref="ThemeForeColor"/>
        /// </summary>
        public static Action<ThemeableLabel, Color> ThemeForeColorPropertyToEdit { get { return (ctrl, value) => ctrl.ForeColor = value; } }
        private Func<Color> themeForeColor = () => Color.White;
        /// <summary>
        /// The theme color that should be used for the ForeColor property of the <see cref="ThemeableLabel"/>
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
        /// Defines what property should be set by the <see cref="ThemeForeColor"/>
        /// </summary>
        public static Action<ThemeableLabel, Color> ThemeBackColorPropertyToEdit { get { return (ctrl, value) => ctrl.BackColor = value; } }
        private Func<Color> themeBackColor = () => Color.White;
        /// <summary>
        /// The theme color that should be used for the ForeColor property of the <see cref="ThemeableLabel"/>
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
