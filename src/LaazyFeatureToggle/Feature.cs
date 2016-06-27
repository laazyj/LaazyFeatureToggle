using System;
using System.Configuration;

namespace laazy.FeatureToggle
{
    /// <summary>
    /// Simple feature-switch class which looks up features in app settings.
    /// </summary>
    public static class Feature
    {
        const string EnabledFormat = "{0}.Enabled";

        /// <summary>
        /// Default backing storage is provided by ConfigurationManager.AppSettings
        /// </summary>
        static readonly Func<string, string> DefaultGetValueFunction =
            name => ConfigurationManager.AppSettings[name];

        /// <summary>
        /// To enable unit testing you can provide a custom backing storage for the configuration values.
        /// </summary>
        public static void UseBackingStorage(Func<string, string> nameValueFunction)
        {
            _getConfigValue = nameValueFunction;
        }

        /// <summary>
        /// Reset to default ConfigurationManager.AppSettings backing storage.
        /// </summary>
        public static void ResetDefaultBackingStorage()
        {
            _getConfigValue = DefaultGetValueFunction;
        }

        static Func<string, string> _getConfigValue = DefaultGetValueFunction;

        /// <summary>
        /// Determines whether a feature is specifically enabled by looking for a ".Enabled" app setting for that feature.
        /// Use for features that are disabled by default.
        /// </summary>
        public static bool Enabled(string feature)
        {
            bool enabled;
            bool.TryParse(_getConfigValue(string.Format(EnabledFormat, feature)), out enabled);
            return enabled;
        }

        /// <summary>
        /// Determines whether a feature has been specifically disabled by looking for a ".Enabled" app setting for that feature.
        /// Use fo features that are enabled by default.
        /// </summary>
        public static bool Disabled(string feature)
        {
            bool enabled;
            return bool.TryParse(_getConfigValue(string.Format(EnabledFormat, feature)), out enabled) &&
                   !enabled;
        }
    }
}