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
    }
}
