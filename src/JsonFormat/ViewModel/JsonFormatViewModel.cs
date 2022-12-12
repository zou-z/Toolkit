using JsonFormat.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JsonFormat.ViewModel
{
    internal class JsonFormatViewModel : IJsonFormat
    {
        public JsonFormatViewModel()
        {
        }

        public async Task Format(string text)
        {
            await Task.Delay(3000);
            Debug.WriteLine("Format()");
            Debug.WriteLine(text);
        }
    }
}
