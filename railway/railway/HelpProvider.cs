using railway.managerSchedule;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using railway.defineDrivingLine;

namespace railway
{
    public class HelpProvider
    {
        public static string GetHelpKey(DependencyObject obj)
        {
            return obj.GetValue(HelpKeyProperty) as string;
        }

        public static void SetHelpKey(DependencyObject obj, string value)
        {
            obj.SetValue(HelpKeyProperty, value);
        }

        public static readonly DependencyProperty HelpKeyProperty=
            DependencyProperty.RegisterAttached("HelpKey", typeof(string), typeof(HelpProvider), new PropertyMetadata("index", HelpKey));
        private static void HelpKey(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //NOOP
        }

        public static void ShowHelp(string key, ManagerSchedule originator)
        {
            HelpViewer hh = new HelpViewer(key, originator);
            hh.Show();
        }


        public static void ShowHelp(string key, AddNewScheduleWindow originator)
        {
            HelpViewer hh = new HelpViewer(key, originator);
            hh.Show();
        }

        public static void ShowHelp(string key, ChangeTrafficDay originator)
        {
            HelpViewer hh = new HelpViewer(key, originator);
            hh.Show();
        }

        public static void ShowHelp(string key, ViewDrivingLines originator)
        {
            HelpViewer hh = new HelpViewer(key, originator);
            hh.Show();
        }

        public static void ShowHelp(string key, DefineSimpleDataForDrivingLine originator)
        {
            HelpViewer hh = new HelpViewer(key, originator);
            hh.Show();
        }

        public static void ShowHelp(string key, DefineEndDateForDrivingLine originator)
        {
            HelpViewer hh = new HelpViewer(key, originator);
            hh.Show();
        }

        public static void ShowHelp(string key, AddDrivingLineSimple originator)
        {
            HelpViewer hh = new HelpViewer(key, originator);
            hh.Show();
        }

        public static void ShowHelp(string key, AddDrivingLine originator)
        {
            HelpViewer hh = new HelpViewer(key, originator);
            hh.Show();
        }

        public static void ShowHelp(string key, ManagerHomePage originator)
        {
            HelpViewer hh = new HelpViewer(key, originator);
            hh.Show();
        }
        public static void ShowHelp(string key, DefineSimpleDataForDrivingLineModal originator)
        {
            HelpViewer hh = new HelpViewer(key, originator);
            hh.Show();
        }

    }
    
}
