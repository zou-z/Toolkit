using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Toolkit.Base.Log;
using Toolkit.Message;

namespace Toolkit.View
{
    internal partial class MainMenuView : Window, IRecipient<MainMenuHideMessage>
    {
        public MainMenuView(Rect notifyIconPosition, Action exit)
        {
            InitializeComponent();
            this.notifyIconPosition = notifyIconPosition;
            this.exit = exit;
            WeakReferenceMessenger.Default.Register(this);
            Logger.Trace("Main Menu Initialized");
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            Size size = GetContentSize();
            Left = notifyIconPosition.Left - BorderThickness.Left;
            Top = notifyIconPosition.Top - size.Height - BorderThickness.Top - BorderThickness.Bottom;
            Logger.Trace($"Main Menu Source Initialized, Position: ({Left},{Top})");
        }

        private Size GetContentSize()
        {
            UIElement uiElement = (UIElement)Content;
            uiElement.Measure(new Size
            {
                Width = SystemParameters.PrimaryScreenWidth,
                Height = SystemParameters.PrimaryScreenHeight,
            });
            return uiElement.DesiredSize;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            exit?.Invoke();
        }

        public void Receive(MainMenuHideMessage message)
        {
            Hide();
        }

        private readonly Action exit;
        private readonly Rect notifyIconPosition;
    }
}
