using System;

namespace QicktimeAtomParserNet.Lib
{
    public class ParsedContainerAtom:ParsedAtom
    {
        

        internal ParsedAtom[] children;

        internal ParsedContainerAtom(long size,
                                       String type,
                                       ParsedAtom[] children):base(size,type)
        {
            this.children = children;
        }

        public ParsedAtom[] getChildren()
        {
            return children;
        }

        public override String ToString()
        {
            return type + " (" + util.SizeSuffix(size) + ") - " +
                children.Length +
                (children.Length == 1 ?
                 " child" :
                 " children");

        }


    }
}
