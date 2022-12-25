using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonFormat.Service
{
    internal interface IJsonFormatView
    {
        string GetText();

        void ClearContent();

        void CompressedContent();

        void AddEscape();

        void RemoveEscape();

        IJsonFormat? GetDataContext();

        void ApplySettingsUpdate();
    }
}
