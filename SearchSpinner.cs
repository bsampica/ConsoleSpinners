using Spectre.Console;

namespace SaveFileWatcher
{
    public sealed class SearchSpinner : Spinner
    { // The interval for each frame
        
        public SearchSpinner() {
            // default constructor
        }
        
        public override TimeSpan Interval => TimeSpan.FromMilliseconds(50);
        

        // Whether or not the spinner contains unicode characters
        public override bool IsUnicode => true;

        // The individual frames of the spinner
        public override IReadOnlyList<string> Frames =>
            [
               ".", "*", "O", "@"
        ];


    }
}