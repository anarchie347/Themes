using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anarchie.Themes
{
    /// <summary>
    /// Collection of <see cref="Color"/> fields to define a <see cref="Theme"/> for a <see cref="ThemeableForm{ThemeType}"/>
    /// </summary>
    public class Theme
    {
        /// <summary>
        /// The primary <see cref="Color"/> for the <see cref="Theme"/>
        /// </summary>
        public Color PrimaryColor { get; init; } = Color.White;

        /// <summary>
        /// The secondary <see cref="Color"/> for the <see cref="Theme"/>
        /// </summary>
        public Color SecondaryColor { get; init; } = Color.White;

        /// <summary>
        /// The <see cref="Color"/> for the background of the <see cref="ThemeableForm{ThemeType}"/>
        /// </summary>
        public Color FormBackColor { get; init; } = Color.White;

    }
}
