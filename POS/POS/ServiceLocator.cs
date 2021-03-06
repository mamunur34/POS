﻿using System;
using System.Windows.Forms;
using POS.Internals;
using POS.Internals.I18N;
using POS.Models;

namespace POS
{
    public static class ServiceLocator
    {
        public static History<Product> ProductHistory = new History<Product>();

        public static string DataPath
        {
            get
            {
                return string.Format("{0}\\data\\", Application.StartupPath);
            }
        }

        public static Catalog LanguageCatalog = new Catalog();
    }
}