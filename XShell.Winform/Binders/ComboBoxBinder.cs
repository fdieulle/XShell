using System;
using System.ComponentModel;
using System.Windows.Forms;
using XShell.Core;

namespace XShell.Winform.Binders
{
    public class ComboBoxBinder : AbstractBinder<ComboBox>
    {
        private readonly ICollectionSelector _selector;
        private IDisposable _itemsSuscription;

        public ComboBoxBinder(ComboBox comboBox, ICollectionSelector selector)
            :base(comboBox)
        {
            _selector = selector;

            Control.SelectedIndex = _selector.SelectedIndex;
            UpdateItems();

            Control.SelectedIndexChanged += OnComboBoxSelectedIndexChanged;
            _selector.PropertyChanged += OnSelectorPropertyChanged;
        }

        private void OnComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            _selector.SelectedIndex = Control.SelectedIndex;
        }

        private void OnSelectorPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case Core.Properties.SELECTED_INDEX:
                    Control.SelectedIndex = _selector.SelectedIndex;
                    break;
                case Core.Properties.ITEMS:
                    UpdateItems();
                    break;
                case Core.Properties.NULL:
                    Control.SelectedIndex = _selector.SelectedIndex;
                    UpdateItems();
                    break;
            }
        }

        private void UpdateItems()
        {
            _itemsSuscription?.Dispose();
            _itemsSuscription = Control.Items.Bind(_selector.Items);
        }

        protected override void Disposing()
        {
            _itemsSuscription?.Dispose();

            Control.SelectedIndexChanged -= OnComboBoxSelectedIndexChanged;
            _selector.PropertyChanged -= OnSelectorPropertyChanged;
        }
    }
}
