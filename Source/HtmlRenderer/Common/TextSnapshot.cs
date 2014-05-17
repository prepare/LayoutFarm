﻿//2014 ,BSD, WinterCore

using System;
using System.Text;

namespace HtmlRenderer 
{
    public class TextSnapshot
    {
        readonly char[] textBuffer;
        readonly int length;
        public TextSnapshot(char[] textBuffer)
        {
            this.textBuffer = textBuffer;
            this.length = textBuffer.Length;
        }
        public TextSnapshot(StringBuilder sb)
        {
            int len = this.length = sb.Length;
            textBuffer = new char[len];
            sb.CopyTo(0, textBuffer, 0, len);
        }
        public TextSnapshot(string str)
        {
            this.textBuffer = str.ToCharArray();
            this.length = textBuffer.Length;
        }
        public int Length
        {
            get
            {
                return this.length;
            }
        }
        public char this[int index]
        {
            get
            {
                return this.textBuffer[index];
            }
        }

        //--------------
        public static char[] UnsafeGetInternalBuffer(TextSnapshot snap)
        {
            return snap.textBuffer;
        }
        public char[] Copy(int index, int length)
        {
            char[] newbuff = new char[length];
            Array.Copy(this.textBuffer, index, newbuff, 0, length);
            return newbuff;
        }
        public string Substring(int index, int length)
        {
            return new string(textBuffer, index, length);
        }
        public int IndexOf(char c)
        {
            return IndexOf(c, 0);
        }


        public int IndexOf(char c, int start)
        {
            char[] tmpChars = this.textBuffer;
            int lim = tmpChars.Length;
            unsafe
            {
                fixed (char* start0 = &this.textBuffer[0])
                {
                    char* curChar = start0 + start;
                    for (int i = start; i < lim; ++i)
                    {
                        if (*curChar == c)
                        {
                            return i;
                        }
                        curChar++;
                    }
                }
            }
            return -1;
        }

        internal int IndexOf(char c1, char c2, char c3, int start)
        {
            char[] tmpChars = this.textBuffer;
            int lim = length - 3;
            if (start < lim)
            {
                //init 
                int i = start;
                char ex1 = tmpChars[i];
                char ex2 = tmpChars[i + 1];
                char ex3 = tmpChars[i + 2];

                do
                {
                    if (ex1 == c1 && ex2 == c2 && ex3 == c3)
                    {
                        return i;
                    }
                    i++;

                    ex1 = ex2;
                    ex2 = ex3;
                    ex3 = tmpChars[i];

                } while (i < lim);
            }
            //not found
            return -1;
        }
    }

}