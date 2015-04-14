﻿using System;
using System.Collections.Generic;
using System.Linq;
using POS.Internals;

namespace POS
{
    public class DbContext
    {
        public static string AddTable<T>()
        {
            var t = typeof(T);

            string sqlsc = "CREATE TABLE " + t.Name + "s (";
            foreach (var p in t.GetProperties())
            {
                sqlsc += "\n " + p.Name + " ";
                if (p.PropertyType.ToString().Contains("System.Int32"))
                    sqlsc += " int ";
                else if (p.PropertyType.ToString().Contains("System.DateTime"))
                    sqlsc += " datetime ";
                else
                    sqlsc += " varchar(255) ";

                sqlsc += ",";
            }
            return sqlsc.Substring(0, sqlsc.Length - 1) + ")";
        }

        public static void Add<T>(T obj)
        {
            SqlHelper.query("INSERT INTO " + typeof(T).Name + "s VALUES ()");
        }

        public static IEnumerable<T> GetItems<T>()
            where T : new()
        {
            var q = SqlHelper.query("SELECT * FROM " + typeof(T).Name + "s");
            var arr = SqlHelper.fetch_array(q);

            var ret = new List<T>();

            foreach (var p in arr)
            {
                foreach (var item in p)
                {
                    var tmp = new T();
                    foreach (var prop in tmp.GetType().GetProperties())
                    {
                        if (prop.Name.ToLower() == item.Key.ToLower())
                        {
                            if (item.Value != null)
                            {
                                object v;

                                if (prop.PropertyType.Name == typeof(byte[]).Name)
                                {
                                    try
                                    {
                                        v = Convert.FromBase64String(item.Value.ToString());
                                    }
                                    catch
                                    {
                                        v = item.Value;
                                    }
                                }
                                if (prop.PropertyType.Name == typeof(double).Name)
                                {
                                    try
                                    {
                                        v = double.Parse(item.Value.ToString());
                                    }
                                    catch
                                    {
                                        v = item.Value;
                                    }
                                }
                                else
                                {
                                    v = item.Value;
                                }

                                if (prop.CanWrite)
                                    prop.SetValue(tmp, v);
                            }
                        }
                    }

                    if (!ret.Contains(tmp))
                        ret.Add(tmp);
                }
            }

            return ret;
        }

        public static string Insert(object obj)
        {
            return null;
        }
    }
}