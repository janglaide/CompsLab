using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompsLab
{
    class Element
    {
        private int _node;
        private List<int> _connections;
        private int _value;
        private bool _isGone;
        private bool _done;
        public Element(params int[] arr)
        {
            _connections = new List<int>();
            for(var i = 0; i < arr.Length; i++)
            {
                if (i == 0)
                    _node = arr[i];
                else
                    _connections.Add(arr[i]);
            }
            Value = -1;
            _isGone = false;
            _done = false;
        }

        public int Node { get => _node; set => _node = value; }
        public List<int> Connections { get => _connections; set => _connections = value; }
        public int Value { get => _value; set => _value = value; }
        public bool IsGone { get => _isGone; set => _isGone = value; }
        public bool Done { get => _done; set => _done = value; }
    }
}
