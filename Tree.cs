using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadixTree
{
    public class Tree
    {
        /// <summary>
        /// store the tree's root
        /// </summary>
        private Node _root;

        /// <summary>
        /// construct a new tree with it's root
        /// </summary>
        public Tree()
        {
            _root = new Node("");
        }

        /// <summary>
        /// insert a word into the tree
        /// </summary>
        /// <param name="word"></param>
        public void Insert(string word)
        {
            InsertRec(word, _root);
        }

        /// <summary>
        /// recursively traverse the tree
        /// carry the word you want inserted until a proper place for it is found and it can be inserted there
        /// if a node already stores a substring of the word(substrnig with the same first letter as the word itself)
        /// then that substring must be removed from the word and it's children checked out next
        /// hence the name wordPart - part of a word
        /// </summary>
        /// <param name="wordPart">the part of the word that is to be inserted that is not already included in any of the tree's nodes</param>
        /// <param name="curNode">the node currently traversed</param>
        private void InsertRec(string wordPart, Node curNode)
        {
            //get the number of characters that the word part that is to be inserted and the current node's label have
            //in common starting from the first position of both strings
            //matching characters in the two strings = have the same value at the same position in both strings
            var matches = MatchingConsecutiveCharacters(wordPart, curNode);

            //if we are at the root node
            //OR
            //the number of characters from the two strings that match is
            //bigger than 0
            //smaller than the the part of the word that is to be inserted
            //bigger than the the label of the current node
            if  ((matches == 0) || (curNode == _root) ||
                ((matches > 0) && (matches < wordPart.Length) && (matches >= curNode.Label.Length)))
                {
                    //remove the current node's label from the word part
                    bool inserted = false;
                    var newWordPart = wordPart.Substring(matches, wordPart.Length - matches);   
                    //search the node's subnodes and if the subnode label's first character matches 
                    //the word part's first character then insert the word part after this node(call the
                    //current method recursively)
                    foreach(var child in curNode.SubNodes)
                        if (child.Label.StartsWith(newWordPart[0].ToString()))
                        {
                            inserted = true;
                            InsertRec(newWordPart, child);
                        }
                    if (inserted == false)
                    {
                        curNode.SubNodes.Add(new Node(newWordPart));
                    }
                }
                else if(matches < wordPart.Length)
                {
                    //in this case we have to nodes that we must add to the tree
                    //one is the node that has a label extracted from the current node's label without the string of 
                    //matching characters(common characters)
                    //the other is the node that has it's label extracted from the current word part minus the string
                    //of matching characters
                    string commonRoot = wordPart.Substring(0, matches);
                    string branchPreviousLabel = curNode.Label.Substring(matches, curNode.Label.Length - matches);
                    string branchNewLabel = wordPart.Substring(matches, wordPart.Length - matches);

                    curNode.Label = commonRoot;

                    var newNodePreviousLabel = new Node(branchPreviousLabel);
                    newNodePreviousLabel.SubNodes.AddRange(curNode.SubNodes);

                    curNode.SubNodes.Clear();
                    curNode.SubNodes.Add(newNodePreviousLabel);

                    var newNodeNewLabel = new Node(branchNewLabel);
                    curNode.SubNodes.Add(newNodeNewLabel);
                }
                else if (matches == curNode.Label.Length)
                {
                    //in this case we don't do anything because the word is already added
                }
                else if (matches > curNode.Label.Length)
                {
                    //add the current word part minus the common characters after the current node
                    string newNodeLabel = curNode.Label.Substring(curNode.Label.Length, wordPart.Length);
                    var newNode = new Node(newNodeLabel);
                    curNode.SubNodes.Add(newNode);
                }
        }

        /// <summary>
        /// given a string and a node the number of characters that the string and the node's label have
        /// in common starting from the first character in each is returned
        /// </summary>
        /// <param name="word">a string that is to be compared with the node's label</param>
        /// <param name="curNode">a node</param>
        /// <returns></returns>
        private int MatchingConsecutiveCharacters(string word, Node curNode)
        {
            int matches = 0;
            int minLength = 0;

            //see which string is smaller and save it's lenght
            //when cycling throught the two strings we won't go any further than that
            if (curNode.Label.Length >= word.Length)
                minLength = word.Length;
            else if (curNode.Label.Length < word.Length)
                minLength = curNode.Label.Length;

            if(minLength > 0)
                //go throught the two streams
                for (int i = 0; i < minLength; i++)
                {
                    //if two characters at the same position have the same value we have one more match
                    if(word[i] == curNode.Label[i])
                        matches++;
                    else
                        //if at any position the two strings have different characters break the cycle
                        break;
                }
            //and return the current number of matches
            return matches;
        }

        public bool Lookup(string word)
        {
            return LookupRec(word, _root);
        }

        /// <summary>
        /// look for a word in the tree begining at the current node 
        /// </summary>
        /// <param name="wordPart"></param>
        /// <param name="curNode"></param>
        /// <returns></returns>
        private bool LookupRec(string wordPart, Node curNode)
        {
            var matches = MatchingConsecutiveCharacters(wordPart, curNode);

            if ((matches == 0) || (curNode == _root) ||
                ((matches > 0) && (matches < wordPart.Length) && (matches >= curNode.Label.Length)))
            {
                
                var newLabel = wordPart.Substring(matches, wordPart.Length - matches);
                foreach (var child in curNode.SubNodes)
                    if (child.Label.StartsWith(newLabel[0].ToString()))
                        return LookupRec(newLabel, child);
                    
                return false;
            }
            else if (matches == curNode.Label.Length)
            {
                return true;
            }
            else return false;
        }


        //Find successor: Locates the smallest string greater than a given string, by lexicographic order.
        public string FindSuccessor(string word)
        {
            return FindSuccessorRec(word, _root,string.Empty);
        }

        private string FindSuccessorRec(string wordPart, Node curNode,string carry)
        {
            var matches = MatchingConsecutiveCharacters(wordPart, curNode);

            if ((matches == 0) || (curNode == _root) ||
                ((matches > 0) && (matches < wordPart.Length) ))
            {

                var newLabel = wordPart.Substring(matches, wordPart.Length - matches);
                foreach (var child in curNode.SubNodes)
                    if (child.Label.StartsWith(newLabel[0].ToString()))
                        return FindSuccessorRec(newLabel, child, carry + curNode.Label);

                return curNode.Label;
            }
            else if (matches < curNode.Label.Length)
            {
                return carry + curNode.Label;
            }
            else if (matches == curNode.Label.Length)
            {
                carry = carry + curNode.Label;

                int min = int.MaxValue;
                int index = -1;
                for (int i = 0; i < curNode.SubNodes.Count;i++ )
                    if (curNode.SubNodes[i].Label.Length < min)
                    {
                        min = curNode.SubNodes[i].Label.Length;
                        index = i;
                    }

                if (index > -1)
                    return carry + curNode.SubNodes[index].Label;
                else
                    return carry;
            }
            else return string.Empty;
        }

        /// <summary>
        ///Find predecessor: Locates the largest string less than a given string, by lexicographic order.
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public string FindPredecessor(string word)
        {
            return FindPredecessorRec(word, _root,string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="wordPart"></param>
        /// <param name="curNode"></param>
        /// <returns></returns>
        private string FindPredecessorRec(string wordPart, Node curNode,string carry)
        {
            var matches = MatchingConsecutiveCharacters(wordPart, curNode);

            if ((matches == 0) || (curNode == _root) ||
                ((matches > 0) && (matches < wordPart.Length) && (matches >= curNode.Label.Length)))
            {

                var newLabel = wordPart.Substring(matches, wordPart.Length - matches);
                foreach (var child in curNode.SubNodes)
                    if (child.Label.StartsWith(newLabel[0].ToString()))
                        return FindPredecessorRec(newLabel, child, carry + curNode.Label);

                return carry + curNode.Label;
            }
            else if (matches == curNode.Label.Length)
            {
                return carry + curNode.Label;
            }
            else return string.Empty;
        }

        /// <summary>
        /// Delete: Delete a string from the tree. First, we delete the corresponding leaf. 
        /// Then, if its parent only has one child remaining, we delete the parent and merge the two incident edges.
        /// </summary>
        /// <param name="label"></param>
        public void Delete(string label)
        {
            DeleteRec(label, _root);
        }

        /// <summary>
        /// delete a word from the tree means delete the last leaf that makes up the stored word
        /// </summary>
        /// <param name="label"></param>
        /// <param name="curNode"></param>
        public void DeleteRec(string wordPart, Node curNode)
        {
            var matches = MatchingConsecutiveCharacters(wordPart, curNode);

            if ((matches == 0) || (curNode == _root) ||
                ((matches > 0) && (matches < wordPart.Length) && (matches >= curNode.Label.Length)))
            {

                var newLabel = wordPart.Substring(matches, wordPart.Length - matches);
                foreach (var child in curNode.SubNodes)
                    if (child.Label.StartsWith(newLabel[0].ToString()))
                    {
                        if (newLabel == child.Label)
                        {
                            if (child.SubNodes.Count == 0)
                            {
                                curNode.SubNodes.Remove(child);
                                return;
                            }
                        }
                        
                        DeleteRec(newLabel, child);
                    }
            }
        }
    }
}
