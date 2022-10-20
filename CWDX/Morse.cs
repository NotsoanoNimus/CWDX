using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;


/// <summary>
/// Terminology:
///    - Symbol = individual DIT, DAH, or SPACE
///    - Character = character created from a sequence of symbols and spacing between symbols
///    - Word = string of characters with appropriate per-character spacing, which don't contain a space CHARACTER (obviously)
/// </summary>
namespace CWDX {

    /// <summary>
    /// Define a list of characters which comprise the Morse "alphabet" when outputting Morse-only text.
    /// </summary>
    public static class MorseSymbolCharacter {
        public const char DIT_CHAR = '.';   // DIT character
        public const char DAH_CHAR = '-';   // DAH character
        public const char SPACE_CHAR = ' ';   // SPACE character
    };

    /// <summary>
    /// Contains constant timing factors used to determine the length of a tone scaled by the selected WPM speed.
    /// </summary>
    public static class MorseSymbolDuration {
        public const double DIT = 1.00;   // Length of one short tone. Unit measurement off of which other lengths are based.
        public const double DAH = (3 * DIT);   // Length of one long tone and also the space between Morse characters.
        public const double SPACE = (1 * DIT);   // Length of each space between whole words. (was 5 DITs)
        public const double PER_SYMBOL = DIT;   // Length of each space between individual symbols.
        public const double PER_CHARACTER = (DAH - PER_SYMBOL);   // Length of the space between characters. Accounts for each symbol having a trailing PER_SYMBOL length.
    };

    /// <summary>
    /// Represents an individual Morse Code symbol. Associates a Character to a Duration.
    /// <see cref="Morse"/>
    /// </summary>
    public sealed record MorseSymbol( char Representation, double Duration );

    /// <summary>
    /// Represents an individual Morse Code character or single Prosign object as a collated sequence of symbols.
    /// </summary>
    public sealed record MorseCharacter( string Representation, MorseSymbol[] Sequence ) {
        internal double _dur;
        public double TotalDuration {
            get => _dur;
            init {
                // Add each symbol and the gap.
                foreach ( var x in Sequence )
                    this._dur += (x.Duration + MorseSymbolDuration.PER_SYMBOL);

                // Lastly, add on the trailing character spacing.
                this._dur += MorseSymbolDuration.PER_CHARACTER;
            }
        }
    };

    /// <summary>
    /// Represents all static definitions and methods for the Morse Code "protocol". Includes dictionaries for mapping symbols back and forth
    /// and also a way to get a time-sequence representation of a transmit string, for passing to the application audio handler.
    /// </summary>
    /// <seealso cref="MorseSymbol"/>
    public static class Morse {

        // Define a set of constant pairings between characters and their time factors.
        public static readonly MorseSymbol DIT = new( MorseSymbolCharacter.DIT_CHAR, MorseSymbolDuration.DIT );
        public static readonly MorseSymbol DAH = new( MorseSymbolCharacter.DAH_CHAR, MorseSymbolDuration.DAH );
        public static readonly MorseSymbol SPACE = new( MorseSymbolCharacter.SPACE_CHAR, MorseSymbolDuration.SPACE );

