using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemExtensions.Collections
{
    /// <summary>
    /// AVL tree implementation.
    /// Thanks to Karim Oumghar for the implementation example.
    /// Read on https://simpledevcode.wordpress.com/2014/09/16/avl-tree-in-c/
    /// </summary>
    /// <typeparam name="T">Provided type.</typeparam>
    [Serializable]
    public class AVLTree<T> : ICollection<T> where T : IComparable<T>
    {
        #region Fields
        [Serializable]
        private class AVLNode<TKey>
        {
            public TKey Value;
            public AVLNode<TKey> Left;
            public AVLNode<TKey> Right;
            public AVLNode(TKey value)
            {
                this.Value = value;
            }
        }
        AVLNode<T> root;
        private int count = 0;
        private bool isReadOnly = false;
        #endregion
        #region Properties
        /// <summary>
        /// Count of items currently stored in the tree.
        /// </summary>
        public int Count {
            get
            {
                return count;
            }
        }
        /// <summary>
        /// True if the collection is readonly. False otherwise.
        /// </summary>
        public bool IsReadOnly => isReadOnly;
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of an AVLTree collection.
        /// </summary>
        /// <param name="comparator"></param>
        public AVLTree()
        {

        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Adds the value to the tree.
        /// </summary>
        /// <param name="value">Value to be added to the tree.</param>
        public void Add(T value)
        {
            count++;
            AVLNode<T> newItem = new AVLNode<T>(value);
            if (root == null)
            {
                root = newItem;
            }
            else
            {
                root = RecursiveInsertion(root, newItem);
            }
        }
        /// <summary>
        /// Checks if the key is contained into the tree.
        /// </summary>
        /// <param name="value">Value to be checked if present in the tree.</param>
        /// <returns>True if the value is in the tree.</returns>
        public bool Contains(T value)
        {
            AVLNode<T> node = Find(value, root);
            if(node == null)
            {
                return false;
            }
            if (node.Value.CompareTo(value) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Removes the specified value from the tree.
        /// </summary>
        /// <param name="value">Value to be deleted.</param>
        public bool Remove(T value)
        {
            root = Delete(root, value);
            return true;
        }
        /// <summary>
        /// Clears the tree.
        /// </summary>
        public void Clear()
        {
            Queue<AVLNode<T>> queue = new Queue<AVLNode<T>>();
            queue.Enqueue(root);

            while(queue.Count > 0)
            {
                AVLNode<T> currentNode = queue.Dequeue();
                if(currentNode.Left != null)
                {
                    queue.Enqueue(currentNode.Left);
                    currentNode.Left = null;
                    count--;
                }
                if(currentNode.Right != null)
                {
                    queue.Enqueue(currentNode.Right);
                    currentNode.Right = null;
                    count--;
                }
            }
            root = null;
            count--;
        }
        /// <summary>
        /// Copies the tree onto the provided array.
        /// </summary>
        /// <param name="array">Array to store the values in the tree.</param>
        /// <param name="arrayIndex">Starting index of the provided array.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Queue<AVLNode<T>> queue = new Queue<AVLNode<T>>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                AVLNode<T> currentNode = queue.Dequeue();
                array[arrayIndex++] = currentNode.Value;
                if (currentNode.Left != null)
                {
                    queue.Enqueue(currentNode.Left);
                }
                if (currentNode.Right != null)
                {
                    queue.Enqueue(currentNode.Right);
                }
            }
        }
        /// <summary>
        /// Enumerator that iterates over the tree.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator(root);
        }
        /// <summary>
        /// Copies the tree structure into an array.
        /// </summary>
        /// <returns>Array containing the values contained in the tree.</returns>
        public T[] ToArray()
        {
            T[] array = new T[count];
            CopyTo(array, 0);
            return array;
        }
        #endregion
        #region Private Methods
        private AVLNode<T> RecursiveInsertion(AVLNode<T> current, AVLNode<T> n)
        {
            if (current == null)
            {
                current = n;
                return current;
            }
            else if (n.Value.CompareTo(current.Value) < 0)
            {
                current.Left = RecursiveInsertion(current.Left, n);
                current = BalanceTree(current);
            }
            else if (n.Value.CompareTo(current.Value) > 0)
            {
                current.Right = RecursiveInsertion(current.Right, n);
                current = BalanceTree(current);
            }
            return current;
        }
        private AVLNode<T> BalanceTree(AVLNode<T> current)
        {
            int b_factor = BalanceFactor(current);
            if (b_factor > 1)
            {
                if (BalanceFactor(current.Left) > 0)
                {
                    current = RotateLL(current);
                }
                else
                {
                    current = RotateLR(current);
                }
            }
            else if (b_factor < -1)
            {
                if (BalanceFactor(current.Right) > 0)
                {
                    current = RotateRL(current);
                }
                else
                {
                    current = RotateRR(current);
                }
            }
            return current;
        }
        private AVLNode<T> Delete(AVLNode<T> current, T target)
        {
            AVLNode<T> parent;
            if (current == null)
            { return null; }
            else
            {
                //left subtree
                if (target.CompareTo(current.Value) < 0)
                {
                    current.Left = Delete(current.Left, target);
                    if (BalanceFactor(current) == -2)//here
                    {
                        if (BalanceFactor(current.Right) <= 0)
                        {
                            current = RotateRR(current);
                        }
                        else
                        {
                            current = RotateRL(current);
                        }
                    }
                }
                //right subtree
                else if (target.CompareTo(current.Value) > 0)
                {
                    current.Right = Delete(current.Right, target);
                    if (BalanceFactor(current) == 2)
                    {
                        if (BalanceFactor(current.Left) >= 0)
                        {
                            current = RotateLL(current);
                        }
                        else
                        {
                            current = RotateLR(current);
                        }
                    }
                }
                //if target is found
                else
                {
                    count--;
                    if (current.Right != null)
                    {
                        //delete its inorder successor
                        parent = current.Right;
                        while (parent.Left != null)
                        {
                            parent = parent.Left;
                        }
                        current.Value = parent.Value;
                        current.Right = Delete(current.Right, parent.Value);
                        if (BalanceFactor(current) == 2)//rebalancing
                        {
                            if (BalanceFactor(current.Left) >= 0)
                            {
                                current = RotateLL(current);
                            }
                            else { current = RotateLR(current); }
                        }
                    }
                    else
                    {   //if current.left != null
                        return current.Left;
                    }
                }
            }
            return current;
        }
        private AVLNode<T> Find(T target, AVLNode<T> current)
        {
            if(current == null)
            {
                return null;
            }
            if (target.CompareTo(current.Value) < 0)
            {
                if (target.CompareTo(current.Value) == 0)
                {
                    return current;
                }
                else
                    return Find(target, current.Left);
            }
            else
            {
                if (target.CompareTo(current.Value) == 0)
                {
                    return current;
                }
                else
                    return Find(target, current.Right);
            }

        }
        private int Max(int l, int r)
        {
            return l > r ? l : r;
        }
        private int GetHeight(AVLNode<T> current)
        {
            int height = 0;
            if (current != null)
            {
                int l = GetHeight(current.Left);
                int r = GetHeight(current.Right);
                int m = Max(l, r);
                height = m + 1;
            }
            return height;
        }
        private int BalanceFactor(AVLNode<T> current)
        {
            int l = GetHeight(current.Left);
            int r = GetHeight(current.Right);
            int b_factor = l - r;
            return b_factor;
        }
        private AVLNode<T> RotateRR(AVLNode<T> parent)
        {
            AVLNode<T> pivot = parent.Right;
            parent.Right = pivot.Left;
            pivot.Left = parent;
            return pivot;
        }
        private AVLNode<T> RotateLL(AVLNode<T> parent)
        {
            AVLNode<T> pivot = parent.Left;
            parent.Left = pivot.Right;
            pivot.Right = parent;
            return pivot;
        }
        private AVLNode<T> RotateLR(AVLNode<T> parent)
        {
            AVLNode<T> pivot = parent.Left;
            parent.Left = RotateRR(pivot);
            return RotateLL(parent);
        }
        private AVLNode<T> RotateRL(AVLNode<T> parent)
        {
            AVLNode<T> pivot = parent.Right;
            parent.Right = RotateLL(pivot);
            return RotateRR(parent);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        private IEnumerator<T> GetEnumerator(AVLNode<T> rootNode)
        {
            Queue<AVLNode<T>> queue = new Queue<AVLNode<T>>();
            queue.Enqueue(rootNode);

            while (queue.Count > 0)
            {
                AVLNode<T> currentNode = queue.Dequeue();
                yield return currentNode.Value;
                if (currentNode.Left != null)
                {
                    queue.Enqueue(currentNode.Left);
                }
                if (currentNode.Right != null)
                {
                    queue.Enqueue(currentNode.Right);
                }
            }
        }
        #endregion


    }
}
