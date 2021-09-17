using System;
using System.Collections.Generic;

namespace assignment2bro
{
    //Simple operation class that contains the informarion of a line
    class Operation
    {
        public string operation;
        public int number;

        //Constructor initializes the variables
        public Operation(string operation, int number)
        {
            this.operation = operation;
            this.number = number;
        }
    }

    //Read the file and create all the tree stacks
    class TreeManager
    {
        TreeStack currentStack;
        int counter = 0;
        string line;
        bool flag = true;
        String path;

        //Constructor 
        public TreeManager(String path)
        {
            this.path = path;
        }

        //Takes values until "#", then redirects the data for the tree operations and keep stats
        public void run()
        {
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            string firstChar;
            string[] splitLine = new string[2];
            TwoThreeTree tree = new TwoThreeTree();
            TwoFourTree twoFourTree = new TwoFourTree();

            //Read line by line
            while ((line = file.ReadLine()) != null)
            {
                splitLine = line.Split(' ');
                firstChar = splitLine[0];

                

                //According to the first char of every line, perform an action
                switch (firstChar)
                {
                    case "#":

                        //if it is the first line just create a new stack
                        if (flag)
                        {
                            currentStack = new TreeStack(int.Parse(splitLine[1]));
                            flag = false;
                            
                        }
                        else
                        {
                            //TODO run tree builder  
                            currentStack = new TreeStack(int.Parse(splitLine[1]));

                            foreach (var o in currentStack.operations)
                            {
                                if (o.operation == "I")
                                {
                                    tree.Insert(o.number);
                                    twoFourTree.Insert(o.number);

                                }
                                else if (o.operation == "D")
                                {
                                    tree.delete(o.number);
                                    twoFourTree.delete(o.number);
                                }

                                /*
                                Console.Clear();
                                tree.root.PrintNode(1);*/

                                /*Console.Clear();
                                 twoFourTree.root.PrintNode(1);*/
                                
                            }

                            twoFourTree.root.PrintNode(1);
                            tree.root.PrintNode(1);
                            Console.WriteLine("Line Numbers " + counter);
                        }

                        break;

                    case "I":
                        currentStack.Push(new Operation("I", int.Parse(splitLine[1])));
                        break;

                    case "D":
                        currentStack.Push(new Operation("D", int.Parse(splitLine[1])));
                        break;
                }
                
                counter++;
            }

          
            //FOr the last set of numbers
            //TwoFourTree twoFourTree = new TwoFourTree();
            foreach (var o in currentStack.operations)
            {
                if (o.operation == "I")
                {
                    tree.Insert(o.number);
                    twoFourTree.Insert(o.number);

                }
                else if (o.operation == "D")
                {
                    tree.delete(o.number);
                    twoFourTree.delete(o.number);
                }

                /*
                Console.Clear();
                tree.root.PrintNode(1);*/

                /*Console.Clear();
                 twoFourTree.root.PrintNode(1);*/

            }

            twoFourTree.root.PrintNode(1);
            tree.root.PrintNode(1);
            Console.WriteLine("Line Numbers " + counter);
           
            //tree.root.PrintNode(1);

            file.Close();
            Console.ReadLine();
        }

        void ApplyOperations(Tree tree)
        {

        }
    }

    /*
     * This is the Stack class for the tree operation that is going to be performed
     */

    class TreeStack
    {
        public List<Operation> operations;
        int pTop;
        int size;

        //Constructor
        public TreeStack(int size)
        {
            this.operations = new List<Operation>();
            this.size = size;
            pTop = -1;
        }

        //Adds a operation to the stack
        public void Push(Operation operation)
        {
            operations.Add(operation);
            pTop++;
        }

        //Retrieves the last inserted operation
        public Operation Pop()
        {
            pTop--;
            return (Operation)operations[pTop + 1];
        }

        //Checks whether the stack is empty or not 
        public bool isEmpty()
        {
            bool b = pTop == -1 ? true : false;
            return b;
        }

    }

    /*
     GENERAL TREE NODE IMPLEMENTATIONS
     */
    public abstract class TreeNode
    {
        public bool isLeaf;
        public int[] elements;                 //elements[0] : leftmost element , elements[1] : middle element , elements[2] : rightmost element
        public TreeNode[] children;    //children[0] : leftmost child , children[1] : middle child , children[2] : rightmost child
        public TreeNode parent;        //parent node
        public int numberOfElements;           //this variable indicates the number of the elements that stored in the node 
        //in case this node isn't a leaf, numberOfElement + 1 gives the number of children

        public TreeNode()
        {
            this.parent = null;
        }

