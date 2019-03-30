/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  屏蔽字处理
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;

namespace Coolape
{
	public class BlockWordsTrie
	{
		private static BlockWordsTrie _self = null;

		private BlockWordsTrie()
		{
		}

		public static BlockWordsTrie getInstanse()
		{
			if (_self == null) {
				_self = new BlockWordsTrie();
			}
			return _self;
		}

		private const int matchLength = 1;
		private Hashtable node = new Hashtable();

		public void init(string shieldWords)
		{
			if (string.IsNullOrEmpty(shieldWords)) {
				return;
			}
			string[] conts = shieldWords.Split("\n\t".ToCharArray());
			foreach (string item in conts) {
				Add(item);
			}
		}

		public void Add(string content)
		{
			if (string.IsNullOrEmpty(content)) {
				//Debug.LogError("==== Trie content null ====");
				return;
			}
			content = content.ToLower();
			char[] contentChars = content.ToCharArray();
			Hashtable parent = node;
			Hashtable currentNode = node;
			int len = contentChars.Length;
			for (int i = 0; i < len; i++) {
				char current = contentChars [i];
				if (parent [current] == null) {
					currentNode = new Hashtable();
					if (i == len - 1) {
						currentNode ["isEnd"] = "1";
					} else {
						currentNode ["isEnd"] = "0";
					}
					parent [current] = currentNode;
					parent = currentNode;
				} else {
					parent = (Hashtable)parent [current];
					if (parent != null) {
						if (i == len - 1) {
							parent ["isEnd"] = "1";
						}
					}
				}
			}
		}

		public bool isExistChar(char first)
		{
			return node [first] != null;
		}

		public bool isExistCharArrays(char[] text, int begin, int length)
		{
			bool result = true;
			Hashtable parent = node;
			int size = begin + length;
			for (int i = begin; i < size; i++) {
				char current = text [i];
				if (parent [current] != null) {
					parent = (Hashtable)parent [current];
				} else {
					return false;
				}
			}
		
			if (parent.Keys.Count > 0) {
				object o_end = parent ["isEnd"];
				if (o_end != null) {
					string isEnd = o_end.ToString();
					if (isEnd == "1") {
						return true;
					}
				}
				return false;
			}
			return result;
		}
		/*
    public bool isExistStr(string content){
        char[] contentChars= content.ToCharArray();
        bool result =true;
        Hashtable parent=node;
        foreach(char current in contentChars){
            if(parent[current]!=null){
                parent= (Hashtable) parent[current];
            }else{
                return false;
            }
        }
        if(parent.Keys.Count>0){
            return false;
        }
        return result;
    }
	*/
	
		/**
     * 过滤关键字
     * @param text
     * @return
     */
		public string filter(string text)
		{
			if (string.IsNullOrEmpty(text)) {
				return text;
			}
			text = text.ToLower();
			char[] chars = text.ToCharArray();
			filterCharArrays(chars, 0, matchLength, null);
			return new string(chars);
		}

		private void filterCharArrays(char[] chars, int begin_, int length, Hashtable replaceMap)
		{
			int all_len = chars.Length;
			if (replaceMap == null) {
				replaceMap = new Hashtable();
			}
			bool is_containKey = replaceMap.ContainsKey(begin_);
			int sum = begin_ + length;
			if (sum > all_len) {
				int end_len = 1;
				if (is_containKey) {
					end_len = (int)replaceMap [begin_];
					for (int k = begin_; k < end_len; k++) {
						chars [k] = '*';
					}
				}
				if (end_len == 1) {
					end_len = begin_ + end_len;
				}
				begin_ = end_len;
				length = matchLength;
			}
			sum = begin_ + length;
		
			if (sum > all_len)
				return;
		
			char c = chars [begin_];
			bool is_exit_c = isExistChar(c);
			if (is_exit_c) {
				bool is_exist = isExistCharArrays(chars, begin_, length);
				if (is_exist) {
					int size = begin_ + length;
					is_containKey = replaceMap.ContainsKey(begin_);
					if (is_containKey) {
						replaceMap [begin_] = size;
					} else {
						replaceMap.Add(begin_, size);
					}
				}
			}
			filterCharArrays(chars, begin_, length + 1, replaceMap);
		}

		/**
     * 文本是否包括非法关键字
     * @param text
     * @return
     */
		public bool isUnlawful(string text)
		{
			if (string.IsNullOrEmpty(text)) {
				return false;
			}
			text = text.ToLower();
			char[] chars = text.ToCharArray();
			return isUnlawfulCharArrays(chars, 0, matchLength);
		}

		private bool isUnlawfulCharArrays(char[] chars, int begin_, int length)
		{
			int all_len = chars.Length;
			int sum = begin_ + length;
			if (sum > all_len) {
				begin_++;
				length = matchLength;
			}
			sum = begin_ + length;
		
			if (sum > all_len)
				return false;
		
			char c = chars [begin_];
			bool is_exit_c = isExistChar(c);
			if (is_exit_c) {
				bool is_exist = isExistCharArrays(chars, begin_, length);
				if (is_exist) {
					return true;
				}
			}
			return isUnlawfulCharArrays(chars, begin_, length + 1);
		}
	}
}
