using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FlacDecoder
{

    // Layout
    /*
     *  fLaC marker
     *  (Required) STREAMINFO block
     *  (Optional) Application -- for use by third-party applications
     *  (Optional) Padding -- Used to pad the data. Useful for editing metadata after encoding.
     *  (Optional) Seektable -- Seek points, bitrate in a flac can vary so seeking is more complicated than wave file
     *  (Optional) Vorbis_comment -- Name/Value pairs encoded using UTF-8. Used for tagging.
     *  (Optional) Cuesheet -- This is used for storing information that can be used in a cue sheet. Track and index points.
     *  (Optional) Picture -- This block is for storing pictures associated with the file, most commonly cover art from CDs.
     *  
     *  Audio Frames
     *  Frame header - contains
     *      Sync code
     *      information about the frame (block size, sample rate, number of channels etc).
     *      8-bit CRC.
     *      For variable-blocksize streams it contains sample number of the first sample in the frame.
     *      For fixed-blocksize streams it contains frame number. These two allow for fast sample-accurate seeking to be performed.
     *  Encoded subframe, one for each channel.
     *      Each subframe has it's own header that specifies how the subframe is encoded.
     *  Zero padding to a byte boundry.
     */
    [StructLayout(LayoutKind.Sequential)]
    unsafe struct StreamInfo
    {
        public fixed byte minBlockSize[16];
        public fixed byte maxBlockSize[16];
        public fixed byte minFrameSize[24];
        public fixed byte maxFrameSize[24];
        public fixed byte sampleRate[20];
        public fixed byte numChannels[3];
        public fixed byte bitsPerSample[5];
        public fixed byte totStreamSamples[36];
        public fixed byte MD5Signature[128];
    }

    [StructLayout(LayoutKind.Sequential)]
    unsafe struct AudioFrame
    {

    }



    class Flac
    {
        private byte[] flac = new byte[32];

    }
}
