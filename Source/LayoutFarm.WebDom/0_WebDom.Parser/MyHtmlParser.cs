﻿//BSD, 2014-present, WinterDev 

using System;
using LayoutFarm.WebLexer;
using HtmlLexerEvent = LayoutFarm.WebDom.Parser.XmlLexerEvent;

namespace LayoutFarm.WebDom.Parser
{
    class MyHtmlParser : HtmlParser
    {
        WebDocument _resultHtmlDoc;
        HtmlStack openEltStack = new HtmlStack();
        DomElement curHtmlNode = null;
        DomAttribute curAttr = null;
        DomTextNode curTextNode = null;
        int parseState = 0;
        TextSnapshot textSnapshot;
        HtmlLexer lexer;
        string waitingAttrName;
        DomDocumentNode domDocNode;
        public MyHtmlParser()
        {
            lexer = HtmlLexer.CreateLexer();
            lexer.LexStateChanged += LexStateChanged;
        }


        void LexStateChanged(HtmlLexerEvent lexEvent, int startIndex, int len)
        {
            switch (lexEvent)
            {
                case HtmlLexerEvent.CommentContent:
                    {
                        //var commentContent = this.textSnapshot.Copy(startIndex, len); 

                    }
                    break;
                case HtmlLexerEvent.FromContentPart:
                    {
                        if (curTextNode == null)
                        {
                            curTextNode = _resultHtmlDoc.CreateTextNode(
                                HtmlDecodeHelper.DecodeHtml(this.textSnapshot, startIndex, len));
                            if (curHtmlNode != null)
                            {
                                curHtmlNode.AddChild(curTextNode);
                            }
                        }
                        else
                        {
                            curTextNode.AppendTextContent(HtmlDecodeHelper.DecodeHtml(this.textSnapshot, startIndex, len));
                        }
                    }
                    break;
                case HtmlLexerEvent.AttributeValueAsLiteralString:
                    {
                        //assign value and add to parent
                        if (parseState == 11)
                        {
                            //document node
                            //doc 
                            domDocNode.AddParameter(textSnapshot.Substring(startIndex, len));
                        }
                        else
                        {
                            curAttr.Value = textSnapshot.Substring(startIndex, len);
                            curHtmlNode.AddAttribute(curAttr);
                        }
                    }
                    break;
                case HtmlLexerEvent.Attribute:
                    {
                        //create attribute node and wait for its value
                        string nodename = textSnapshot.Substring(startIndex, len);
                        curAttr = _resultHtmlDoc.CreateAttribute(null, nodename);
                    }
                    break;
                case HtmlLexerEvent.NodeNameOrAttribute:
                    {
                        //the lexer dose not store state of element name or attribute name
                        //so we use parseState to decide here


                        string name = textSnapshot.Substring(startIndex, len);
                        switch (parseState)
                        {
                            case 0:
                                {
                                    //create element 
                                    DomElement elem = _resultHtmlDoc.CreateElement(null, name);
                                    if (curHtmlNode != null)
                                    {
                                        curHtmlNode.AddChild(elem);
                                        openEltStack.Push(curHtmlNode);
                                    }
                                    curHtmlNode = elem;
                                    parseState = 1;//attribute
                                    curTextNode = null;
                                    curAttr = null;
                                    waitingAttrName = null;
                                }
                                break;
                            case 1:
                                {
                                    //wait for attr value 
                                    if (waitingAttrName != null)
                                    {
                                        //push waiting attr
                                        curAttr = _resultHtmlDoc.CreateAttribute(null, waitingAttrName);
                                        curAttr.Value = "";
                                        curHtmlNode.AddAttribute(curAttr);
                                        curAttr = null;
                                    }
                                    waitingAttrName = name;
                                }
                                break;
                            case 2:
                                {
                                    //****
                                    //node name after open slash
                                    //TODO: review here,avoid direct string comparison
                                    if (curHtmlNode.LocalName == name)
                                    {
                                        if (openEltStack.Count > 0)
                                        {
                                            waitingAttrName = null;
                                            curTextNode = null;
                                            curAttr = null;
                                            curHtmlNode = openEltStack.Pop();
                                        }
                                        parseState = 3;
                                    }
                                    else
                                    {
                                        //if not equal then check if current node need close tag or not
                                        if (HtmlTagMatching.IsSingleTag(curHtmlNode.LocalNameIndex))
                                        {
                                            if (openEltStack.Count > 0)
                                            {
                                                waitingAttrName = null;
                                                curHtmlNode = openEltStack.Pop();
                                                curAttr = null;
                                                curTextNode = null;
                                            }
                                            if (curHtmlNode.LocalName == name)
                                            {
                                                if (openEltStack.Count > 0)
                                                {
                                                    curTextNode = null;
                                                    curAttr = null;
                                                    curHtmlNode = openEltStack.Pop();
                                                    waitingAttrName = null;
                                                }
                                                parseState = 3;
                                            }
                                            else
                                            {
                                                //implement err handling here!
                                                throw new NotSupportedException();
                                            }
                                        }
                                        else
                                        {
                                            //implement err handling here!
                                            throw new NotSupportedException();
                                        }
                                    }
                                }
                                break;
                            case 4:
                                {
                                    //attribute value as id
                                    if (curAttr != null)
                                    {
                                        curAttr.Value = name;
                                        curAttr = null;
                                        parseState = 0;
                                        waitingAttrName = null;
                                    }
                                    else
                                    {
                                    }
                                }
                                break;
                            case 10:
                                {
                                    //document node 

                                    parseState = 11;
                                    //after docnodename , this may be attr of the document node
                                    this.domDocNode = (DomDocumentNode)_resultHtmlDoc.CreateDocumentNodeElement();
                                    domDocNode.DocNodeName = name;
                                }
                                break;
                            case 11:
                                {
                                    //doc 
                                    domDocNode.AddParameter(name);
                                }
                                break;
                            default:
                                {
                                }
                                break;
                        }
                    }
                    break;
                case HtmlLexerEvent.VisitCloseAngle:
                    {
                        //close angle of current new node
                        //enter into its content

                        if (parseState == 11)
                        {
                            //add doctype to html
                            _resultHtmlDoc.RootNode.AddChild(this.domDocNode);
                            domDocNode = null;
                        }

                        if (waitingAttrName != null)
                        {
                            curAttr = _resultHtmlDoc.CreateAttribute(null, waitingAttrName);
                            curAttr.Value = "";
                            curHtmlNode.AddAttribute(curAttr);
                            curAttr = null;
                        }


                        waitingAttrName = null;
                        parseState = 0;
                        curTextNode = null;
                        curAttr = null;
                    }
                    break;
                case HtmlLexerEvent.VisitAttrAssign:
                    {
                        parseState = 4;
                    }
                    break;
                case HtmlLexerEvent.VisitOpenSlashAngle:
                    {
                        parseState = 2;
                    }
                    break;
                case HtmlLexerEvent.VisitCloseSlashAngle:
                    {
                        if (openEltStack.Count > 0)
                        {
                            curTextNode = null;
                            curAttr = null;
                            waitingAttrName = null;
                            curHtmlNode = openEltStack.Pop();
                        }
                        parseState = 0;
                    }
                    break;
                case HtmlLexerEvent.VisitOpenAngleExclimation:
                    {
                        //eg. doctype
                        parseState = 10;
                    }
                    break;
                default:
                    {
                        //1. visit open angle
                    }
                    break;
            }
        }

        public override void ResetParser()
        {
            _resultHtmlDoc = null;
            this.openEltStack.Clear();
            this.curHtmlNode = null;
            this.curAttr = null;
            this.curTextNode = null;
            this.parseState = 0;
            this.textSnapshot = null;
        }
        public override void Parse(TextSource textSnapshot, WebDocument htmldoc, DomElement currentNode)
        {
            this.Parse(textSnapshot.ActualSnapshot, htmldoc, currentNode);
        }
        /// <summary>
        /// parse to htmldom
        /// </summary>
        /// <param name="stbuilder"></param>
        internal void Parse(TextSnapshot textSnapshot, WebDocument htmldoc, DomElement currentNode)
        {
            this.textSnapshot = textSnapshot;
            //1. lex 
            lexer.BeginLex();
            //2. mini parser    
            this.curHtmlNode = currentNode;
            _resultHtmlDoc = htmldoc;
            lexer.Analyze(textSnapshot);
            lexer.EndLex();
        }
    }
}