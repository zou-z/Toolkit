﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonFormat.Service
{
    internal interface IJsonFormatView
    {
        string GetText();

        IJsonFormat? GetDataContext();

        void ApplySettingsUpdate();
    }
}
