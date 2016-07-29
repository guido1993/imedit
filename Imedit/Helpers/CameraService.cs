using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;

namespace Imedit.Helpers
{
    public class CameraService
    {
        public static Task<bool> HasCamera
        {
            get { return FindCameraDeviceByPanelAsync(Panel.Back); }
        }

        private static async Task<bool> FindCameraDeviceByPanelAsync(Panel desiredPanel)
        {
            // Get available devices for capturing pictures
            var allVideoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
            return allVideoDevices.Count > 0;
        }
    }
}