        /// <summary>
        /// Create the Morse alphabet for all characters and prosigns, and their corresponding (unspaced) symbol sequences.
        /// </summary>
        public static readonly MorseCharacter[] Alphabet = {
            new( " ", new[]{ SPACE } ),

            new( "A", new[]{ DIT, DAH } ),
            new( "B", new[]{ DAH, DIT, DIT } ),
            new( "C", new[]{ DAH, DIT, DAH, DIT } ),
            new( "D", new[]{ DAH, DIT, DIT } ),
            new( "E", new[]{ DIT } ),
            new( "F", new[]{ DIT, DIT, DAH, DIT } ),
            new( "G", new[]{ DAH, DAH, DIT } ),
            new( "H", new[]{ DIT, DIT, DIT, DIT } ),
            new( "I", new[]{ DIT, DIT } ),
            new( "J", new[]{ DIT, DAH, DAH, DAH } ),
            new( "K", new[]{ DAH, DIT, DAH } ),
            new( "L", new[]{ DIT, DAH, DIT, DIT } ),
            new( "M", new[]{ DAH, DAH } ),
            new( "N", new[]{ DAH, DIT } ),
            new( "O", new[]{ DAH, DAH, DAH } ),
            new( "P", new[]{ DIT, DAH, DAH, DIT } ),
            new( "Q", new[]{ DAH, DAH, DIT, DAH } ),
            new( "R", new[]{ DIT, DAH, DIT } ),
            new( "S", new[]{ DIT, DIT, DIT } ),
            new( "T", new[]{ DAH } ),
            new( "U", new[]{ DIT, DIT, DAH } ),
            new( "V", new[]{ DIT, DIT, DIT, DAH } ),
            new( "W", new[]{ DIT, DAH, DAH } ),
            new( "X", new[]{ DAH, DIT, DIT, DAH } ),
            new( "Y", new[]{ DAH, DIT, DAH, DAH } ),
            new( "Z", new[]{ DAH, DAH, DIT, DIT } ),

            new( "0", new[]{ DAH, DAH, DAH, DAH, DAH } ),
            new( "1", new[]{ DIT, DAH, DAH, DAH, DAH } ),
            new( "2", new[]{ DIT, DIT, DAH, DAH, DAH } ),
            new( "3", new[]{ DIT, DIT, DIT, DAH, DAH } ),
            new( "4", new[]{ DIT, DIT, DIT, DIT, DAH } ),
            new( "5", new[]{ DIT, DIT, DIT, DIT, DIT } ),
            new( "6", new[]{ DAH, DIT, DIT, DIT, DIT } ),
            new( "7", new[]{ DAH, DAH, DIT, DIT, DIT } ),
            new( "8", new[]{ DAH, DAH, DAH, DIT, DIT } ),
            new( "9", new[]{ DAH, DAH, DAH, DAH, DIT } ),

            new( ".", new[]{ DIT, DAH, DIT, DAH, DIT, DAH } ),
            new( ",", new[]{ DAH, DAH, DIT, DIT, DAH, DAH } ),
            new( "?", new[]{ DIT, DIT, DAH, DAH, DIT, DIT } ),
            new( "'", new[]{ DIT, DAH, DAH, DAH, DAH, DIT } ),
            new( "!", new[]{ DAH, DIT, DAH, DIT, DAH, DAH } ),
            new( "/", new[]{ DAH, DIT, DIT, DAH, DIT } ),
            new( ":", new[]{ DAH, DAH, DAH, DIT, DIT, DIT } ),
            new( ";", new[]{ DAH, DIT, DAH, DIT, DAH, DIT } ),
            new( "=", new[]{ DAH, DIT, DIT, DIT, DAH } ),
            new( "+", new[]{ DIT, DAH, DIT, DAH, DIT } ),
            new( "-", new[]{ DAH, DIT, DIT, DIT, DIT, DAH } ),
            new( "_", new[]{ DIT, DIT, DAH, DAH, DIT, DAH } ),
            new( "\"", new[]{ DIT, DAH, DIT, DIT, DAH, DIT } ),
            new( "@",  new[]{ DIT, DAH, DAH, DIT, DAH, DIT } ),

            // BEGIN PROSIGNS
            new( "<AR>",    new[]{ DIT, DAH, DIT, DAH, DIT } ),   // OUT
            new( "<AS>",    new[]{ DIT, DAH, DIT, DIT, DIT } ),   // WAIT
            new( "<VE>",    new[]{ DIT, DIT, DIT, DAH, DIT } ),   // VERIFIED
            new( "<INT>",   new[]{ DIT, DIT, DAH, DIT, DAH } ),   // INTERROGATIVE
            new( "<HH>",    new[]{ DIT, DIT, DIT, DIT, DIT, DIT, DIT, DIT } ),   // CORRECTION/NEVERMIND
            new( "<BT>",    new[]{ DAH, DIT, DIT, DIT, DAH } ),   // BREAK (new section)
            new( "<AA>",    new[]{ DIT, DAH, DIT, DAH } ),   // NEWLINE
            new( "<CT>",    new[]{ DAH, DIT, DAH, DIT, DAH } ),   // START OF TRANSMISSION (commencing transmission)
            new( "<KN>",    new[]{ DAH, DIT, DAH, DAH, DIT } ),   // INVITATION
            new( "<NJ>",    new[]{ DAH, DIT, DIT, DAH, DAH, DAH } ),   // WABUN-CODE
            new( "<SK>",    new[]{ DIT, DIT, DIT, DAH, DIT, DAH } ),   // END OF WORK (silent key)
            new( "<SOS>",   new[]{ DIT, DIT, DIT, DAH, DAH, DAH, DIT, DIT, DIT } ),   // DISTRESS
        };

