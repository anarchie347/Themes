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
		/// Called when <see cref="CurrentTheme"/> is changed
		/// </summary>
		public event ThemeChangedEventHandler ThemeChanged;

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

			ThemeChanged = delegate { };
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

                ThemeChangedEventArgs tcev = new(currentTheme, value);
				currentTheme = value;
                UpdateThemeables(tcev.OldTheme, tcev.NewTheme);	
				ThemeChanged?.Invoke(this, tcev);

			}
		}

		/// <summary>
		/// Determines whether all child controls are changed when the theme changes
		/// </summary>
		public bool SearchAllChildControlsOnThemeChange { get; set; }


		/// <summary>
		/// Updates all themeables on a form. Is run automatically when the theme changes
		/// </summary>
		public void UpdateThemeables(Theme oldTheme, Theme newTheme)
		{
			List<IThemeableControl> allChildren = GetAllChildControls(this);
			foreach (IThemeableControl ctrl in allChildren)
			{
				UpdateSingleThemeable(ctrl);
				ctrl.OnThemeChange(oldTheme, newTheme);
			}
			base.BackColor = this.currentTheme.FormBackColor;
			base.BackgroundImage = this.currentTheme.FormBackgroundImage;

		}

		private static void UpdateSingleThemeable(IThemeableControl ctrl)
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
					throw new Exception($"Required method {ctrlThemeProperties[i].Name}PropertyToEdit was not found in class {ctrlType.Name}");
				ctrlEditingProperties[i] = singleEditingProperty;
			}

			for (int i = 0; i < ctrlThemeProperties.Length; i++) {
                UpdateSingleProperty(ctrlThemeProperties[i], ctrlEditingProperties[i], ctrl);
			}
			
		}
		private static void UpdateSingleProperty(PropertyInfo themeProperty, PropertyInfo editingProperty, IThemeableControl ctrl)
		{
			Type ctrlType = ctrl.GetType();

			ValidateTypesOfThemeableProperties(themeProperty, editingProperty, ctrl);

			dynamic? newValueFunc;
			object? newValue;
			dynamic? baseControlEditSetMethod;

			newValueFunc = themeProperty.GetValue(ctrl);
			if (newValueFunc == null)
				return;
			newValue = newValueFunc.Invoke();

			baseControlEditSetMethod = editingProperty.GetValue(null);


            if (baseControlEditSetMethod == null)
			{
				throw new Exception($"{editingProperty.Name} was null, so {themeProperty.Name} could not edit any property of the base control for class {ctrlType.Name}");
			}

			try
			{ 
				baseControlEditSetMethod.DynamicInvoke(ctrl, newValue);
			} catch
			{
				throw new Exception($"Could not set {baseControlEditSetMethod.Name} to the the value returned by {newValueFunc.Name} because it returned {newValue.GetType()}");
			}

		}

		private static void ValidateTypesOfThemeableProperties(PropertyInfo themeProperty, PropertyInfo editingProperty, IThemeableControl ctrl)
		{
			Type themePropertyType = themeProperty.PropertyType;
			Type editingPropertyType = editingProperty.PropertyType;
			Type ctrlType = ctrl.GetType();

            if (!(themePropertyType.IsGenericType && (themePropertyType.GetGenericTypeDefinition() == typeof(Func<>) || themePropertyType.IsSubclassOf(typeof(Func<>)))))
                throw new Exception($"{themeProperty.Name} in class {ctrlType.Name} was of type {themePropertyType.Name}, when it should have been an implementation of Func`1");

            if (!(editingPropertyType.IsGenericType && (editingPropertyType.GetGenericTypeDefinition() == typeof(Action<,>) || editingPropertyType.IsSubclassOf(typeof(Action<,>)))))
                throw new Exception($"{editingProperty.Name} in class {ctrlType.Name} was of type {editingPropertyType.Name}, when it should have been an implementation of Action`2");

            if (!(themePropertyType.GetGenericArguments().Length == 1))
				throw new Exception($"{themeProperty.Name} in class {ctrlType.Name} had {themePropertyType.GetGenericArguments().Length} generic arguements. It should have had 1");

            if (!(editingPropertyType.GetGenericArguments().Length == 2))
                throw new Exception($"{editingProperty.Name} in class {ctrlType.Name} had {editingPropertyType.GetGenericArguments().Length} generic arguements. It should have had 2");

            //types required and provided by the theme and editing properties
            Type themePropertyReturnType = themePropertyType.GetGenericArguments()[0];
			Type editingPropertyThemeableType = editingPropertyType.GetGenericArguments()[0];
			Type editingPropertyValueType = editingPropertyType.GetGenericArguments()[1];

			if (!typeof(IThemeableControl).IsAssignableFrom(editingPropertyThemeableType))
				throw new Exception($"{editingProperty.Name} in class {ctrlType.Name} did not have a first parameter that inherits from IThemeableControl");

			if (themePropertyReturnType != editingPropertyValueType)
				throw new Exception($"{themeProperty.Name} in class {ctrlType.Name} returns type {themePropertyReturnType.Name}, but {editingProperty.Name} accepts a value of type {editingPropertyValueType.Name}");

			if (!(editingPropertyThemeableType == ctrlType))
				throw new Exception($"The first generic arguement for {editingProperty.Name} was {editingPropertyThemeableType.Name} which did not correspond to the type of the class which was {ctrlType.Name}");

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
