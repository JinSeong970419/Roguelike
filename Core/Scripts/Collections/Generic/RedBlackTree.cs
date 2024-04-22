using System;
using TMPro;
using UnityEngine;

namespace Roguelike.Core
{
    [System.Serializable]
    public class RedBlackTree<T> where T : IComparable
    {
        public enum Color
        {
            Red,
            Black,
        }

        [System.Serializable]
        public class Node
        {
            public Color Color;
            public Node Left;
            public Node Right;
            public Node Parent;
            public T Data;

            public Node Grandparent
            {
                get
                {
                    if (Parent == null) return null;
                    return Parent.Parent;
                }
            }

            public Node Uncle
            {
                get
                {
                    Node grandparent = Grandparent;
                    if (grandparent == null) return null;

                    if (Parent == grandparent.Left)
                        return grandparent.Right;
                    else
                        return grandparent.Left;
                }
            }

            public Node Sibling
            {
                get
                {
                    if (Parent == null) return null;

                    if (this == Parent.Left)
                    {
                        return Parent.Right;
                    }
                    else
                    {
                        return Parent.Left;
                    }
                }
            }

            public Node() { }


        }


        [SerializeField] private Node root;
        [SerializeField] private Node nil;

        public RedBlackTree()
        {
            nil = new Node();
            nil.Color = Color.Black;

        }

        public void Insert(T data)
        {
            Insert(root, data);
        }

        public void Remove(T data)
        {
            Remove(root, data);
        }

        public void Preorder()
        {
            Preorder(root);
        }

        public void Inorder()
        {
            Inorder(root);
        }

        public void Postorder()
        {
            Postorder(root);
        }

        public void MakeTree(GameObject target, Sprite sprite)
        {
            MakeTree(target,sprite, root);
        }

        private void MakeTree(GameObject target,Sprite sprite, Node node)
        {
            if (node == nil)
            {
                return;
            }

            GameObject nodeObject = new GameObject();
            nodeObject.name = node.Data.ToString();
            SpriteRenderer renderer = nodeObject.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            if(node.Color == Color.Black)
            {
                renderer.color = new UnityEngine.Color(0, 0, 0, 1);
            }
            else
            {
                renderer.color = new UnityEngine.Color(1, 0, 0, 1);
            }
            nodeObject.transform.SetParent(target.transform);
            if(node == root)
            {
                nodeObject.transform.localPosition = Vector3.zero;
            }
            else
            {
                if (node == node.Parent.Left)
                {
                    nodeObject.transform.localPosition = new Vector3(-10f, -3f, 0f);
                }
                else
                {
                    nodeObject.transform.localPosition = new Vector3(10f, -3f, 0f);
                }
            }

            nodeObject.AddComponent<UnityEngine.UI.Text>().text = node.Data.ToString();

            MakeTree(nodeObject,sprite, node.Left);
            MakeTree(nodeObject,sprite, node.Right);
        }

        private bool IsNil(Node node)
        {
            return node == nil;
        }

        private Node Grandparent(Node node)
        {
            if(!IsNil(node) && !IsNil(node.Parent))
            {
                return node.Parent.Parent;
            }

            return nil;
        }

        private Node Uncle(Node node)
        {
            Node grandparent = Grandparent(node);
            if(IsNil(grandparent))
            {
                return nil;
            }

            if(node.Parent == grandparent.Left)
            {
                return grandparent.Right;
            }

            return grandparent.Left;
        }

        private void Insert(Node node, T data)
        {
            if (root == null)
            {
                root = new Node();
                root.Data = data;
                root.Right = nil;
                root.Left = nil;
                root.Color = Color.Black;
                root.Parent = nil;
                InsertCase1(root);
                return;
            }

            int compare = data.CompareTo(node.Data);
            if (compare < 0)
            {
                if (!IsNil(node.Left))
                {
                    Insert(node.Left, data);
                }
                else
                {
                    Node newNode = new Node();
                    newNode.Data = data;
                    newNode.Right = nil;
                    newNode.Left = nil;
                    node.Left = newNode;
                    newNode.Parent = node;
                    newNode.Color = Color.Red;
                    InsertCase1(newNode);
                }
            }
            else if (compare > 0)
            {
                if (!IsNil(node.Right))
                {
                    Insert(node.Right, data);
                }
                else
                {
                    Node newNode = new Node();
                    newNode.Data = data;
                    newNode.Right = nil;
                    newNode.Left = nil;
                    node.Right = newNode;
                    newNode.Parent = node;
                    newNode.Color = Color.Red;
                    InsertCase1(newNode);
                }
            }
            else
            {
                Debug.LogWarning("Duplicated value.");
            }

        }

