using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Anarchie.Themes
{

    /// <summary>
    /// Provides a base class for a Form that supports Themes
    /// </summary>
    /// <typeparam name="ThemeType">What implementation of the <see cref="Theme"/> class the form should use<br/>
    /// If a separate implementation of <see cref="Theme"/> has not been created, pass the <see cref="Theme"/> type as the generic
    /// </typeparam>
    [System.ComponentModel.DesignerCategory("Code")]
    public class ThemeableForm<ThemeType> : Form where ThemeType : Theme
    {
        /// <summary>
        /// Initilialises the form and sets the default <see cref="Theme"/>
        /// </summary>
        /// <param name="defaultTheme">Initial theme for the form</param>
        public ThemeableForm(ThemeType defaultTheme) : base()
        {
            //Set the default theme
            currentTheme = defaultTheme;
            base.ControlAdded += NewControlAdded;
        }

        private ThemeType currentTheme;
        /// <summary>
        /// The current <see cref="Theme"/> of the <see cref="Form"/>. When changed, all themeable controls will be updated
        /// </summary>
        public ThemeType CurrentTheme
        {
            get { return currentTheme; }
            set
            {
                if (currentTheme == value)
                    return;
                currentTheme = value;
                UpdateThemeables();
            }
        }

        /// <summary>
        /// Determines whether all child controls are changed when the theme changes
        /// </summary>
        public bool SearchAllChildControlsOnThemeChange { get; set; }


        /// <summary>
        /// Updates all themeables on a form. Is run automatically when the theme changes
        /// </summary>
        public void UpdateThemeables()
        {
            //MAKE THIS WORK FOR ALL IThemeableControl, including ones not created in this assembly
            //ThemeableButton control;
            List<IThemeableControl> allChildren = GetAllChildControls(this);
            Type ctrlType;
            PropertyInfo[] ctrlThemedProperties;
            Func<Color>? newColorFunc;
            Color newColor;
            dynamic? propertyToSet;
            foreach (IThemeableControl ctrl in allChildren)
            {
                UpdateSingleThemeable(ctrl);
            }
            base.BackColor = this.currentTheme.FormBackColor;


        }

        private void UpdateSingleThemeable(IThemeableControl ctrl)
        {
            Type ctrlType;
            PropertyInfo[] ctrlThemedProperties;
            Func<Color>? newColorFunc;
            Color newColor;
            dynamic? propertyToSet;

            ctrlType = ctrl.GetType();
            ctrlThemedProperties = ctrlType.GetProperties().Where(p => p.Name.StartsWith("Theme") && !p.Name.EndsWith("PropertyToEdit")).ToArray();
            if (ctrlThemedProperties == null)
                return;
            for (int i = 0; i < ctrlThemedProperties.Length; i++)
            {
                if (ctrlThemedProperties[i] == null)
                    throw new Exception();
                newColorFunc = (Func<Color>?)ctrlThemedProperties[i].GetValue(ctrl);
                if (newColorFunc == null)
                    throw new Exception(string.Format($"No color found for the new color of the control. Either the current theme is not set, or it does not contain a color for {ctrlThemedProperties[i].Name}"));
                newColor = newColorFunc.Invoke();
                propertyToSet = ctrlType.InvokeMember(ctrlThemedProperties[i].Name + "PropertyToEdit", BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy, null, null, null);
                if (propertyToSet == null)
                    throw new Exception(string.Format($"No property of name {ctrlThemedProperties[i].Name}PropertyToEdit found in class {ctrlType}"));
                propertyToSet.DynamicInvoke(ctrl, newColor);


            }
        }

        private List<IThemeableControl> GetAllChildControls(Control control)
        {
            List<IThemeableControl> childControls = new();

            foreach (Control childControl in control.Controls)
            {
                if (childControl is IThemeableControl)
                {
                    childControls.Add((IThemeableControl)childControl);
                    childControls.AddRange(GetAllChildControls(childControl));
                }
            }

            return childControls;
        }

        private void NewControlAdded(object? sender, ControlEventArgs e)
        {
            if (e.Control is IThemeableControl)
                UpdateSingleThemeable((IThemeableControl)e.Control);
        }
    }
}
