using System.ComponentModel;
using mrfpc.modloader.Template.Configuration;
using FileEmulationFramework.Lib.Utilities;

namespace mrfpc.modloader.Configuration
{
    public class Config : Configurable<Config>
    {
        [DisplayName("Log Level")]
        [Description("Declares which elements should be logged to the console.\nMessages less important than this level will not be logged.")]
        [DefaultValue(LogSeverity.Information)]
        public LogSeverity LogLevel { get; set; } = LogSeverity.Information;

        [DisplayName("Intro Skip")]
        [DefaultValue(false)]
        public bool IntroSkip { get; set; } = false;

        [DisplayName("Render In Background")]
        [DefaultValue(false)]
        public bool RenderInBackground { get; set; } = false;

        [DisplayName("Force 4k Assets")]
        [DefaultValue(false)]
        public bool Force4k { get; set; } = false;
    }

    /// <summary>
    /// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
    /// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
    /// </summary>
    public class ConfiguratorMixin : ConfiguratorMixinBase
    {
        // 
    }
}
