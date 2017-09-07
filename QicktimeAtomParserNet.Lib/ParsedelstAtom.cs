using System;
using RandomAccessFile = System.IO.Stream;
using System.Numerics;


namespace QicktimeAtomParserNet.Lib
{
    public class ParsedelstAtom : ParsedLeafAtom
    {
        int version;
        Edit[] edits;

        public ParsedelstAtom(long size,
            String type,
            RandomAccessFile raf):base(size, type, raf)
        {
        }

        public void init(RandomAccessFile raf)
        {
            BigInteger longConverter = 0;
            // elst contains a 1-byte version, 3 bytes of (unused) flags,
            // a byte of table count, then 12-byte table entries
            // (4 each of trackDuration, mediaTime, and mediaRate)
            byte[] buffy = new byte[4];
            raf.Read(buffy, 0, 1);
            version = buffy[0];
            // flags are defined as 3 bytes of 0, I just read & forget
            raf.Read(buffy, 0, 3);
            // how many table entries are there?
            raf.Read(buffy, 0, 4);
            longConverter = new BigInteger(buffy);
            int tableCount = (int)longConverter;//.intValue();
            edits = new Edit[tableCount];
            for (int i = 0; i < tableCount; i++)
            {
                // TODO: also bounds-check that we don't go past size
                // track duration
                raf.Read(buffy, 0, 4);
                longConverter = new BigInteger(buffy);
                long trackDuration = (long)longConverter;//.longValue();
                // media time
                raf.Read(buffy, 0, 4);
                longConverter = new BigInteger(buffy);
                long mediaTime = (long)longConverter;//.longValue();
                // media rate
                // TODO: wrong, these 4 bytes are a fixed-point
                // float, 16-bytes left of decimal, 16 right
                // I don't get how apple does this, so I'm just reading
                // the integer part
                raf.Read(buffy, 0, 2);
                longConverter = new BigInteger(buffy);
                float mediaRate = (long)longConverter;//.floatValue();
                raf.Read(buffy, 0, 2);
                // make an Edit object
                Edit edit = new Edit(trackDuration, mediaTime, mediaRate);
                edits[i] = edit;
            }
        }

        public int getVersion()
        {
            return version;
        }

        public Edit[] getEdits()
        {
            return edits;
        }

        public override String ToString()
        {
            return base.ToString() + "[" +
                   edits.Length +
                   ((edits.Length != 1) ? " edits]" : " edit]");
        }


        public class Edit
        {
            long trackDuration;
            long mediaTime;
            float mediaRate;

            public Edit(long d, long t, float r)
            {
                trackDuration = d;
                mediaTime = t;
                mediaRate = r;
            }

            public long getTrackDuration()
            {
                return trackDuration;
            }

            public long getMediaTime()
            {
                return mediaTime;
            }

            public float getMediaRate()
            {
                return mediaRate;
            }
        }

    }
}
