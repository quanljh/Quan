using System.Windows;
using System.Windows.Input;

namespace Quan.Word.Core
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Field

        private Window mWindow;

        #endregion

        #region Properties
        /// <summary>
        /// The smallest width the window can go to
        /// </summary>
        public double WindowMinimumWidth { get; set; } = 800;

        /// <summary>
        /// The smallest height the window can go to
        /// </summary>
        public double WindowMinimumHeight { get; set; } = 500;

        //private bool _borderless;

        //public bool Borderless
        //{
        //    get => _borderless;
        //    set => SetProperty(ref _borderless, mWindow.WindowState == WindowState.Maximized || mDockPosition != WindowDockPosition.Undocked);
        //}

        private int _resizeBorder = 6;

        public int ResizeBorder
        {
            get => _resizeBorder;
            set => SetProperty(ref _resizeBorder, value);
        }

        private int _outerMarginSize = 10;

        public int OuterMarginSize
        {
            get => mWindow.WindowState == WindowState.Maximized ? 0 : _outerMarginSize;
            set => SetProperty(ref _outerMarginSize, value);
        }

        private int _windowRadius = 10;

        public int WindowRadius
        {
            get => mWindow.WindowState == WindowState.Maximized ? 0 : _windowRadius;
            set => SetProperty(ref _windowRadius, value);
        }

        public Thickness ResizeBorderThickness => new Thickness(ResizeBorder + OuterMarginSize);

        /// <summary>
        /// The padding of the inner content of the main window
        /// </summary>
        public Thickness InnerContentPadding => new Thickness(0);

        public Thickness OuterMarginSizeThickness => new Thickness(OuterMarginSize);

        public CornerRadius WindowCornerRadius => new CornerRadius(WindowRadius);

        public int CaptionHeight { get; set; } = 42;

        public GridLength TitleHeightGridLength => new GridLength(CaptionHeight + ResizeBorder);

        /// <summary>
        /// True if we should show the settings menu
        /// </summary>
        public bool DimmedOverlayVisible { get; set; }

        #endregion

        #region Commands

        public ICommand MinimizeCommand { get; set; }

        public ICommand MaximizeCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        public ICommand MenuCommand { get; set; }

        #endregion

        #region Constructor

        public MainWindowViewModel(Window window)
        {
            mWindow = window;
            mWindow.StateChanged += (Sender, e) => { WindowResize(); };
            //Command
            MaximizeCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
            MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            CloseCommand = new RelayCommand(() => mWindow.Close());
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(mWindow, GetMousePosition()));
        }

        #endregion

        #region Method

        private void WindowResize()
        {
            //To notify the UI or the bound Elements that the data has changed.So that Elements can show the new value
            RaisePropertyChanged(nameof(ResizeBorderThickness));
            RaisePropertyChanged(nameof(OuterMarginSize));
            RaisePropertyChanged(nameof(OuterMarginSizeThickness));
            RaisePropertyChanged(nameof(WindowRadius));
            RaisePropertyChanged(nameof(WindowCornerRadius));
        }

        public Point GetMousePosition()
        {
            var position = Mouse.GetPosition(mWindow);

            if (mWindow.WindowState == WindowState.Maximized)
            {
                return new Point(position.X - ResizeBorder, position.Y - ResizeBorder);
            }
            else
            {
                return new Point(position.X + mWindow.Left, position.Y + mWindow.Top);
            }

        }

        #endregion
    }
}



