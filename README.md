# Morse Code Desktop Transceiver

A simple Morse Code transmitter and (tentatively) receiver that can run on most Win10 machines.

The interface is intended to be _easy to use_ and intuitive for any level of CW familiarity or Windows experience. It can also be used to train oneself on the "music" of CW, as well as testing your keying skills.

### Future Considerations
- Possible HAM equipment integration.
- "Practice" window for keying your own code.
  - Could include some kind of integration with iambic paddles (?).
  - Would use the SPACE bar or some other input method to act as a _straight key_.

### TODOs Right Now
- ~~Implement prosigns.~~
- Allow audio I/O device selections.
- Fix the GUI to scale better.
- Add Macros functionality for transmitting.
- Implement the CW interpreter function:
  - Signal Reception (FFT)
  - Peak readings (dB signal strength)
  - Speed detection
  - Decoded output (RECEIVER panel)
- Package the application nicely and publish a "release" for easy download.


# Transmitting (TX)

Aside from being able to choose the output audio device, transmitting code works with this tool.
You're able to modify some TX parameters to your liking, and soon to come are macros for easy, templated responses to oft-discussed topics at your whims.

Here's a demonstration:

![TX Demonstration](https://raw.githubusercontent.com/NotsoanoNimus/CWDX/master/docs/img/CWDX_TX_Testing.gif)
