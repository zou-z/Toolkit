using System;

namespace JsonFormat.Model
{
    internal static class MessageToken
    {
        public static string SettingsUpdated { get; } = Guid.NewGuid().ToString();

        public static string SettingsUpdateFinished { get; } = Guid.NewGuid().ToString();
    }
}
