using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace JuliService.library
{
    public enum AggregateType
    {
        DS,
        Yager,
        Avg
    }

    class EvidentialReason
    {
        private int _CountOfElement;
        private int _CountOfEvidence;
        private double _scale;
        private AggregateType _AggregateType;
        private string[] _set;
        private double[][] _data;
        private double[] _result;

        public EvidentialReason()
        {
            _AggregateType = AggregateType.DS;
            _scale = 1;
        }

        public EvidentialReason(int countOfElement, int countOfEvidence)
        {
            _CountOfElement = countOfElement;
            _CountOfEvidence = countOfEvidence;

            _set = new string[_CountOfElement];
            _data = new double[_CountOfEvidence][];

            for (int i = 0; i < _CountOfEvidence; i++)
                _data[i] = new double[_CountOfElement];

            _AggregateType = AggregateType.DS;
            _scale = 1;

        }

        public bool SetData(double[][] list, int countElement, int countOfEvidence, ArrayList setList)
        {
           /* FileStream input = new FileStream(filename, FileMode.Open, FileAccess.Read);
            StreamReader fileReader = new StreamReader(input);

            try
            {
                //第一行
                string firstLine = fileReader.ReadLine();
                while (firstLine == "start")
                {
                    string inputRecord = fileReader.ReadLine();
                    string[] valueField;

                    if (inputRecord != null)
                    {
                        valueField = inputRecord.Split(' ');
                        _CountOfElement = Convert.ToInt32(valueField[0]);
                        _CountOfEvidence = Convert.ToInt32(valueField[1]);

                        if (valueField.Length > 2)
                            _scale = Convert.ToDouble(valueField[2]);
                    }

                    _set = new string[_CountOfElement];
                    _data = new double[_CountOfEvidence][];

                    for (int i = 0; i < _CountOfEvidence; i++)
                        _data[i] = new double[_CountOfElement];

                    for (int i = 0; i < _CountOfElement; i++)
                    {
                        inputRecord = fileReader.ReadLine();
                        if (inputRecord != null)
                        {
                            int col = 0;

                            valueField = inputRecord.Split(' ');
                            _set[i] = valueField[col];

                            for (col = 1; col <= _CountOfEvidence; col++)
                                _data[col - 1][i] = Convert.ToDouble(valueField[col]);
                        }
                        else
                        {
                            fileReader.Close();
                            input.Close();
                            return false;
                        }

                    }
                }
                
            }
            catch (IOException)
            {

                return false;
            }

            fileReader.Close();
            input.Close();

            return true;*/
            _CountOfElement = countElement;
            _CountOfEvidence = countOfEvidence;
            _data = new double[_CountOfEvidence][];
            _set = new string[_CountOfElement];
            for (int i = 0; i < _CountOfEvidence;i++ )
                _data[i]=new double[_CountOfElement];
           /* for (int i = 0; i <_CountOfElement; i++)
            {
                _data[0][i] = Convert.ToDouble(list_1[i]);
                _data[1][i] = Convert.ToDouble(list_2[i]);
            }*/
            for (int i = 0; i < _CountOfEvidence; i++)
            {
                for (int j = 0; j < _CountOfElement; j++)
                {
                    _data[i][j] = list[i][j];
                }
            }
           for (int i = 0; i < setList.Count; i++)
           {
                    _set[i] = Convert.ToString(setList[i]);
           }
            return true;
        }

        public void SetData(string[] elements, double[][] data)
        {
            for (int i = 0; i < _CountOfElement; i++)
                _set[i] = elements[i];

            for (int i = 0; i < _CountOfEvidence; i++)
                for (int j = 0; j < _CountOfElement; j++)
                    _data[i][j] = data[i][j];
        }

        public void SetData(int countOfElement, int countOfEvidence, string[] elements, double[][] data)
        {
            _CountOfElement = countOfElement;
            _CountOfEvidence = countOfEvidence;

            _set = new string[_CountOfElement];
            _data = new double[_CountOfEvidence][];

            for (int i = 0; i < _CountOfEvidence; i++)
                _data[i] = new double[_CountOfElement];

            SetData(elements, data);
        }

        public bool Reasoning()
        {
            _result = new double[_CountOfElement];

            for (int i = 1; i < _CountOfEvidence; i++)
            {
                if (i == 1)
                    switch (_AggregateType)
                    {
                        case AggregateType.DS:
                            _result = Aggregate_DS(_data[i - 1], _data[i]);
                            break;
                        case AggregateType.Yager:
                            _result = Aggregate_Yager(_data[i - 1], _data[i]);
                            break;
                        case AggregateType.Avg:
                            _result = Aggregate_Avg(_data[i - 1], _data[i]);
                            break;
                        default:
                            _result = Aggregate_DS(_data[i - 1], _data[i]);
                            break;
                    }
                else
                    switch (_AggregateType)
                    {
                        case AggregateType.DS:
                            _result = Aggregate_DS(_result, _data[i]);
                            break;
                        case AggregateType.Yager:
                            _result = Aggregate_Yager(_result, _data[i]);
                            break;
                        case AggregateType.Avg:
                            _result = Aggregate_Avg(_result, _data[i]);
                            break;
                        default:
                            _result = Aggregate_DS(_result, _data[i]);
                            break;
                    }
            }

            return true;
        }

        //----------------------------------------------------------------------------------------------------------
        //功能：DS合成公式
        //----------------------------------------------------------------------------------------------------------
        private double[] Aggregate_DS(double[] data1, double[] data2)
        {
            double[] result = new double[_CountOfElement];
            double k = CalcK(data1, data2);

            for (int i = 0; i < _CountOfElement; i++)
            {
                double w = 0.0;

                for (int j = 0; j < _CountOfElement; j++)
                {
                    if (_set[j].Contains(_set[i]))
                        if (i != j)
                            w = w + data1[i] * data2[j] + data2[i] * data1[j];
                        else
                            w = w + data1[i] * data2[j];
                }
                w = w * _scale / (1.0d * _scale * _scale - k);
                result[i] = w;
            }


            return result;

        }

        //Yager合成公式
        private double[] Aggregate_Yager(double[] data1, double[] data2)
        {
            double[] result = new double[_CountOfElement];
            double k = CalcK(data1, data2);

            for (int i = 0; i < _CountOfElement; i++)
            {
                double w = 0.0;

                for (int j = 0; j < _CountOfElement; j++)
                {
                    if (_set[j].Contains(_set[i]))
                        if (i != j)  
                            w = w + data1[i] * data2[j] + data2[i] * data1[j];
                        else
                            w = w + data1[i] * data2[j];

                    //未分配信任度
                    if (i == 0 && j == 0)
                        w = w + k;
                }

                result[i] = w / _scale;
            }

            return result;
        }

        private double[] Aggregate_Avg(double[] data1, double[] data2)
        {
            double[] result = new double[_CountOfElement];
            double k = CalcK(data1, data2);

            for (int i = 0; i < _CountOfElement; i++)
            {
                double w = 0.0;
                int count = 0;

                for (int j = 0; j < _CountOfElement; j++)
                {
                    if (_set[j].Contains(_set[i]))
                        if (i != j)
                        {
                            double d1 = data1[i] + data2[j];
                            double d2 = data2[i] + data1[j];
                            count = count + 2;
                            w = w + d1 + d2;
                        }
                        else
                        {
                            double d = data1[i] + data2[j];
                            count++;
                            w = w + d;
                        }
                }
                if (count == 0)
                    w = 0;
                else
                    w = w / count;
                result[i] = w;
            }


            return result;

        }

        private double CalcK(double[] data1, double[] data2)
        {
            double k = 0.0;

            for (int l = 0; l < _CountOfElement; l++)
            {
                for (int m = 0; m < _CountOfElement; m++)
                {
                    if (data1[l] > 0 && data2[m] > 0)
                        if (IntersectNull(_set[l], _set[m]))
                            k = k + data1[l] * data2[m];
                }

            }

            return k;
        }

        private static bool IntersectNull(string elem1, string elem2)
        {
            for (int i = 0; i < elem1.Length; i++)
            {
                string s = new string(elem1[i], 1);
                if (elem2.Contains(s))
                    return false;
            }

            return true;
        }

        public double[] GetResult()
        {
            return _result;
        }

        public string[] GetElements()
        {
            return _set;
        }

        public double[] GetData(int index)
        {
            return _data[index];
        }

        public double GetData(int evidence, int element)
        {
            return _data[evidence][element];
        }

        public int ElementCount
        {
            get
            {
                return _CountOfElement;
            }
        }

        public int EvidenceCount
        {
            get
            {
                return _CountOfEvidence;
            }
        }
        public double Scale
        {
            get
            {
                return _scale;
            }
        }

        public AggregateType AggregateType
        {
            get
            {
                return _AggregateType;
            }
            set
            {
                _AggregateType = value;
            }
        }
    }
}
