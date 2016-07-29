using Windows.System.Profile;

namespace Imedit.Helpers
{
    public class Platform
    {
        public static PlatformEnum GetCurrent()
        {
            string what = AnalyticsInfo.VersionInfo.DeviceFamily.ToLower();

            switch (what)
            {
                case "windows.mobile":
                    return PlatformEnum.Mobile;
                case "windows.desktop":
                    return PlatformEnum.Desktop;
                default:
                    return PlatformEnum.Other;
            }
        }

        public enum PlatformEnum
        {
            Mobile,
            Desktop,
            Other
        }
    }
}