        public bool IsLeaf()
        {
            bool result = true;
            for (int i = 0; i < this.children.Length; i++)
            {
                if (this.children[i] != null)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        public int NumberOfElements()
        {
            int count = 0;
            for (int i = 0; i < this.elements.Length; i++)
            {
                if (this.elements[i] != 0)
                    count++;
            }
            return count;
        }

        public void PrintNode(int indent)
        {
            int i = 0;

            // print indent spaces
            for (i = 0; i < indent; i++)
                Console.Write(" ");
            Console.Write("<-");
            // print the data from this node
            for (i = 0; i < this.elements.Length; i++)
            {
                if (true)
                    Console.Write(this.elements[i] + "-");

            }

            Console.Write(">");
            // print endl at end of the root data
            Console.WriteLine();

            // recursively print children
            for (i = 0; i < this.children.Length; i++)
            {
                if (this.children[i] != null)
                    this.children[i].PrintNode(indent + 14);
            }

        }
    }

    //Represents a node of 2,3 Tree
    public class TwoThreeTreeNode : TreeNode
    {
        public TwoThreeTreeNode()
        {
            this.elements = new int[2];
            this.children = new TwoThreeTreeNode[4];
        }

        public static implicit operator TwoFourTreeNode(TwoThreeTreeNode twoTreeNode)
        {
            return new TwoFourTreeNode() {elements = twoTreeNode.elements,children = twoTreeNode.children, parent=twoTreeNode.parent, numberOfElements = twoTreeNode.NumberOfElements()};
        }
    }

    //Represents a node of 2,4 Tree
    public class TwoFourTreeNode : TreeNode
    {
        public TwoFourTreeNode()
        {
            this.elements = new int[3];
            this.children = new TwoFourTreeNode[5];
        }

        public static implicit operator TwoThreeTreeNode(TwoFourTreeNode twoFourNode)
        {
            return new TwoThreeTreeNode() { elements = twoFourNode.elements,children = twoFourNode.children, parent=twoFourNode.parent, numberOfElements = twoFourNode.NumberOfElements()};
        }
    }
    
    /*
     GENERAL TREE IMPLEMENTATIONS
     */

    public abstract class Tree
    {
        public TreeNode root;
        public int limit;

        public Tree() {
            this.limit = 0; 
        }

        public void initRoot()
        {
            this.root.isLeaf = true;
            this.root.parent = null;
        }

        public void Insert(int element)
        {
            TreeNode treeNode = this.FindSubtreeLeaf(this.root, element);
            if (!this.TryInsert(treeNode, element))
                this.Split(treeNode, element);
        }

        //Tries to insert the elements, if not returns false
        public bool TryInsert(TreeNode treeNode, int element)
        {
            bool result = true;
       
            //If it is root just add the element to the root
            if (treeNode.NumberOfElements() == 0 && treeNode.parent == null)
            {
                treeNode.elements[0] = element;
                
            }
            
            else if (treeNode.NumberOfElements() < this.limit)
            {
                treeNode.elements[treeNode.NumberOfElements()] = element;
                
                this.SortElements(treeNode.elements, treeNode.NumberOfElements());
                
            }
            else
            {
                result = false;
            }

            return result;
        }

        public virtual void Split(TreeNode treeNode, int element)
        {
            Console.WriteLine("This method should be overrided");
        }

        public TreeNode DeleteElement(int element)
        {
            //variables;
            int inOrderInd;
            TreeNode leafNode;
            TreeNode inOrderSuccessor;
            int tempElement;
            TreeNode shouldFixed = null;

            //return the node that contains the element
            TreeNode node = this.FindNode(this.root, element);

            //Check the node
            if (node != null)
            {
                //index of the element in that node
                int ind = node.elements[0] == element ? 0 : node.elements[1] == element ? 1 : 2;

                if (node.IsLeaf() == false)
                {
                    //find the inorder succsesor of the element
                    inOrderSuccessor = this.InOrderSuccessor(element, node);

                    //normally inorder successor must be at the first index but just to be sure checking the values
                    inOrderInd = 0;
                   

                    //swap the item with the inorder successor
                    tempElement = node.elements[ind];
                    node.elements[ind] = inOrderSuccessor.elements[inOrderInd];
                    inOrderSuccessor.elements[inOrderInd] = tempElement;

                    leafNode = inOrderSuccessor;
                }
                //if node in the leaf
                else
                {
                    leafNode = node;
                }

                //delete the element
                if (element == leafNode.elements[0])
                {
                    //move the second element to the first then assign the second to 0
                    if (leafNode.NumberOfElements() == 1)
                    {
                        leafNode.elements[0] = 0;
                    }
                    else if (leafNode.NumberOfElements() == 2)
                    {
                        leafNode.elements[0] = leafNode.elements[1];
                        leafNode.elements[1] = 0;
                    }
                    else
                    {
                        leafNode.elements[0] = leafNode.elements[1];
                        leafNode.elements[1] = leafNode.elements[2];
                        leafNode.elements[2] = 0;
                    }
                }
                else if (element == leafNode.elements[1])
                {
                    //if it is second element just delete it, don't move anything

                    if (leafNode.NumberOfElements() == 2)
                    {
                        leafNode.elements[1] = 0;
                    }
                    else
                    {
                        leafNode.elements[1] = leafNode.elements[2];
                        leafNode.elements[2] = 0;
                    }
                }

                else if (element == leafNode.elements[2])
                {
                    //if it is third element just delete it, don't move anything
                    leafNode.elements[2] = 0;

                }
                else
                {
                    //Due to previous step, first or second element must be match, but just to be sure
                    Console.WriteLine("There must be a mistake, element couldn't be found in the node!");
                }

                //after deleting the element, if the node is empty, fix the tree
                if (leafNode.NumberOfElements() == 0)
                {
                    //Fix(leafNode);
                    shouldFixed = leafNode;
                }
            }
            else
            {
                Console.WriteLine(element + " Couldn't found in the tree, thus process terminated");
            }

            return shouldFixed;
        }


        public TreeNode FindSubtreeLeaf(TreeNode node, int element)
        {
            if (node.IsLeaf())
                return node;

            else
            {
                if (element < node.elements[0])
                    return this.FindSubtreeLeaf(node.children[0], element);

                else if (node.NumberOfElements() == 1 || (element > node.elements[0] && element < node.elements[1]))
                    return this.FindSubtreeLeaf(node.children[1], element);

                else if (node.NumberOfElements() == 2 || (element > node.elements[1] && element < node.elements[2]))
                    return this.FindSubtreeLeaf(node.children[2], element);

                else
                    return this.FindSubtreeLeaf(node.children[3], element);
            }
        }

        public TreeNode FindNode(TreeNode node, int element)
        {
            bool isFound = false;
            if (node != null)
            {
                for (int i = 0; i < node.NumberOfElements(); i++)
                {
                    if (node.elements[i] == element)
                        isFound = true;
                }

                if (isFound == true)
                    return node;

                else if (node.NumberOfElements() == 1)
                {
                    if (element < node.elements[0])
                        return FindNode(node.children[0], element);
                    else
                        return FindNode(node.children[1], element);
                }
                else if (node.NumberOfElements() == 2)
                {
                    if (element < node.elements[0])
                    {
                        return FindNode(node.children[0], element);
                    }
                    else if (element > node.elements[1])
                    {
                        return FindNode(node.children[2], element);
                    }
                    else
                    {
                        return FindNode(node.children[1], element);
                    }
                }
                else if (node.NumberOfElements() == 3)
                {
                    if (element < node.elements[0])
                    {
                        return FindNode(node.children[0], element);
                    }
                    else if (element > node.elements[0] && element < node.elements[1])
                    {
                        return FindNode(node.children[1], element);
                    }
                    else if (element > node.elements[1] && element < node.elements[2])
                    {
                        return FindNode(node.children[2], element);
                    }
                    else
                    {
                        return FindNode(node.children[3], element);
                    }
                }
            }

            return null;
        }

        public TreeNode InOrderSuccessor(int element, TreeNode node)
        {
            TreeNode next;

            // When this method is called, key will equal smallValue or largeValue, and we must do a comparison.
            // We check if location is a three node and, if it is, we compare key to smallValue. If equal, go down middleChild.
            if (node.children[0] != null && node.children[1] != null && node.children[2] != null && node.children[3] != null)
            {
                if (node.elements[0] == element)
                {
                    next = node.children[1];
                }
                else if (node.elements[1] == element)
                {
                    next = node.children[2];
                }
                else
                {
                    next = node.children[3];
                }
            }

            else if (node.children[0] != null && node.children[1] != null && node.children[2] != null)
            {
                if (node.elements[0] == element)
                {
                    next = node.children[1];
                }
                else
                {
                    next = node.children[2];
                }
            }
            else
            {
                next = node.children[1];
            }

            // Continue down left branches until we encounter a leaf.
            while (next.IsLeaf() == false)
            {
                next = next.children[0];
            }

            return next;
        }

        //Look for the right or the closest siblings with two or three elements, if not returns -1
        public int HasChildWithTwoElement(TreeNode node)
        {
            int result = -1;

            //check the number of the children equality to 2
            if (node.NumberOfElements() == 1)
            {
                for (int i = 0; i < node.children.Length; i++)
                {
                    if (node.children[i] != null)
                    {
                        if (node.children[i].NumberOfElements() == 2 || node.children[i].NumberOfElements() == 3)
                        {
                            result = i;
                            result = result + node.children[i].NumberOfElements() * 10;
                            break;
                        }
                    }
                }
            }

            //number of the children equality to 3
            else if (node.NumberOfElements() == 2)
            {
                if (node.children[0].NumberOfElements() == 0)
                {
                    for (int i = 0; i < node.children.Length; i++)
                    {
                        if (node.children[i] != null)
                        {
                            if (node.children[i].NumberOfElements() == 2 || node.children[i].NumberOfElements() == 3)
                            {
                                result = i;
                                result = result + node.children[i].NumberOfElements() * 10;
                                break;
                            }
                        }
                    }
                }

                // I should take from the right first so the loop counter is decreasing
                else
                {
                    for (int i = node.children.Length - 1; i >= 0; i--)
                    {
                        if (node.children[i] != null)
                        {
                            if (node.children[i].NumberOfElements() == 2 || node.children[i].NumberOfElements() == 3)
                            {
                                result = i;
                                result = result + node.children[i].NumberOfElements() * 10;
                                break;
                            }
                        }
                    }
                }
            }

            //number of the children equality to 4
            else if (node.NumberOfElements() == 3)
            {
                if (node.children[0].NumberOfElements() == 0)
                {
                    for (int i = 0; i < node.children.Length; i++)
                    {
                        if (node.children[i] != null)
                        {
                            if (node.children[i].NumberOfElements() == 2 || node.children[i].NumberOfElements() == 3)
                            {
                                result = i;
                                result = result + node.children[i].NumberOfElements() * 10;
                                break;
                            }
                        }
                    }
                }

               // I should take from the right first so the loop counter is decreasing
                else
                {

                    if (node.children[1].NumberOfElements() == 0)
                    {
                        //Check for the element at the right first
                        for (int i = 1; i < node.children.Length; i++)
                        {
                            if (node.children[i] != null)
                            {
                                if (node.children[i].NumberOfElements() == 2 || node.children[i].NumberOfElements() == 3)
                                {
                                    result = i;
                                    result = result + node.children[i].NumberOfElements() * 10;
                                    break;
                                }
                            }
                        }

                        //If doesn't exist in the right look left
                        if (result == -1)
                        {
                            if (node.children[0].NumberOfElements() == 2 || node.children[0].NumberOfElements() == 3)
                            {
                                result = node.children[0].NumberOfElements() * 10;
                            }
                        }
                    }

                    else
                    {
                        for (int i = node.children.Length - 1; i >= 0; i--)
                        {
                            if (node.children[i] != null)
                            {
                                if (node.children[i].NumberOfElements() == 2 || node.children[i].NumberOfElements() == 3)
                                {
                                    result = i;
                                    result = result + node.children[i].NumberOfElements() * 10;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        public void Merge(TreeNode node)
        {
            if (node.parent.NumberOfElements() == 1)
            {
                //empty node at the left
                if (node.parent.children[0] == node)
                {
                    node.parent.children[1].elements[1] = node.parent.children[1].elements[0];
                    node.parent.children[1].elements[0] = node.parent.elements[0];
                    node.parent.elements[0] = 0;



                    //remove the node
                    node.parent.children[0] = node.parent.children[1];
                    node.parent.children[1] = null;

                    //if node is internal
                    if (node.IsLeaf() == false)
                    {
                        node.parent.children[0].children[2] = node.parent.children[0].children[1];
                        node.parent.children[0].children[1] = node.parent.children[0].children[0];
                        node.parent.children[0].children[0] = node.children[0];
                        //update the parent
                        node.parent.children[0].children[0].parent = node.parent.children[0];
                    }
                }
                //empty node at the right
                else
                {
                    node.parent.children[0].elements[1] = node.parent.elements[0];
                    node.parent.elements[0] = 0;

                    node.parent.children[1] = null;

                    //if node is internal
                    if (node.IsLeaf() == false)
                    {
                        node.parent.children[0].children[2] = node.children[0];
                        node.parent.children[0].children[2].parent = node.parent.children[0];
                    }
                }
            }
            else if (node.parent.NumberOfElements() == 2)
            {
                //empty node at the left
                if (node.parent.children[0] == node)
                {
                    node.parent.children[1].elements[1] = node.parent.children[1].elements[0];
                    node.parent.children[1].elements[0] = node.parent.elements[0];
                    node.parent.elements[0] = node.parent.elements[1];
                    node.parent.elements[1] = 0;


                    node.parent.children[0] = node.parent.children[1];
                    node.parent.children[1] = node.parent.children[2];
                    node.parent.children[2] = null;

                    //if node is internal
                    if (node.IsLeaf() == false)
                    {
                        node.parent.children[0].children[2] = node.parent.children[0].children[1];
                        node.parent.children[0].children[1] = node.parent.children[0].children[0];
                        node.parent.children[0].children[0] = node.children[0];

                        node.parent.children[0].children[0].parent = node.parent.children[0];
                    }

                }

                //empty node at the middle
                else if (node.parent.children[1] == node)
                {
                    node.parent.children[0].elements[1] = node.parent.elements[0];
                    node.parent.elements[0] = node.parent.elements[1];
                    node.parent.elements[1] = 0;


                    node.parent.children[1] = node.parent.children[2];
                    node.parent.children[2] = null;

                    //if node is internal
                    if (node.IsLeaf() == false)
                    {
                        node.parent.children[0].children[2] = node.children[0];
                        node.parent.children[0].children[2].parent = node.parent.children[0];
                    }
                }

                //empty node at the right
                else
                {
                    node.parent.children[1].elements[1] = node.parent.elements[1];
                    node.parent.elements[1] = 0;

                    node.parent.children[2] = null;

                    //if node is internal
                    if (node.IsLeaf() == false)
                    {
                        node.parent.children[1].children[2] = node.children[0];
                        node.parent.children[1].children[2].parent = node.parent.children[1];
                    }
                }
            }
            else if (node.parent.NumberOfElements() == 3)
            {
                if (node.parent.children[0] == node)
                {
                    node.parent.children[1].elements[1] = node.parent.children[1].elements[0];
                    node.parent.children[1].elements[0] = node.parent.elements[0];
                    node.parent.elements[0] = node.parent.elements[1];
                    node.parent.elements[1] = node.parent.elements[2];
                    node.parent.elements[2] = 0;


                    node.parent.children[0] = node.parent.children[1];
                    node.parent.children[1] = node.parent.children[2];
                    node.parent.children[2] = node.parent.children[3];
                    node.parent.children[3] = null;

                    //if node is internal
                    if (node.IsLeaf() == false)
                    {
                        node.parent.children[0].children[2] = node.parent.children[0].children[1];
                        node.parent.children[0].children[1] = node.parent.children[0].children[0];
                        node.parent.children[0].children[0] = node.children[0];

                        node.parent.children[0].children[0].parent = node.parent.children[0];
                    }
                }
                else if (node.parent.children[1] == node)
                {
                    node.parent.children[0].elements[1] = node.parent.elements[0];
                    node.parent.elements[0] = node.parent.elements[1];
                    node.parent.elements[1] = node.parent.elements[2];
                    node.parent.elements[2] = 0;

                    node.parent.children[1] = node.parent.children[2];
                    node.parent.children[2] = node.parent.children[3];
                    node.parent.children[3] = null;

                    //if node is internal
                    if (node.IsLeaf() == false)
                    {
                        node.parent.children[0].children[2] = node.children[0];
                        node.parent.children[0].children[2].parent = node.parent.children[0];
                    }
                }
                else if (node.parent.children[2] == node)
                {
                    node.parent.children[1].elements[1] = node.parent.elements[1];
                    node.parent.elements[1] = node.parent.elements[2];
                    node.parent.elements[2]= 0;

                    node.parent.children[2] = node.parent.children[3];
                    node.parent.children[3] = null;

                    //if node is internal
                    if (node.IsLeaf() == false)
                    {
                        node.parent.children[1].children[2] = node.children[0];
                        node.parent.children[1].children[2].parent = node.parent.children[1];
                    }
                }
                else if (node.parent.children[3] == node)
                {
                    node.parent.children[2].elements[1] = node.parent.elements[2];
                    node.parent.elements[2] = 0;

                    node.parent.children[3] = null;

                    //if node is internal
                    if (node.IsLeaf() == false)
                    {
                        node.parent.children[2].children[2] = node.children[0];
                        node.parent.children[2].children[2].parent = node.parent.children[2];
                    }
                }
            }
        }

        public void SortElements(int[] elements,int numberOfElement){
            int temp = 0;

            for (int write = 0; write < numberOfElement; write++) {
                for (int sort = 0; sort < numberOfElement - 1; sort++) {
                    if (elements[sort] > elements[sort + 1]) {
                        temp = elements[sort + 1];
                        elements[sort + 1] = elements[sort];
                        elements[sort] = temp;
                    }
                }
            }
        }

        public void TakeElementFromNextRightSibling(TreeNode p, TreeNode node, int n)
        {
            node.elements[0] = p.elements[n];
            p.elements[n] = p.children[n + 1].elements[0];

            if (this.limit == 2)
            {
                p.children[n + 1].elements[0] = p.children[n + 1].elements[1];
                p.children[n + 1].elements[1] = 0;
            }
            else 
            {
               
                p.children[n + 1].elements[0] = p.children[n + 1].elements[1];
                p.children[n + 1].elements[1] = p.children[n + 1].elements[2];
                p.children[n + 1].elements[2] = 0;
            }

            if (node.IsLeaf() == false)
            {
                node.children[1] = p.children[n + 1].children[0];

                if (this.limit == 2)
                {
                    p.children[n + 1].children[0] = p.children[n + 1].children[1];
                    p.children[n + 1].children[1] = p.children[n + 1].children[2];
                    p.children[n + 1].children[2] = null;
                }

                else 
                {
                   
                    p.children[n + 1].children[0] = p.children[n + 1].children[1];
                    p.children[n + 1].children[1] = p.children[n + 1].children[2];
                    p.children[n + 1].children[2] = p.children[n + 1].children[3];
                    p.children[n + 1].children[3] = null;
                }

                node.children[1].parent = node;
            }
        }

        public void TakeElementFromNextLeftSibling(TreeNode p, TreeNode node, int n)
        {
            node.elements[0] = p.elements[n];

            if (p.children[n].NumberOfElements() == 2)
            {
                p.elements[n] = p.children[n].elements[1];
                p.children[n].elements[1] = 0;
            }
            else 
            {
              
                p.elements[n] = p.children[n].elements[2];
                p.children[n].elements[2] = 0;
            }

            if (node.IsLeaf() == false)
            {
                node.children[1] = node.children[0];

                if (p.children[n].NumberOfElements() == 1)
                {
                    node.children[0] = p.children[n].children[2];
                    p.children[n].children[2] = null;
                }
                else 
                {
                 
                    node.children[0] = p.children[n].children[3];
                    p.children[n].children[3] = null;
                }

                node.children[0].parent = node;
            }
        }

        public void TakeElementFromTwoNextRightSibling(TreeNode p, TreeNode node, int n)
        {
            node.elements[0] = p.elements[n];
            p.elements[n] = p.children[n + 1].elements[0];
            p.children[n + 1].elements[0] = p.elements[n + 1];
            p.elements[n + 1] = p.children[n + 2].elements[0];

            if (p.children[n + 2].NumberOfElements() == 2)
            {
                p.children[n + 2].elements[0] = p.children[n + 2].elements[1];
                p.children[n + 2].elements[1] = 0;
            }

            else if (p.children[n + 2].NumberOfElements() == 3)
            {
                p.children[n + 2].elements[0] = p.children[n + 2].elements[1];
                p.children[n + 2].elements[1] = p.children[n + 2].elements[2];
                p.children[n + 2].elements[2] = 0;
            }

            if (node.IsLeaf() == false)
            {
                node.children[1] = p.children[n + 1].children[0];
                p.children[n + 1].children[0] = p.children[n + 1].children[1];
                p.children[n + 1].children[1] = p.children[n + 2].children[0];

                if (p.children[n + 2].NumberOfElements() == 1)
                {
                    p.children[n + 2].children[0] = p.children[n + 2].children[1];
                    p.children[n + 2].children[1] = p.children[n + 2].children[2];
                    p.children[n + 2].children[2] = null;
                }
                else if (p.children[n + 2].NumberOfElements() == 2)
                {
                    p.children[n + 2].children[0] = p.children[n + 2].children[1];
                    p.children[n + 2].children[1] = p.children[n + 2].children[2];
                    p.children[n + 2].children[2] = p.children[n + 2].children[3];
                    p.children[n + 2].children[3] = null;
                }

                node.children[1].parent = node;
                p.children[n + 1].children[1].parent = p.children[n + 1];
            }
        }

        public void TakeElementFromTwoNextLeftSibling(TreeNode p, TreeNode node, int n)
        {
            node.elements[0] = p.elements[n - 1];
            p.elements[n - 1] = p.children[n - 1].elements[0];
            p.children[n - 1].elements[0] = p.elements[n-2];

            if (p.children[n - 2].NumberOfElements() == 2)
            {
                p.elements[n - 2] = p.children[n - 2].elements[1];
                p.children[n - 2].elements[1] = 0;
            }

            else if (p.children[n - 2].NumberOfElements() == 3)
            {
                p.elements[n - 2] = p.children[n - 2].elements[2];
                p.children[n - 2].elements[2] = 0;
            }

            if (node.IsLeaf() == false)
            {

                node.children[1] = node.children[0];
                node.children[0] = p.children[n-1].children[1];
                p.children[n - 1].children[1] = p.children[n - 1].children[0];

                if (p.children[n - 2].NumberOfElements() == 1)
                {
                    p.children[n - 1].children[0] = p.children[n - 2].children[2];
                    p.children[n - 2].children[2] = null;
                }

                else if (p.children[n - 2].NumberOfElements() == 2)
                {

                    p.children[n - 1].children[0] = p.children[n - 2].children[3];
                    p.children[n - 2].children[3] = null;
                }

                node.children[0].parent = node;
                p.children[n - 1].children[0].parent = p.children[n - 1];
            }
        }
    }
    

    //Represents a 2,4 Tree
    public class TwoFourTree : Tree {
      
        //Constructor
        public TwoFourTree(){
            this.root = new TwoFourTreeNode();
            this.limit = 3;
            this.initRoot();
        }

        public override void Split(TreeNode twoFourNode, int element)
        {
            TwoFourTreeNode p;
            twoFourNode = (TwoFourTreeNode) twoFourNode;
            

            if (twoFourNode.parent == null)
            {
                p = new TwoFourTreeNode();
            }
            else
            {
                p = (TwoFourTreeNode) twoFourNode.parent;
            }

            TwoFourTreeNode n1 = new TwoFourTreeNode();
            TwoFourTreeNode n2 = new TwoFourTreeNode();

            int[] elements = { element, twoFourNode.elements[0], twoFourNode.elements[1], twoFourNode.elements[2] };
            int middle;
            this.SortElements(elements, elements.Length);

            n1.elements[0] = elements[0];
            n1.elements[1] = elements[1];
            n2.elements[0] = elements[3];
            middle = elements[2];
            n1.parent = p;
            n2.parent = p;

            if (p.NumberOfElements() == 0)
            {
                p.children[0] = n1;
                p.children[1] = n2;
                this.root = p;   
            }

            else if (p.NumberOfElements() == 1)
            {
                if (n2.elements[0] < p.elements[0])
                {
                    p.children[2] = p.children[1];
                    p.children[0] = n1;
                    p.children[1] = n2;
                }
                else
                {
                    p.children[1] = n1;
                    p.children[2] = n2;
                } 
            }
            else if (p.NumberOfElements() == 2)
            {
                if (n2.elements[0] < p.elements[0] && n2.elements[0] < p.elements[1])
                {
                    p.children[3] = p.children[2];
                    p.children[2] = p.children[1];
                    p.children[0] = n1;
                    p.children[1] = n2;
                }
                else if (n1.elements[1] > p.elements[0] && n1.elements[1] > p.elements[1])
                {
                    p.children[2] = n1;
                    p.children[3] = n2;
                }
                else
                {
                    p.children[3] = p.children[2];
                    p.children[1] = n1;
                    p.children[2] = n2;
                }
            }
            else if (p.NumberOfElements() == 3)
            {
                if (n2.elements[0] < p.elements[0])
                {
                    p.children[4] = p.children[3];
                    p.children[3] = p.children[2];
                    p.children[2] = p.children[1];
                    p.children[1] = n2;
                    p.children[0] = n1;
                }

                else if (n1.elements[0] > p.elements[0] && n2.elements[0] < p.elements[1])
                {
                    p.children[4] = p.children[3];
                    p.children[3] = p.children[2];
                    p.children[2] = n2;
                    p.children[1] = n1;
                }

                else if (n1.elements[0] > p.elements[1] && n2.elements[0] < p.elements[2])
                {
                    p.children[4] = p.children[3];
                    p.children[3] = n2;
                    p.children[2] = n1;
                }
                else
                {
                    p.children[3] = n1;
                    p.children[4] = n2;
                }
            }

            //if it is not a leaf check
            if (twoFourNode.IsLeaf() == false)
            {
                twoFourNode.children[0].parent = n1;
                twoFourNode.children[1].parent = n1;
                twoFourNode.children[2].parent = n1;

                twoFourNode.children[3].parent = n2;
                twoFourNode.children[4].parent = n2;

                n1.children[0] = twoFourNode.children[0];
                n1.children[1] = twoFourNode.children[1];
                n1.children[2] = twoFourNode.children[2];

                n2.children[0] = twoFourNode.children[3];
                n2.children[1] = twoFourNode.children[4];
            }

            if (p.NumberOfElements() == 3)
            {
                this.Split(p, middle);             
            }

            else if (p.NumberOfElements() < this.limit)
            {
                p.elements[p.NumberOfElements()] = middle;
                
                this.SortElements(p.elements, p.NumberOfElements());
            }
        }

        public void delete(int element)
        {
            TwoFourTreeNode shouldFixed = (TwoFourTreeNode) this.DeleteElement(element);
            if (shouldFixed != null)
                Fix(shouldFixed);
        }

        //completes the deletion when node n is empty by either, removing the root, redistributing values, or merging nodes.
        //Note : if n is internal, it has only one child
        public void Fix(TwoFourTreeNode node)
        {
            TwoFourTreeNode p;

            if (node.parent == null) //No parent means it is root node
            {
                if (node.children[0] == null) { return; }
                //remove root and set the children as a root
                this.root = node.children[0];
                this.root.parent = null;
            }

            else
            {
                p = (TwoFourTreeNode)node.parent;

                //situation represents both the possibility of merge and the sibling with two elements
                //if it returns -1, means there is no siblings with two elements
                //if it is bigger than -1, means that index has the two elements
                int situation = this.HasChildWithTwoElement(p);

                //check for redistrubution is possible or not
                if (situation > -1)
                {
                    //redistrubute
                    this.Redistrubute(node, p, situation);
                }

                //redistrubuting not possible, thus merge
                //for merging we have 5 different situation; 2 for parent with one element, 3 for parent with two element
                else
                {
                    this.Merge(node);
                }

                if (node.parent.NumberOfElements() == 0)
                {
                    this.Fix(p);
                }
            }
        }

        void Redistrubute(TwoFourTreeNode node, TwoFourTreeNode p, int situation)
        {

            //there is two different situation if the parent has two nodes
            if (p.NumberOfElements() == 1)
            {
                if (situation == 20 || situation == 30)
                {
                    this.TakeElementFromNextLeftSibling(p, node, 0);
                }
                else
                {
                    this.TakeElementFromNextRightSibling(p, node, 0);
                }
            }

             //there is six different situation if the parent has three nodes
            else if (p.NumberOfElements() == 2)
            {
                //Check for which children is empty
                if (p.children[0].NumberOfElements() == 0)
                {
                    //situation can be 1 or 2
                    if (situation == 21 || situation == 31)
                    {
                        this.TakeElementFromNextRightSibling(p, node, 0);
                    }
                    else
                    {
                        this.TakeElementFromTwoNextRightSibling(p, node, 0);
                    }
                }
                else if (p.children[1].NumberOfElements() == 0)
                {
                    //situation can be 0 or 2
                    if (situation == 20 || situation == 30)
                    {
                        this.TakeElementFromNextLeftSibling(p, node, 0);
                    }
                    else
                    {
                        this.TakeElementFromNextRightSibling(p, node, 1);
                    }
                }
                else if (p.children[2].NumberOfElements() == 0)
                {
                    //situation can be 0 or 1
                    if (situation == 20 || situation == 30)
                    {
                        this.TakeElementFromTwoNextLeftSibling(p, node, 2);
                    }
                    else
                    {
                        this.TakeElementFromNextLeftSibling(p, node, 1);
                    }
                }
            }
            else if (p.NumberOfElements() == 3)
            {
                //Check for which children is empty
                if (p.children[0].NumberOfElements() == 0)
                { 
                    //situation can be 1 2 or 3
                    switch (situation)
                    {
                        case 21:
                            this.TakeElementFromNextRightSibling(p, node, 0);
                            break;
                        case 31:
                            this.TakeElementFromNextRightSibling(p, node, 0);
                            break;
                        case 22:
                            this.TakeElementFromTwoNextRightSibling(p, node, 0);
                            break;
                        case 32:
                            this.TakeElementFromTwoNextRightSibling(p, node, 0);
                            break;
                        case 23:
                            this.TakeElementFromThreeNextRightSibling(p, node);
                            break;
                        case 33:
                            this.TakeElementFromThreeNextRightSibling(p, node);
                            break;
                    }
                }
                else if (p.children[1].NumberOfElements() == 0)
                {
                    //situation can be 0 2 or 3
                    switch (situation)
                    {
                        case 20:
                            this.TakeElementFromNextLeftSibling(p, node, 0);
                            break;
                        case 30:
                            this.TakeElementFromNextLeftSibling(p, node, 0);
                            break;
                        case 22:
                            this.TakeElementFromNextRightSibling(p, node, 1);
                            break;
                        case 32:
                            this.TakeElementFromNextRightSibling(p, node, 1);
                            break;
                        case 23:
                            this.TakeElementFromTwoNextRightSibling(p, node, 1);
                            break;
                        case 33:
                            this.TakeElementFromTwoNextRightSibling(p, node, 1);
                            break;
                    }
                }
                else if (p.children[2].NumberOfElements() == 0)
                {
                    //situation can be 0 1 or 3
                    switch (situation)
                    {
                        case 20:
                            this.TakeElementFromTwoNextLeftSibling(p, node, 2);
                            break;
                        case 30:
                            this.TakeElementFromTwoNextLeftSibling(p, node, 2);
                            break;
                        case 21:
                            this.TakeElementFromNextLeftSibling(p, node, 1);
                            break;
                        case 31:
                            this.TakeElementFromNextLeftSibling(p, node, 1);
                            break;
                        case 23:
                            this.TakeElementFromNextRightSibling(p, node, 2);
                            break;
                        case 33:
                            this.TakeElementFromNextRightSibling(p, node, 2);
                            break;
                    }
                }
                else
                {
                    //situation can be 0 1 or 2
                    switch (situation)
                    {
                        case 20:
                            this.TakeElementFromThreeNextLeftSibling(p, node);
                            break;
                        case 30:
                            this.TakeElementFromThreeNextLeftSibling(p, node);
                            break;
                        case 21:
                            this.TakeElementFromTwoNextLeftSibling(p, node, 3);
                            break;
                        case 31:
                            this.TakeElementFromTwoNextLeftSibling(p, node, 3);
                            break;
                        case 22:
                            this.TakeElementFromNextLeftSibling(p, node, 2);
                            break;
                        case 32:
                            this.TakeElementFromNextLeftSibling(p, node, 2);
                            break;
                    }
                }
            }
        }

        void TakeElementFromThreeNextRightSibling(TwoFourTreeNode p, TwoFourTreeNode node)
        {
            node.elements[0] = p.elements[0];
            p.elements[0] = p.children[1].elements[0];
            p.children[1].elements[0] = p.elements[1];
            p.elements[1] = p.children[2].elements[0];
            p.children[2].elements[0] = p.elements[2];
            p.elements[2] = p.children[3].elements[0];

            if (p.children[3].NumberOfElements() == 2)
            {
                p.children[3].elements[0] = p.children[3].elements[1];
                p.children[3].elements[1] = 0;
            }

            else if (p.children[3].NumberOfElements() == 3)
            {
                p.children[3].elements[0] = p.children[3].elements[1];
                p.children[3].elements[1] = p.children[3].elements[2];
                p.children[3].elements[2] = 0;
            }

            if (node.IsLeaf() == false)
            {
                node.children[1] = p.children[1].children[0];
                p.children[1].children[0] = p.children[1].children[1];
                p.children[1].children[1] = p.children[2].children[0];
                p.children[2].children[0] = p.children[2].children[1];
                p.children[2].children[1] = p.children[3].children[0];

                if (p.children[3].NumberOfElements() == 1)
                {
                    p.children[3].children[0] = p.children[3].children[1];
                    p.children[3].children[1] = p.children[3].children[2];
                    p.children[3].children[2] = null;
                }
                else if (p.children[3].NumberOfElements() == 2)
                {
                    p.children[3].children[0] = p.children[3].children[1];
                    p.children[3].children[1] = p.children[3].children[2];
                    p.children[3].children[2] = p.children[3].children[3];
                    p.children[3].children[3] = null;
                }

                node.children[1].parent = node;
                p.children[1].children[1].parent = p.children[1];
                p.children[2].children[1].parent = p.children[2];
            }
        }

        void TakeElementFromThreeNextLeftSibling(TwoFourTreeNode p, TwoFourTreeNode node)
        {
            node.elements[0] = p.elements[2];
            p.elements[2] = p.children[2].elements[0];
            p.children[2].elements[0] = p.elements[1];

            p.elements[1] = p.children[1].elements[0];
            p.children[1].elements[0] = p.elements[0];

            if (p.children[0].NumberOfElements() == 2)
            {
                p.elements[0] = p.children[0].elements[1];
                p.children[0].elements[1] = 0;
            }

            else if (p.children[0].NumberOfElements() == 3)
            {
                p.elements[0] = p.children[0].elements[2];
                p.children[0].elements[2] = 0;
            }

            if (node.IsLeaf() == false)
            {

                node.children[1] = node.children[0];
                node.children[0] = p.children[2].children[1];
                p.children[2].children[1] = p.children[2].children[0];
                p.children[2].children[0] = p.children[1].children[1];
                p.children[1].children[1] = p.children[1].children[0];


                if (p.children[0].NumberOfElements() == 1)
                {
                    p.children[1].children[0] = p.children[0].children[2];
                    p.children[0].children[2] = null;
                }

                else if (p.children[0].NumberOfElements() == 2)
                {
                 
                    p.children[1].children[0] = p.children[0].children[3];
                    p.children[0].children[3] = null;
                }

                node.children[0].parent = node;
                p.children[2].children[0].parent = p.children[2];
                p.children[1].children[0].parent = p.children[1];
            }
        }
    }

    //Represents a 2,3 Tree
    public class TwoThreeTree : Tree
    {
        //Constructor that initializes the root of the tree
        public TwoThreeTree()
        {
            this.root = new TwoThreeTreeNode();
            this.limit = 2;
            this.initRoot();
        }

        //Recursively split to balance the tree
        public override void Split(TreeNode twoThreeNode, int element)
        {

            TwoThreeTreeNode p;

            if (twoThreeNode.parent == null)
            {
                p = new TwoThreeTreeNode();
             
            }
            else
            {
                p = (TwoThreeTreeNode) twoThreeNode.parent;
            }

            TwoThreeTreeNode n1 = new TwoThreeTreeNode();
            TwoThreeTreeNode n2 = new TwoThreeTreeNode();

            //finding the smallest, middle and large elements
            int small, middle, large;

            if (element < twoThreeNode.elements[0])
            {
                small = element;
                middle = twoThreeNode.elements[0];
                large = twoThreeNode.elements[1];
            }
            else if (element > twoThreeNode.elements[1])
            {
                small = twoThreeNode.elements[0];
                middle = twoThreeNode.elements[1];
                large = element;
            }
            else
            {
                small = twoThreeNode.elements[0];
                middle = element;
                large = twoThreeNode.elements[1];
            }

            //set smallest and largest keys to the n1 and n2 respectively
            n1.elements[0] = small;
            n2.elements[0] = large;

            //Make p the parent node of n1 and n2
            n1.parent = p;
            n2.parent = p;

            if (p.NumberOfElements() == 0)
            {
                p.children[0] = n1;
                p.children[1] = n2;
                this.root = p;
                
                
                n1.isLeaf = true;
                n2.isLeaf = true;
            }
            else if (p.NumberOfElements() == 1)
            {
                if (n2.elements[0] < p.elements[0])
                {
                    p.children[2] = p.children[1];
                    p.children[0] = n1;
                    p.children[1] = n2;
                }
                else
                {
                    p.children[1] = n1;
                    p.children[2] = n2;
                }
                n1.isLeaf = true;
                n2.isLeaf = true;
                
                
            }
            else
            {
                if (n2.elements[0] < p.elements[0] && n2.elements[0] < p.elements[1])
                {
                    p.children[3] = p.children[2];
                    p.children[2] = p.children[1];
                    p.children[0] = n1;
                    p.children[1] = n2;
                }
                else if (n1.elements[0] > p.elements[0] && n1.elements[0] > p.elements[1])
                {
                    p.children[2] = n1;
                    p.children[3] = n2;
                }

                else
                {
                    p.children[3] = p.children[2];
                    p.children[1] = n1;
                    p.children[2] = n2;
                }
            }


            //if it is not a leaf check
            if (twoThreeNode.IsLeaf() == false)
            {
                twoThreeNode.children[0].parent = n1;
                twoThreeNode.children[1].parent = n1;
                twoThreeNode.children[2].parent = n2;
                twoThreeNode.children[3].parent = n2;
                n1.children[0] = twoThreeNode.children[0];
                n1.children[1] = twoThreeNode.children[1];
                n2.children[0] = twoThreeNode.children[2];
                n2.children[1] = twoThreeNode.children[3];
                n1.isLeaf = false;
                n2.isLeaf = false;
            }

            if (p.NumberOfElements() == 2)
            {
                this.Split(p, middle);
                if (n1.children[0] != null || n2.children[0] != null)
                {
                    if (n1.children[0].IsLeaf() || n2.children[0].IsLeaf())
                    {
                        n1.isLeaf = false;
                        n2.isLeaf = false;
                    }
                }
                else
                {
                    n1.isLeaf = true;
                    n2.isLeaf = true;
                }

                n1.parent.isLeaf = false;
                n2.parent.isLeaf = false;
                
                
            }

            else if (p.NumberOfElements() == 1)
            {
                if (p.elements[0] < middle)
                {
                    p.elements[1] = middle;
                }
                else
                {
                    p.elements[1] = p.elements[0];
                    p.elements[0] = middle;
                }

                
            }

            else
            {
                p.elements[0] = middle;
                
            }


        }

        public void delete(int element)
        {
            TwoThreeTreeNode shouldFixed = (TwoThreeTreeNode) this.DeleteElement(element);
            if (shouldFixed != null)
                Fix(shouldFixed);
        }

        
        void Redistrubute(TwoThreeTreeNode node, TwoThreeTreeNode p, int situation)
        {

            //there is two different situation if the parent has two nodes
            if (p.NumberOfElements() == 1)
            {
                if (situation == 20)
                {
                    this.TakeElementFromNextLeftSibling(p, node, 0);
                }
                else
                {
                    this.TakeElementFromNextRightSibling(p, node, 0);
                }
            }

             //there is six different situation if the parent has three nodes
            else if (p.NumberOfElements() == 2)
            {
                //Check for which children is empty
                if (p.children[0].NumberOfElements() == 0)
                {
                    //situation can be 1 or 2
                    if (situation == 21)
                    {
                        this.TakeElementFromNextRightSibling(p, node,0);
                    }
                    else
                    {
                        this.TakeElementFromTwoNextRightSibling(p, node, 0);
                    }
                }
                else if (p.children[1].NumberOfElements() == 0)
                {
                    //situation can be 0 or 2
                    if (situation == 20)
                    {
                        this.TakeElementFromNextLeftSibling(p, node, 0);
                    }
                    else
                    {
                        this.TakeElementFromNextRightSibling(p, node, 1);
                    }
                }
                else if (p.children[2].NumberOfElements() == 0)
                {
                    //situation can be 0 or 1
                    if (situation == 20)
                    {
                        this.TakeElementFromTwoNextLeftSibling(p, node, 2);
                    }
                    else
                    {
                        this.TakeElementFromNextLeftSibling(p, node, 1);
                    }
                }
            }
        }
        //completes the deletion when node n is empty by either, removing the root, redistributing values, or merging nodes.
        //Note : if n is internal, it has only one child
        public void Fix(TwoThreeTreeNode node)
        {
            TwoThreeTreeNode p;

            if (node.parent == null) //No parent means it is root node
            {
                if (node.children[0] == null) { return; }
                //remove root and set the children as a root
                this.root = node.children[0];
                this.root.parent = null;
                //this.root.children[2] = node.children[2]; 
            } 

            else
            {
                p = (TwoThreeTreeNode) node.parent;

                //situation represents both the possibility of merge and the sibling with two elements
                //if it returns -1, means there is no siblings with two elements
                //if it is bigger than -1, means that index has the two elements
                int situation = this.HasChildWithTwoElement(p);

                //check for redistrubution is possible or not
                if (situation > -1)
                {
                    //redistrubute
                    this.Redistrubute(node, p, situation);
                }

                //redistrubuting not possible, thus merge
                //for merging we have 5 different situation; 2 for parent with one element, 3 for parent with two element
                else
                {
                    this.Merge(node);
                }

                if (node.parent.NumberOfElements() == 0)
                {
                     this.Fix( (TwoThreeTreeNode) node.parent);
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TreeManager treeManager = new TreeManager("d:\\data.txt");
            treeManager.run();

            
        }
    }
}