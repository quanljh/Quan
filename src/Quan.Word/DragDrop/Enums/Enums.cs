using System;

namespace Quan.Word
{

    public enum EventType
    {
        Auto,
        Tunneled,
        Bubbled,
        TunneledBubbled
    }

    /// <summary>
    /// Specifies how <see cref="T:System.Windows.Controls.ScrollViewer" /> reacts to drop operation.
    /// </summary>
    public enum ScrollingMode
    {
        None,
        HorizontalOnly,
        VerticalOnly,
        Both
    }

    [Flags]
    public enum RelativeInsertPosition
    {
        None = 0,
        BeforeTargetItem = 1,
        AfterTargetItem = 2,
        TargetItemCenter = 4
    }
}