        private void Remove(Node node, T data)
        {
            if (root == null)
            {
                return;
            }

            int compare = data.CompareTo(node.Data);
            if (compare < 0)
            {
                if (!IsNil(node.Left))
                {
                    Remove(node.Left, data);
                }
            }
            else if (compare > 0)
            {
                if (!IsNil(node.Right))
                {
                    Remove(node.Right, data);
                }
            }
            else
            {
                if (node == root)
                {
                    root = null;
                    return;
                }

                if (node == node.Parent.Left)
                {
                    node.Parent.Left = nil;
                }
                else
                {
                    node.Parent.Right = nil;
                }
            }
        }

        private void Preorder(Node node)
        {
            if (node == nil)
            {
                return;
            }

            Debug.Log(node.Data.ToString());
            Preorder(node.Left);
            Preorder(node.Right);
        }

        private void Inorder(Node node)
        {
            if (node == nil)
            {
                return;
            }

            Inorder(node.Left);
            Debug.Log(node.Data.ToString());
            Inorder(node.Right);
        }

        private void Postorder(Node node)
        {
            if (node == nil)
            {
                return;
            }

            Postorder(node.Left);
            Postorder(node.Right);
            Debug.Log(node.Data.ToString());
        }


        private void RotateLeft(Node node)
        {
            Node child = node.Right;
            Node parent = node.Parent;

            if(!IsNil(child.Left))
            {
                child.Left.Parent = node;
            }

            node.Right = child.Left;
            node.Parent = child;
            child.Left = node;
            child.Parent = parent;

            if(!IsNil(parent))
            {
                if(parent.Left == node)
                {
                    parent.Left = child;
                }
                else
                {
                    parent.Right = child;
                }
            }
        }

        private void RotateRight(Node node)
        {
            Node child = node.Left;
            Node parent = node.Parent;

            if (!IsNil(child.Right))
            {
                child.Right.Parent = node;
            }

            node.Left = child.Right;
            node.Parent = child;
            child.Right = node;
            child.Parent = parent;

            if (!IsNil(parent))
            {
                if (parent.Right == node)
                {
                    parent.Right = child;
                }
                else
                {
                    parent.Left = child;
                }
            }
        }

        private void InsertCase1(Node node)
        {
            if(node == root)
            {
                node.Color = Color.Black;
            }
            else
            {
                InsertCase2(node);
            }
        }

        private void InsertCase2(Node node)
        {
            if(node.Parent.Color == Color.Black)
            {
                return;
            }

            InsertCase3(node);
        }

        private void InsertCase3(Node node)
        {
            Node uncle = Uncle(node);
            if(!IsNil(uncle) && uncle.Color == Color.Red)
            {
                node.Parent.Color = Color.Black;
                uncle.Color = Color.Black;
                Node grandparent = Grandparent(node);
                grandparent.Color = Color.Red;
                InsertCase1(node);
            }
            else
            {
                InsertCase4(node);
            }
        }

        private void InsertCase4(Node node)
        {
            Node grandparent = Grandparent(node);
            if(node == node.Parent.Right && node.Parent == grandparent.Left)
            {
                RotateLeft(node.Parent);
                node = node.Left;
            }
            else if(node == node.Parent.Left && node.Parent == grandparent.Right)
            {
                RotateRight(node.Parent);
                node = node.Right;
            }
            InsertCase5(node);
        }

        private void InsertCase5(Node node)
        {
            Node grandparent = Grandparent(node);

            node.Parent.Color = Color.Black;
            grandparent.Color = Color.Red;
            if(node == node.Parent.Left)
            {
                RotateRight(grandparent);
            }
            else
            {
                RotateLeft(grandparent);
            }
        }
    }
}
