using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Imedit.Views
{
    [TemplatePart(Name = "DelButton", Type = typeof(Button))]
    public sealed class CustomEditBox : RichEditBox
    {
        private Action DeleteAction { get; }

        public int CreationIndex { get; }

        private Button _delButton;
        
        public CustomEditBox(int creationIndex, Action action)
        {
            DefaultStyleKey = typeof(CustomEditBox);

            CreationIndex = creationIndex;

            DeleteAction = action;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _delButton = GetTemplateChild("DelButton") as Button;
            if (_delButton != null)
                _delButton.Click += DelClicked;
        }

        private void DelClicked(object sender, RoutedEventArgs e)
        {
            DeleteAction.Invoke();
        }
    }
}
