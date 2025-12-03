using System;

namespace Blog.Presentation.Shared.Options;

public sealed class AppSettingDataContext
{
    private AppSettingDataContext()
        {
        }

        private static AppSettingDataContext instance = null;

        public static AppSettingDataContext Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppSettingDataContext();
                }
                return instance;
            }
        }

        public bool EnableErrorDetail { get; set; }
}
