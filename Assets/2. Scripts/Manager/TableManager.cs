using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SimpleJSON;
public class TableManager: CSSingleTon<TableManager>
{
    public enum eTableJsonNames
    {
        Items = 0,

        Count
    }

    // �ֿ�Ǵ� �з��� �θ�Ŭ����, �ƴϸ� ����
    public enum eIndex
    {
        Index,
        Name,
        Text,

        Count
    }
    Dictionary<string, Dictionary<int, Dictionary<string, string>>> _tables;

    public void CreateFiles()
    {
        _tables = new Dictionary<string, Dictionary<int, Dictionary<string, string>>>();

        for (int fileIndex = 0; fileIndex < (int)eTableJsonNames.Count; fileIndex++)
        {
            string tableName = ((eTableJsonNames)fileIndex).ToString();
            TextAsset data = Resources.Load<TextAsset>("JsonFiles/" + tableName);
            StringReader sr = new StringReader(data.text);
            Dictionary<int, Dictionary<string, string>> datas = new Dictionary<int, Dictionary<string, string>>();

            string Line = sr.ReadLine();
            string dataLine = Line.Split("[{\"")[1];
            dataLine = dataLine.Split("\"}]}")[0];

            string[] columnData = dataLine.Split("\"},{\"");
            for (int i = 0; i < columnData.Length; i++)
            {
                Dictionary<string, string> tmpData = new Dictionary<string, string>();
                string[] dicData = columnData[i].Split("\",\"");
                for (int j = 0; j < dicData.Length; j++)
                {
                    string[] onlydata = dicData[j].Split("\":\"");
                    tmpData.Add(unicodeConvert(onlydata[0]), unicodeConvert(onlydata[1]));
                }
                datas.Add(i, tmpData);
            }
            _tables.Add(tableName, datas);
            //ShowData(tableName);
        }
    }
    void ShowData(string FileName)
    {
        Debug.Log(FileName);
        // ���̺�� [�ε����ѹ� : (�÷���, ��) ...]
        foreach (int index in _tables[FileName].Keys)
        {
            string print = "";
            print += "[" + index + " :";
            foreach (KeyValuePair<string, string> data in _tables[FileName][index])
            {
                print += string.Format(" ({0}, {1})", data.Key, data.Value);
            }
            Debug.Log(print);
        }
    }
    //(etablejsonNames, �ε��� ��ȣ �÷��̸�)���� �ش������� ���ڿ���, ������, �Ǽ�, bool�� �޴� �Լ�
    public string TakeString(eTableJsonNames table, int index, string column)
    {
        if (_tables.ContainsKey(table.ToString()))
        {
            Dictionary<int, Dictionary<string, string>> record = _tables[table.ToString()];
            if (record.ContainsKey(index))
            {
                Dictionary<string, string> colData = record[index];
                if (colData.ContainsKey(column))
                {
                    return colData[column];
                }
            }
            else return string.Empty;
        }
        return string.Empty;
    }
    public int TakeInt(eTableJsonNames table, int index, string column)
    {
        string data = TakeString(table, index, column);
        if (data.Length == 0) return -1;
        else return int.Parse(data);
    }
    public bool TakeBool(eTableJsonNames table, int index, string column)
    {
        return bool.Parse(_tables[table.ToString()][index][column]);
    }
    public float Takefloat(eTableJsonNames table, int index, string column)
    {
        string data = TakeString(table, index, column);
        if (data.Length == 0) return -1;
        else return float.Parse(data);
    }
    private string unicodeConvert(string str)
    {
        string sb = "";
        char ch;
        for (int i = 0; i < str.Length; i++)
        {
            ch = str[i];
            if (ch == '\\' && str[i + 1] == 'u')
            {
                sb+= ((char)System.Convert.ToInt32(str.Substring(i + 2, 4), 16));
                i += 5;
                continue;
            }
            sb+= (ch);
        }
        return sb;
    }
}
