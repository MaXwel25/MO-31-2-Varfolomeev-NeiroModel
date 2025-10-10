using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_31_2_Varfolomeev_NeiroModel.NeiroNet
{
    internal class HiddenLayer : Layer
    {
        public HiddenLayer(int non, int nopn, NeironType nt, string nm_Layer)
                : base(non, nopn, nt, nm_Layer)
        {
        }
    }
}
