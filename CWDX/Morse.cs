using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CWDX {

    /// <summary>
    /// Represents an individual Morse Code symbol object in a sequence, through three different supplied paramters.
    /// </summary>
    class MorseSymbol {
        internal (bool, double, string) _row;   //ex: (false, 270, "") or (true, 740, "<BR>")
        /// <summary>
        /// Creates a Morse symbol used as a single unit when synchronously playing audio with the output sound generator.
        /// </summary>
        public MorseSymbol(bool hasSound, double durationMs, string representation) {
            this._row = (hasSound, durationMs, representation);
        }
        /// <summary>
        /// Gets whether the symbol is a played sound, or a moment of silence.
        /// </summary>
        public bool hasSound() { return _row.Item1; }
        /// <summary>
        /// Gets the duration in milliseconds for which to play the sound, if the symbol has sound.
        /// </summary>
        public double getDuration() { return _row.Item2; }
        /// <summary>
        /// Gets the string representing the symbol. This could be a single character or a complete prosign.
        /// </summary>
        public string getRepresentation() { return _row.Item3; }
    }

    /// <summary>
    /// Takes a string of Morse Code characters [-. ] and generates a sequence of MorseSymbols at a given WPM to represent it.
    /// </summary>
    /// <seealso cref="MorseSymbol"/>
    class MorseSequence {
        /*
         * Terminology:
         * - Symbol = individual DIT, DAH, or SPACE
         * - Character = character created from a sequence of symbols and spacing between symbols
         * - Word = string of characters with appropriate per-character spacing, which don't contain a space CHARACTER (obviously)
         */
        // Timing information, which is later affected by the WPM when the baseSequence is retrieved.
        private static readonly double DIT = 1.00;
        private static readonly double DAH = 3 * DIT;
        private static readonly double SPACE = 5 * DIT;  // Time between each WORD
        private static readonly double PER_SYMBOL = DIT;   // Time between each SYMBOL
        /// <summary>
        /// Time taken between each CHARACTER unit.
        /// Always equal to the length of a DAH, representing space between individual characters in a string.
        /// </summary>
        public static readonly double PER_CHARACTER = DAH;   // Time between each CHARACTER

        // Will contain the base sequence generated in the constructor.
        private List<MorseSymbol> baseSequence = new List<MorseSymbol>();
        /// <summary>
        /// String representation of the Morse Code used to build this object.
        /// </summary>
        public readonly string symbolSequence = null;

        /// <summary>
        /// Creates a new MorseSequence object using the provided sequence of Morse symbols from a string.
        /// This is the primary workhorse for setting up a sequence of DITs and DAHS into an audible sound with appropriate spacing.
        /// </summary>
        /// <param name="symbolSequence">The symbol sequence of the character to be converted into a sound.</param>
        /// <param name="prosign">Supplied if the symbolSequence represents a prosign rather than a single character.</param>
        /// <seealso cref="Morse"/>
        public MorseSequence(string symbolSequence) {
            if(symbolSequence.Length < 1) { return; }
            this.symbolSequence = symbolSequence;
            foreach(char c in symbolSequence) {
                // Add the symbol value.
                switch(c) {
                    case Morse.DIT_CHAR: {
                        this.appendSequence(true, DIT, c.ToString());
                        break;
                    }
                    case Morse.DAH_CHAR: {
                        this.appendSequence(true, DAH, c.ToString());
                        break;
                    }
                    case Morse.SPACE_CHAR: {
                        this.appendSequence(false, SPACE, c.ToString());
                        break;
                    }
                    default: {
                        throw new Exception("MorseSequence ctor: 'symbolSequence' contained an invalid Morse symbol!");
                    }
                }
                // Add the PER_SYMBOL spacing.
                this.appendSequence(false, PER_SYMBOL, "");
            }
            // Pop the final per-symbol (silent) dit off the stack.
            this.baseSequence.RemoveAt(this.baseSequence.Count - 1);
        }

        /// <summary>
        /// Wrapper function to append a new MorseSymbol object onto the base-sequence.
        /// Wrapper exists in case extra options are needed specifically here at a later time.
        /// </summary>
        /// <seealso cref="MorseSymbol"/>
        private void appendSequence(bool playSound, double duration, string repr) {
            this.baseSequence.Add(new MorseSymbol(playSound, duration, repr));
        }

        /// <summary>
        /// Gets the MorseSymbol sequence (has-sound, timing/spacing, string)
        /// </summary>
        /// <param name="wpm"></param>
        /// <returns></returns>
        public List<MorseSymbol> GetSequence(int wpm) {
            /*
             * Multiply the 'duration' value from each sequence item by the new rate. Ex w/ five WPM:
             * - DIT = 1.00 * 240 = 240ms
             * - DAH = 3.00 * 240 = 720ms
             */
            List<MorseSymbol> timeSequence = new List<MorseSymbol>();
            foreach(MorseSymbol sym in this.baseSequence) {
                timeSequence.Add(new MorseSymbol(
                    sym.hasSound(),
                    Math.Round(
                        (double)(sym.getDuration() * Morse.GetWPMFactor(wpm)), 2
                    ),
                    sym.getRepresentation()
                ));
            }
            return timeSequence;
        }
    }

    /// <summary>
    /// Represents all static definitions and methods for the Morse Code "protocol". Includes dictionaries for mapping symbols back and forth
    /// and also a way to get a time-sequence representation of a transmit string, for passing to the application audio handler.
    /// </summary>
    /// <seealso cref="MorseSequence"/>
    /// <seealso cref="MorseSymbol"/>
    static class Morse {
        /// <summary>
        /// Repesentation of a DIT Morse character.
        /// </summary>
        public const char DIT_CHAR = '.';
        /// <summary>
        /// Repesentation of a DAH Morse character.
        /// </summary>
        public const char DAH_CHAR = '-';
        /// <summary>
        /// Repesentation of an EMPTY (silent) Morse character for separating Morse symbols.
        /// </summary>
        public const char SPACE_CHAR = ' ';

        /// <summary>
        /// Defines a dictionary of Morse Code sequences to their character representations. For prosigns, these values will be special
        /// char values that are later converted to string representations in the final TX output.
        /// </summary>
        public static readonly Dictionary<char, MorseSequence> Characters = new Dictionary<char, MorseSequence>() {
            // Ordinary ASCII stuff
            { 'A', new MorseSequence(".-") }, { 'B', new MorseSequence("-...") }, { 'C', new MorseSequence("-.-.") },
            { 'D', new MorseSequence("-..") }, { 'E', new MorseSequence(".") }, { 'F', new MorseSequence("..-.") },
            { 'G', new MorseSequence("--.") }, { 'H', new MorseSequence("....") }, { 'I', new MorseSequence("..") },
            { 'J', new MorseSequence(".---") }, { 'K', new MorseSequence("-.-") }, { 'L', new MorseSequence(".-..") },
            { 'M', new MorseSequence("--") }, { 'N', new MorseSequence("-.") }, { 'O', new MorseSequence("---") },
            { 'P', new MorseSequence(".--.") }, { 'Q', new MorseSequence("--.-") }, { 'R', new MorseSequence(".-.") },
            { 'S', new MorseSequence("...") }, { 'T', new MorseSequence("-") }, { 'U', new MorseSequence("..-") },
            { 'V', new MorseSequence("...-") }, { 'W', new MorseSequence(".--") }, { 'X', new MorseSequence("-..-") },
            { 'Y', new MorseSequence("-.--") }, { 'Z', new MorseSequence("--..") },
            { '1', new MorseSequence(".----") }, { '2', new MorseSequence("..---") }, { '3', new MorseSequence("...--") },
            { '4', new MorseSequence("....-") }, { '5', new MorseSequence(".....") }, { '6', new MorseSequence("-....") },
            { '7', new MorseSequence("--...") }, { '8', new MorseSequence("---..") }, { '9', new MorseSequence("----.") },
            { '0', new MorseSequence("-----") },
            { ' ', new MorseSequence(" ") }, { '.', new MorseSequence(".-.-.-") }, { ',', new MorseSequence("--..--") },
            { '?', new MorseSequence("..--..") }, { '\'', new MorseSequence(".----.") }, { '!', new MorseSequence("-.-.--") },
            { '/', new MorseSequence("-..-.") }, { ':', new MorseSequence("---...") }, { ';', new MorseSequence("-.-.-.") },
            { '=', new MorseSequence("-...-") }, { '+', new MorseSequence(".-.-.") }, { '-', new MorseSequence("-....-") },
            { '_', new MorseSequence("..--.-") }, { '"', new MorseSequence(".-..-.") }, { '@', new MorseSequence(".--.-.") },
        };

        /// <summary>
        /// Defines a dictionary of prosigns to their sequence equivalents. Used primarily in the "RX" section of the application
        /// for identifying and labeling sequences that APPEAR to be a prosign (e.g. '-.--.' ==> {KN}).
        /// </summary>
        // A "prosign" is any run-on of characters designed to create a
        //   single symbol that conveys a certain meaning -- e.g. <AR> ==> "Over and out"
        // These symbols are really only used in RX mode, as the user can __-->transmit<--__ whatever prosigns they please with: <texthere>
        // See: http://www.asciitable.com/
        // Prosigns List: https://en.wikipedia.org/wiki/Prosigns_for_Morse_code
        public static Dictionary<string, MorseSequence> Prosigns = new Dictionary<string, MorseSequence>() {
            { "AR",    new MorseSequence(".-.-.") },   //OUT
            { "AS",    new MorseSequence(".-...") },   //WAIT
            { "VE",    new MorseSequence("...-.") },   //VERIFIED
            { "INT",   new MorseSequence("..-.-") },   //INTERROGATIVE
            { "HH",    new MorseSequence("........") },   //CORRECTION
            { "HH AR", new MorseSequence("........ .-.-.") },   //NEVERMIND, OUT
            { "BT",    new MorseSequence("-...-") },   //BREAK (new section)
            { "AA",    new MorseSequence(".-.-") },   //NEWLINE
            { "CT",    new MorseSequence("-.-.-") },   //START-OF-TRANSMISSION (commencing transmission)
            { "KN",    new MorseSequence("-.--.") },   //INVITATION
            { "NJ",    new MorseSequence("-..---") },   //WABUN-CODE
            { "SK",    new MorseSequence("...-.-") },   //END-OF-WORK (silent key)
            { "SOS",   new MorseSequence("...---...") },   //DISTRESS
        };

        /// <summary>
        /// Gets a WPM factor based on the provided WPM. Uses the "PARIS" standard to determine the character spacing.
        /// </summary>
        /// <param name="wpm">WPM for which to get the multiplier.</param>
        /// <returns>A factor representing how close/far audio signals (symbols) should be spaced.</returns>
        public static double GetWPMFactor(int wpm) {
            wpm = Math.Max(1, wpm); //WPM can't be below 1, no matter what
            // Convert WPM metrics to a measure of milliseconds per symbol.
            //  This calculation is based off of the 'PARIS' model for CW speeds.
            // PARIS is 50 symbols all accounted for, so WPM factor in MILLISECONDS is = [ 60 sec / (50 symbols * wpm) ] * 1000
            double wpmFactor = Math.Round(((60 / (50 * (double)wpm)) * 1000), 2);
            return wpmFactor;
        }

        /// <summary>
        /// Generate a morse time-sequence for the given string (converted to upper-case of course) for the given speed.
        /// This function is the "core" of the Morse module, to get an audio sequence for the audio handler to deal with.
        /// </summary>
        /// <param name="txText">The text to transmit; case-insensitive.</param>
        /// <param name="wpmSpeed">The WPM speed of the sequence.</param>
        /// <returns>A list of Morse symbols representing the txText parameter at the given WPM. Audio-player-ready.</returns>
        /// <seealso cref="MorseSymbol"/>
        public static (List<MorseSymbol>, string) GetTimeSequence(string txText, int wpmSpeed) {
            List<MorseSymbol> fullTimeSequence = new List<MorseSymbol>();
            StringBuilder outputString = new StringBuilder();
            // Trim the provided string and force upper-case.
            txText = txText.ToUpper().Trim();
            // Get all valid prosigns, map their unique positions to a tuple of their (length, symbols [ie '...---'], text).
            MatchCollection mc = Regex.Matches(txText, "<([^>]+)>");
            var matchedProsigns = new Dictionary<int, (int lengthPastIndex, string psSymbolSequence, string psRepresentation)>();
            foreach(Match m in mc) {
                // For each character between the <>, get its symbols (... --- etc) and string them all together via a StringBuilder.
                var prosignText = new StringBuilder();
                foreach(char c in m.Groups[1].Value) {
                    try {
                        MorseSequence seq; Morse.Characters.TryGetValue(c, out seq);
                        if(seq is null) { throw new Exception(); }
                        prosignText.Append(seq.symbolSequence);
                    } catch { throw new Exception("Morse.GetTimeSequence: invalid character in prosign: " + c); }
                }
                // Add it to the capture Dictionary.
                matchedProsigns.Add(m.Groups[0].Index, (  m.Groups[0].Length, prosignText.ToString(), m.Groups[1].Value  ));
            }

            // For each item in the (parsed) text, get the timing information and append the output string.
            for(int i = 0; i < txText.Length; i++) {
                // Check if the current index is the location of a known prosign.
                if(matchedProsigns.ContainsKey(i)) {
                    try {
                        // Get the prosign contents.
                        (int len, string seq, string rep) prosign;
                        matchedProsigns.TryGetValue(i, out prosign);
                        // Set up a new sequence with the full symbol string from an earlier step.
                        var x = new MorseSequence(prosign.seq);
                        if(x == null) { continue; }
                        // For each symbol, add it to the overall sequencer.
                        foreach(MorseSymbol sym in x.GetSequence(wpmSpeed)) { fullTimeSequence.Add(sym); }
                        // Write the prosign contents to the output string as-is.
                        outputString.Append(string.Format("<{0}>", prosign.rep));
                        // Add a sequence gapping at the end of this "character" (prosigns are considered one character unit).
                        fullTimeSequence.Add(new MorseSymbol(false, Morse.GetWPMFactor(wpmSpeed) * MorseSequence.PER_CHARACTER, "  "));
                        // Increment the index to 'skip over' the rest of the prosign. If this lands on '>', no big deal; it will be skipped.
                        i += (prosign.len - 1);
                    } catch { throw new Exception("Morse.GetTimeSequence: unknown prosign is the TX sequence at position: " + i); }
                    // Forcibly continue.
                    continue;
                }

                // If no prosign was detected at the current position, proceed normally.
                char c = txText[i];
                try {
                    MorseSequence morseChar; Morse.Characters.TryGetValue(c, out morseChar);
                    if(morseChar == null) { continue; }
                    // Append all symbols and string representations
                    foreach(MorseSymbol sym in morseChar.GetSequence(wpmSpeed)) {
                        fullTimeSequence.Add(sym);
                        outputString.Append(sym.getRepresentation());
                    }
                    outputString.Append(string.Format("({0}) ", c.ToString()));   //append a space between characters
                    // Add the gap between individual characters
                    fullTimeSequence.Add(new MorseSymbol(false, Morse.GetWPMFactor(wpmSpeed) * MorseSequence.PER_CHARACTER, "  "));
                } catch { throw new Exception(string.Format("Morse.GetTimeSequence: unknown character \"{0}\" in the TX sequence.", c)); }
            }
            // Pop off the last PER_CHARACTER item in the time sequence, as well as the trailing space in the outputString.
            fullTimeSequence.RemoveAt(fullTimeSequence.Count - 1);
            outputString.Remove(outputString.Length - 1, 1);
            // Send back the information.
            return (fullTimeSequence, outputString.ToString());
        }

    }
}
