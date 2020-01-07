using System;

namespace MagnetSearch.Models.SukebeiNyaa
{
    public class SukebeiNyaaException : Exception
    {
        public SukebeiNyaaException(SukebeiNyaaError error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            Error = error;
        }

        public SukebeiNyaaError Error { get; }
    }
}
