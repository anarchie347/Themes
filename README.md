# Themes

* [About](#about)
* [Getting started](#getting-started)
* [Contributing](#contributing)
* [Credits](#credits)

## About
A library for easily(ish) created themes in Windows Forms .NET to allow you to easily change colours of lots of controls

This package requires .NET 6, and can only be used on Windows version 7 or greater

Usage: [Github wiki](https://github.com/anarchie347/Themes/wiki)

## Getting started:
Replace 
```csharp
internal partial class Form1 : Form
```
With
```csharp
internal partial class Form1 : ThemeableForm<Theme>
```
`Theme` is the implementation of the `Theme` class used for this form. If you want to define your own schema for a theme, put that class inside the `<>`. You can see how to create a new schema [here](https://github.com/anarchie347/Themes/wiki/Defining-a-Theme-schema)


```csharp
ThemeableButton myButton = new();
myButton.ThemeBackColor = () => CurrentTheme.PrimaryColor;
myButton.ThemeForeColor = () => CurrentTheme.SecondaryColor;
myButton.ThemeImage = () => CurrentTheme.someImage;

myButton.Click += (sender, e) =>
{
    if (CurrentTheme == Themes.Dark)
        CurrentTheme = Themes.Light;
    else
        CurrentTheme = Themes.Dark;
};
this.Controls.Add(myButton);
```
Creates a ThemeableButton that swaps the theme between light and dark. When the Theme changes, the colours and image of the button will also update. This code is inside the `Form1` class.

`Themes` is a class defined by the project using the Themes library, which contains instances of the Theme used for this form

## Contributing
Created by [anarchie347](https://github.com/anarchie347)

Issue tracker: [Github issue tracker](https://github.com/anarchie347/Themes/issues)

Contribute on [Github](https://github.com/anarchie347/Themes)


## Credits
Icon: Photo by [RhondaK Native Florida Folk Artist](https://unsplash.com/@rhondak?utm_source=unsplash&utm_medium=referral&utm_content=creditCopyText) on [Unsplash](https://unsplash.com/photos/_Yc7OtfFn-0?utm_source=unsplash&utm_medium=referral&utm_content=creditCopyText)
  
