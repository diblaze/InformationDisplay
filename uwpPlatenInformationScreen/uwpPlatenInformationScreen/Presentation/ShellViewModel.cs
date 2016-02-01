using System;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using Intense.Presentation;

namespace uwpPlatenInformationScreen.Presentation
{
    public class ShellViewModel : NotifyPropertyChanged
    {
        private bool isSplitViewPaneOpen;
        private NavigationItem selectedBottomItem;
        private NavigationItem selectedTopItem;

        public ShellViewModel()
        {
            ToggleSplitViewPaneCommand = new RelayCommand(() => IsSplitViewPaneOpen = !IsSplitViewPaneOpen);

            // open splitview pane in wide state
            IsSplitViewPaneOpen = IsWideState();
        }

        public ICommand ToggleSplitViewPaneCommand
        {
            get;
            private set;
        }

        public bool IsSplitViewPaneOpen
        {
            get
            {
                return isSplitViewPaneOpen;
            }
            set
            {
                Set(ref isSplitViewPaneOpen, value);
            }
        }

        public NavigationItem SelectedTopItem
        {
            get
            {
                return selectedTopItem;
            }
            set
            {
                if (Set(ref selectedTopItem, value) && value != null)
                {
                    OnSelectedItemChanged(true);
                }
            }
        }

        public NavigationItem SelectedBottomItem
        {
            get
            {
                return selectedBottomItem;
            }
            set
            {
                if (Set(ref selectedBottomItem, value) && value != null)
                {
                    OnSelectedItemChanged(false);
                }
            }
        }

        public NavigationItem SelectedItem
        {
            get
            {
                return selectedTopItem ?? selectedBottomItem;
            }
            set
            {
                SelectedTopItem = TopItems.FirstOrDefault(m => m == value);
                SelectedBottomItem = BottomItems.FirstOrDefault(m => m == value);
            }
        }

        public Type SelectedPageType
        {
            get
            {
                return SelectedItem?.PageType;
            }
            set
            {
                // select associated menu item
                SelectedTopItem = TopItems.FirstOrDefault(m => m.PageType == value);
                SelectedBottomItem = BottomItems.FirstOrDefault(m => m.PageType == value);
            }
        }

        public NavigationItemCollection TopItems
        {
            get;
        } = new NavigationItemCollection();

        public NavigationItemCollection BottomItems
        {
            get;
        } = new NavigationItemCollection();

        private void OnSelectedItemChanged(bool top)
        {
            if (top)
            {
                SelectedBottomItem = null;
            }
            else
            {
                SelectedTopItem = null;
            }
            OnPropertyChanged("SelectedItem");
            OnPropertyChanged("SelectedPageType");

            // auto-close split view pane (only when not in widestate)
            if (!IsWideState())
            {
                IsSplitViewPaneOpen = false;
            }
        }

        // a helper determining whether we are in a wide window state
        // mvvm purists probably don't appreciate this approach
        private bool IsWideState()
        {
            return Window.Current.Bounds.Width >= 1024;
        }
    }
}