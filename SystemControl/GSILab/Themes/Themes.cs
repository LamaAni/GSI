using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Extends the windows forms.
namespace System.Windows.Forms
{
    /// <summary>
    /// The base for the theme
    /// </summary>
    public static class Themes
    {
        /// <summary>
        /// A collection of themes to be applied according to the control
        /// </summary>
        static Dictionary<Type, Action<Control>> themes =
            new Dictionary<Type, Action<Control>>();

        /// <summary>
        /// A collection of actions to be applied to each type if action list is not found.
        /// Faster implementation of type mapping.
        /// </summary>
        static Dictionary<Type, List<Action<Control>>> themeActions =
            new Dictionary<Type, List<Action<Control>>>();

        static void PopulateThemeActions(Type t)
        {
            Type cur = t;
            List<Action<Control>> actions = new List<Action<Control>>();
            lock (themeActions)
            {
                do
                {
                    if (themes.ContainsKey(cur))
                    {
                        actions.Add(themes[cur]);
                    }
                    if (Attribute.IsDefined(cur, typeof(ThemesBlockBaseThemeAttribute)))
                        break;
                }
                while (cur != typeof(Object));
            }
            actions.Reverse();
            themeActions[cur] = actions;
        }

        /// <summary>
        /// Applies one or more themes according to the windows forms control.
        /// </summary>
        /// <param name="ctrl"></param>
        public static void ApplayThemes(this Control ctrl)
        {
            Type t=ctrl.GetType();
            if(!themeActions.ContainsKey(t))
            {
                PopulateThemeActions(t);
            }

            // applay the themes.
            themeActions[t].ForEach(a => a(ctrl));
        }

        /// <summary>
        /// Sets a new theme by the type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        public static void SetTheme<T>(Action<T> action)
            where T:Control
        {
            themes[typeof(T)] = (c) => { action((T)c); };
            themeActions.Clear();
        }
    }
}
