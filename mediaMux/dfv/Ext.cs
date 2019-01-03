using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace df
{
    public static class Ext
    {

        static Dictionary<Control, EventHandler> controlEvent = new Dictionary<Control, EventHandler>();

        public static List<T> MapToList<T>(this int num, Func<int, T> func)
        {
            var list = new List<T>();
            for (int i = 0; i < num; i++)
            {
                list.Add(func(i));
            }
            return list;
        }

        public static void setIcon(this Form form)
        {
            try
            {
                form.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// bind control text to obj property
        /// </summary>
        /// <param name="control"></param>
        /// <param name="obj"></param>
        /// <param name="onSet"></param>
        public static void bindText<T>(this T control, Expression<Func<string>> obj, Action onSet = null) where T : Control
        {
            var info = obj.Body;


            controlEvent.GetVal(control, oldEvent =>
            {
                control.TextChanged -= oldEvent;
            });

            var ret = GetExpressionLastObject(info);
            if (ret != null)
                control.Text = ret + "";


            var newEvent = new EventHandler((s, e) =>
              {
                  if (onSet != null)
                      onSet();
                  else
                      SetExpressionLastObject(info, control.Text);
              });
            controlEvent[control] = newEvent;
            control.TextChanged += newEvent;
        }

        /// <summary>
        /// bind ComboBox index to obj property
        /// </summary>
        /// <param name="control"></param>
        /// <param name="obj"></param>
        /// <param name="onSet"></param>
        public static void bindIndex(this ComboBox control, Expression<Func<int>> obj, Action onSet = null)
        {
            var info = obj.Body;

            controlEvent.GetVal(control, oldEvent =>
            {
                control.SelectedIndexChanged -= oldEvent;
            });

            var ret = GetExpressionLastObject(info);
            if (ret != null)
                control.SelectedIndex = (int)ret;


            var newEvent = new EventHandler((s, e) =>
            {
                if (onSet != null)
                    onSet();
                else
                    SetExpressionLastObject(info, control.SelectedIndex);
            });
            controlEvent[control] = newEvent;
            control.SelectedIndexChanged += newEvent;
        }

        public static string GetStrVal(this IDictionary<string, string> source, string key)
        {
            var val = "";
            if (source.TryGetValue(key, out val))
                return val;
            return "";
        }
        public static bool GetVal<T, T2>(this IDictionary<T, T2> source, T key, Action<T2> func)
        {
            T2 val;
            if (source.TryGetValue(key, out val))
            {
                func(val);
                return true;
            }
            return false;
        }

        public static string JoinStr<TSource>(this IEnumerable<TSource> source, string op, Func<TSource, string> func)
        {
            var ret = "";
            foreach (var a in source)
            {
                var ss = func(a);
                if (ss != null) { 
                    ret += ss + op;
                }
            }
            if (ret.Length > 0)
                ret = ret.Substring(0, ret.Length - op.Length);
            return ret;
        }

        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> func)
        {
            foreach (var a in source)
            {
                func(a);
            }
            return source;
        }

        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource, int> func)
        {
            int i = 0;
            foreach (var a in source)
            {
                func(a, i);
                i++;
            }
            return source;
        }

        public static List<TSource> Flatten<TSource>(this IEnumerable<IEnumerable<TSource>> source)
        {
            List<TSource> t = new List<TSource>();
            foreach (var a in source)
            {
                foreach (var a2 in a)
                {
                    t.Add(a2);
                }
            }
            return t;
        }

        public static List<TSource> Flatten<TSource>(this IEnumerable<IEnumerable<TSource>> source, Func<TSource, bool> func)
        {
            List<TSource> t = new List<TSource>();
            foreach (var a in source)
            {
                foreach (var a2 in a)
                {
                    if (func(a2))
                        t.Add(a2);
                }
            }
            return t;
        }



        //
        static object GetExpressionLastObject(Expression member)
        {
            if (member == null)
                return null;

            if (member.NodeType == ExpressionType.Constant)
                return (member as ConstantExpression).Value;
            else if (member.NodeType == ExpressionType.MemberAccess)
            {
                var field = (member as MemberExpression);

                if (field.Member.MemberType == MemberTypes.Field)
                {
                    var obj = GetExpressionLastObject(field.Expression);
                    return (field.Member as FieldInfo).GetValue(obj);
                }
                else if (field.Member.MemberType == MemberTypes.Property)
                    return (field.Member as PropertyInfo).GetValue(GetExpressionLastObject(field.Expression), null);
                else
                    return null;
            }
            return null;
        }

        //
        static void SetExpressionLastObject(Expression member, object val)
        {
            if (member == null)
                return;

            if (member.NodeType == ExpressionType.MemberAccess)
            {
                var field = (member as MemberExpression);
                var parent = GetExpressionLastObject(field.Expression);

                if (field.Member.MemberType == MemberTypes.Field)
                {
                    (field.Member as FieldInfo).SetValue(parent, val);
                }
                else if (field.Member.MemberType == MemberTypes.Property)
                {
                    (field.Member as PropertyInfo).SetValue(parent, val);
                }
            }
        }
    }
}
