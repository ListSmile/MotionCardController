using MotionController.Core.Interface;
using MotionController.ZMotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionController.MotionController.ZMotion.Entities
{
    public class ZMotionInput : IInput
    {
        private string _name;
        private int _indexport;
        private IntPtr _handle;
        public ZMotionInput(string name, int indexport, IntPtr handle)
        {
            _name = name;
            _indexport = indexport;
            _handle = handle;
        }
        public string Name => _name;

        public int IndexPort => _indexport;

        public bool Value => GetValue();
        private bool GetValue() 
        {
            uint val = 0;
            int ret = Zmcaux.ZAux_Direct_GetIn(_handle, _indexport,ref val);
            if (ret != 0) 
            {
                return false;
            }
            return val>0;
        }
    }
}
