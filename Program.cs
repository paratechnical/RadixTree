using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadixTree
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = new Tree();
            tree.Insert("romane");
            tree.Insert("romanus");
            tree.Insert("romulus");
            tree.Insert("rubens");
            tree.Insert("ruber");
            tree.Insert("rubicon");
            tree.Insert("rubicundus");

            Console.WriteLine("predecessor of the word \"romanes\":" + tree.FindPredecessor("romanes"));
            Console.WriteLine("successor of the word \"rom\":" + tree.FindSuccessor("rom"));

            Console.WriteLine(tree.Lookup("romulus") ? "there" : "not there");

            tree.Delete("romanus");
        }
    }
}
