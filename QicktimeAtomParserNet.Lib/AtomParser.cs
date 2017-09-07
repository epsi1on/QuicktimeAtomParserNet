using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using RandomAccessFile = System.IO.FileStream;


namespace QicktimeAtomParserNet.Lib
{
    public class AtomParser
    {
        /** QuickTime container atoms - these atoms contain only
       other atoms, so if one is encoutered, we recur to pick
       up the children
    */

        public static readonly String[] ATOM_CONTAINER_TYPE_STRINGS =
        {
            "moov", "trak", "udta", "tref", "imap",
            "mdia", "minf", "stbl", "edts", "mdra",
            "rmra", "imag", "vnrp", "dinf"
        };

        /** A Set for fast lookup of possible atom-containers,
            includes all the members of ATOM_CONTAINER_TYPE_STRINGS
         */
        /**/

        public static readonly System.Collections.Generic.HashSet<string> ATOM_CONTAINER_TYPES =
            new HashSet<string>();

        static AtomParser()
        {
            for (int i = 0; i < ATOM_CONTAINER_TYPE_STRINGS.Length; i++)
            {
                ATOM_CONTAINER_TYPES.Add(ATOM_CONTAINER_TYPE_STRINGS[i]);
            } // for
        }

        /**/


        static byte[] atomSizeBuf = new byte[4];
        static byte[] atomTypeBuf = new byte[4];
        static byte[] extendedAtomSizeBuf = new byte[8];


/** return top level atoms (and thus the entire 
    structure) parsed from the given file.
 */

        public static ParsedAtom[] parseAtoms(FileInfo f)

        {
            RandomAccessFile raf = f.OpenRead(); // new RandomAccessFile(f, "r");
            ParsedAtom[] atoms = parseAtoms(raf, 0, raf.Length);
            raf.Close();
            return atoms;
        }

        protected static ParsedAtom[] parseAtoms(RandomAccessFile raf,
            long firstOff,
            long stopAt)

        {
            // off is the atom's offset into the file (gets reset
            // for next sibling at bottom of loop, after preceding 
            // sibling's size is read)
            long off = firstOff;
            ArrayList parsedAtomList = new ArrayList();
            //        while (raf.getFilePointer() <= stopAt) {
            while (off < stopAt)
            {
                raf.Seek(off, SeekOrigin.Begin);

                // 1. first 32 bits are atom size
                // use BigInteger to convert bytes to long
                // (instead of signed int)
                int bytesRead = raf.Read(atomSizeBuf, 0, atomSizeBuf.Length);
                if (bytesRead < atomSizeBuf.Length)
                    throw new IOException("couldn't read atom length");
                BigInteger atomSizeBI = new BigInteger(atomSizeBuf.Reverse().ToArray());
                
                long atomSize = (long) atomSizeBI; //.longValue();

                // this is kind of a hack to handle the udta problem
                // (see below) when the parent didn't have children,
                // meaning we've read 4 bytes of 0 and the atom is
                // already over
                if (raf.Position == stopAt)
                    break;

                // 2. next, the atom type
                bytesRead = raf.Read(atomTypeBuf, 0, atomTypeBuf.Length);
                if (bytesRead != atomTypeBuf.Length)
                    throw new IOException("Couldn't read atom type");
                String atomType = Encoding.ASCII.GetString(atomTypeBuf);

                // 3. if atomSize was 1, then there are 64 bits of extended size
                if (atomSize == 1)
                {
                    bytesRead = raf.Read(extendedAtomSizeBuf, 0,
                        extendedAtomSizeBuf.Length);
                    if (bytesRead != extendedAtomSizeBuf.Length)
                        throw new IOException("Couldn't read extended atom size");
                    BigInteger extendedSizeBI =
                        new BigInteger(extendedAtomSizeBuf);
                    atomSize = (long) extendedSizeBI; //.longValue();
                }

                // if this atom size is negative, or extends past end
                // of file, it's extremely suspicious (ie, we're not
                // really in a quicktime file)
                if ((atomSize < 0) ||
                    ((off + atomSize) > raf.Length))
                    throw new IOException("atom has invalid size: " +
                                          atomSize);

// 4. if a container atom, then parse the children
                ParsedAtom parsedAtom = null;
                if (ATOM_CONTAINER_TYPES.Contains(atomType))
                {
                    // children run from current point to the end of the atom
                    ParsedAtom[] children =
                        parseAtoms(raf,
                            raf.Position,
                            off + atomSize);
                    parsedAtom =
                        new ParsedContainerAtom(atomSize,
                            atomType,
                            children);
                }
                else
                {
                    parsedAtom =
                        AtomFactory.getInstance().createAtomFor(atomSize,
                            atomType,
                            raf);

                    ParsedAtom.SetOffset(parsedAtom, off);
                }

                // add atom to the list
                parsedAtomList.Add(parsedAtom);

                // now set offset to next atom (or end-of-file
                // in special case (atomSize = 0 means atom goes
                // to EOF)
                if (atomSize == 0)
                    off = raf.Length;
                else
                    off += atomSize;

                // if a 'udta' container atom, then jump ahead 4 
                // to work around Apple's QT 1.0 workaround
                // (http://developer.apple.com/technotes/qt/qt_03.html )
                if (atomType.Equals("udta"))
                    off += 4;
            } // while not at stopAt

            // convert the array list into an array
            //ParsedAtom[] atomArray =
            //    new ParsedAtom[parsedAtomList.Count];

            ParsedAtom[] atomArray = parsedAtomList.Cast<ParsedAtom>().ToArray();

            //parsedAtomList.ToArray(typeof(ParsedAtom));

            return atomArray;
        } // parseAtoms


        /** debug - parse the atom chosen by user
     */

        public static void main(String[] args)
        {
        } // main


/** helper for main, recursively prints atoms and their
    children, adding further indent for each generation
 */

        protected static void printAtomTree(ParsedAtom[] atomTree,
            String indent)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < atomTree.Length; i++)
            {
                sb.Append(indent);
                ParsedAtom atom = atomTree[i];
                sb.AppendLine(atom.ToString());

                if (atom is ParsedContainerAtom)
                {
                    ParsedAtom[] children =
                        ((ParsedContainerAtom) atom).getChildren();
                    printAtomTree(children, indent + "  ");
                }
            }
        }
    }
}