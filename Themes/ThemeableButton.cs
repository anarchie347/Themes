using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.CodeDom;
using System.Diagnostics;

namespace Anarchie.Themes
{
	/// <summary>
	/// An implementation of <see cref="IThemeableControl"/> for <see cref="Button"/> controls
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	public class ThemeableButton : Button, IThemeableControl
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
        public static Action<ThemeableButton, Color>? ThemeForeColorPropertyToEdit { get { return (ctrl, value) => ctrl.ForeColor = value; } }
		private Func<Color>? themeForeColor = null;
		/// <summary>
		/// The theme color that should be used for the ForeColor property of the <see cref="ThemeableButton"/>
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
		public static Action<ThemeableButton, Color>? ThemeBackColorPropertyToEdit { get { return (ctrl, value) => ctrl.BackColor = value; } }
		private Func<Color>? themeBackColor = null;
		/// <summary>
		/// The theme color that should be used for the BackColor property of the <see cref="ThemeableButton"/>
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


		/// <summary>
		/// Defines what property should be set by the <see cref="ThemeBorderColor"/>
		/// </summary>
		public static Action<ThemeableButton, Color>? ThemeBorderColorPropertyToEdit { get { return (ctrl, value) => ctrl.FlatAppearance.BorderColor = value; } }
		private Func<Color>? themeBorderColor = null;
		/// <summary>
		/// The theme color that should be used for the FlatAppearnace.BorderColor property of the <see cref="ThemeableButton"/><br/>
		/// Requires FlatStyle = FlatStyle.Flat
		/// </summary>
		public Func<Color>? ThemeBorderColor
		{
			get { return themeBorderColor; }
			set
			{
				themeBorderColor = value;
				if (base.FlatStyle == FlatStyle.Flat)
					if (themeBorderColor != null)
						base.FlatAppearance.BorderColor = themeBorderColor();
			}
		}

		/// <summary>
		/// Defines what property should be set by the <see cref="ThemeImage"/>
		/// </summary>
		public static Action<ThemeableButton, Image>? ThemeImagePropertyToEdit { get { return (ctrl, value) => ctrl.BackgroundImage = value; } }
		private Func<Image>? themeImage = null;
		/// <summary>
		/// The image that should be used for the BackgroundImage property of the <see cref="ThemeableButton"/><br/>
		/// </summary>
		public Func<Image>? ThemeImage
		{
			get { return themeImage; }
			set
			{
				themeImage = value;
				if (themeImage != null)
					base.BackgroundImage = themeImage();
			}
		}
	}
}

