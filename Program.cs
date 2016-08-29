// Composed by Pratik Gupta
// Dated: 24 July 2016
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
 
namespace MarkupTagChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            //Process each arguments as input
            foreach (string input in args)
            {
                //Use LIFO stack collection with string to track the opening and closing tags
                Stack<string> openTagsStk = new Stack<string>();
                int i = 0;              
                // Using Regular Expression matching input with opening and closing tag tokens, 
                // following tag rules explicitly stated in the question
                //The matched result will be stored in a MatchCollection object
                MatchCollection allTags = Regex.Matches(input, "<[A-Z\\/]+>");
                bool isUnmatched = false;
                bool finished = false;
                string unmatchedCloseTag = "";
                string popedStkOpenTag;
                //Iterate the MatchCollection object
                foreach (Match m in allTags)
                {
                    //Strip out chars '<' & '>' in tags for easy comparison
                    string tag = m.Value.Replace("<", "").Replace(">", "");
                    //Push open tags in stack
                    //Static method from static class to check for opening tag, using methods to avoid repeating functionality (DRY principle).
                    if (MarkupUtil.IsOpeningTag(tag))
                        openTagsStk.Push(tag);
                    else
                    {
                        if (openTagsStk.Count != 0)
                        {
                            popedStkOpenTag = openTagsStk.Pop();
                            //Static method from static class to check for matching tag, using methods to avoid repeating functionality (DRY principle).
                            if (!MarkupUtil.AreMatchingTags(popedStkOpenTag, tag))
                            {
                                //Static method from static class to print the tag in correct format, using methods to avoid repeating functionality (DRY principle).
                                Console.WriteLine("Expected " + MarkupUtil.PrintCloseTag(popedStkOpenTag) + " found " + MarkupUtil.PrintCloseTag(tag));
                                finished = true;
                                break;
                            }
                        }
                        else
                        {
                            isUnmatched = true;
                            unmatchedCloseTag = tag;
                        }
                    }

                }
                //Analyse stack and after end of paragraph
                if (openTagsStk.Count == 0)
                {
                    // If even number of tags and no unmatched tags then correctly tagged paragraph
                    if (allTags.Count % 2 == 0 && !isUnmatched)
                        Console.WriteLine("Correctly tagged paragraph");
                    //If unmatched tag found then print as per rule
                    else
                        Console.WriteLine("Expected # found " + MarkupUtil.PrintCloseTag(unmatchedCloseTag));
                } else if (allTags.Count % 2 != 0 && !finished)
                {
                    Console.WriteLine("Expected "+ MarkupUtil.PrintCloseTag(openTagsStk.Peek()) + " found #");
                }
            }
          
            Console.ReadLine();
        }
       
        //Defining static class and functions for common functionality to use without repeating functinality across the code (DRY principle)
        static class MarkupUtil
        {
            public static bool AreMatchingTags(string openingTag, string closingTag)
            {
                return openingTag.Equals(closingTag.Substring(1));
            }
            public static bool IsOpeningTag(string tag)
            {
                return !(tag.Contains("/"));
            }
            public static string PrintCloseTag(string tag)
            {
                if (!tag.Contains("/"))
                    return "</" + tag + ">";
                else
                    return "<" + tag + ">";
            }
        }
    }
}
