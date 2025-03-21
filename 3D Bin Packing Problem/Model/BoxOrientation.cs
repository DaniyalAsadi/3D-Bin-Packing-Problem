using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Bin_Packing_Problem.Model
{
    /// <summary>
    /// Box orientation
    /// </summary>
    public enum BoxOrientation
    {
        /// <summary>
        /// Width Height Length
        /// </summary>
        WHL,
        /// <summary>
        /// Width Length Height
        /// </summary>
        WLH,
        /// <summary>
        /// Length Width Height
        /// </summary>
        LWH,
        /// <summary>
        /// Length Height Width
        /// </summary>
        LHW,
        /// <summary>
        /// Height Length Width
        /// </summary>
        HLW,
        /// <summary>
        /// Height Width Length
        /// </summary>
        HWL
    }
}

