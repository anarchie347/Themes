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
			base.BackColor = currentTheme.FormBackColor;
			base.BackgroundImage = currentTheme.FormBackgroundImage;
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
			foreach (IThemeableControl ctrl in allChildren)
			{
				NewUpdateSingleThemeable(ctrl);
			}
			base.BackColor = this.currentTheme.FormBackColor;
			base.BackgroundImage = this.currentTheme.FormBackgroundImage;

		}

		private static void NewUpdateSingleThemeable(IThemeableControl ctrl)
		{
			Type ctrlType;
			PropertyInfo[] ctrlThemeProperties, ctrlEditingProperties;

			ctrlType = ctrl.GetType();
			//get theme properties
			ctrlThemeProperties = ctrlType.GetProperties().Where(prop => prop.Name.StartsWith("Theme") && !prop.Name.EndsWith("PropertyToEdit")).ToArray();
			ctrlEditingProperties = new PropertyInfo[ctrlThemeProperties.Length];
			PropertyInfo? singleEditingProperty;
			//have to get editing properties individually to make sure the arrays are in the same order
			for (int i = 0; i < ctrlThemeProperties.Length; i++)
			{
				
				singleEditingProperty = ctrlType.GetProperty(ctrlThemeProperties[i].Name + "PropertyToEdit", BindingFlags.FlattenHierarchy | BindingFlags.Static | BindingFlags.Public);
				if (singleEditingProperty == null)
					throw new Exception($"Required method {ctrlThemeProperties[i].Name}PropertyToEdit was not found in class {ctrlType}");
				ctrlEditingProperties[i] = singleEditingProperty;
            }

			for (int i = 0; i < ctrlThemeProperties.Length; i++) {
				UpdateSingleProperty(ctrlThemeProperties[i], ctrlEditingProperties[i], ctrl);
			}

			
		}
		private static void UpdateSingleProperty(PropertyInfo themeProperty, PropertyInfo editingProperty, IThemeableControl ctrl)
		{
            Type editingPropertyType = editingProperty.PropertyType;
            Type ctrlType = ctrl.GetType();

            Type editingPropertyValueType = editingPropertyType.GetGenericArguments()[1];

            ValidateTypesOfThemeableProperties(themeProperty, editingProperty, ctrl);

			dynamic? newValueFunc;
			object? newValue;
			dynamic? baseControlEditSetMethod;

			

			newValueFunc = themeProperty.GetValue(ctrl);
			if (newValueFunc == null)
				return;
			newValue = newValueFunc.Invoke();
			if (newValue == null)
				//throw new Exception($"{newValueFunc} returned null, so no value could be set for {themeProperty} in class {ctrlType} because {editingPropertyValueType} is not nullable");
				return;
			baseControlEditSetMethod = editingProperty.GetValue(ctrl);

			if (baseControlEditSetMethod == null)
			{
				throw new Exception($"{editingProperty} was null, so {themeProperty} could not edit any property of the base control for class {ctrlType}");
			}

			baseControlEditSetMethod.DynamicInvoke(ctrl, newValue);

		}

		private static void ValidateTypesOfThemeableProperties(PropertyInfo themeProperty, PropertyInfo editingProperty, IThemeableControl ctrl)
		{
			Type themePropertyType = themeProperty.PropertyType;
			Type editingPropertyType = editingProperty.PropertyType;
			Type ctrlType = ctrl.GetType();

			//types required and provided by the theme and editing properties
			Type themePropertyReturnType = themePropertyType.GetGenericArguments()[0];
            Type editingPropertyThemeableType = editingPropertyType.GetGenericArguments()[0];
            Type editingPropertyValueType = editingPropertyType.GetGenericArguments()[1];

			if (!typeof(IThemeableControl).IsAssignableFrom(editingPropertyThemeableType))
				throw new Exception($"{editingProperty} in class {ctrlType} did not have a first parameter that inherits from IThemeableControl");

			if (themePropertyReturnType != editingPropertyValueType)
				throw new Exception($"In class {ctrlType}, {themeProperty} returns type {themePropertyReturnType}, but {editingProperty} accepts a value of type {editingPropertyValueType}");

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
				NewUpdateSingleThemeable((IThemeableControl)e.Control);
		}
	}
}
