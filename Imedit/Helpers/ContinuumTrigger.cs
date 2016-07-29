using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Imedit.Helpers
{
    public class ContinuumTrigger : StateTriggerBase
    {
        public bool WithContinuum
        {
            get { return (bool)GetValue(DeviceTypeProperty); }
            set { SetValue(DeviceTypeProperty, value); }
        }

        public static readonly DependencyProperty DeviceTypeProperty =
                DependencyProperty.Register("WithContinuum", typeof(bool), typeof(ContinuumTrigger),
                new PropertyMetadata(false, OnDeviceTypePropertyChanged));

        private static void OnDeviceTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (ContinuumTrigger)d;
            var val = (bool)e.NewValue;
            var isContinuum = ProjectionManager.ProjectionDisplayAvailable;

            obj.SetActive(val == isContinuum);
        }
    }
}