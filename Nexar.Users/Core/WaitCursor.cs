using System;
using System.Windows.Forms;

namespace Nexar.Users
{
    public class WaitCursor : IDisposable
    {
        readonly Cursor _lastCursor;

        public WaitCursor()
        {
            _lastCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
        }

        public void Dispose()
        {
            Cursor.Current = _lastCursor;
        }
    }
}
