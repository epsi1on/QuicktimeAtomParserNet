using System;

namespace QicktimeAtomParserNet.Lib
{
    public class ParsedAtom
    {

        public ParsedAtom this[string key,int index=0]
        {
            get
            {

                if (this is ParsedContainerAtom)
                {
                    var childs = (this as ParsedContainerAtom).children;
                    var tmp = index;

                    for (int i = 0; i < childs.Length; i++)
                    {
                        if (childs[i].type == key)
                        {
                            if (tmp <= 0)
                                return childs[i];
                            else
                                tmp--;
                        }
                    }
                }
                return null;
            }
        }

        public long offset;

        public long size;
        public String type;

        protected ParsedAtom(long size,
                              String type)
        {
            this.size = size;
            this.type = type;
        }

        public long getSize()
        {
            return size;
        }
        public String getType()
        {
            return type;
        }


        public static void SetOffset(object atom, long offset)
        {
            var tt = atom as ParsedAtom;

            if (tt == null)
                return;

            tt.offset = offset;
        }
    }
}