        /// <summary>
        /// Gets a WPM factor based on the provided WPM. Uses the "PARIS" standard to determine the character spacing.
        /// </summary>
        /// <param name="wpm">WPM for which to get the multiplier.</param>
        /// <returns>A factor representing how close/far audio signals (symbols) should be spaced.</returns>
        public static double GetMillisecondsPerSymbolAtWPM( int wpm ) {
            wpm = Math.Max( 1, wpm ); //WPM can't be below 1, no matter what

            // Convert WPM metrics to a measure of milliseconds per symbol.
            //  This calculation is based off of the 'PARIS' model for CW speeds.
            // PARIS is 50 symbols all accounted for, so WPM factor in MILLISECONDS is = [ 60 sec / (50 symbols * wpm) ] * 1000
            return Math.Round(((60 / (50 * (double)wpm)) * 1000), 2);
        }

        /// <summary>
        /// Digests a string and returns a sequence of MorseCharacter objects which represent the Morse signal
        /// ultimately generated by the Transmit text.
        /// </summary>
        /// <param name="txText">The text to digest.</param>
        /// <returns>A sequence of ordered, valid Morse characters, ready to be scaled to a time factor and output as audio.</returns>
        /// <see cref="MorseCharacter"/>
        public static (MorseCharacter[], string) GetMorseSequence( string txText ) {
            if( null == txText )
                throw new ArgumentNullException( nameof( txText ) );

            // Initial declarations.
            var fullTimeSequence = new List<MorseCharacter>();
            var morseString = new StringBuilder();

            // Capitalize and trim the input string.
            txText = txText.ToUpper().Trim();
            if ( txText.Length < 1 )
                throw new ArgumentException( "Transmit text must be more than only whitespace." );

            // Iterate the input string.
            for ( int i = 0; i < txText.Length; i++ ) {
                string rep = txText[i].ToString();

                // A '<' character indicates a prosign is being used, which spans multiple characters.
                if ( rep == "<" ) {
                    // Start at the next character and seek the first closing angle bracket.
                    int end = i + 1;
                    while ( end < txText.Length && '>' != txText[end] ) end++;

                    // If the scan hit the end of the transmit string, or the prosign is empty, throw an exception.
                    if ( end == txText.Length )
                        throw new ArgumentException( "Unclosed prosign angle bracket ('<') at position " + i );
                    else if ( end == (i + 1) )
                        throw new ArgumentException( "Empty prosign at position " + i );

                    // Otherwise, set 'rep' to the substring.
                    rep = txText[i..(end+1)];
                    i = end;   // scroll the counter
                }

                // Search for the matching static object in the declared Alphabet.
                //   If the character isn't found in the lookup, skip it.
                if ( !Alphabet.Any( x => x.Representation == rep ) ) continue;

                fullTimeSequence.Add( Alphabet.First( x => x.Representation == rep ) );
            }

            // Make sure there's something to return in the list.
            if ( fullTimeSequence.Count < 1 )
                throw new ArgumentException( "The transmit text did not generate any Morse sequences." );

            // Collate the final Morse string. This is an "easy" way to just return the string of valid parsed TX text as Morse.
            foreach ( var x in fullTimeSequence ) { 
                foreach ( var y in x.Sequence )
                    morseString.Append( y.Representation );

                morseString.Append( ' ' );
            }

            // Return the final sequence.
            return (fullTimeSequence.ToArray(), morseString.ToString());
        }

    }
}
