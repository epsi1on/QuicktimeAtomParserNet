using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using RandomAccessFile = System.IO.Stream;


namespace QicktimeAtomParserNet.Lib
{
    public class AtomFactory
    {
        static AtomFactory instance;

        private static Type[] LEAF_ATOM_CONSTRUCTOR_ARGS =
        {
            typeof(long),
            typeof(String),
            typeof(Stream)
        };

        //protected Properties props;
        protected Dictionary<string, string> props = new Dictionary<string, string>()
        {
            {"WLOC", "ParsedWLOCAtom"},
            {"elst", "ParsedelstAtom"},
            {"hdlr", "ParsedhdlrAtom"}
        };

        /** location of the props file as found with ClassLoader
        getResource()... putting it in class' package structure
        keeps things tidy (works really well inside a jar too)
     */

        public static readonly String PROPS_RESOURCE_NAME =
            "com/mac/invalidname/qtatomparse/atomfactory.properties";

        /** private constructor can only be called by the first
        getInstance()
     */

        private AtomFactory()
        {
            /*
            // get props file
            props = new Properties();

            try
            {
                InputStream propsIn =
                    getClass().getClassLoader().getResourceAsStream(PROPS_RESOURCE_NAME);


                if (propsIn != null)
                    props.load(propsIn);
            }
            catch (IOException ioe)
            {
                Console.WriteLine(ioe); // ioe.printStackTrace();
            }
            */
        }

        /** returns the singleton, creating it if necessary
     */

        public static AtomFactory getInstance()
        {
            if (instance == null)
                instance = new AtomFactory();
            return instance;
        }

        /** Returns a ParsedLeafAtom (or subclass) for the given
        type.  Uses reflection to call the constructor with 
        the supplied args.  Default is basic ParsedLeafAtom but
        will give you a more specific atom if you have mapped
        the atom type to a class with the
        <code>atomfactory.properties</code> file.
     */

        public ParsedLeafAtom createAtomFor(long size, String type,
            RandomAccessFile raf)

        {
            string className;

            if (props.TryGetValue(type, out className))
            {
                
            }
            else
            {
                return new ParsedLeafAtom(size, type, raf);
            }
            //String className = props[type];
            
            // now try to instantiate and populate (scary-ass reflection)
            try
            {
                //Class atomClass = Class.forName(className);
                Type atomClass = Type.GetType(className);

                //Constructor constructor =
                //    atomClass.getDeclaredConstructor(LEAF_ATOM_CONSTRUCTOR_ARGS);


                Object[] args =
                {
                    (long) (size),
                    type,
                    raf
                };


                //var buf = (ParsedLeafAtom) constructor.newInstance(args);

                var buf =
                    (ParsedLeafAtom) Activator.CreateInstance(atomClass, BindingFlags.CreateInstance, null, args, null);

                return buf;
            }
            catch (Exception e)
            {
                //e.printStackTrace();
                // if anything went wrong, return the simple leaf atom
                return new ParsedLeafAtom(size, type, raf);
            }
        }
    }
}