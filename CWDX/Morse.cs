using System;
using System.Collections.Generic;
using System.Text;

namespace CWDX {
    class MorseSymbol {
        internal (bool, double, string) _row;   //ex: (false, 270, "") or (true, 740, "<BR>")
        public MorseSymbol(bool hasSound, double durationMs, string representation) {
            this._row = (hasSound, durationMs, representation);
        }
        public bool hasSound() { return _row.Item1; }
        public double getDuration() { return _row.Item2; }
        public string getRepresentation() { return _row.Item3; }
    }
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
        public static readonly double PER_CHARACTER = DAH;   // Time between each CHARACTER

        // Will contain the base sequence generated in the constructor.
        //private List<(bool, double)> baseSequence = new List<(bool, double)>();
        private List<MorseSymbol> baseSequence = new List<MorseSymbol>();
        public readonly string originalProsign = null;
        public readonly string symbolSequence = null;

        public MorseSequence(string symbolSequence, string prosign = "") {
            this.symbolSequence = symbolSequence;
            if(!string.IsNullOrEmpty(prosign)) { this.originalProsign = prosign; }
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

        // Wrapper function
        //private void appendSequence(bool playSound, double duration) {
        private void appendSequence(bool playSound, double duration, string repr) {
            //this.baseSequence.Add((playSound, duration));
            this.baseSequence.Add(new MorseSymbol(playSound, duration, repr));
        }

        // Public function to return a WPM factor based on a given WPM, based on the "PARIS" standard.
        public static double GetWPMFactor(int wpm) {
            // Convert WPM metrics to a measure of milliseconds per symbol.
            //  This calculation is based off of the 'PARIS' model for CW speeds.
            // PARIS is 50 symbols all accounted for, so WPM factor in MILLISECONDS is = [ 60 sec / (50 symbols * wpm) ] * 1000
            double wpmFactor = Math.Round(((60 / (50 * (double)wpm)) * 1000), 2);
            return wpmFactor;
        }

        // Numbers returned get sent to the sine wave generator from NAudio.
        //public List<(bool, double)> GetSequence(int wpm) {
        public List<MorseSymbol> GetSequence(int wpm) {
            /*
             * Multiply the 'duration' value from each sequence item by the new rate. Ex w/ five WPM:
             * - DIT = 1.00 * 240 = 240ms
             * - DAH = 3.00 * 240 = 720ms
             */
            //List<(bool, double)> timeSequence = new List<(bool, double)>();
            List<MorseSymbol> timeSequence = new List<MorseSymbol>();
            //foreach((bool b, double d) in this.baseSequence) {
            foreach(MorseSymbol sym in this.baseSequence) {
                timeSequence.Add(new MorseSymbol(sym.hasSound(),
                    Math.Round((double)(sym.getDuration() * GetWPMFactor(wpm)), 2), sym.getRepresentation()));
            }
            return timeSequence;
        }
    }
    static class Morse {
        // Symbol recognition.
        public const char DIT_CHAR = '.';
        public const char DAH_CHAR = '-';
        public const char SPACE_CHAR = ' ';

        // Morse characters and prosigns
        public static Dictionary<char, MorseSequence> Characters = new Dictionary<char, MorseSequence>() {
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
            // Special prosign substitutions
        };

        // Morse prosigns (for conversion to a single char index item)
        public static Dictionary<string, char> ProsignsToChars = new Dictionary<string, char>();

        // Generate a morse time-sequence for the given string (converted to upper-case of course) for the given speed.
        //public static (List<(bool, double)>, string) GetTimeSequence(string txText, int wpmSpeed) {
        public static (List<MorseSymbol>, string) GetTimeSequence(string txText, int wpmSpeed) {
            //var fullTimeSequence = new List<(bool, double)>();
            List<MorseSymbol> fullTimeSequence = new List<MorseSymbol>();
            StringBuilder outputString = new StringBuilder();
            // Trim the provided string and force upper-case.
            txText = txText.ToUpper().Trim();
            // Convert prosigns to their "char" representations in the txText param

            // For each item in the (cleaned) text, get the timing information and append the output string.
            foreach(char c in txText) {
                try {
                    MorseSequence morseChar; Morse.Characters.TryGetValue(c, out morseChar);
                    if(morseChar == null) { continue; }
                    //outputString.Append(morseChar.symbolSequence + " ");
                    // Append all symbols
                    //foreach((bool b, double d) in morseChar.GetSequence(wpmSpeed)) { fullTimeSequence.Add((b, d)); }
                    foreach(MorseSymbol sym in morseChar.GetSequence(wpmSpeed)) {
                        fullTimeSequence.Add(sym);
                        outputString.Append(sym.getRepresentation());
                    }
                    outputString.Append(string.Format("({0}) ", c.ToString()));   //append a space between characters
                    // Add the gap between individual characters
                    fullTimeSequence.Add(new MorseSymbol(false, MorseSequence.GetWPMFactor(wpmSpeed) * MorseSequence.PER_CHARACTER, "  "));
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
