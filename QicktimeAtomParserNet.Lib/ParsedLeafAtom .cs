using System;
using RandomAccessFile = System.IO.Stream;


namespace QicktimeAtomParserNet.Lib
{
    public class ParsedLeafAtom: ParsedAtom
    {
        public ParsedLeafAtom(long size,
                          String type,
                          RandomAccessFile raf) :base(size,type)
        {
            init(raf);
        }


        /** Called by the constructor, the init method assumes
       that the RandomAccessFile is on the first byte after
       the type (or extended size, if present) and reads
       in the contents of the atom.  That means that there are
       size-8 bytes left to be read, unless size > 0xffffffff (unsigned)
       in which case there was an extended size and there are thus
       size-16 bytes left to be read
       <p>
       The default does nothing.  Atom-specific subclasses
       can override this method to handle the specific
       structures of their atoms.
    */
        public void init(RandomAccessFile raf)
        {
            // does nothing
        }

        /** By default, leaf atoms return "type (size bytes)" */
        public override String ToString()
        {
            return type + " (" + util.SizeSuffix(size) + ") ";
        }

    }
}
