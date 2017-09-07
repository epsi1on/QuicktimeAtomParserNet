using System;
using RandomAccessFile = System.IO.Stream;

namespace QicktimeAtomParserNet.Lib
{
    public class ParsedWLOCAtom: ParsedLeafAtom
    {
        int x;
        int y;

        public ParsedWLOCAtom(long size,
                               String type,
                               RandomAccessFile raf) :base(size, type, raf)
        {
        }

        public void init(RandomAccessFile raf) 
        {
        // WLOC contains 16-bit x,y values
        byte[] value = new byte[4];
        raf.Read (value, 0, value.Length);
        x = (value[0] << 8) | value[1];
        y = (value[2] << 8) | value[3]; 
    }

    public int getX()
    {
        return x;
    }

    public int getY()
    {
        return y;
    }

    public override String ToString()
    {
        return base.ToString() +
            " (x,y) == (" +
            x + "," + y + ")";

    }

}
}
