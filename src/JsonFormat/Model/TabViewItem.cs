using CommunityToolkit.Mvvm.ComponentModel;

namespace JsonFormat.Model
{
    internal class TabViewItem : ObservableObject
    {
        public string Header
        {
            get => header;
            set => SetProperty(ref header, value);
        }

        public object? Content { get; set; } = null;

        private string header = string.Empty;
    }
}
