using System;
using System.Collections.Generic;


namespace CWDX {

    [Serializable()]
    public class TXMacro {

    }

    public static class TXMacroConfig {
        // 
        public static List<TXMacro> LoadSavedMacros() {
            return null;
        }

        // Save the macros
        public static bool SaveMacros(List<TXMacro> macrosList) {
            return true;
        }
    }
}
