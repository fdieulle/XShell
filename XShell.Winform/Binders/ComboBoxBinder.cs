using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using XShell.Core;

namespace XShell.Winform.Binders
{
    public class ComboBoxBinder : IDisposable
    {
        private readonly ComboBox comboBox;
        private readonly ICollectionSelector selector;
        private IDisposable itemsSuscription;

        public ComboBoxBinder(ComboBox comboBox, ICollectionSelector selector)
        {
            this.comboBox = comboBox;
            this.selector = selector;

            this.comboBox.SelectedIndex = this.selector.SelectedIndex;
            this.UpdateItems();

            this.comboBox.SelectedIndexChanged += OnComboBoxSelectedIndexChanged;
            this.selector.PropertyChanged += OnSelectorPropertyChanged;
        }

        private void OnComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            this.selector.SelectedIndex = this.comboBox.SelectedIndex;
        }

        private void OnSelectorPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case Properties.SELECTED_INDEX:
                    this.comboBox.SelectedIndex = this.selector.SelectedIndex;
                    break;
                case Properties.ITEMS:
                    this.UpdateItems();
                    break;
            }
        }

        private void UpdateItems()
        {
            if(this.itemsSuscription != null)
                this.itemsSuscription.Dispose();

            if (this.selector.Items is INotifyCollectionChanged)
                this.itemsSuscription = this.comboBox.Items.Bind(this.selector.Items);
            else
            {
                this.comboBox.Items.Clear();
                if (this.selector.Items != null)
                {
                    foreach (var item in this.selector.Items)
                        this.comboBox.Items.Add(item);
                }
            }
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (this.itemsSuscription != null) this.itemsSuscription.Dispose();

            this.comboBox.SelectedIndexChanged += OnComboBoxSelectedIndexChanged;
            this.selector.PropertyChanged -= OnSelectorPropertyChanged;
        }

        #endregion
    }
}
