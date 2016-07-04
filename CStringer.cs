using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Stringer
{
    public class CStringer
    {
        public Collection<object> Items = new Collection<object>();
        public CStringer() { }
        public CStringer(String value) { Items.Add(value); }
        public CStringer(char value) { Items.Add(value); }
        public CStringer(int value) { Items.Add(value); }
        public CStringer(float value) { Items.Add(value); }
        public CStringer(double value) { Items.Add(value); }
        public CStringer(bool value) { Items.Add(value); }
        public CStringer(object value) { Items.Add(value); }
        public CStringer(Collection<string> value) { foreach (string x in value) { Items.Add(x); } }
        public CStringer(CStringer value) { foreach (string x in value.Items) { Items.Add(x); } }
        public CStringer(Collection<char> value) { foreach (char x in value) { Items.Add(x); } }
        public CStringer(Collection<int> value) { foreach (int x in value) { Items.Add(x); } }
        public CStringer(Collection<float> value) { foreach (float x in value) { Items.Add(x); } }
        public CStringer(Collection<double> value) { foreach (double x in value) { Items.Add(x); } }
        public CStringer(Collection<bool> value) { foreach (bool x in value) { Items.Add(x); } }
        public CStringer(Collection<object> value) { foreach (object x in value) { Items.Add(x); } }
        public CStringer(String[] value) { foreach (String x in value) { Items.Add(x); } }
        public CStringer(char[] value) { foreach (char x in value) { Items.Add(x); } }
        public CStringer(int[] value) { foreach (int x in value) { Items.Add(x); } }
        public CStringer(float[] value) { foreach (float x in value) { Items.Add(x); } }
        public CStringer(double[] value) { foreach (double x in value) { Items.Add(x); } }
        public CStringer(bool[] value) { foreach (bool x in value) { Items.Add(x); } }
        public CStringer(object[] value) { foreach (object x in value) { Items.Add(x); } }
        public CStringer(ListBox.ObjectCollection value) { foreach (object x in value) { Items.Add(x); } }
        public CStringer(ListBox.SelectedObjectCollection value) { foreach (object x in value) { Items.Add(x); } }

        public void Add(String value) { Items.Add(value); }
        public void Add(char value) { Items.Add(value); }
        public void Add(int value) { Items.Add(value); }
        public void Add(float value) { Items.Add(value); }
        public void Add(double value) { Items.Add(value); }
        public void Add(bool value) { Items.Add(value); }
        public void Add(object value) { Items.Add(value); }
        public void Add(Collection<string> value) { foreach (string x in value) { Items.Add(x); } }
        public void Add(CStringer value) { foreach (string x in value.Items) { Items.Add(x); } }
        public void Add(Collection<char> value) { foreach (char x in value) { Items.Add(x); } }
        public void Add(Collection<int> value) { foreach (int x in value) { Items.Add(x); } }
        public void Add(Collection<float> value) { foreach (float x in value) { Items.Add(x); } }
        public void Add(Collection<double> value) { foreach (double x in value) { Items.Add(x); } }
        public void Add(Collection<bool> value) { foreach (bool x in value) { Items.Add(x); } }
        public void Add(Collection<object> value) { foreach (object x in value) { Items.Add(x); } }
        public void Add(Collection<CStringer> value) { foreach (object x in value) { Items.Add(x); } }
        public void Add(String[] value) { foreach (String x in value) { Items.Add(x); } }
        public void Add(char[] value) { foreach (char x in value) { Items.Add(x); } }
        public void Add(int[] value) { foreach (int x in value) { Items.Add(x); } }
        public void Add(float[] value) { foreach (float x in value) { Items.Add(x); } }
        public void Add(double[] value) { foreach (double x in value) { Items.Add(x); } }
        public void Add(bool[] value) { foreach (bool x in value) { Items.Add(x); } }
        public void Add(object[] value) { foreach (object x in value) { Items.Add(x); } }
        public void Add(ListBox.ObjectCollection value) { foreach (object x in value) { Items.Add(x); } }
        public void Add(ListBox.SelectedObjectCollection value) { foreach (object x in value) { Items.Add(x); } }

        public void Insert(int index, String value) { Items.Insert(index, value); }
        public override String ToString()
        {
            String result = "";
            foreach (object x in Items) { result += x + "\r\n"; }
            return result;
        }
        public String ToString(String delimiter)
        {
            String result = "";
            foreach (object x in Items) { result += x.ToString() + delimiter; }
            if (result.Length > delimiter.Length)
                result = result.Substring(0, result.Length - delimiter.Length);
            return result;
        }
        public CStringer GetSorted(Collection<string> sortlist)
        {
            CStringer result = new CStringer();
            foreach (string x in sortlist)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    string temp = Items[i].ToString();
                    if (temp.Contains(x))
                    {
                        result.Add(temp);
                    }
                }
            }
            return result;
        }
        public string GetValue(string name, char delimiter)
        {
            string[] temp;
            foreach (string x in Items)
            {
                temp = x.Split(delimiter);
                if (temp.Length > 1)
                    if (temp[0] == name)
                        return x.Split(delimiter)[1];
            }
            return "";
        }
        public void RemoveAt(int index)
        {
            Items.RemoveAt(index);
        }
    }
}
