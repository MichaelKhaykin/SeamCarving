using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeamCarving
{
    static class Program
    {
        static void Day20()
        {
            var input = System.IO.File.ReadAllLines("Input.txt");

            Dictionary<int, (string, string)> map = new Dictionary<int, (string, string)>();

            List<string> s = new List<string>();
            
            for(int i = 0; i < input.Length; i++)
            {
                if(input[i] == "")
                {
                    var id = int.Parse(s[0].Split(' ')[1].Replace(":", ""));
                    var first = s[1];
                    var last = s[s.Count - 1];

                    map.Add(id, (first, last));

                    s = new List<string>();

                    continue;
                }

                s.Add(input[i]);
            }

            var squaresize = (int)Math.Sqrt(map.Keys.Count);

        }

        static string[] Update(string[] file)
        {
            List<string> newString = new List<string>();
            for (int i = 0; i < file.Length; i++)
            {
                string current = file[i];
                if (file[i].Split(':')[0] == "8")
                {
                    current = "8: 42 | 42 8";
                }
                if (file[i].Split(':')[0] == "11")
                {
                    current = "11: 42 31 | 42 11 31";
                }
                newString.Add(current);
            }
            return newString.ToArray();
        }

        public class Node
        {
            public int Value { get; set; }
            public char Terminal { get; set; }
            public List<Node> Nodes { get; set; }
            public List<List<Node>> Optionals { get; set; }
            public Node(int value)
            {
                Value = value;
                Terminal = ' ';
                Nodes = new List<Node>();
                Optionals = new List<List<Node>>();
            }
        }

        static void ZeroesLeafNodes(HashSet<int> nodes, Node curr, Node prev)
        {
            if(curr.Terminal != ' ')
            {
                nodes.Add(prev.Value);

                return;
            }

            foreach(var node in curr.Nodes)
            {
                ZeroesLeafNodes(nodes, node, curr);
            }
            foreach(var list in curr.Optionals)
            {
                foreach(var node in list)
                {
                    ZeroesLeafNodes(nodes, node, curr);
                }
            }
        }

        static (Dictionary<int, Node>, int) GenerateGraph(string[] file)
        {
            Dictionary<int, Node> nodes = new Dictionary<int, Node>();
            int resumeIndex = -1;

            for (int k = 0; k < file.Length; k++)
            {
                var line = file[k];

                if (line == "")
                {
                    resumeIndex = k + 1;
                    break;
                }

                var split = line.Split(':');

                var a = int.Parse(split[0]);

                if (nodes.ContainsKey(a) == false)
                {
                    nodes.Add(a, new Node(a));
                }

                Node node = nodes[a];

                var rest = split[1];

                var fx = Fix(rest);

                if (rest.Contains('|'))
                {
                    var sides = rest.Split('|');

                    foreach (var side in sides)
                    {
                        List<Node> sideNodes = new List<Node>();
                        var minor = side.Split(' ').Where((x) => x != "").Select((x) => int.Parse(x)).ToList();
                        for (int i = 0; i < minor.Count; i++)
                        {
                            if (!nodes.ContainsKey(minor[i]))
                            {
                                nodes.Add(minor[i], new Node(minor[i]));
                            }
                            sideNodes.Add(nodes[minor[i]]);
                        }

                        node.Optionals.Add(sideNodes);
                    }
                }
                else if (fx == "a" || fx == "b")
                {
                    node.Terminal = fx[0];
                }
                else
                {
                    var neighbors = rest.Split(' ').Where((x) => x != "").Select((x) => int.Parse(x)).ToList();

                    for (int i = 0; i < neighbors.Count; i++)
                    {
                        if (!nodes.ContainsKey(neighbors[i]))
                        {
                            nodes.Add(neighbors[i], new Node(neighbors[i]));
                        }
                        node.Nodes.Add(nodes[neighbors[i]]);
                    }
                }
            }

            return (nodes, resumeIndex);
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ClockDemo());
        }

        static string Fix(string a)
        {
            StringBuilder be = new StringBuilder();
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] >= 'a' && a[i] <= 'b')
                {
                    be.Append(a[i]);
                }
            }
            return be.ToString();
        }

        static int currentIndex = 0;
        static bool didWork = false;
        static bool didReachANYLeaf = false;
        static int DoTheseMatch(Node node, string currentStringToMatch)
        { 
            if (currentIndex >= currentStringToMatch.Length) return 0;
            
            if (node.Terminal != ' ')
            {
                int res = node.Terminal == currentStringToMatch[currentIndex] ? 1 : -1;

                if (res == 1 && currentIndex + 1 >= currentStringToMatch.Length)
                {
                    didWork = true;
                }

                return res;
            }

            var mandatories = node.Nodes;

            int oldIndex = currentIndex;
            for (int i = 0; i < mandatories.Count; i++)
            {
                int result = DoTheseMatch(mandatories[i], currentStringToMatch);
                if (result == -1)
                {
                    return -1;
                }

                currentIndex += result;

            }

            if (currentIndex > oldIndex)
            {
                return 0;
            }

            var optionals = node.Optionals;

            for (int i = 0; i < optionals.Count; i++)
            {
                var list = optionals[i];
                int ogIndex = currentIndex;

                foreach (var item in list)
                {
                    int result = DoTheseMatch(item, currentStringToMatch);

                    if (result == -1)
                    {
                        currentIndex = ogIndex;
                        break;
                    }

                    currentIndex += result;
                }

                if (currentIndex > ogIndex)
                {
                    return 0;
                }
            }

            return -1;
        }

        static StringBuilder TestBuilder(Node current)
        {
            if (current.Terminal != ' ')
            {
                return new StringBuilder(current.Terminal.ToString());
            }

            StringBuilder curr = new StringBuilder();

            for (int i = 0; i < current.Nodes.Count; i++)
            {
                curr.Append(TestBuilder(current.Nodes[i]));
                if (i == current.Nodes.Count - 1)
                {
                    //curr.Append($"({TestBuilder(current.Nodes[i])})");
                }
                else
                {
                    //curr.Append($"({TestBuilder(current.Nodes[i])}) ");
                }
            }

            for (int i = 0; i < Math.Min(current.Optionals.Count, 1); i++)
            {
                StringBuilder cOption = new StringBuilder();
                //cOption.Append("(");

                var inner = current.Optionals[i];

                for (int j = 0; j < inner.Count; j++)
                {
                    //var add = j == inner.Count - 1 ? "" : " ";
                    //cOption.Append(TestBuilder(inner[j]) + add);
                    cOption.Append(TestBuilder(inner[j]));
                }
                //cOption.Append(")");
                //if (i == current.Optionals.Count - 1)
                //{
                //    curr.Append(cOption);
                //}
                //else
                //{
                //    curr.Append(cOption + "|");
                //}
                curr.Append(cOption);
            }

            return curr;
        }
        static StringBuilder TestBuilderWithParen(Node current)
        {
            if (current.Terminal != ' ')
            {
                return new StringBuilder($"{current.Terminal}");
            }

            StringBuilder curr = new StringBuilder();

            for (int i = 0; i < current.Nodes.Count; i++)
            {
                curr.Append($"{TestBuilderWithParen(current.Nodes[i])}");
            }

            if (current.Optionals.Count == 0) return curr;

            curr.Append("(");
            for (int i = 0; i < current.Optionals.Count; i++)
            {
                StringBuilder cOption = new StringBuilder();

                var inner = current.Optionals[i];

                for (int j = 0; j < inner.Count; j++)
                {
                    cOption.Append(TestBuilderWithParen(inner[j]));
                }

                if (i == current.Optionals.Count - 1)
                {
                    curr.Append(cOption);
                }
                else
                {
                    curr.Append(cOption + "|");
                }
            }
            curr.Append(")");

            return curr;
        }
    }
}
