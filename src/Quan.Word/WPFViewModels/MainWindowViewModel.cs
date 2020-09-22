using Quan.Word.Core;
using System.Windows;
using System.Windows.Input;

namespace Quan.Word
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Private Members

        /// <summary>
        /// The window this view model controls
        /// </summary>
        private Window mWindow;

        /// <summary>
        /// The window resizer helper that keeps the window size correct in various states
        /// </summary>
        private WindowResizer mWindowResizer;

        /// <summary>
        /// The last known dock position
        /// </summary>
        private WindowDockPosition mDockPosition = WindowDockPosition.Undocked;

        #endregion

        #region Public Properties

        /// <summary>
        /// The smallest width the window can go to
        /// </summary>
        private double _windowMinimumWidth = 800;

        public double WindowMinimumWidth
        {
            get => _windowMinimumWidth;
            set => SetProperty(ref _windowMinimumWidth, value);
        }

        /// <summary>
        /// The smallest height the window can go to
        /// </summary>
        private double _windowMinimumHeight = 500;

        public double WindowMinimumHeight
        {
            get => _windowMinimumHeight;
            set => SetProperty(ref _windowMinimumHeight, value);
        }

        /// <summary>
        /// True if the window is currently being moved/dragged
        /// </summary>
        private bool _beingMoved;

        public bool BeingMoved
        {
            get => _beingMoved;
            set => SetProperty(ref _beingMoved, value);
        }

        /// <summary>
        /// True if the window should be borderless because it is docked or maximized
        /// </summary>
        public bool Borderless => (mWindow.WindowState == WindowState.Maximized || mDockPosition != WindowDockPosition.Undocked);

        /// <summary>
        /// The size of the resize border around the window
        /// </summary>
        public int ResizeBorder => mWindow.WindowState == WindowState.Maximized ? 0 : 4;

        /// <summary>
        /// The size of the resize border around the window, taking into account the outer margin
        /// </summary>
        public Thickness ResizeBorderThickness => new Thickness(OuterMarginSize.Left + ResizeBorder,
                                                                OuterMarginSize.Top + ResizeBorder,
                                                                OuterMarginSize.Right + ResizeBorder,
                                                                OuterMarginSize.Bottom + ResizeBorder);

        /// <summary>
        /// The padding of the inner content of the main window
        /// </summary>
        private Thickness _innerContentPadding = new Thickness(0);

        public Thickness InnerContentPadding
        {
            get => _innerContentPadding;
            set => SetProperty(ref _innerContentPadding, value);
        }

        /// <summary>
        /// The margin around the window to allow for a drop shadow
        /// </summary>
        private Thickness _outerMarginSize = new Thickness(5);

        public Thickness OuterMarginSize
        {
            get => mWindow.WindowState == WindowState.Maximized ? mWindowResizer.CurrentMonitorMargin : (Borderless ? new Thickness(0) : _outerMarginSize);
            set
            {
                if (SetProperty(ref _outerMarginSize, value))
                    RaisePropertyChanged(nameof(ResizeBorderThickness));
            }
        }

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        private int _windowRadius = 10;

        public int WindowRadius
        {
            get => Borderless ? 0 : _windowRadius;
            set => SetProperty(ref _windowRadius, value);
        }

        /// <summary>
        /// The rectangle border around the window when docked
        /// </summary>
        public int FlatBorderThickness => Borderless && mWindow.WindowState != WindowState.Maximized ? 1 : 0;

        /// <summary>
        /// The radius of the edges of the window
        /// </summary>
        public CornerRadius WindowCornerRadius => new CornerRadius(WindowRadius);


        /// <summary>
        /// The height of the title bar / caption of the window
        /// </summary>
        private int _titleHeight = 42;

        public int TitleHeight
        {
            get => _titleHeight;
            set => SetProperty(ref _titleHeight, value);
        }

        /// <summary>
        /// The height of the title bar / caption of the window
        /// </summary>
        public GridLength TitleHeightGridLength => new GridLength(TitleHeight + ResizeBorder);


        /// <summary>
        /// True if we should have a dimmed overlay on the window
        /// such as when a popup is visible or the window is not focused
        /// </summary>
        private bool _dimmableOverlayVisible;

        public bool DimmableOverlayVisible
        {
            get => _dimmableOverlayVisible;
            set => SetProperty(ref _dimmableOverlayVisible, value);
        }

        #endregion

        #region Commands

        /// <summary>
        /// The command to minimize the window
        /// </summary>
        public ICommand MinimizeCommand { get; set; }

        /// <summary>
        /// The command to maximize the window
        /// </summary>
        public ICommand MaximizeCommand { get; set; }

        /// <summary>
        /// The command to close the window
        /// </summary>
        public ICommand CloseCommand { get; set; }

        /// <summary>
        /// The command to show the system menu of the window
        /// </summary>
        public ICommand MenuCommand { get; set; }

        #endregion

        #region Constructor

        public MainWindowViewModel(Window window)
        {
            mWindow = window;

            // Listen out for the window resizing
            mWindow.StateChanged += (s, e) =>
            {
                // Fire off events for all properties that are affected by a resize
                WindowResized();
            };

            // Create Commands
            MaximizeCommand = new RelayCommand(() => mWindow.WindowState ^= WindowState.Maximized);
            MinimizeCommand = new RelayCommand(() => mWindow.WindowState = WindowState.Minimized);
            CloseCommand = new RelayCommand(() => mWindow.Close());
            MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(mWindow, GetMousePosition()));

            // Fix window resize issue
            mWindowResizer = new WindowResizer(mWindow);

            // Listen out for dock changes
            mWindowResizer.WindowDockChanged += (dock) =>
            {
                // Store last position
                mDockPosition = dock;

                // Fire off resize events
                WindowResized();
            };

            // On window being moved/dragged
            mWindowResizer.WindowStartedMove += () =>
            {
                // Update being moved flag
                BeingMoved = true;
            };

            // Fix dropping an undocked window at top which should be positioned at the
            // very top of screen
            mWindowResizer.WindowFinishedMove += () =>
            {
                // Update being moved flag
                BeingMoved = false;

                // Check for moved to top of window and not at an edge
                if (mDockPosition == WindowDockPosition.Undocked && mWindow.Top == mWindowResizer.CurrentScreenSize.Top)
                    // If so, move it to the true top (the border size)
                    mWindow.Top = -OuterMarginSize.Top;
            };
        }

        #endregion

        #region Method

        /// <summary>
        /// If the window resizes to a special position (docked or maximized)
        /// this will update all required property change events to set the borders and radius values
        /// </summary>
        private void WindowResized()
        {
            // Fire off events for all properties that are affected by a resize
            RaisePropertyChanged(nameof(Borderless));
            RaisePropertyChanged(nameof(FlatBorderThickness));
            RaisePropertyChanged(nameof(ResizeBorderThickness));
            RaisePropertyChanged(nameof(OuterMarginSize));
            RaisePropertyChanged(nameof(WindowRadius));
            RaisePropertyChanged(nameof(WindowCornerRadius));
        }

        /// <summary>
        /// Gets the current mouse position on the screen
        /// </summary>
        /// <returns></returns>
        public Point GetMousePosition()
        {
            return mWindowResizer.GetCursorPosition();
        }

        #endregion
    }
}



