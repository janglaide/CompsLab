using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CompsLab
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter filename: ");
            var filename = Console.ReadLine();

            if (File.Exists(filename))
            {
                string[] textInFile = File.ReadAllLines(filename, Encoding.Default);
                List<Element> incidentList = new List<Element>();
                int[] splitted = null;
                for (var i = 0; i < textInFile.Length; i++)
                {
                    splitted = SplitText(textInFile[i]);
                    incidentList.Add(new Element(splitted));
                }
                Console.WriteLine("Enter start node: ");
                var start = Console.ReadLine();
                var Start = int.Parse(start);
                Console.WriteLine("Enter end node:");
                var end = Console.ReadLine();
                var End = int.Parse(end);

                List<int> wave = new List<int>();
                wave.Add(Start);
                SetValues(incidentList, Start, End, wave);

                var finalValue = incidentList[End - 1].Value;

                var flag = true;
                List<string> resultPaths = new List<string>();

                while (flag == true)
                {
                    List<int> path = new List<int>();

                    FindPath(incidentList, path, End, Start, finalValue, End);
                    

                    if(incidentList[End - 1].IsGone == true)
                    {
                        break;
                    }
                    var s = "";
                    foreach(var i in path)
                    {
                        s += i.ToString();
                        s += " ";
                    }
                    Console.WriteLine(" The path found: " + s);
                    incidentList[End - 1].Done = false;
                    resultPaths.Add(s);
                    File.WriteAllLines("output.txt", resultPaths);
                }

            }
            else
            {
                Console.WriteLine("The file does not exist");
            }
            Console.ReadKey();
        }

        static void FindPath(List<Element> list, List<int> res, int currentNode, int start, int valueCounter, int end)
        {
            while(currentNode != start)
            {
                res.Add(currentNode);
                var connectionsQuantity = list[currentNode - 1].Connections.Count;
                var forFlag = 0;
                for(var i = 0; i < connectionsQuantity; i++)
                {
                    if(list[list[currentNode - 1].Connections[i] - 1].Value < valueCounter && list[list[currentNode - 1].Connections[i] - 1].IsGone == false)
                    {
                        FindPath(list, res, list[currentNode - 1].Connections[i], start, list[list[currentNode - 1].Connections[i] - 1].Value, end);
                    }
                    if (list[end - 1].Done == true)
                    {
                        break;
                    }
                    forFlag++;
                }
                if(forFlag == connectionsQuantity)
                {
                    list[currentNode - 1].IsGone = true;
                    break;
                }
                if(list[end - 1].Done == true)
                {
                    break;
                }
            }
            if (currentNode == start)
            {
                res.Add(currentNode);
                SetFlags(list, res);
                list[end - 1].Done = true;
            }

        }

        static void SetFlags(List<Element> list, List<int> res)
        {
            for(var i = 0; i < res.Count; i++)
            {
                if(i != 0 && i != res.Count - 1)
                {
                    list[res[i] - 1].IsGone = true;
                }
            }
        }

        static bool CheckWaves(int connections, List<Element> list, int node)
        {
            for(var i = 0; i < connections; i++)
            {
                if(list[list[node - 1].Connections[i] - 1].Value == -1)
                {
                    return true;
                }
            }
            return false;
        }

        static void SetValues(List<Element> list, int currentNode, int end, List<int> waveList)
        {
            var counter = 0;
            while(counter < waveList.Count)
            {
                currentNode = waveList[counter];
                if (currentNode == end)
                {
                    var flag = CheckWaves(list[currentNode - 1].Connections.Count, list, currentNode);
                    SetMaxValue(list[currentNode - 1].Connections.Count, list, currentNode);
                    if (flag == false)
                    {
                        break;
                    }

                }
                else
                {
                    if (list[currentNode - 1].Value == -1 || IfEndIsClose(list, currentNode, end))
                    {
                        SetMaxValue(list[currentNode - 1].Connections.Count, list, currentNode);
                        for(var i = 0; i < list[currentNode - 1].Connections.Count; i++)
                        {
                            if (list[list[currentNode - 1].Connections[i] - 1].Value == -1 || list[currentNode - 1].Connections[i] == end)
                            {
                                waveList.Add(list[currentNode - 1].Connections[i]);
                            }
                        }
                    }
                }
                counter++;
            }

        }

        static bool IfEndIsClose(List<Element> list, int currentNode, int end)
        {
            for(var i = 0; i < list[currentNode - 1].Connections.Count; i++)
            {
                if(list[list[currentNode - 1].Connections[i] - 1].Value == end)
                {
                    return true;
                }
            }
            return false;
        }

        static void SetMaxValue(int connections, List<Element> list, int currentNode)
        {
            for(var i = 0; i < connections; i++)
            {
                if(list[list[currentNode - 1].Connections[i] - 1].Value >= list[currentNode - 1].Value)
                {
                    list[currentNode - 1].Value = list[list[currentNode - 1].Connections[i] - 1].Value + 1;
                }
            }
        }
            
       
        static int[] SplitText(string text)
        {
            List<int> numbers = new List<int>();
            var flag = false;
            var ch = "";
            for(var i = 0; i < text.Length; i++)
            {
                if(text[i] != ' ' && flag == false) //не пробел и предыдущий символ -- пробел
                {
                    if (i + 1 != text.Length)       //не последний символ
                    {
                        if (text[i + 1] != ' ')     //следующий символ -- не пробел
                        {
                            ch = text[i].ToString();//запомнить данный символ
                            flag = true;
                        }
                        else
                        {
                            numbers.Add(int.Parse(text[i].ToString()));
                        }
                    }
                    else //последний символ -- значит точно однозначное число
                    {
                        numbers.Add(int.Parse(text[i].ToString()));
                    }

                }
                else if (text[i] != ' ' && flag == true)// не пробел и предыдущий сивол -- не пробел
                {
                    var s = ch + text[i];
                    numbers.Add(int.Parse(s));
                    flag = false;
                }
                
            }
            int[] result = new int[numbers.Count];
            for(var i = 0; i < numbers.Count; i++)
            {
                result[i] = numbers[i];
            }
            return result;
        }
    }
}