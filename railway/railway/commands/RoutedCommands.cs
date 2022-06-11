using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace railway.Commands
{
    public static class RoutedCommands
    {

        public static readonly RoutedUICommand CloseWindow = new RoutedUICommand(
            "Close Window",
            "CloseWindow",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.W, ModifierKeys.Control),
            }
            );

        public static readonly RoutedUICommand BuyTicket = new RoutedUICommand(
            "Buy Ticket",
            "BuyTicket",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.B, ModifierKeys.Control),
            }
            );
        
        
        public static readonly RoutedUICommand DrivingLines = new RoutedUICommand(
            "Driving Lines",
            "DrivingLines",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.M, ModifierKeys.Control),
            }
        );
        
        
        public static readonly RoutedUICommand ScheduleLines = new RoutedUICommand(
            "Schedule Lines",
            "ScheduleLines",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.R, ModifierKeys.Control),
            }
        );
        
        public static readonly RoutedUICommand Trains = new RoutedUICommand(
            "Trains",
            "Trains",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.T, ModifierKeys.Control),
            }
        );
        
        
        public static readonly RoutedUICommand Report = new RoutedUICommand(
            "Report",
            "Report",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.I, ModifierKeys.Control),
            }
        );
        
        public static readonly RoutedUICommand Save = new RoutedUICommand(
            "Save",
            "Save",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.S, ModifierKeys.Control),
            }
        );
        
        public static readonly RoutedUICommand StartTutorial = new RoutedUICommand(
            "Save",
            "Save",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.P, ModifierKeys.Control),
            }
        );


        public static readonly RoutedUICommand Report2 = new RoutedUICommand(
            "Report2",
            "Report2",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.O, ModifierKeys.Control),
            }   
        );
        
        public static readonly RoutedUICommand CloseMessageBox = new RoutedUICommand(
            "Close",
            "Close",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Enter),
            }
        );

    }
}
