using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using QicktimeAtomParserNet.Lib;

namespace QicktimeAtomParserNet.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Qicktime ATOM Parser in .NET");

            var atom = AtomParser.parseAtoms(new FileInfo("Aa.mov"));

            printAtomTree(atom, "");

            Console.WriteLine("Press any key to continue...");

            Console.ReadKey();
        }


        protected static void printAtomTree(ParsedAtom[] atomTree,
            String indent)
        {
            for (int i = 0; i < atomTree.Length; i++)
            {
                Console.Write(indent);
                ParsedAtom atom = atomTree[i];
                Console.WriteLine(atom.ToString());
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
