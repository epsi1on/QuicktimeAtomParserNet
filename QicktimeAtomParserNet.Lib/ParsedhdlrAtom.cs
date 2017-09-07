using System;
using System.IO;
using System.Numerics;
using System.Text;
using RandomAccessFile=System.IO.Stream;
//using BigInteger = System.Int64;

namespace QicktimeAtomParserNet.Lib
{
    public class ParsedhdlrAtom : ParsedLeafAtom
    {
        int version;
        String componentType;
        String componentSubType;
        String componentManufacturer;
        String componentName;

        public ParsedhdlrAtom(long size,
            String type,
            RandomAccessFile raf):base(size, type, raf)
        {
        }



        public void init(RandomAccessFile raf)
        {
            BigInteger longConverter = 0;
            // hdlr contains a 1-byte version, 3 bytes of (unused) flags,
            // 4-char component type, 4-char component subtype,
            // 4-byte fields for comp mfgr, comp flags, and flag mask
            // then a pascal string for component name

            byte[] buffy = new byte[4];
            raf.Read(buffy, 0, 1);
            version = buffy[0];
            // flags are defined as 3 bytes of 0, I just read & forget
            raf.Read(buffy, 0, 3);
            // component type and subtype (4-byte strings)
            raf.Read(buffy, 0, 4);
            componentType = Encoding.ASCII.GetString(buffy);
            raf.Read(buffy, 0, 4);
            componentSubType = Encoding.ASCII.GetString(buffy);
            // component mfgr (4 bytes, apple says "reserved- set to 0")
            raf.Read(buffy, 0, 4);
            componentManufacturer = Encoding.ASCII.GetString(buffy);
            // component flags & flag mask 
            // (4 bytes each, apple says "reserved- set to 0", skip for now)
            raf.Read(buffy, 0, 4);
            raf.Read(buffy, 0, 4);
            // length of pascal string
            raf.Read(buffy, 0, 1);
            int compNameLen = buffy[0];
            /* undocumented hack:
           in .mp4 files (as opposed to .mov's), the component name
           seems to be a C-style (null-terminated) string rather
           than Pascal-style (length-byte then run of characters).
           However, the name is the last thing in this atom, so
           if the String size is wrong, assume we're in MPEG-4
           and just read to end of the atom.  In other words, the
           string length *must* always be atomSize - 33, since there
           are 33 bytes prior to the string, and it's the last thing
           in the atom.
        */
            if (compNameLen != (size - 33))
            {
                // MPEG-4 case
                compNameLen = (int) size - 33;
                // back up one byte (since what we thought was
                // length was actually first char of string)
                raf.Seek(raf.Position - 1, SeekOrigin.Begin);
            }

            byte[] compNameBuf = new byte[compNameLen];

            raf.Read(compNameBuf, 0, compNameLen);
            componentName =
                Encoding.ASCII.GetString(compNameBuf);
        }

        public int getVersion()
        {
            return version;
        }

        public String getComponentType()
        {
            return componentType;
        }

        public String getComponentSubType()
        {
            return componentSubType;
        }

        public String getComponentManufacturer()
        {
            return componentManufacturer;
        }

        public String getComponentName()
        {
            return componentName;
        }

        public override String ToString()
        {
            return base.ToString() + "[" +
                   componentType + "/" + componentSubType +
                   " - " + componentName + "]";
        }

    }
}
